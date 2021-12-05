using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class Player
    {
        public string Name { get => _name; }
        public bool CanSpin { get => _canSpin; set { _canSpin = value; } }
        public bool CanBuyVowel { get => _canBuyVowel; set { _canBuyVowel = value; } }
        public bool CanAnswer { get => _canAnswer; set { _canAnswer = value; } }
        public int Score { get => _score; }
        public int Id { get => _id; }

        private string _name;
        private int _score;
        private int _id;
        private bool _isHuman;
        private int _difficultyLevel;

        private bool _canSpin;
        private bool _canBuyVowel;
        private bool _canAnswer;


        public Player(string name, int id)
        {
            _name = name;
            _score = 0;
            _isHuman = true;
            _id = id;
        }
        public Player(string name, int id, int difficultyLevel)
        {
            _name = name;
            _score = 0;
            _isHuman = false;
            _difficultyLevel = difficultyLevel;
            _id = id;
        }

        public void PlayerPicekd()
        {
            SetPlayerOptionsRoundBeginning();
            PlayerEvents.RaiseScoreChangedEvent();
            PlayerEvents.RaisePlayerChangedEvent();
        }

        public void SetPlayerOptionsRoundBeginning()
        {
            _canSpin = true;
            _canAnswer = true;
            _canBuyVowel = false;
            PlayerEvents.RaiseOptionsChangedEvent();
        }

        public void SetPlayerOptionsDisabled()
        {
            _canSpin = false;
            _canAnswer = false;
            _canBuyVowel = false;
            PlayerEvents.RaiseOptionsChangedEvent();
        }
            
        public void SetPlayerOptionsAfterCorrectAswer()
        {
            _canSpin = true;
            _canAnswer = true;
            if (_score > 250)
            {
                _canBuyVowel = true;
            }
            else _canBuyVowel = false;
            PlayerEvents.RaiseOptionsChangedEvent();
        }

        

        public void SetScore(int score)
        {
            _score = score;
            PlayerEvents.RaiseScoreChangedEvent();
        }

        public void AddToScore(int score)
        {
            _score += score;
            PlayerEvents.RaiseScoreChangedEvent();
        }

        
    }
}
