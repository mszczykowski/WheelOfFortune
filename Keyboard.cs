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
        public bool IsBackspaceEnabled { get => _isBackspaceEnabled; }

        private List<KeyboardLetter> _letters;
        private static List<Char> vowels = new List<char> { 'A', 'E', 'I', 'O', 'U' };
        private bool _isBackspaceEnabled;
        public event EventHandler KeyboardChange;
        public Keyboard()
        {
            _letters = new List<KeyboardLetter>();
            string qwerty = "QWERTYUIOPASDFGHJKLZXCVBNM";
            var qwertyAr = qwerty.ToCharArray();
            foreach (var c in qwertyAr)
            {
                if (vowels.Contains(c))
                {
                    _letters.Add(new KeyboardLetter(c, true));
                }
                else _letters.Add(new KeyboardLetter(c, false));

            }
        }
        public void EnableAll()
        {
            foreach (var c in _letters)
            {
                if (!c.wasPressed)
                {
                    c.isEnabled = true;
                }
            }
            _isBackspaceEnabled = true;
            RaiseChangedEvent();
        }
        public void DisableAll()
        {
            foreach(var c in _letters)
            {
                c.isEnabled = false;
            }
            _isBackspaceEnabled = false;
            RaiseChangedEvent();
        }

        public void EnableOnlyVowels()
        {
            foreach (var c in _letters)
            {
                if (!c.wasPressed && c.isVowel)
                {
                    c.isEnabled = true;
                }
                else c.isEnabled = false;
            }
            _isBackspaceEnabled = false;
            RaiseChangedEvent();
        }

        public void EnableOnlyConsonants()
        {
            foreach (var c in _letters)
            {
                if (!c.wasPressed && !c.isVowel)
                {
                    c.isEnabled = true;
                }
                else c.isEnabled = false;
            }
            _isBackspaceEnabled = false;
            RaiseChangedEvent();
        }

        public void RaiseChangedEvent()
        {
            EventHandler raiseEvent = KeyboardChange;
            if (raiseEvent != null)
            {
                raiseEvent(this, EventArgs.Empty);
            }
        }

        public bool SetWasPressed(char c)
        {
            foreach(var l in _letters)
            {
                if(l.c == c)
                {
                    l.wasPressed = true;
                    return l.isVowel;
                }
            }
            return false;
        }
    }
    class KeyboardLetter
    {
        public char c;
        public bool isEnabled;
        public bool wasPressed;
        public bool isVowel;
        public KeyboardLetter(char c, bool isVowel)
        {
            this.c = c;
            this.isVowel = isVowel;
            isEnabled = false;
            wasPressed = false;
        }
    }
}
