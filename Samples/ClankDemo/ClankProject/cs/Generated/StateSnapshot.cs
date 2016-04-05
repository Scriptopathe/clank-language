using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClankDemo.Generated
{

	public class StateSnapshot
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
		/// <summary>
		/// </summary>
		public int currentPlayer;	
		/// <summary>
		/// </summary>
		public List<Player> players;	
		public StateSnapshot() {
			players = new List<Player>();
		}

		public static StateSnapshot Deserialize(System.IO.StreamReader input) {
			StateSnapshot _obj =  new StateSnapshot();
			// currentPlayer
			int _obj_currentPlayer = Int32.Parse(input.ReadLine());
			_obj.currentPlayer = (int)_obj_currentPlayer;
			// players
			List<Player> _obj_players = new List<Player>();
			int _obj_players_count = Int32.Parse(input.ReadLine());
			for(int _obj_players_i = 0; _obj_players_i < _obj_players_count; _obj_players_i++) {
				Player _obj_players_e = Player.Deserialize(input);
				_obj_players.Add((Player)_obj_players_e);
			}
			_obj.players = (List<Player>)_obj_players;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// currentPlayer
			output.WriteLine(((int)this.currentPlayer).ToString());
			// players
			output.WriteLine(this.players.Count.ToString());
			for(int players_it = 0; players_it < this.players.Count;players_it++) {
				this.players[players_it].Serialize(output);
			}
		}

	}
}
