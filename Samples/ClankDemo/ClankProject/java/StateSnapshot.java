import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;
import Player.*;
import java.util.ArrayList;


@SuppressWarnings("unused")
public class StateSnapshot
{

	public Integer currentPlayer;
	public ArrayList<Player> players;
	public StateSnapshot() {
		currentPlayer = 0;
		players = new ArrayList<Player>();
	}

	public static StateSnapshot deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		StateSnapshot _obj =  new StateSnapshot();
		// currentPlayer
		int _obj_currentPlayer = Integer.valueOf(input.readLine());
		_obj.currentPlayer = _obj_currentPlayer;
		// players
		ArrayList<Player> _obj_players = new ArrayList<Player>();
		int _obj_players_count = Integer.valueOf(input.readLine());
		for(int _obj_players_i = 0; _obj_players_i < _obj_players_count; _obj_players_i++) {
			Player _obj_players_e = Player.deserialize(input);
			_obj_players.add((Player)_obj_players_e);
		}
		_obj.players = _obj_players;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// currentPlayer
		output.append(((Integer)this.currentPlayer).toString() + "\n");
		// players
		output.append(String.valueOf(this.players.size()) + "\n");
		for(int players_it = 0; players_it < this.players.size();players_it++) {
			this.players.get(players_it).serialize(output);
		}
	}

}
