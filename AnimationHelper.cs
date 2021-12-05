using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WheelOfFortune
{
    static class AnimationHelper
    {
        private static readonly DispatcherTimer timer = new DispatcherTimer();
        
        
        public static void FadeInAndOut(double percent, Grid fadeGrid, DependencyProperty opacityProperty, EventHandler animationEnd)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = percent;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.5);
            fadeInAnimation.Completed += (object sender, EventArgs e) =>
            {
                timer.Interval = TimeSpan.FromSeconds(0.7);
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    FadeOut(fadeGrid, opacityProperty, animationEnd);
                };
            };
            fadeGrid.BeginAnimation(opacityProperty, fadeInAnimation);
        }
        public static void FadeIn(double percent, Grid fadeGrid, DependencyProperty opacityProperty)
        {
            fadeGrid.Background = Brushes.Black;
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = percent;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.5);
            fadeGrid.BeginAnimation(opacityProperty, fadeInAnimation);
        }
        public static void FadeOut(Grid fadeGrid, DependencyProperty opacityProperty)
        {
            DoubleAnimation fadeOutAnimation = new DoubleAnimation();
            fadeOutAnimation.From = fadeGrid.Opacity;
            fadeOutAnimation.To = 0;
            fadeOutAnimation.Duration = TimeSpan.FromSeconds(0.5);
            fadeOutAnimation.Completed += (object sender, EventArgs e) =>
            {
                fadeGrid.Background = null;
            };
            fadeGrid.BeginAnimation(opacityProperty, fadeOutAnimation);
        }
        public static void FadeOut(Grid fadeGrid, DependencyProperty opacityProperty, EventHandler animationEnd)
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = fadeGrid.Opacity;
            fadeInAnimation.To = 0;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.5);
            fadeInAnimation.Completed += (object sender, EventArgs e) =>
            {
                fadeGrid.Background = null;
                RaiseEvent(animationEnd);
            };
            fadeGrid.BeginAnimation(opacityProperty, fadeInAnimation);
        }

        public static void SlidePanelUp(double windowHeight, double target, TranslateTransform slide)
        {
            DoubleAnimation slideUp = new DoubleAnimation();
            slideUp.From = windowHeight;
            slideUp.To = target;
            slideUp.Duration = TimeSpan.FromSeconds(0.5);
            slide.BeginAnimation(TranslateTransform.YProperty, slideUp);
        }

        private static void RaiseEvent(EventHandler e)
        {
            EventHandler eCopy = e;
            if(eCopy != null)
            {
                eCopy(null, EventArgs.Empty);
            }
        }
    }
}
