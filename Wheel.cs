using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    class Wheel
    {
        public double ImageShift { get => _imageShift; }
        public double FinalAngle { get => _rotationAngle + _previousAngle; }
        public int CurrentPick { get => _currentPick; }
        public double PreviousAngle { get => _previousAngle; }




        private List<int> values = new List<int>
        {
            500, 600, 700, 600, 650, 500, 700, 1000000, 600, 550, 500, 600,
            0, 650, 600, 700, -1, 800, 500, 650, 500, 900, 0, 2500
        };

        private Random random;
        private double _previousAngle;
        private double _valueSpan;
        private double _imageShift;
        private double _rotationAngle;
        private double _currentPositon;
        private int _currentPick;
        private int minAngle = 800;
        private int maxAngle = 1800;

        public Wheel()
        {
            random = new Random();
            _valueSpan = 360 / values.Count;
            _imageShift = _valueSpan / 2;

            _currentPositon = _imageShift;

            values.Reverse();

        }
        public void DrawNumber()
        {
            _previousAngle = _currentPositon;
            _rotationAngle = random.Next(minAngle, maxAngle);

            _currentPositon = (_previousAngle + _rotationAngle) %360;

            int pick = (int)(_currentPositon / _valueSpan);

            _currentPick = values[pick];
            if(_currentPick == 1000000)
            {
                double precisePick = _currentPositon % _valueSpan;
                double smallValueSpan = _valueSpan / 3;
                if (precisePick <= smallValueSpan || precisePick > (2 * smallValueSpan))
                {
                    _currentPick = 0;
                }
            }
        }

        public String CurrentPickToString()
        {
            if (_currentPick == 0) return "Bankrupt!";
            else if (_currentPick == -1) return "Lose a turn!";
            else return _currentPick + "$";
        }
    }
}
