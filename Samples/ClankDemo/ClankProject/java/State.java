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
import StateSnapshot.*;


@SuppressWarnings("unused")
public class State
{

	// * 
	public Boolean EndOfTurn()
	{
		try {
		System.out.println("[EndOfTurn]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)0).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		boolean returnValue = Integer.valueOf(input.readLine()) == 0 ? false : true;
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	// *
	public Player GetPlayer(Integer player)
	{
		try {
		System.out.println("[GetPlayer]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)1).toString() + "\n");
		output.append(((Integer)player).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		Player returnValue = Player.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public StateSnapshot Move(Integer playerId,Integer x,Integer y)
	{
		try {
		System.out.println("[Move]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)2).toString() + "\n");
		output.append(((Integer)playerId).toString() + "\n");
		output.append(((Integer)x).toString() + "\n");
		output.append(((Integer)y).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		StateSnapshot returnValue = StateSnapshot.deserialize(input);
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public Integer GetMyId()
	{
		try {
		System.out.println("[GetMyId]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)3).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public Integer GetPlayerCount()
	{
		try {
		System.out.println("[GetPlayerCount]");
		ByteArrayOutputStream s = new ByteArrayOutputStream();
		OutputStreamWriter output = new OutputStreamWriter(s, "UTF-8");
		output.append(((Integer)4).toString() + "\n");
		output.close();
		TCPHelper.Send(s.toByteArray());
		byte[] response = TCPHelper.Receive();
		ByteArrayInputStream s2 = new ByteArrayInputStream(response);
		BufferedReader input = new BufferedReader(new InputStreamReader(s2, "UTF-8"));
		int returnValue = Integer.valueOf(input.readLine());
		return returnValue;
		} catch (UnsupportedEncodingException e) { 
		} catch (IOException e) { }
		return null;
	}

	public State() {
	}

	public static State deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		State _obj =  new State();
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
	}

}
