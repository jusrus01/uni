using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using WinBioWrapper.Types;

namespace ConsoleApplication
{
    public class ManagedException : Exception
    {
        public ManagedException(string msg) : base(msg)
        {
        }
    }

    public class Lab
    {
        // Could be not supported on that device.
        // Can be checked by "Demo" app before defense.
        private WINBIO_BIOMETRIC_SUBTYPE[] _types = new WINBIO_BIOMETRIC_SUBTYPE[]
        {
            WINBIO_BIOMETRIC_SUBTYPE.WINBIO_ANSI_381_POS_LH_THUMB,
            WINBIO_BIOMETRIC_SUBTYPE.WINBIO_ANSI_381_POS_RH_THUMB
        };

        private readonly Dictionary<Guid, Action> _behaviors = new Dictionary<Guid, Action>();

        private WINBIO_IDENTITY _currentIdentity = new WINBIO_IDENTITY
        {
            Type = WINBIO_IDENTITY_TYPE.WINBIO_ID_TYPE_GUID // if this does not work with/without then - introduce own GUID and store in memory.
        };

        private IntPtr _currentSessionHandle = IntPtr.Zero;
        private uint _currentSessionUnitId = 0;

        public void Run()
        {
            try
            {
                OnStart();

                for (int i = 0; i < _types.Length; i++)
                {
                    var templateId = EnrollFingerprintTemplate(_types[i]);
                    AttachBehavior(templateId, i);
                }

                Log($"{_types.Length} templates were read. Going into only identification mode. Please scan finger to invoke function");

                for (int i = 0; i < _types.Length; i++)
                {
                    Guid readTemplateId;

                    do
                    {
                        readTemplateId = Identify(_types[i]);
                    }
                    while (!ExecuteAttachedBehavior(readTemplateId));
                }
            }
            catch (ManagedException e)
            {
                Log(e.Message);
            }
            catch (Exception ex)
            {
                Log($"Something went very wrong: {JsonConvert.SerializeObject(ex, Formatting.Indented)}");
            }
            finally
            {
                OnExit();

                Log("Press any key to close application");
                Console.ReadKey();
            }
        }

        private bool ExecuteAttachedBehavior(Guid templateId)
        {
            if (!_behaviors.TryGetValue(templateId, out var action))
            {
                Log("Could not find associated behavior");
                return false;
            }

            action();
            return true;
        }

        private Guid Identify(WINBIO_BIOMETRIC_SUBTYPE type)
        {
            const int retryCount = 10;
            int count = 0;

            while (count < retryCount)
            {
                Log("Identifying...");
                Log("Swipe finger on a sensor");
                WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_SUCCESS;
                WinBioHelpers.BringConsoleToFront();
                uint ret = WinBio.Native.WinBioIdentify(
                    _currentSessionHandle,
                    ref _currentSessionUnitId,
                    ref _currentIdentity,
                    ref type,
                    ref rejectDetail);

                if (ret == 0)
                {
                    return _currentIdentity.TemplateGuid;
                }

                if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_E_BAD_CAPTURE)
                {
                    Log("Bad capture, reason: " + rejectDetail.ToString());
                }
                else
                {
                    EnsureSuccess(ret, "Something went wrong while identifying");
                }

                count++;
            }

            throw new ManagedException($"Failed to identify after {retryCount} retries");
        }

        private void AttachBehavior(Guid templateId, int index)
        {
            Action action;

            if (index == 0)
            {
                action = () => Log("Laba");
            }
            else if (index == 1)
            {
                action = () => Log("diena");
            }
            else
            {
                action = () => Log("More than two templates are not supported");
            }

            Log($"For template {templateId} assigned function will print: ");
            action();

            _behaviors.Add(templateId, action);
        }

        private Guid EnrollFingerprintTemplate(WINBIO_BIOMETRIC_SUBTYPE type)
        {
            Log($"Starting enrollment for {type} type");

            WinBioHelpers.BringConsoleToFront();

            uint ret = WinBio.Native.WinBioEnrollBegin(_currentSessionHandle, type, _currentSessionUnitId);
            EnsureSuccess(ret, "Failed to begin enrollment process");

            WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_SUCCESS;

            for (int swipeCount = 0; ; ++swipeCount)
            {
                Log("Swipe the sensor to capture a sample...");

                ret = WinBio.Native.WinBioEnrollCapture(_currentSessionHandle, ref rejectDetail);

                if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_I_MORE_DATA)
                {
                    Log("More data required");
                    continue;
                }

                if (ret != 0)
                {
                    if ((WINBIO_ERRORS)ret == WINBIO_ERRORS.WINBIO_E_BAD_CAPTURE)
                    {
                        Log("Bad capture, reason: " + rejectDetail.ToString());
                    }
                    else
                    {
                        EnsureSuccess(ret, "Something went wrong while doing capture");
                    }
                }
                else
                {
                    Log("Template completed");
                    break;
                }
            }

            Log("Trying to save template to database");

            bool isNew = false;
            ret = WinBio.Native.WinBioEnrollCommit(_currentSessionHandle, ref _currentIdentity, ref isNew);
            EnsureSuccess(ret, "Could not commit template");

            Log("Template is new: " + isNew);
            Log("Template identity:");
            Log(JsonConvert.SerializeObject(_currentIdentity, Formatting.Indented));

            return _currentIdentity.TemplateGuid;
        }

        private void OnExit()
        {
            for (int i = 0; i < _types.Length; i++)
            {
                CleanUp(_types[i]);
            }

            CloseSession();
        }

        private void CleanUp(WINBIO_BIOMETRIC_SUBTYPE type)
        {
            Log("Start clean up");

            // Get all devices

            int n_units = 0;
            IntPtr units_ptr = IntPtr.Zero;
            uint ret = WinBio.Native.WinBioEnumBiometricUnits(WINBIO_BIOMETRIC_TYPE.WINBIO_TYPE_FINGERPRINT, ref units_ptr, ref n_units);
            if (!IsSuccess(ret))
            {
                return;
            }

            WINBIO_UNIT_SCHEMA[] units = new WINBIO_UNIT_SCHEMA[n_units];
            WinBioHelpers.MarshalUnmananagedArray2Struct<WINBIO_UNIT_SCHEMA>(units_ptr, n_units, out units);

            foreach (var unit in units)
            {
                IntPtr identity_ptr = IntPtr.Zero;
                WINBIO_IDENTITY identity = default(WINBIO_IDENTITY);
                uint foundUnitId = unit.UnitId;
                WINBIO_REJECT_DETAIL rejectDetail = WINBIO_REJECT_DETAIL.WINBIO_FP_MERGE_FAILURE;

                // Try to identify each enrolled identity for the unit
                ret = WinBio.Native.WinBioIdentify(
                    _currentSessionHandle,
                    ref foundUnitId,
                    ref identity,
                    ref type,
                    ref rejectDetail);

                if (!IsSuccess(ret))
                {
                    continue;
                }

                int n_subfactors = 0;
                IntPtr subfactors_ptr = IntPtr.Zero;
                ret = WinBio.Native.WinBioEnumEnrollments(
                    _currentSessionHandle,
                    unit.UnitId,
                    identity,
                    ref subfactors_ptr,
                    ref n_subfactors);

                if (!IsSuccess(ret))
                {
                    continue;
                }

                byte[] subfactors = new byte[n_subfactors];
                Marshal.Copy(subfactors_ptr, subfactors, 0, n_subfactors);

                // Delete each template for the identity
                foreach (byte subfactor in subfactors)
                {
                    ret = WinBio.Native.WinBioDeleteTemplate(
                        _currentSessionHandle,
                        unit.UnitId,
                        identity,
                        (WINBIO_BIOMETRIC_SUBTYPE)subfactor);

                    if (IsSuccess(ret))
                    {
                        Console.WriteLine($"Deleted template for subfactor {(WINBIO_BIOMETRIC_SUBTYPE)subfactor}");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to delete template for subfactor {(WINBIO_BIOMETRIC_SUBTYPE)subfactor}");
                    }
                }
            }

            Log("Clean up done");
        }

        private void OnStart()
        {
            OpenSession();
            TryLocateFingerprintDevice();
        }

        private void OpenSession()
        {
            // https://github.com/tpn/winsdk-10/blob/9b69fd26ac0c7d0b83d378dba01080e93349c2ed/Include/10.0.14393.0/shared/winbio_types.h#L1027
            // Well-known database IDs used by WinBioOpenSession
            // #define WINBIO_DB_DEFAULT           ((GUID *)1)
            // #define WINBIO_DB_BOOTSTRAP         ((GUID *)2)
            // #define WINBIO_DB_ONCHIP            ((GUID *)3)
            const int defaultDatabaseId = 1; // NOTE: when WINBIO_POOL_TYPE is "private" then cannot use.

            Log("Trying to open session");

            uint ret = WinBio.Native.WinBioOpenSession(
                WINBIO_BIOMETRIC_TYPE.WINBIO_TYPE_FINGERPRINT,
                WINBIO_POOL_TYPE.WINBIO_POOL_SYSTEM,
                WINBIO_SESSION_FLAGS.WINBIO_FLAG_DEFAULT,
                IntPtr.Zero,
                0,
                defaultDatabaseId,
                ref _currentSessionHandle);
            EnsureSuccess(ret, "Failed to open session");

            Log("Session succesfully open");
        }

        private void CloseSession()
        {
            Log("Trying to close session");

            if (_currentSessionHandle != IntPtr.Zero)
            {
                uint ret = WinBio.Native.WinBioCloseSession(_currentSessionHandle);
                EnsureSuccess(ret, "Failed to close session");

                _currentSessionHandle = IntPtr.Zero;

                Log("Session succesfully closed");
            }
            else
            {
                Log("Nothing to close as session was not open");
            }
        }

        private void TryLocateFingerprintDevice()
        {
            const int retryCount = 3;
            int count = 0;

            while (count < retryCount)
            {
                Log("Trying to find sensor to use. Please tap the sensor.");

                WinBioHelpers.BringConsoleToFront(); // ?

                uint ret = WinBio.Native.WinBioLocateSensor(_currentSessionHandle, ref _currentSessionUnitId);
                
                if (IsSuccess(ret))
                {
                    Log($"Found succesfully device ID = {_currentSessionUnitId}");
                    return;
                }

                count++;
            }

            throw new ManagedException("Could not determine fingerprint device");
        }

        private void Log(string msg, [CallerMemberName] string callerName = "")
        {
            Console.WriteLine($"[ func: { callerName } ]: {msg}");
        }

        private static void EnsureSuccess(uint retVal, string msg)
        {
            if (retVal != 0)
            {
                var details = "Error: " + ((WINBIO_ERRORS)retVal).ToString();
                throw new ManagedException($"{msg} (Details: {details})");
            }
        }

        private bool IsSuccess(uint retVal)
        {
            if (retVal == 0)
            {
                return true;
            }

            Log("Error: " + ((WINBIO_ERRORS)retVal).ToString());

            return false;
        }
    }
}
