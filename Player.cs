using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class Player
    {
        private string _name;
        private int _score;
        private bool _isHuman;
        private int _difficultyLevel;

        public Player(string name)
        {
            _name = name;
            _score = 0;
            _isHuman = true;
        }
        public Player(string name, int difficultyLevel)
        {
            _name = name;
            _score = 0;
            _isHuman = false;
            _difficultyLevel = difficultyLevel;
        }
    }
}
