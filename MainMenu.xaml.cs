using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WheelOfFortune
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
            PageTransitions.fadeOut(fadeCanvas);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitWindow window = new ExitWindow { Owner = Window.GetWindow(this) };
            window.ShowDialog();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PageTransitions.FadeIn<PuzzleView>(this, fadeCanvas);
        }
    }
}
