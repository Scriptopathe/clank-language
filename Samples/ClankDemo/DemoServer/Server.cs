using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClankDemo.Generated;
namespace ClankDemo
{
    public class Server
    {
        CommandServer m_commandServer;
        State m_state;
        /// <summary>
        /// Main server.
        /// </summary>
        public Server()
        {
            m_commandServer = new CommandServer();
            Console.WriteLine("Waiting for 2 connections...");
            m_commandServer.WaitForConnections(8080, "127.0.0.1", 2);
            Console.WriteLine("Clients connected !");

            m_commandServer.CommandReceived += M_commandServer_CommandReceived;
            m_commandServer.Start(CommandServer.Mode.Polling);

            // State initialization
            m_state = new State();
            m_state.current = new StateSnapshot();
            for(int i = 0; i < m_commandServer.ClientsCount;i++)
                m_state.current.players.Add(new Player());
            m_state.current.currentPlayer = 0;
            m_state.server = this;
        }

        /// <summary>
        /// Callback called whenever a command is received.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="command"></param>
        private void M_commandServer_CommandReceived(int clientId, byte[] command)
        {
            m_commandServer.Send(clientId, m_state.ProcessRequest(command, clientId));
        }

        /// <summary>
        /// Runs the demo server.
        /// </summary>
        public void Run()
        {
            while(true)
            {
                int player = m_state.current.currentPlayer;

                m_commandServer.ProcessNext(player, 500);
            }
        }



        public void EndOfTurn(int clientId)
        {
            if (m_state.current.currentPlayer == clientId)
            {
                m_state.current.currentPlayer++;
                m_state.current.currentPlayer %= m_state.current.players.Count;
                Console.WriteLine("Player {0} playing", m_state.current.currentPlayer);
            }
        }
    }
}
