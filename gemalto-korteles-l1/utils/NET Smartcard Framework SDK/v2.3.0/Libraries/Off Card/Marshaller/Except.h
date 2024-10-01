/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Except.h;3 */
/*       3*[588478] 07-SEP-2007 17:13:30 (GMT) LCassier */
/*         "Add exception constructors with char * parameter to avoid crash when call with NULL string" */
/*       2*[583689] 08-MAR-2007 05:53:34 (GMT) sprevost */
/*         "Add Security & Verification exceptions." */
/*       1*[576428] 13-AUG-2006 17:55:24 (GMT) ksachdeva */
/*         "Initial:" */
/*+- OmniWorks Replacement History - scnet2`tools`cppMarshaller:Except.h;3 */


/* Below is the omniworks history from reference baseline */


// (c) Copyright Axalto Inc., 2006-2007
// ALL RIGHTS RESERVED UNDER COPYRIGHT LAWS.           
// CONTAINS CONFIDENTIAL AND TRADE SECRET INFORMATION. 

#ifndef _include_marshaller_except_h
#define _include_marshaller_except_h

MARSHALLER_NS_BEGIN

// .NET specific exception classes
class Exception  : public std::runtime_error{
	
public:	
	explicit Exception(std::string msg): std::runtime_error(msg) { }	
	const char *what() const throw(){
		return std::runtime_error::what();
	}
};

class SystemException : public Exception{

public:
	explicit SystemException(std::string msg) : Exception(msg) { }
	explicit SystemException(char *msg) : Exception(NULL != msg ? msg : "") { }
};

class ArgumentException : public Exception{

public:
	explicit ArgumentException(std::string msg) : Exception(msg) { }
	explicit ArgumentException();
	explicit ArgumentException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ArgumentNullException : public Exception{

public:
	explicit ArgumentNullException(std::string msg) : Exception(msg) { }
	explicit ArgumentNullException();
	explicit ArgumentNullException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ArgumentOutOfRangeException : public Exception{

public:
	explicit ArgumentOutOfRangeException(std::string msg) : Exception(msg) { }
	explicit ArgumentOutOfRangeException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class IndexOutOfRangeException : public Exception{

public:
	explicit IndexOutOfRangeException(std::string  msg) : Exception(msg) { }
	explicit IndexOutOfRangeException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class InvalidCastException : public Exception{

public:
	explicit InvalidCastException(std::string  msg) : Exception(msg) { }
	explicit InvalidCastException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class InvalidOperationException : public Exception{

public:
	explicit InvalidOperationException(std::string msg) : Exception(msg) { }
	explicit InvalidOperationException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class NotImplementedException : public Exception{

public:
	explicit NotImplementedException(std::string msg) : Exception(msg) { }
	explicit NotImplementedException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class NotSupportedException : public Exception{

public:
	explicit NotSupportedException(std::string msg) : Exception(msg) { }
	explicit NotSupportedException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class NullReferenceException : public Exception{

public:
	explicit NullReferenceException(std::string msg) : Exception(msg) { }
	explicit NullReferenceException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class OutOfMemoryException : public Exception{

public:
	explicit OutOfMemoryException(std::string msg) : Exception(msg) { }
	explicit OutOfMemoryException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class UnauthorizedAccessException : public Exception{

public:
	explicit UnauthorizedAccessException(std::string msg) : Exception(msg) { }
	explicit UnauthorizedAccessException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ObjectDisposedException : public Exception{

public:
	explicit ObjectDisposedException(std::string msg) : Exception(msg) { }
	explicit ObjectDisposedException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ApplicationException : public Exception{

public:
	explicit ApplicationException(std::string msg) : Exception(msg) { }
	explicit ApplicationException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ArithmeticException : public Exception{

public:
	explicit ArithmeticException(std::string msg) : Exception(msg) { }
	explicit ArithmeticException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class ArrayTypeMismatchException : public Exception{

public:
	explicit ArrayTypeMismatchException(std::string msg) : Exception(msg) { }
	explicit ArrayTypeMismatchException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class BadImageFormatException : public Exception{

public:
	explicit BadImageFormatException(std::string msg) : Exception(msg) { }
	explicit BadImageFormatException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class CryptographicException : public Exception{

public:
	explicit CryptographicException(std::string msg) : Exception(msg) { }
	explicit CryptographicException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class DirectoryNotFoundException : public Exception{

public:
	explicit DirectoryNotFoundException(std::string msg) : Exception(msg) { }
	explicit DirectoryNotFoundException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class DivideByZeroException : public Exception{

public:
	explicit DivideByZeroException(std::string msg) : Exception(msg) { }
	explicit DivideByZeroException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class FileNotFoundException : public Exception{

public:
	explicit FileNotFoundException(std::string msg) : Exception(msg) { }
	explicit FileNotFoundException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class FormatException : public Exception{

public:
	explicit FormatException(std::string msg) : Exception(msg) { }
	explicit FormatException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class IOException : public Exception{

public:
	explicit IOException(std::string msg) : Exception(msg) { }
	explicit IOException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class RankException : public Exception{

public:
	explicit RankException(std::string msg) : Exception(msg) { }
	explicit RankException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class RemotingException : public Exception{

private:
	s4 resultCode;	// this code is for PCSC releated error/success codes

public:
	explicit RemotingException(std::string msg) : Exception(msg) { 
		this->resultCode = 0;
	}
	explicit RemotingException(char *msg) : Exception(NULL != msg ? msg : "") { 
		this->resultCode = 0;
	}
	explicit RemotingException(std::string msg,s4 resultCode) : Exception(msg){
		this->resultCode = resultCode;
	}
	explicit RemotingException(char *msg,s4 resultCode) : Exception(NULL != msg ? msg : ""){
		this->resultCode = resultCode;
	}
	explicit RemotingException(s4 resultCode) : Exception(""){
		this->resultCode = resultCode;
	}
	s4 getResultCode(){
		return this->resultCode;
	}
};

class StackOverflowException : public Exception{

public:
	explicit StackOverflowException(std::string msg) : Exception(msg) { }
	explicit StackOverflowException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class TypeLoadException : public Exception{

public:
	explicit TypeLoadException(std::string msg) : Exception(msg) { }
	explicit TypeLoadException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class MemberAccessException : public Exception{

public:
	explicit MemberAccessException(std::string msg) : Exception(msg) { }
	explicit MemberAccessException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class MissingFieldException : public Exception{

public:
	explicit MissingFieldException(std::string msg) : Exception(msg) { }
	explicit MissingFieldException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class MissingMemberException : public Exception{

public:
	explicit MissingMemberException(std::string msg) : Exception(msg) { }
	explicit MissingMemberException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class MissingMethodException : public Exception{

public:
	explicit MissingMethodException(std::string msg) : Exception(msg) { }
	explicit MissingMethodException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class OverflowException : public Exception{

public:
	explicit OverflowException(std::string msg) : Exception(msg) { }
	explicit OverflowException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class SecurityException : public Exception{

public:
	explicit SecurityException(std::string msg) : Exception(msg) { }
	explicit SecurityException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class VerificationException : public Exception{

public:
	explicit VerificationException(std::string msg) : Exception(msg) { }
	explicit VerificationException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

class SerializationException : public Exception{

public:
	explicit SerializationException(std::string msg) : Exception(msg) { }
	explicit SerializationException(char *msg) : Exception(NULL != msg ? msg : "") { }
	
};

MARSHALLER_NS_END

#endif

