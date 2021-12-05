using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class Answer
    {
        public List<char> letters;
        public List<int> positions;
        public Answer(List<int> positions)
        {
            this.positions = positions;
            letters = new List<char>();
        }
    }
}
