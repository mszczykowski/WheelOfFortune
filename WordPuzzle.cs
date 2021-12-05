using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class WordPuzzle
    {
        public List<Letter> Puzzle { get => _puzzle; }
        public string[] PuzzleSplitted { get => _puzzleSplitted; }
        public string Category { get => _category; }

        private string _category;
        private List<Letter> _puzzle;
        private string[] _puzzleSplitted;
        public WordPuzzle(string category, string puzzle)
        {
            puzzle = puzzle.ToUpper();

            _puzzle = new List<Letter>();
            this._category = category;
            for (int i = 0; i < puzzle.Length; i++)
            {
                if(puzzle[i] != ' ') _puzzle.Add(new Letter(puzzle[i]));
            }
            _puzzleSplitted = puzzle.Split(' ');
        }
        public int Uncover(char c)
        {
            int result = 0;
            foreach (var letter in _puzzle)
            {
                if (letter.Character == c)
                {
                    letter.Uncover();
                    result++;
                }
            }
            return result;
        }

        public List<int> GetCoveredPositions()
        {
            List<int> positions = new List<int>();
            for (int i = 0; i < _puzzle.Count; i++)
            {
                if (_puzzle[i].IsCovered)
                {
                    positions.Add(i);
                }
            }
            return positions;
        }

        public string PuzzleToString()
        {
            string result = "";
            foreach (Letter l in _puzzle)
            {
                if (l.IsCovered) result += "-";
                else result += l.Character;
            }
            return result;
        }

        public bool IsAnswerCorrect(Answer answer)
        {
            int i = 0;
            foreach (var l in _puzzle)
            {
                if(l.IsCovered)
                {
                    if (l.Character != answer.letters[i]) return false;
                    i++;
                }
            }
            return true;
        }
    }
    
    class Letter
    {
        public char Character { get => _character; }
        public bool IsCovered { get => _isCovered; }

        private char _character;
        private bool _isCovered;
        public Letter(char _character)
        {
            this._character = _character;
            _isCovered = true;
        }
        public void Uncover()
        {
            _isCovered = false;
        }
    }
}
