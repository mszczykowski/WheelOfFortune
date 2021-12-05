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
        private List<Button> keyboardButtons;
        private List<ScoreboardScore> scoreboardScores;
        public static event EventHandler AnimationEnded;
        public static event EventHandler AnimationEnded2;

        private readonly List<int> maxLinesLenght = new List<int>
        {
            12, 14, 14, 12
        };
        public PuzzleView()
        {
            GameLogic.keyboard.KeyboardChange += ChangeKeyboardButtons;
            GameLogic.timer.TimerUpdated += UpdateTimer;
            GameLogic.CursorUpdated += UpdateCursor;
            GameLogic.LetterEntered += PrintLetter;
            GameLogic.PuzzleChanged += SetUpPuzzle;
            GameLogic.IncorrectPick += IncorrectPickMessage;
            GameLogic.IncorrectAnswer += IncorrectAnswerMessage;
            GameLogic.CorrectAnswer += CorrectAnswerMessage;
            GameLogic.RoundChanged += ShowScoreboard;
            GameLogic.timer.TimeEnded += TimeEndMessage;
            PlayerEvents.ScoreChanged += ChangeScore;
            PlayerEvents.OptionsChanged += ChangeActionButtons;
            PlayerEvents.PlayerChanged += ChangePlayer;

            InitializeComponent();
            BuildKeyboard();
            
            VowelPriceLabel.Content = "Vowel price: " + GameLogic.vowelPrice + "$";;

            GameLogic.InitialiseGame();
        }

        private void TimeEndMessage(object sender, EventArgs e)
        {
            ShowMessage("Time's up!");
        }
        private void IncorrectPickMessage(object sender, EventArgs e)
        {
            ShowMessage("There is no \"" + GameLogic.lastPick + "\"");
        }

        private void IncorrectAnswerMessage(object sender, EventArgs e)
        {
            ShowMessage("Answer incorrect!");
        }

        private void CorrectAnswerMessage(object sender, EventArgs e)
        {
            ShowMessageWithoutEndEvent("Answer correct! +5000$");
        }
        private void RoundEnd(object sender, EventArgs e)
        {
            ShowMessageWithoutEndEvent("Round end!\nAnswer is: " + GameLogic.currentPuzzle.ToString());
        }
        private void UpdateCategoryAndRoundScoreboard()
        {
            RoundChangeRound.Content = "Round: " + GameLogic.round;
            RoundChangeCategory.Content = "Category: " + GameLogic.currentPuzzle.Category;
        }

        private void InitialiseScoreboard()
        {
            int i = 2;
            scoreboardScores = new List<ScoreboardScore>();
            foreach(var player in Players.players)
            {
                Label name = new Label();
                name.SetValue(Grid.ColumnProperty, 0);
                name.SetValue(Grid.RowProperty, i);
                name.Style = this.FindResource("LabelPoints") as Style;
                name.Content = player.Name;

                Label score = new Label();
                score.SetValue(Grid.ColumnProperty, 1);
                score.SetValue(Grid.RowProperty, i);
                score.Style = this.FindResource("LabelPoints") as Style;
                score.Content = player.Score + "$";

                scoreboardScores.Add(new ScoreboardScore(player.Id, score));

                Scoreboard.Children.Add(name);
                Scoreboard.Children.Add(score);
                i++;
            }
        }
        private void PrintLetter(object sender, EventArgs e)
        {
            TextBlock letter = lettersPanels[GameLogic.cursorPositon].Children[0] as TextBlock;
            letter.Text = GameLogic.answer.letters[GameLogic.answer.letters.Count - 1].ToString();
            fadeOutLetter(letter);
        }
        private void UpdateCursor(object sender, EventArgs e)
        {
            foreach(int pos in GameLogic.answer.positions)
            {
                lettersPanels[pos].Background = Brushes.LightYellow;
            }
            lettersPanels[GameLogic.cursorPositon].Background = Brushes.Yellow;
        }

        private void UpdateTimer(object sender, EventArgs e)
        {
            if(GameLogic.timer.Time < 4)
            {
                TimerText.Foreground = Brushes.Red;
            }
            else
            {
                TimerText.Foreground = Brushes.White;
            }
            if(GameLogic.timer.Time < 10)
            {
                TimerText.Content = "00:0" + GameLogic.timer.Time;
            }
            else
            {
                TimerText.Content = "00:" + GameLogic.timer.Time;
            }
        }
        private void ChangeScore(object sender, EventArgs e)
        {
            if (scoreboardScores == null) InitialiseScoreboard();
            ScoreText.Text = GameLogic.currentPlayer.Score.ToString() + "$";
            foreach(var label in scoreboardScores)
            {
                if(GameLogic.currentPlayer.Id == label.id)
                {
                    label.label.Content = GameLogic.currentPlayer.Score.ToString() + "$";
                    break;
                }
            }
        }
        private void ChangeActionButtons(object sender, EventArgs e)
        {
            answerButton.IsEnabled = GameLogic.currentPlayer.CanAnswer;
            buyVowel.IsEnabled = GameLogic.currentPlayer.CanBuyVowel;
            spinButton.IsEnabled = GameLogic.currentPlayer.CanSpin;
        }

        private void ChangeKeyboardButtons(object sender, EventArgs e)
        {
            for (int i = 0; i < keyboardButtons.Count; i++)
            {
                keyboardButtons[i].IsEnabled = GameLogic.keyboard.Letters[i].isEnabled;
            }
        }

        private void BuildKeyboard()
        {
            var alphabet = GameLogic.keyboard.Letters;
            keyboardButtons = new List<Button>();
            for (int i = 0; i < alphabet.Count; i++)
            {
                Button button = new Button();
                button.Style = this.FindResource("ButtonNormal") as Style;
                button.Content = alphabet[i].c;
                button.IsEnabled = false;
                button.Margin = new Thickness(5);
                button.Click += keyboard_Click;
                button.FontSize = 20;
                keyboardButtons.Add(button);
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

        private void SetUpPuzzle(object sender, EventArgs e)
        {
            Category.Content = GameLogic.currentPuzzle.Category;
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
            UncoverLetters();
        }

        private void UncoverLetters()
        {
            for(int i = 0; i < lettersPanels.Count; i++)
            {
                if (!GameLogic.currentPuzzle.Puzzle[i].IsCovered) fadeOutLetter((TextBlock)lettersPanels[i].Children[0]);
                else lettersPanels[i].Children[0].Opacity = 0;
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
            char pressedLetter = pressed.Content.ToString().ToCharArray()[0];
            GameLogic.KeyboardAction(pressedLetter);
            UncoverLetters();
        }

        private void aswerButton_Click(object sender, RoutedEventArgs e)
        {
            GameLogic.StartAnswering();
        }

        private void buyVowel_Click(object sender, RoutedEventArgs e)
        {
            GameLogic.BuyVowel();
        }

        private void quitButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeRound(object sender, EventArgs e)
        {

        }

        private void StartGame(object sender, EventArgs e)
        {

        }

        private void ChangePlayer(object sender, EventArgs e)
        {
            
            AnimationEnded += (object sender, EventArgs e) =>
            {
                infoGrid.Background = this.FindResource("FullInfoBackground") as Brush;
                infoText.Text = "Player: " + GameLogic.currentPlayer.Name;
                infoText.Foreground = this.FindResource("FontGradient") as Brush;
                AnimationEnded = null;
                AnimationEnded += (object sender, EventArgs e) =>
                {
                    AnimationEnded = null;
                    PlayerNameText.Content = GameLogic.currentPlayer.Name;
                    GameLogic.StartTimerAfterAnimation();
                };
                AnimationHelper.FadeInAndOut(1, infoGrid, OpacityProperty, AnimationEnded);
            };
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
        
        private void ShowMessage(String message)
        {
            infoGrid.Background = this.FindResource("SmallInfoNormalBackground") as Brush;
            infoText.Foreground = this.FindResource("FontGradient") as Brush;

            infoText.Text = message;

            AnimationHelper.FadeIn(0.8, fadeGrid, OpacityProperty);
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.3);
            fadeInAnimation.Completed += (object sender, EventArgs e) => {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    AnimationHelper.FadeOut(infoGrid, OpacityProperty);
                    AnimationHelper.FadeOut(fadeGrid, OpacityProperty, AnimationEnded);
                };
            };
            infoGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ShowMessageWithoutEndEvent(String message)
        {
            infoGrid.Background = this.FindResource("FullInfoBackground") as Brush;
            infoText.Foreground = this.FindResource("FontGradient") as Brush;

            infoText.Text = message;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.2);
            fadeInAnimation.Completed += (object sender, EventArgs e) => {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    AnimationHelper.FadeOut(infoGrid, OpacityProperty);
                };
            };
            infoGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ShowSpinResult()
        {
            if(GameLogic.wheel.CurrentPrize == 0 || GameLogic.wheel.CurrentPrize == -1 || GameLogic.wheel.CurrentPrize == 1000000)
            {
                infoGrid.Background = this.FindResource("SmallInfoInportantBackground") as Brush;
                infoText.Foreground = this.FindResource("FontInportantGradient") as Brush;
            }
            else
            {
                infoGrid.Background = this.FindResource("SmallInfoNormalBackground") as Brush;
                infoText.Foreground = this.FindResource("FontGradient") as Brush;
            }
                
            infoText.Text = GameLogic.wheel.CurrentPrizeToString();
            
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
                    AnimationHelper.FadeOut(fadeGrid, OpacityProperty, AnimationEnded);
                    CurrentPrize.Text = GameLogic.wheel.CurrentPrizeDisplayToString();
                    GameLogic.StartTimerAfterAnimation();
                }; 
            };
            infoGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }

        private void ShowScoreboard(object sender, EventArgs e)
        {
            
            UpdateCategoryAndRoundScoreboard();
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            RoundChangeGrid.Visibility = Visibility.Visible;
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.3);
            fadeInAnimation.Completed += (object sender, EventArgs e) => {
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
                timer.Start();
                timer.Tick += (sender, args) =>
                {
                    timer.Stop();
                    RoundChangeGrid.Visibility = Visibility.Hidden;
                    EventHelper.RaiseEvent(AnimationEnded);
                };
            };
            RoundChangeGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }
        private void HideScoreboard()
        {
            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 1;
            fadeInAnimation.To = 0;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(0.3);
            fadeInAnimation.Completed += (object sender, EventArgs e) => {
                RoundChangeGrid.Visibility = Visibility.Hidden;
                EventHelper.RaiseEvent(AnimationEnded);
                
            };
            RoundChangeGrid.BeginAnimation(OpacityProperty, fadeInAnimation);
        }
    }

    class ScoreboardScore
    {
        public int id;
        public Label label;
        public ScoreboardScore(int id, Label label)
        {
            this.id = id;
            this.label = label;
        }
    }
}
