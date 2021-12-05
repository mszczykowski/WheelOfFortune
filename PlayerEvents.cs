using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    static class PlayerEvents
    {
        public static event EventHandler OptionsChanged;
        public static event EventHandler ScoreChanged;
        public static event EventHandler PlayerChanged;

        public static void RaiseOptionsChangedEvent()
        {
            RaiseEvent(OptionsChanged);
        }

        public static void RaiseScoreChangedEvent()
        {
            RaiseEvent(ScoreChanged);
        }

        public static void RaisePlayerChangedEvent()
        {
            RaiseEvent(PlayerChanged);
        }

        private static void RaiseEvent(EventHandler eventToRaise)
        {
            EventHandler eventCopy = eventToRaise;
            if (eventCopy != null)
            {
                eventCopy(null, EventArgs.Empty);
            }
        }
    }
}
