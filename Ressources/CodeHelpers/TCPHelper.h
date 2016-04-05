#include <iostream>
#include <string>
#include <vector>
#include <sstream>

#ifdef WIN32
#pragma comment(lib, "wsock32.lib")
#include <WinSock2.h>
#else

#endif



class TCPHelper
{
private:
	static SOCKET s_sock;
	static char* s_smallBuff;
	static char* s_bigBuff;
	static const int BIGBUF_LEN;
public:
	static bool initialize(std::string ipaddr, int port, std::string nickname);
	/**
	Envoie le contenu du stream dans un socket TCP.
	*/
	static void tcpsend(std::ostringstream& stream);
	/**
	* Re�oit un paquet de donn�es Clank et le positionne dans le
	* stream pass� en param�tre.
	*/
	static void tcpreceive(std::istringstream& stream);

	/**
	* Lib�re la m�moire etc...
	*/
	static void terminate();
};