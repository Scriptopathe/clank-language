using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ClankDemo.Generated
{

	public class Player
	{

		static Encoding BOMLESS_UTF8 = new UTF8Encoding(false);
	
		int m_x;	
		int m_y;	
		public int getX()
		{
			return this.m_x;
		}	
		public int getY()
		{
			return this.m_y;
		}	
		public void setX(int value)
		{
			this.m_x = value;
		}	
		public void setY(int value)
		{
			this.m_y = value;
		}	
		/// <summary>
		/// Définit la position du joueur
		/// </summary>
		/// <param name="x">position x</param>
		/// <param name="y">position y</param>
		public void setPosition(int x, int y)
		{
			this.m_x = x;
			this.m_y = y;
		}	
		public Player() {
		}

		public static Player Deserialize(System.IO.StreamReader input) {
			Player _obj =  new Player();
			// m_x
			int _obj_m_x = Int32.Parse(input.ReadLine());
			_obj.m_x = (int)_obj_m_x;
			// m_y
			int _obj_m_y = Int32.Parse(input.ReadLine());
			_obj.m_y = (int)_obj_m_y;
			return _obj;
		}

		public void Serialize(System.IO.StreamWriter output) {
			// m_x
			output.WriteLine(((int)this.m_x).ToString());
			// m_y
			output.WriteLine(((int)this.m_y).ToString());
		}

	}
}
