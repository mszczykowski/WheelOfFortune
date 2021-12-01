using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WheelOfFortune
{
    /// <summary>
    /// Interaction logic for PuzzleView.xaml
    /// </summary>
    public partial class PuzzleView : Page
    {
        private List<DockPanel> lettersPanels;
        private readonly List<int> maxLinesLenght = new List<int>
        {
            12, 14, 14, 12
        };
        public PuzzleView()
        {

            InitializeComponent();
            BuildKeyboard();
            GameLogic.DrawPuzzle();
            Category.Content = GameLogic.currentPuzzle.Category;
            SetUpPuzzle();
        }

        private void BuildKeyboard()
        {
            var alphabet = GameLogic.alphabet.Letters;
            for (int i = 0; i < alphabet.Count; i++)
            {
                Button button = new Button();
                button.Style = this.FindResource("ButtonNormal") as Style;
                button.Content = alphabet[i].c;
                button.IsEnabled = alphabet[i].isEnabled;
                button.Margin = new Thickness(5);
                button.Click += keyboard_Click;
                button.FontSize = 20;
                if (i < 10)
                {
                    button.SetValue(Grid.ColumnProperty, i);
                    first_row.Children.Add(button);
                }
                else if (i < 19)
                {
                    button.SetValue(Grid.ColumnProperty, i - 9);
                    second_row.Children.Add(button);
                }
                else
                {
                    button.SetValue(Grid.ColumnProperty, i - 18);
                    third_row.Children.Add(button);
                }
            }
        }

        private void SetUpPuzzle()
        {
            lettersPanels = new List<DockPanel>();
            List<string>[] lines = new List<string>[4]
            {
                new List<string>(),
                new List<string>(),
                new List<string>(),
                new List<string>()
            };
            int[] linesLenght = new int[4];
            var puzzleSplitted = GameLogic.currentPuzzle.PuzzleSplitted;

            int lineLength = 0;
            int n = 0;
            foreach(var word in puzzleSplitted)
            {
                if(lineLength + word.Length > 14)
                {
                    n++;
                }
                else
                {
                    linesLenght[n] += word.Length + 1;
                    lines[n].Add(word);
                }
            }

            int row;
            if(n == 0 || n == 1)
            {
                row = 1;
            }
            else
            {
                row = 0;
            }
            int column;
            column = (maxLinesLenght[row] - linesLenght[0]) / 2;

            for(int i = 0; i < n + 1; i++)
            {
                foreach(var word in lines[i])
                {
                    for(int j = 0; j < word.Length; j++)
                    {
                        DockPanel letterBackground = new DockPanel();
                        letterBackground.Style = this.FindResource("LetterBackground") as Style;
                        letterBackground.SetValue(Grid.ColumnProperty, column);
                        letterBackground.SetValue(Grid.RowProperty, row);
                        

                        TextBlock letter = new TextBlock();
                        letter.Style = this.FindResource("LetterTextBlock") as Style;
                        letterBackground.Children.Add(letter);
                        letter.Text = word[j].ToString().ToUpper();
                        letter.Opacity = 0;
                        lettersGrid.Children.Add(letterBackground);
                        lettersPanels.Add(letterBackground);
                        column++;
                    }
                    column++;
                }
                column = maxLinesLenght[row + 1] - linesLenght[i + 1] / 2;
                row++;
            }
        }

        private void UncoverLetters()
        {
            for(int i = 0; i < lettersPanels.Count; i++)
            {
                if (!GameLogic.currentPuzzle.Puzzle[i].IsCovered) fadeOutLetter((TextBlock)lettersPanels[i].Children[0]);
            }
        }

        private void fadeOutLetter(TextBlock letter)
        {
            if(letter.Opacity == 0)
            {
                DoubleAnimation fadeInAnimation = new DoubleAnimation();
                fadeInAnimation.From = 0;
                fadeInAnimation.To = 1;
                fadeInAnimation.Duration = TimeSpan.FromSeconds(0.2);
                letter.BeginAnimation(OpacityProperty, fadeInAnimation);
            }
        }

        private void spinButton_Click(object sender, RoutedEventArgs e)
        {
            GameLogic.SpinTheWheel();

            AnimationHelper.FadeIn(0.8, fadeGrid, OpacityProperty);
            AnimationHelper.SlidePanelUp(this.ActualHeight, 200, wheelSlide);
            SpinSequence();
        }

        private void keyboard_Click(object sender, RoutedEventArgs e)
        {
            Button pressed = sender as Button;
            String pressedLetter = pressed.Content.ToString();
            GameLogic.Uncover(pressedLetter);
            UncoverLetters();
        }

        private void aswerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buyVowel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SpinSequence()
        {
            DoubleAnimation spin = new DoubleAnimation();
            spin.From = GameLogic.wheel.PreviousAngle - GameLogic.wheel.ImageShift;
            spin.To = GameLogic.wheel.FinalAngle - GameLogic.wheel.ImageShift;
            spin.Duration = TimeSpan.FromSeconds(1);
            spin.DecelerationRatio = 0.6;
            spin.Completed += WaitAndSlidePanelDown;
            wheelSpin.BeginAnimation(RotateTransform.AngleProperty, spin);
        }

        private void WaitAndSlidePanelDown(object sender, EventArgs e)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                DoubleAnimation slideDown = new DoubleAnimation();
                slideDown.From = 200;
                slideDown.To = this.ActualHeight;
                slideDown.Duration = TimeSpan.FromSeconds(0.3);
                slideDown.AccelerationRatio = 1;
                wheelSlide.BeginAnimation(TranslateTransform.YProperty, slideDown);
                ShowSpinResult();
            };
        }

        private void ShowSpinResult()
        {
            if(GameLogic.wheel.CurrentPick == 0 || GameLogic.wheel.CurrentPick == -1 || GameLogic.wheel.CurrentPick == 1000000)
            {
                infoGrid.Background = this.FindResource("SmallInfoInportantBackground") as Brush;
                infoText.Foreground = this.FindResource("FontInportantGradient") as Brush;
            }
            else
            {
                infoGrid.Background = this.FindResource("SmallInfoNormalBackground") as Brush;
                infoText.Foreground = this.FindResource("FontGradient") as Brush;
            }
                
            infoText.Text = GameLogic.wheel.CurrentPickToString();
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.3);
            fadeInAnimation.Completed += (object sender, EventArgs e) => {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.3) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    AnimationHelper.FadeOut(infoGrid, OpacityProperty);
                    AnimationHelper.FadeOut(fadeGrid, OpacityProperty);
                }; 
            };
            infoGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }
    }
}
