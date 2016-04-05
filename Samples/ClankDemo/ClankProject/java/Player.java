import java.lang.*;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.io.UnsupportedEncodingException;


@SuppressWarnings("unused")
public class Player
{


	Integer m_x;
	Integer m_y;
	public Integer getX()
	{
		return this.m_x;
	}
	public Integer getY()
	{
		return this.m_y;
	}
	public void setX(Integer value)
	{
		this.m_x = value;
	}
	public void setY(Integer value)
	{
		this.m_y = value;
	}
	/// <summary>
	/// *
	/// </summary>
	public void setPosition(Integer x, Integer y)
	{
		this.m_x = x;
		this.m_y = y;
	}
	public Player() {
		m_x = 0;
		m_y = 0;
	}

	public static Player deserialize(BufferedReader input) throws UnsupportedEncodingException, IOException {
		Player _obj =  new Player();
		// m_x
		int _obj_m_x = Integer.valueOf(input.readLine());
		_obj.m_x = _obj_m_x;
		// m_y
		int _obj_m_y = Integer.valueOf(input.readLine());
		_obj.m_y = _obj_m_y;
		return _obj;
	}

	public void serialize(OutputStreamWriter output) throws UnsupportedEncodingException, IOException {
		// m_x
		output.append(((Integer)this.m_x).toString() + "\n");
		// m_y
		output.append(((Integer)this.m_y).toString() + "\n");
	}

}
