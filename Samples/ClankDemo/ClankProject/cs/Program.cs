using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClankDemo.Generated;
namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new State();
            Console.WriteLine("Initializing...");
            TCPHelper.Initialize(8080, "127.0.0.1", "test");
            int id = state.GetMyId();
            while(true)
            {
                Player p = state.GetPlayer(id);
                state.Move(id, p.getX() + 1, p.getY() + 1);
                Console.WriteLine(p.getX() + " " + p.getY());
                state.EndOfTurn();
            }
        }
    }
}
