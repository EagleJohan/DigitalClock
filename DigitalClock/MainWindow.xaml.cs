using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace DigitalClock
{
    public class Settings
    {
        public static int minutes { get; set; } = 1;
        public static int seconds { get; set; } = 0;
        public static int intervals { get; set; } = 22;
        public static int trainingType { get; set; } = 0;
    }

    public partial class MainWindow : Window
    {
        private Label leftLabel;
        private Label colonLeftLabel;
        private Label middleLabel;
        private Label colonRightLabel;
        private Label rightLabel;
        public CountDownTimer timer = new CountDownTimer();
        public DispatcherTimer asyncTimer;

        //Countdown sound effect
        public SoundPlayer voiceCountDown = new SoundPlayer(Path.Combine(Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin")), @"Sounds\", "CountDown.wav"));
        public SoundPlayer beepCountDown = new SoundPlayer(Path.Combine(Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin")), @"Sounds\", "beepCountDown.wav"));
        public double threeSegmentFontSize = 335;
        public double twoSegmentFontSize = 530;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Start()
        {
            //Timer in background to update DateTime
            asyncTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            asyncTimer.Start();
            asyncTimer.Tick += AsyncTimer_Tick;
            var thickness = new Thickness { Top = 0, Left = 0, Bottom = 0, Right = 0 };

            //Set fontfamily to digitalclock
            FontFamily digitalClockFont = new FontFamily("DSEG7 Modern");

            // Window options
            Title = "Räddningstjänst timer";
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.CanResizeWithGrip;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowState = WindowState.Maximized;
            Background = Brushes.Black;

            // Scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            root.VerticalContentAlignment = VerticalAlignment.Center;
            root.HorizontalContentAlignment = HorizontalAlignment.Center;
            Content = root;

            // Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(10);
            grid.RowDefinitions.Add(new RowDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            leftLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = "00",
                BorderThickness = thickness,
                FontSize = twoSegmentFontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontFamily = digitalClockFont
            };
            leftLabel.MouseDown += Hours_MouseDown;
            grid.Children.Add(leftLabel);
            Grid.SetColumn(leftLabel, 0);
            Grid.SetRow(leftLabel, 0);
            colonLeftLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = ":",
                BorderThickness = thickness,
                FontSize = threeSegmentFontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = digitalClockFont
            };
            grid.Children.Add(colonLeftLabel);
            Grid.SetColumn(colonLeftLabel, 1);
            Grid.SetRow(colonLeftLabel, 0);
            middleLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = "00",
                BorderThickness = thickness,
                FontSize = threeSegmentFontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontFamily = digitalClockFont
            };
            middleLabel.MouseDown += Minutes_MouseDown;
            grid.Children.Add(middleLabel);
            Grid.SetColumn(middleLabel, 2);
            colonRightLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = ":",
                BorderThickness = thickness,
                FontSize = threeSegmentFontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = digitalClockFont
            };
            grid.Children.Add(colonRightLabel);
            Grid.SetColumn(colonRightLabel, 3);
            Grid.SetRow(colonRightLabel, 0);
            rightLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = "00",
                BorderThickness = thickness,
                FontSize = threeSegmentFontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontFamily = digitalClockFont
            };
            grid.Children.Add(rightLabel);
            Grid.SetColumn(rightLabel, 4);
        }

        private void AsyncTimer_Tick(object sender, EventArgs e)
        {
            //Sätter tid enbart om timer inte körs
            if (!timer.IsRunning)
            {
                //Left label as hours during digital clock
                leftLabel.Foreground = Brushes.Red;
                leftLabel.FontSize = twoSegmentFontSize;
                leftLabel.Content = DateTime.Now.ToString("HH");
                //Left colon label
                colonLeftLabel.Foreground = Brushes.Red;
                colonLeftLabel.FontSize = twoSegmentFontSize;
                colonLeftLabel.Content = ":";
                //Middle label set as minutes
                middleLabel.Foreground = Brushes.Red;
                middleLabel.FontSize = twoSegmentFontSize;
                middleLabel.Content = DateTime.Now.ToString("mm");
                //Remove right colon and right label
                colonRightLabel.Content = "";
                rightLabel.Content = "";
            }
        }

        private void Minutes_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Pausa eller starta timer
            if (timer.IsRunning)
            {
                timer.Stop();
                timer.Dispose();
            }
            else
            {
                //Left label as hours during digital clock
                leftLabel.Foreground = Brushes.Red;
                leftLabel.FontSize = twoSegmentFontSize;
                //Left colon label
                colonLeftLabel.Foreground = Brushes.Red;
                colonLeftLabel.FontSize = twoSegmentFontSize;
                colonLeftLabel.Content = " ";
                //Middle label set as minutes
                middleLabel.Foreground = Brushes.Red;
                middleLabel.FontSize = 700;
                //Remove right colon and right label
                colonRightLabel.Content = " ";
                rightLabel.Content = "";
                //Countdown
                voiceCountDown.Load();
                voiceCountDown.Play();
                timer.Reset();
                timer.SetTime(0, 11);
                timer.Start();
                timer.TimeChanged += () => leftLabel.Content = "  ";
                timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("ss");
                timer.TimeChanged += () => rightLabel.Content = "  ";
                timer.StepMs = 63;
                timer.TimeChanged += Check_TimeLeft;
            }
        }

        private void Check_TimeLeft()
        {
            //Startar timer, kallas även rekursivt från intervallprogram
            if (timer.TimeLeft == TimeSpan.Zero)
            {
                Console.Beep();
                Start_Timers();
            }
            else if (timer.TimeLeft >= TimeSpan.FromMilliseconds(3000) && timer.TimeLeft <= TimeSpan.FromMilliseconds(3063) && leftLabel.FontSize == threeSegmentFontSize)
            {
                beepCountDown.Load();
                beepCountDown.Play();
            }
        }

        private string SettingsIntervalToString()
        {
            if (Settings.intervals < 10)
            {
                return $"0{Settings.intervals}";
            }
            else
            {
                return Settings.intervals.ToString();
            }
        }

        private void Start_Timers()
        {
            switch (Settings.trainingType)
            {
                //Räkna upp
                case 0:
                    //Left label as hours 
                    leftLabel.Foreground = Brushes.Red;
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.Foreground = Brushes.Red;
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = ":";
                    //Middle label set as minutes
                    middleLabel.Foreground = Brushes.Red;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.Foreground = Brushes.Red;
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    middleLabel.Foreground = Brushes.Red;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //Räkna upp till 60min
                    timer.Reset();
                    timer.SetTime(180, 0);
                    timer.Start();
                    timer.TimeChanged += () => leftLabel.Content = timer.stopWatch.Elapsed.ToString("hh");
                    timer.TimeChanged += () => middleLabel.Content = timer.stopWatch.Elapsed.ToString("mm");
                    timer.TimeChanged += () => rightLabel.Content = timer.stopWatch.Elapsed.ToString("ss");
                    timer.CountDownFinished += timer.Dispose;
                    break;

                case 1:
                    //Left label as hours 
                    leftLabel.Foreground = Brushes.Blue;
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.Foreground = Brushes.Red;
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = "";
                    //Middle label set as minutes
                    middleLabel.Foreground = Brushes.Red;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.Foreground = Brushes.Red;
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    rightLabel.Foreground = Brushes.Red;
                    rightLabel.FontSize = threeSegmentFontSize;
                    //Intervall träning
                    if (Settings.intervals >= 0)
                    {
                        timer.Reset();
                        timer.SetTime(Settings.minutes, Settings.seconds);
                        timer.Start();

                        timer.TimeChanged += () => leftLabel.Content = SettingsIntervalToString();
                        timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("ss");
                        timer.TimeChanged += () => rightLabel.Content = timer.TimeLeft.ToString("ff");
                        Settings.intervals--;
                        timer.TimeChanged += Check_TimeLeft;
                    }
                    else
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    break;

                case 2:
                    //Left label as hours 
                    leftLabel.Foreground = Brushes.Red;
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.Foreground = Brushes.Red;
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = ":";
                    //Middle label set as minutes
                    middleLabel.Foreground = Brushes.Red;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.Foreground = Brushes.Red;
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    rightLabel.Foreground = Brushes.Red;
                    rightLabel.FontSize = threeSegmentFontSize;
                    //Räkna ner från inställd tid
                    timer.Reset();
                    timer.SetTime(Settings.minutes, Settings.seconds);

                    timer.Start();
                    timer.TimeChanged += () => leftLabel.Content = timer.TimeLeft.ToString("hh");
                    timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("mm");
                    timer.TimeChanged += () => rightLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.CountDownFinished += timer.Dispose;
                    break;

                case 3:
                    //Left label as hours 
                    leftLabel.Foreground = Brushes.Blue;
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.Foreground = Brushes.Red;
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = "";
                    //Middle label set as minutes
                    middleLabel.Foreground = Brushes.Red;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.Foreground = Brushes.Red;
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    rightLabel.Foreground = Brushes.Red;
                    rightLabel.FontSize = threeSegmentFontSize;
                    //Tabata
                    timer.Reset();
                    timer.SetTime(0, 20);
                    timer.Start();
                    timer.TimeChanged += () => leftLabel.Content = SettingsIntervalToString();
                    timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.TimeChanged += () => rightLabel.Content = timer.TimeLeft.ToString("ff");
                    timer.StepMs = 61;
                    Settings.intervals--;
                    if (Settings.intervals > 0)
                    {
                        Settings.trainingType = 5;
                    }
                    timer.CountDownFinished += timer.Dispose;
                    break;

                case 4:
                    //Fight gone bad
                    //Left label as hours 
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = "";
                    //Middle label set as minutes
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    rightLabel.FontSize = threeSegmentFontSize;
                    if (Settings.intervals % 6 == 0)
                    {
                        leftLabel.Foreground = Brushes.Blue;
                        colonLeftLabel.Foreground = Brushes.Green;
                        middleLabel.Foreground = Brushes.Green;
                        colonRightLabel.Foreground = Brushes.Green;
                        rightLabel.Foreground = Brushes.Green;
                    }
                    else if (Settings.intervals % 2 == 0)
                    {
                        leftLabel.Foreground = Brushes.Blue;
                        colonLeftLabel.Foreground = Brushes.Yellow;
                        middleLabel.Foreground = Brushes.Yellow;
                        colonRightLabel.Foreground = Brushes.Yellow;
                        rightLabel.Foreground = Brushes.Yellow;
                    }
                    else
                    {
                        leftLabel.Foreground = Brushes.Blue;
                        colonLeftLabel.Foreground = Brushes.Red;
                        middleLabel.Foreground = Brushes.Red;
                        colonRightLabel.Foreground = Brushes.Red;
                        rightLabel.Foreground = Brushes.Red;
                    }
                    if (Settings.intervals >= 0)
                    {
                        timer.Reset();
                        timer.SetTime(1, 5);
                        timer.Start();
                        timer.TimeChanged += () => leftLabel.Content = SettingsIntervalToString();
                        timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("mm");
                        timer.TimeChanged += () => rightLabel.Content = timer.TimeLeft.ToString("ss");
                        Settings.intervals--;
                        timer.TimeChanged += Check_TimeLeft;
                    }
                    else
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    break;

                case 5:
                    //Left label as hours 
                    leftLabel.Foreground = Brushes.Blue;
                    leftLabel.FontSize = threeSegmentFontSize;
                    //Left colon label
                    colonLeftLabel.Foreground = Brushes.Green;
                    colonLeftLabel.FontSize = threeSegmentFontSize;
                    colonLeftLabel.Content = "";
                    //Middle label set as minutes
                    middleLabel.Foreground = Brushes.Green;
                    middleLabel.FontSize = threeSegmentFontSize;
                    //right colon label
                    colonRightLabel.Foreground = Brushes.Green;
                    colonRightLabel.FontSize = threeSegmentFontSize;
                    colonRightLabel.Content = ":";
                    //right Label set as seconds
                    rightLabel.Foreground = Brushes.Green;
                    rightLabel.FontSize = threeSegmentFontSize;
                    //10 sekunders vila för tabata
                    timer.Reset();
                    timer.SetTime(0, 10);
                    timer.Start();
                    timer.TimeChanged += () => leftLabel.Content = SettingsIntervalToString();
                    timer.TimeChanged += () => middleLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.TimeChanged += () => rightLabel.Content = timer.TimeLeft.ToString("ff");
                    timer.StepMs = 61;
                    if (Settings.intervals > 0)
                    {
                        Settings.trainingType = 3;
                    }
                    timer.CountDownFinished += timer.Dispose;
                    break;
            }
        }

        private void Hours_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!timer.IsRunning)
            {
                TimerSettings window = new TimerSettings();
                window.Show();
            }
        }
    }
}