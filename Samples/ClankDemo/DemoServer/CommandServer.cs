using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace ClankDemo
{
    /// <summary>
    /// Contient un ensemble de fonction permettant au serveur d'envoyer des données vers
    /// un socket.
    /// Le serveur peut fonctionner en évenementiel et en polling.
    /// - Evenementiel : utiliser Start() -> un event sera envoyé pour chaque commande reçue.
    /// - Polling : l'utilisateur doit appeler Send et Receive.
    /// </summary>
    public class CommandServer
    {
        /// <summary>
        /// Represents the different server modes.
        /// </summary>
        public enum Mode
        {
            Polling, 
            Event
        }

        #region Delegates / events
        public delegate void ClientConnectedDelegate(int clientId, string nickname);
        public delegate void CommandReceivedDelegate(int clientId, byte[] command);

        /// <summary>
        /// Event lancé lorsqu'un client se connecte.
        /// </summary>
        public event ClientConnectedDelegate ClientConnected;
        /// <summary>
        /// Event lancé lorsqu'une commande est reçue.
        /// </summary>
        public event CommandReceivedDelegate CommandReceived;


        /// <summary>
        /// Représente l'encoding UTF8 sans BOM.
        /// </summary>
        public static Encoding UTF8 = new UTF8Encoding(false);
        #endregion

        #region Variables
        /// <summary>
        /// Socket utilisé pour accepter les connexions entrantes.
        /// </summary>
        Socket m_listenSocket;
        /// <summary>
        /// Sockets client -> id du socket.
        /// </summary>
        Dictionary<Socket, int> m_socketToIds;
        /// <summary>
        /// Id du socket -> socket client.
        /// </summary>
        Dictionary<int, Socket> m_idToSocket;
        /// <summary>
        /// Contains the number of consecutive timeouts for each socket.
        /// </summary>
        Dictionary<Socket, int> m_consecutiveTimeouts;
        /// <summary>
        /// Contains the message queue used in polling mode.
        /// </summary>
        Dictionary<int, Queue<byte[]>> m_messageQueue;
        Dictionary<int, AutoResetEvent> m_waitHandles;

        object m_clientSocketsLock = new object();
        object m_queueLock = new object();
        Dictionary<int, byte[]> m_smallBuffer;
        Dictionary<int, byte[]> m_buffer;
        #endregion

        #region Properties
        public bool IsWaitingForConnections { get; set; }
        public int ClientsCount { get { return m_idToSocket.Count; } }
        #endregion

        public CommandServer()
        {
            m_socketToIds = new Dictionary<Socket, int>();
            m_idToSocket = new Dictionary<int, Socket>();
            m_consecutiveTimeouts = new Dictionary<Socket, int>();
            m_smallBuffer = new Dictionary<int, byte[]>();
            m_buffer = new Dictionary<int, byte[]>();
            m_messageQueue = new Dictionary<int, Queue<byte[]>>();
            m_waitHandles = new Dictionary<int, AutoResetEvent>();
            // Forces default culture to en-us.
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        #region Wait for connections
        /// <summary>
        /// Accepts incoming connexions until the given number of incoming connections is reached.
        /// </summary>
        public void WaitForConnections(int port, string address, int maxConn)
        {
            m_listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listenSocket.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            IsWaitingForConnections = true;
            m_listenSocket.Listen(10);
            int id = 0;
            while (true)
            {
                Socket sock = m_listenSocket.Accept();
                sock.NoDelay = true;
                lock (m_clientSocketsLock)
                {
                    m_socketToIds.Add(sock, id);
                    m_idToSocket.Add(id, sock);
                    m_consecutiveTimeouts.Add(sock, 0);
                    m_smallBuffer.Add(id, new byte[1]);
                    m_buffer.Add(id, new byte[512]);
                }

                lock(m_queueLock)
                {
                    m_messageQueue.Add(id, new Queue<byte[]>());
                    m_waitHandles.Add(id, new AutoResetEvent(false));
                }

                string name = UTF8.GetString(Receive(id));
                if (ClientConnected != null)
                    ClientConnected(id, name);

                id++;

                // If max conn is reached, get out of the loop.
                if (id == maxConn)
                {
                    break;
                }
            }
        }
        /// <summary>
        /// Accepts incoming connexions until a call to StopWaitingForConnections is made.
        /// </summary>
        public void WaitForConnectionsAsync(int port, string address, int maxConn=-1)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                try
                {
                    WaitForConnections(port, address, maxConn);
                }
                catch(ObjectDisposedException)
                {

                }
                catch (SocketException)
                {
                    // Ce bloc est éxécuté quand StopWaitingForConnections est appelé.
                    // Console.WriteLine("Exception : " + e.Message);
                }
                finally
                {
                    IsWaitingForConnections = false;
                }
            }));
            thread.Start();
        }

        /// <summary>
        /// Stops the server from waiting for incoming connexions.
        /// </summary>
        public void StopWaitingForConnections()
        {
            m_listenSocket.Close();
            IsWaitingForConnections = false; // force
        }
        #endregion

        #region Run
        /// <summary>
        /// Démarre un thread par client, qui envoie / reçoit des commandes via un event.
        /// If mode == Mode.Event : CommandReceived will be called each time a command enters
        /// If mode == Mode.Polling : The commands received will be queue until Process() or ProcessAll() is called.
        /// </summary>
        public void Start(Mode mode)
        {
            foreach(var kvp in m_idToSocket)
            {
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    while(true)
                    {
                        try
                        {
                            if (mode == Mode.Event)
                                CommandReceived(kvp.Key, Receive(kvp.Key));
                            else
                            {
                                // Blocks the threads wanting to dequeue
                                if (m_messageQueue[kvp.Key].Count == 0)
                                    m_waitHandles[kvp.Key].Reset();

                                var bytes = Receive(kvp.Key);

                                lock (m_queueLock)
                                {
                                    m_messageQueue[kvp.Key].Enqueue(bytes);
                                }

                                m_waitHandles[kvp.Key].Set();
                            }
                        }
                        catch(SocketException)
                        {
                            // le client a planté.
                            break;
                        }
                    }
                }));
                thread.Start();
            }
        }

        /// <summary>
        /// Processes the next incoming message.
        /// This call will block until an incoming messages is received or the given timeout
        /// expires (default : no timeout)
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="timeoutMs">timeout in milliseconds. -1 for no timeout (default).</param>
        /// <returns>true if a message has been processed, false if timeout.</returns>
        public bool ProcessNext(int clientId, int timeoutMs=-1)
        {
            bool receives = m_waitHandles[clientId].WaitOne(timeoutMs);
            if(receives)
            {
                lock(m_queueLock)
                {
                    var received = m_messageQueue[clientId].Dequeue();
                    CommandReceived(clientId, received);
                }
            }
            return receives;
        }

        #endregion

        #region Send / Receive
        public void Send(int clientId, byte[] data)
        {
            Send(m_idToSocket[clientId], data);
        }
        /// <summary>
        /// Envoie une commande dans le socket.
        /// </summary>
        /// <param name="data"></param>
        public void Send(Socket s, byte[] data)
        {
#if DEBUG
            string d = UTF8.GetString(data);
            string d2 = UTF8.GetString(UTF8.GetBytes(data.Length.ToString() + "\n"));
#endif
            try
            {
                s.Send(UTF8.GetBytes(data.Length.ToString() + "\n"));
                s.Send(data);
            }
            catch(SocketException e)
            {
                
            }
        }

        public byte[] Receive(int clientId) { return Receive(m_idToSocket[clientId], clientId); }
        /// <summary>
        /// Recoit une commande depuis le socket.
        /// </summary>
        /// <returns></returns>
        public byte[] Receive(Socket s, int clientId, int timeout=0)
        {
            // Représente le caractère '\n'.
            byte last = UTF8.GetBytes(new char[] { '\n' })[0];
            s.ReceiveTimeout = timeout;
            // Récupère le nombre de données à lire
#if DEBUG
            List<byte> allBytes = new List<byte>();
#endif
            List<byte> dataBytes = new List<byte>();
            while (true)
            {
                int bytes = s.Receive(m_smallBuffer[clientId]);
                if (m_smallBuffer[clientId][0] == last)
                    break;
                dataBytes.Add(m_smallBuffer[clientId][0]);
#if DEBUG
                allBytes.Add(m_smallBuffer[clientId][0]);
#endif
            }


            int dataLength = int.Parse(UTF8.GetString(dataBytes.ToArray()));
            dataBytes.Clear();
            int totalBytes = 0;
            while (totalBytes < dataLength)
            {
                int bytes = s.Receive(m_buffer[clientId], Math.Min(dataLength - totalBytes, m_buffer[clientId].Length), SocketFlags.None);
                totalBytes += bytes;
                for (int i = 0; i < bytes; i++)
                {
                    dataBytes.Add(m_buffer[clientId][i]);
#if DEBUG
                    allBytes.Add(m_buffer[clientId][i]);
#endif
                }
            }

#if DEBUG
            string data = UTF8.GetString(allBytes.ToArray());

#endif
            return dataBytes.ToArray();
        }
        #endregion
    }
}
