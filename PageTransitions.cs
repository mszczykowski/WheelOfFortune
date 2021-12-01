using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WheelOfFortune
{
    static class PageTransitions
    {
        public static void SplashScreen<T>(Page page, Canvas fadeCanvas)
        {
            if (GameLogic.Debug)
            {
                FadeAndChangeScreen<T>(page, fadeCanvas, 0, 0);
            }
            else FadeAndChangeScreen<T>(page, fadeCanvas, 40, 75);
        }
        public static void fadeOut(Canvas fadeCanvas)
        {
            if(!GameLogic.Debug)
            {
                var dispatcherTimer = new DispatcherTimer();

                int counter = 0;

                fadeCanvas.Background = Brushes.Black;
                fadeCanvas.Opacity = 100;

                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Start();

                void dispatcherTimer_Tick(object sender, EventArgs e)
                {
                    if (counter == 50)
                    {
                        fadeCanvas.Background = null;
                        dispatcherTimer.Stop();
                    }
                    fadeCanvas.Opacity -= 0.05;
                    counter += 1;
                }
            }
        }

        public static void FadeIn<T>(Page page, Canvas fadeCanvas)
        {
            if(GameLogic.Debug)
            {
                FadeAndChangeScreen<T>(page, fadeCanvas, 0, 0);
            }
            else FadeAndChangeScreen<T>(page, fadeCanvas, 0, 40);
        }
        private static void FadeAndChangeScreen<T>(Page page, Canvas fadeCanvas, int fadeStart, int chageTime)
        {
            var dispatcherTimer = new DispatcherTimer();

            int counter = 0;

            fadeCanvas.Background = Brushes.Black;
            fadeCanvas.Opacity = 0;

            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();

            void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                if (counter == chageTime)
                {
                    fadeCanvas.Opacity = 100;
                    page.NavigationService.Navigate((T)Activator.CreateInstance(typeof(T)));
                }
                else if (counter > fadeStart)
                {
                    fadeCanvas.Opacity += 0.05;
                }
                counter += 1;
            }
        }
    }
}
