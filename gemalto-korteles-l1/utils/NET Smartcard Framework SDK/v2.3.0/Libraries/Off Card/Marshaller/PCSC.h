/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:PCSC.h;4 */
/*       4*[589286] 26-FEB-2008 18:54:14 (GMT) AMALI */
/*         "Use _MARSHALLER_LITE_ for xCL option" */
/*       3*[588604] 18-SEP-2007 19:51:33 (GMT) sprevost */
/*         "Add extra index parameter to PCSC constructor." */
/*       2*[576487] 14-AUG-2006 19:03:53 (GMT) sprevost */
/*         "Add SetCardHandle() method declaration." */
/*       1*[576428] 13-AUG-2006 17:55:24 (GMT) ksachdeva */
/*         "Initial:" */
/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:PCSC.h;4 */


/* Below is the omniworks history from reference baseline */


// (c) Copyright Axalto Inc., 2005-2006                    
// ALL RIGHTS RESERVED UNDER COPYRIGHT LAWS.           
// CONTAINS CONFIDENTIAL AND TRADE SECRET INFORMATION. 

#ifndef _XCL_

#ifndef _include_marshaller_pcsc_h
#define _include_marshaller_pcsc_h

MARSHALLER_NS_BEGIN

class PCSC
{

private:
    SCARDCONTEXT hContext;
    SCARDHANDLE  hCard;        
    std::string*    readerName;		

public:        
    PCSC(SCARDHANDLE cardHandle);
	PCSC(M_SAL_IN std::string* readerName);	
	PCSC(M_SAL_IN std::string* readerName, u2* portNumber, M_SAL_IN std::string* uri, u4 nameSpaceHivecode, u2 typeHivecode, u4 index);	
    SCARDHANDLE GetCardHandle(void);
    void SetCardHandle(SCARDHANDLE hCard);
    std::string* GetReaderName(void);
    void BeginTransaction(void);
    void EndTransaction(void);
    void ExchangeData(u1Array &dataIn, u1Array &dataout);
    ~PCSC(void);

};

MARSHALLER_NS_END

#endif

#else

// Include xCL functionality for all cases where PC/SC API was used
#include <xcl_broker.h> 

#endif /* _XCL_ */


