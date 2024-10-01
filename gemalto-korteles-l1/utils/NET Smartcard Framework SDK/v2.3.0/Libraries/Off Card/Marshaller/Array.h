/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Array.h;5 */
/*       5*[588713] 05-OCT-2007 14:59:42 (GMT) sprevost */
/*         "Add missing SAL annotations." */
/*       4*[586611] 15-MAY-2007 00:56:07 (GMT) sprevost */
/*         "Add ComputeUTF8Length(), UTF8Encode(), ComputeLPSTRLength() & UTF8Decode() function declarations." */
/*       3*[586538] 02-MAY-2007 04:23:34 (GMT) sprevost */
/*         "Add SetU2At(), SetU4At() & SetU8At() method declaration to u2Array, u4Array & u8Array classes." */
/*       2*[583689] 08-MAR-2007 05:53:34 (GMT) sprevost */
/*         "Remove un-used methods + add u1Array(u1Array, offset, length) constructor declaration." */
/*       1*[576428] 13-AUG-2006 17:55:24 (GMT) ksachdeva */
/*         "Initial:" */
/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Array.h;5 */


/* Below is the omniworks history from reference baseline */



// (c) Copyright Axalto Inc., 2005-2006                    
// ALL RIGHTS RESERVED UNDER COPYRIGHT LAWS.           
// CONTAINS CONFIDENTIAL AND TRADE SECRET INFORMATION. 

#ifndef _include_marshaller_array_h
#define _include_marshaller_array_h

MARSHALLER_NS_BEGIN

class SMARTCARDMARSHALLER_DLLAPI StringArray
{

private:    
	std::string** buffer;	
    s4 _length;

public:    
    StringArray(s4 nelement);
    StringArray(const StringArray &rhs);
    ~StringArray(void);

    u1 IsNull(void);
    u4 GetLength(void);

	std::string* GetStringAt(u4 index);
	void  SetStringAt(u4 index,M_SAL_IN std::string* str);
};

#define s8Array u8Array
class SMARTCARDMARSHALLER_DLLAPI u8Array
{

private:
    u8* buffer;
    s4 _length;    

public:
    u8Array(s4 nelement);
    u8Array(const u8Array &rhs);
    ~u8Array(void);

    u1 IsNull(void);
    u4 GetLength(void);
    
    void  SetBuffer(u8* buffer);
    u8*   GetBuffer(void);    	

	u8 ReadU8At(u4 pos);    
    void SetU8At(u4 pos, u8 val);

    u8Array& operator +(u8 val);
    u8Array& operator +=(u8 val);
    u8Array& operator +(u8Array &cArray);
    u8Array& operator +=(u8Array &cArray);

};

#define s4Array u4Array
class SMARTCARDMARSHALLER_DLLAPI u4Array
{

private:
    u4* buffer;
    s4 _length;    

public:
    u4Array(s4 nelement);
    u4Array(const u4Array &rhs);
    ~u4Array(void);

    u1 IsNull(void);
    u4 GetLength(void);
    
    void  SetBuffer(u4* buffer);
    u4*   GetBuffer(void);
        
	u4 ReadU4At(u4 pos);
    void SetU4At(u4 pos, u4 val);

    u4Array& operator +(u4 val);
    u4Array& operator +=(u4 val);
    u4Array& operator +(u4Array &cArray);
    u4Array& operator +=(u4Array &cArray);
};

#define s2Array u2Array
#define charArray u2Array
class SMARTCARDMARSHALLER_DLLAPI u2Array
{

private:
    u2* buffer;
    s4 _length;

public:
    u2Array(s4 nelement);
    u2Array(const u2Array &rhs);
    ~u2Array(void);

    u1    IsNull(void);
    u4    GetLength(void);
    
    void  SetBuffer(u2* buffer);
    u2*   GetBuffer(void);
    
	u2    ReadU2At(u4 pos);	
    void  SetU2At(u4 pos, u2 val);

    u2Array& operator +(u2 val);
    u2Array& operator +=(u2 val);
    u2Array& operator +(u2Array &cArray);
    u2Array& operator +=(u2Array &cArray);
};

#define s1Array u1Array
#define MemoryStream u1Array
class SMARTCARDMARSHALLER_DLLAPI u1Array
{

private:
    u1* buffer;
    s4 _length;

public:
    u1Array(s4 nelement);
    u1Array(const u1Array &rhs);
	u1Array(u1Array &array, u4 offset, u4 len);
    ~u1Array(void);

    u1  IsNull(void);    
    u4  GetLength(void);
    
    void  SetBuffer(u1* buffer);
    u1*  GetBuffer(void);
    
    u1   ReadU1At(u4 pos);		    	
	void SetU1At(u4 pos, u1 val);

	u1Array& Append(std::string* str);	

    u1Array& operator +(u1 val);
    u1Array& operator +=(u1 val);
    u1Array& operator +(u2 val);
    u1Array& operator +=(u2 val);
    u1Array& operator +(u4 val);
    u1Array& operator +=(u4 val);
	u1Array& operator +(u8 val);
    u1Array& operator +=(u8 val);
    u1Array& operator +(u1Array &bArray);
    u1Array& operator +=(u1Array &bArray);    
};

extern u2 ComputeUTF8Length(M_SAL_IN lpCharPtr str);
extern void UTF8Encode(M_SAL_IN lpCharPtr str, u1Array &utf8Data);
extern u2 ComputeLPSTRLength(u1Array &array, u4 offset, u4 len);
extern void UTF8Decode(u1Array &array, u4 offset, u4 len, M_SAL_INOUT lpCharPtr &charData);

MARSHALLER_NS_END

#endif

