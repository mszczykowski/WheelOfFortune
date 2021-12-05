using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    static class Players
    {
        private static int currentPlayer;
        public static List<Player> players = new List<Player>();

        public static void AddPlayer(string name)
        {
            //players.Add(new Player(name));
        }

        public static void Initialise()
        {
            players.Add(new Player("Player1", 1));
            players.Add(new Player("Player2", 2));
            players.Add(new Player("Player3", 3));
            currentPlayer = -1;
        }


        public static Player GetNextPlayer()
        {
            currentPlayer++;
            if (currentPlayer >= players.Count) currentPlayer = 0;
            return players[currentPlayer];
            
        }

    }
}
