using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class Keyboard
    {
        public List<KeyboardLetter> Letters { get => _letters; }

        private List<KeyboardLetter> _letters;
        private static List<Char> vowels = new List<char> { 'A', 'E', 'I', 'O', 'U' };
        public Keyboard()
        {
            _letters = new List<KeyboardLetter>();
            string qwerty = "QWERTYUIOPASDFGHJKLZXCVBNM";
            var qwertyAr = qwerty.ToCharArray();
            foreach(var c in qwertyAr)
            {
                _letters.Add(new KeyboardLetter(c));
            }

            EnableOnlyConsonants();
        }
        public void EnableAll()
        {
            foreach (var c in _letters)
            {
                c.isEnabled = true;
            }
        }
        public void DisableAll()
        {
            foreach(var c in _letters)
            {
                c.isEnabled = false;
            }
        }
        public void EnableOnlyVowels()
        {
            foreach (var c in _letters)
            {
                if (vowels.Contains(c.c)) c.isEnabled = true;
                else c.isEnabled = false;
            }
        }

        public void EnableOnlyConsonants()
        {
            foreach (var c in _letters)
            {
                if (!vowels.Contains(c.c)) c.isEnabled = true;
                else c.isEnabled = false;
            }
        }
    }
    class KeyboardLetter
    {
        public char c;
        public bool isEnabled;
        public KeyboardLetter(char c)
        {
            this.c = c;
            isEnabled = false;
        }
    }
}
