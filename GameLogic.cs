using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    static class GameLogic
    {
        public static bool Debug = true;
        public static Keyboard alphabet = new Keyboard();
        public static Wheel wheel = new Wheel();
        public static List<WordPuzzle> wordPuzzles = new List<WordPuzzle>
        {
            new WordPuzzle("Test Category", "Test Puzzle")
        };
        public static WordPuzzle currentPuzzle;


        public static void SpinTheWheel()
        {
            wheel.DrawNumber();
        }

        public static void DrawPuzzle()
        {
            //todo

            currentPuzzle = wordPuzzles[0];
        }

        public static int Uncover(string letter)
        {
            return currentPuzzle.Uncover(letter.ToCharArray()[0]);
        }
    }
}
