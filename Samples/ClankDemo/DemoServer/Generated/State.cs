using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClankDemo.Generated
{

	public class State
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
		/// <summary>
		/// </summary>
		public Server server;	
		public StateSnapshot current;	
		/// <summary>
		/// @brief termine le tour de l'IA et attend le tour suivant avant de reprendre l'exécution.
		/// </summary>
		public bool EndOfTurn(int clientId)
		{
			 this.server.EndOfTurn(clientId); ;
			return true;
		}	
		/// <summary>
		/// Player[index] dsokoqs
		/// </summary>
		/// <param name="player">player index</param>
		/// <returns>the player</returns> 
		public Player GetPlayer(int player, int clientId)
		{
			return this.current.players[player];
		}	
		/// <summary>
		/// @brief Déplace le joueur playerId
		/// </summary>
		public StateSnapshot Move(int playerId, int x, int y, int clientId)
		{
			if((playerId >= this.current.players.Count))
			{
				return this.current;
			}
			this.current.players[playerId].setPosition(x,y);
			return this.current;
		}	
		public int GetMyId(int clientId)
		{
			return clientId;
		}	
		public int GetPlayerCount(int clientId)
		{
			return this.current.players.Count;
		}	
		// Génère le code pour la fonction de traitement des messages.
		public byte[] ProcessRequest(byte[] request, int clientId)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream(request);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
				System.IO.StreamWriter output;
			int functionId = Int32.Parse(input.ReadLine());
			switch(functionId)
			{
			case 0:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				bool retValue0 = EndOfTurn(clientId);
				output.WriteLine(retValue0 ? 1 : 0);
				output.Close();
				return s.ToArray();
			case 1:
				int arg1_0 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				Player retValue1 = GetPlayer(arg1_0, clientId);
				retValue1.Serialize(output);
				output.Close();
				return s.ToArray();
			case 2:
				int arg2_0 = Int32.Parse(input.ReadLine());
				int arg2_1 = Int32.Parse(input.ReadLine());
				int arg2_2 = Int32.Parse(input.ReadLine());
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				StateSnapshot retValue2 = Move(arg2_0, arg2_1, arg2_2, clientId);
				retValue2.Serialize(output);
				output.Close();
				return s.ToArray();
			case 3:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue3 = GetMyId(clientId);
				output.WriteLine(((int)retValue3).ToString());
				output.Close();
				return s.ToArray();
			case 4:
				s = new System.IO.MemoryStream();
				output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
				int retValue4 = GetPlayerCount(clientId);
				output.WriteLine(((int)retValue4).ToString());
				output.Close();
				return s.ToArray();
			}
			return new byte[0];
		}
	
		public State() {
		}



	}
}
