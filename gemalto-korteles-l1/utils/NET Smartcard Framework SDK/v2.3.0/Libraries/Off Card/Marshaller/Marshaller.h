/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Marshaller.h;6 */
/*       6*[589368] 06-MAR-2008 19:14:48 (GMT) ksachdeva */
/*         "Add GetCardHandle() method definition." */
/*       5*[589286] 26-FEB-2008 18:54:14 (GMT) AMALI */
/*         "Use _MARSHALLER_LITE_ for xCL interface" */
/*       4*[588604] 18-SEP-2007 19:51:33 (GMT) sprevost */
/*         "Add extra index parameter to SmartCardMarshaller constructor." */
/*       3*[586321] 30-APR-2007 18:11:25 (GMT) sprevost */
/*         "Add GetReaderName() method declaration." */
/*       2*[576487] 14-AUG-2006 19:03:53 (GMT) sprevost */
/*         "Rename GetPCSC() into UpdatePCSCCardHandle()." */
/*       1*[576428] 13-AUG-2006 17:55:24 (GMT) ksachdeva */
/*         "Initial:" */
/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Marshaller.h;6 */


/* Below is the omniworks history from reference baseline */


// (c) Copyright Axalto Inc., 2005-2006                    
// ALL RIGHTS RESERVED UNDER COPYRIGHT LAWS.           
// CONTAINS CONFIDENTIAL AND TRADE SECRET INFORMATION. 

#ifndef _include_marshaller_h
#define _include_marshaller_h

MARSHALLER_NS_BEGIN

typedef void (*pCommunicationStream)(u1Array& st,u1Array& stM);

class SMARTCARDMARSHALLER_DLLAPI SmartCardMarshaller
{

private:
    u4            nameSpaceHivecode;
    u2            typeHivecode;    
    u2            portNumber;
    std::string*  uri;

#ifdef _XCL_
    XCLBroker*    pcsc;
#else
    PCSC*         pcsc;
#endif

    pCommunicationStream ProcessInputStream;
    pCommunicationStream ProcessOutputStream;
    
public:
    // Existing PCSC connection
    SmartCardMarshaller(SCARDHANDLE pcscCardHandle, u2 portNumber,M_SAL_IN std::string* uri, u4 nameSpaceHivecode, u2 typeHivecode);

    // PCSC compatible readers
    SmartCardMarshaller(M_SAL_IN std::string* readerName, u2 portNumber,M_SAL_IN std::string* uri, u4 nameSpaceHivecode, u2 typeHivecode, u4 index);

    // destructor
    ~SmartCardMarshaller(void);
        
    // Remoting marshalling method
    void Invoke(s4 nParam, ...);

    void UpdatePCSCCardHandle(SCARDHANDLE hCard);    

    void SetInputStream(pCommunicationStream inStream);
    void SetOutputStream(pCommunicationStream outStream);

    std::string* GetReaderName();
    SCARDHANDLE  GetCardHandle();
};

MARSHALLER_NS_END

#endif


