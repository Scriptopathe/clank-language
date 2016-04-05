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
		/// @brief termine le tour de l'IA et attend le tour suivant avant de reprendre l'exécution.
		/// </summary>
		public bool EndOfTurn()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)0).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			bool returnValue = Int32.Parse(input.ReadLine()) == 0 ? false : true;
			return (bool)returnValue;
		}
	
		/// <summary>
		/// Player[index] dsokoqs
		/// </summary>
		/// <param name="player">player index</param>
		/// <returns>the player</returns> 
		public Player GetPlayer(int player)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)1).ToString());
			output.WriteLine(((int)player).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			Player returnValue = Player.Deserialize(input);
			return (Player)returnValue;
		}
	
		/// <summary>
		/// @brief Déplace le joueur playerId
		/// </summary>
		public StateSnapshot Move(int playerId,int x,int y)
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)2).ToString());
			output.WriteLine(((int)playerId).ToString());
			output.WriteLine(((int)x).ToString());
			output.WriteLine(((int)y).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			StateSnapshot returnValue = StateSnapshot.Deserialize(input);
			return (StateSnapshot)returnValue;
		}
	
		public int GetMyId()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)3).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		public int GetPlayerCount()
		{
			System.IO.MemoryStream s = new System.IO.MemoryStream();
			System.IO.StreamWriter output = new System.IO.StreamWriter(s, BOMLESS_UTF8);
				output.NewLine = "\n";
			output.WriteLine(((int)4).ToString());
			output.Close();
			TCPHelper.Send(s.ToArray());
			byte[] response = TCPHelper.Receive();
			s = new System.IO.MemoryStream(response);
			System.IO.StreamReader input = new System.IO.StreamReader(s, BOMLESS_UTF8);
			int returnValue = Int32.Parse(input.ReadLine());
			return (int)returnValue;
		}
	
		public State() {
		}



	}
}
