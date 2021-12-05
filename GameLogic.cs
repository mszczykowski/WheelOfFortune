using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WheelOfFortune
{
    static class GameLogic
    {
        public static bool Debug = true;
        public static Keyboard keyboard = new Keyboard();
        public static Wheel wheel = new Wheel();
        public static GameTimer timer = new GameTimer();
        public static WordPuzzle currentPuzzle;
        public static Player currentPlayer;
        public static int round;
        public static readonly int vowelPrice = 250;
        public static readonly int timeForAction = 30;
        public static int cursorPositon;
        public static char lastPick;
        
        public static Answer answer;

        public static event EventHandler RoundChanged;
        public static event EventHandler CursorUpdated;
        public static event EventHandler LetterEntered;
        public static event EventHandler PuzzleChanged;

        public static event EventHandler IncorrectAnswer;
        public static event EventHandler IncorrectPick;
        public static event EventHandler CorrectAnswer;

        public static void InitialiseGame()
        {
            round = 0;
            Players.Initialise();
            WordPuzzles.DrawPuzzles();

            timer.TimeEnded += TimerEnd;
         
            NextRound();
        }
        private static void TimerEnd(object sender, EventArgs e)
        {
            ChangePlayer();
        }


        public static void SpinTheWheel()
        {
            timer.StopTimer();
            timer.SetTimerDecison();
            wheel.DrawNumber();
            int result = wheel.CurrentPrize;
            if (result > 0)
            {
                keyboard.EnableOnlyConsonants();
                currentPlayer.SetPlayerOptionsDisabled();
            }
            else
            {
                if(result == 0)
                {
                    currentPlayer.SetScore(0);
                }
                ChangePlayer();
            }
        }

        public static void PlayerStart()
        {
            currentPlayer.SetPlayerOptionsRoundBeginning();
            keyboard.DisableAll();
            timer.SetTimerDecison();
        }

        public static void PlayerContinue()
        {
            currentPlayer.SetPlayerOptionsAfterCorrectAswer();
            keyboard.DisableAll();
            timer.SetTimerDecison();
            timer.StartTimer();
        }

        public static void KeyboardAction(char c)
        {
            lastPick = c;          
            if(answer == null)
            {
                timer.StopTimer();
                bool isVowel = keyboard.SetWasPressed(c);
                int result = currentPuzzle.Uncover(c);
                if (result > 0)
                {
                    if (!isVowel) currentPlayer.AddToScore(result * wheel.CurrentPrize);
                    else currentPlayer.AddToScore(-vowelPrice);
                    if (currentPuzzle.GetCoveredPositions().Count == 0) NextRound();
                    else PlayerContinue();
                }
                else
                {
                    if (isVowel) currentPlayer.AddToScore(-vowelPrice);
                    EventHelper.RaiseEvent(IncorrectPick);
                    ChangePlayer();
                }
            }
            else
            {
                answer.letters.Add(c);
                EventHelper.RaiseEvent(LetterEntered);
                
                if(answer.positions.Count == answer.letters.Count)
                {
                    bool result = currentPuzzle.IsAnswerCorrect(answer);
                    answer = null;
                    if(result)
                    {
                        timer.StopTimer();
                        currentPlayer.AddToScore(5000);
                        EventHelper.RaiseEvent(CorrectAnswer);
                        NextRound();
                    }
                    else
                    {
                        EventHelper.RaiseEvent(IncorrectAnswer);
                        ChangePlayer();
                    }
                }
                else
                {
                    cursorPositon = answer.positions[answer.letters.Count];
                    EventHelper.RaiseEvent(CursorUpdated);
                }
            }
        }

        public static void NextRound()
        {
            currentPuzzle = WordPuzzles.GetNextPuzzle();
            ChangePlayer();
            round++;
            EventHelper.RaiseEvent(RoundChanged);
            EventHelper.RaiseEvent(PuzzleChanged);
            PlayerStart();
        }

        public static void StartAnswering()
        {
            keyboard.EnableAll();
            answer = new Answer(currentPuzzle.GetCoveredPositions());
            cursorPositon = answer.positions[0];
            EventHelper.RaiseEvent(CursorUpdated);
            currentPlayer.SetPlayerOptionsDisabled();
            timer.SetTimerAnswer();
            timer.StartTimer();
        }

        public static void ChangePlayer()
        {
            currentPlayer = Players.GetNextPlayer();
            currentPlayer.PlayerPicekd();
            EventHelper.RaiseEvent(PuzzleChanged);
            PlayerStart();
        }

        public static void BuyVowel()
        {
            keyboard.EnableOnlyVowels();
            timer.SetTimerDecison();
        }

        public static void StartTimerAfterAnimation()
        {
            timer.StartTimer();
        }
    }

}
