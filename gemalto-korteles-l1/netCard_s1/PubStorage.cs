using System.IO;

namespace MyCompany.MyOnCardApp
{
    public static class PubStorage
    {
        private const byte NewLineByte = 0x0A;
        public static string PubAbsolutePath = "D:\\Pub\\";

        /// <summary>
        /// Example from Gemalto docs:
        /// 
        /// public void Example(string filename, byte[] data)
        /// {
        ///    string[] dirs = Directory.GetDirectories(@"D:\Pub");
        ///    foreach (string directory in dirs)
        ///    {
        ///        FileStream fs = new FileStream(@"D:\Pub\" + directory + @"\" + filename,
        ///        FileMode.Create);
        ///        fs.Write(data, 0, data.Length);
        ///        fs.Close();
        ///    }
        /// }
        /// </summary>
        public static bool AppendLineToFileEnd(string fileName, byte[] content)
        {
            if (fileName == null)
            {
                return false;
            }

            if (content == null)
            {
                return false;
            }

            FileStream fs = null;
            try
            {
                // https://learn.microsoft.com/en-us/dotnet/api/system.io.filemode?view=net-8.0
                fs = new FileStream(PubAbsolutePath + fileName, FileMode.Append);
                fs.Write(content, 0, content.Length);

                byte[] newline = new byte[] { NewLineByte };
                fs.Write(newline, 0, newline.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                fs?.Close();
            }

            return true;
        }

        public static string[] ReadLinesFromLinePosition(string fileName, int fromLinePosition)
        {
            const string emptyResponseContent = "0";
            const int maxLinesReadPerFuctionCall = 10;

            if (fromLinePosition < 0 || fileName == null)
            {
                return new string[] { emptyResponseContent };
            }

            string[] readLines = new string[maxLinesReadPerFuctionCall + 1];

            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                fs = new FileStream(PubAbsolutePath + fileName, FileMode.OpenOrCreate);
                reader = new StreamReader(fs);

                // This just moves position to "from" position
                for (int i = 0; i < fromLinePosition; i++)
                {
                    if (reader.ReadLine() == null)
                    {
                        // Invalid "from", so return 0 to indicate, that nowhere to go
                        return new string[] { emptyResponseContent };
                    }
                }

                int readWords;
                for (readWords = 0; readWords < maxLinesReadPerFuctionCall; readWords++)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        readWords--;
                        break;
                    }

                    readLines[readWords] = line;
                }

                if (readWords < maxLinesReadPerFuctionCall)
                {
                    readLines[readWords + 1] = emptyResponseContent;
                }
                else
                {
                    readLines[readWords] = (readWords + fromLinePosition).ToString();
                }

                return readLines;
            }
            catch
            {
                return null;
            }
            finally
            {
                fs?.Close();
                reader?.Close();
            }
        }

        public static bool RemoveLineStartsWith(string fileName, string lineStartsWith)
        {
            if (fileName == null || lineStartsWith == null)
            {
                return false;
            }

            string fullPath = PubAbsolutePath + fileName;
            string tempFilePath = PubAbsolutePath + "temp_" + fileName;

            FileStream fs = null;
            StreamReader reader = null;
            StreamWriter writer = null;

            bool lineFound = false;
            try
            {
                fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs);

                writer = new StreamWriter(tempFilePath);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(lineStartsWith))
                    {
                        lineFound = true;
                        continue;
                    }

                    writer.WriteLine(line);
                }

                writer.Flush();
            }
            catch
            {
                return false;
            }
            finally
            {
                reader?.Close();
                writer?.Close();
                fs?.Close();
            }

            if (lineFound)
            {
                CopyFrom(tempFilePath, fullPath);

                return true;
            }
            else
            {
                File.Delete(tempFilePath);
                return false;
            }
        }

        private static void CopyFrom(string fromPath, string toPath)
        {
            if (fromPath == null || toPath == null)
            {
                return;
            }

            FileStream sourceStream = null;
            StreamReader sourceReader = null;
            FileStream destStream = null;
            StreamWriter destWriter = null;
            try
            {
                File.Delete(toPath);

                sourceStream = new FileStream(fromPath, FileMode.Open);
                sourceReader = new StreamReader(sourceStream);
                destStream = new FileStream(toPath, FileMode.Create);
                destWriter = new StreamWriter(destStream);

                string line = sourceReader.ReadLine();
                while (line != null)
                {
                    destWriter.WriteLine(line);
                    line = sourceReader.ReadLine();
                }
            }
            finally
            {
                sourceReader?.Close();
                destWriter?.Close();

                sourceStream?.Close();
                destStream?.Close();

                File.Delete(fromPath);
            }
        }
    }
}