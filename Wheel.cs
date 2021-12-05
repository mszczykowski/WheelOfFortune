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
        public int CurrentPrize { get => _currentPrize; }
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
        private int _currentPrize;
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

            _currentPrize = values[pick];
            if(_currentPrize == 1000000)
            {
                double precisePick = _currentPositon % _valueSpan;
                double smallValueSpan = _valueSpan / 3;
                if (precisePick <= smallValueSpan || precisePick > (2 * smallValueSpan))
                {
                    _currentPrize = 0;
                }
            }
        }

        public String CurrentPrizeDisplayToString()
        {
            if (_currentPrize == 0 || _currentPrize == -1) return "0$";
            else return _currentPrize + "$";
        }

        public String CurrentPrizeToString()
        {
            if (_currentPrize == 0) return "Bankrupt!";
            else if (_currentPrize == -1) return "Lose a turn!";
            else return _currentPrize + "$";
        }
    }
}
