using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WheelOfFortune
{
    static class EventHelper
    {
        public static void RaiseEvent(EventHandler eventToRaise)
        {
            EventHandler eventCopy = eventToRaise;
            if (eventCopy != null)
            {
                eventCopy(null, EventArgs.Empty);
            }
        }
    }
}
