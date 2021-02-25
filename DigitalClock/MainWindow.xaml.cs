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
        private Label hoursLabel;
        private Label minutesLabel;
        private Label colonLabel;
        public CountDownTimer timer = new CountDownTimer();
        public DateTime dateTimeNow;

        //Countdown sound effect
        public SoundPlayer soundEffect = new SoundPlayer(Path.Combine(Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin")), @"Sounds\", "CountDown.wav"));

        public double fontSize = SystemParameters.PrimaryScreenHeight - SystemParameters.PrimaryScreenHeight / 2;

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
            // The old contents of Main go here
            //Timer in background to update DateTime
            DispatcherTimer asyncTimer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
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
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40, GridUnitType.Star) });

            hoursLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = "00",
                BorderThickness = thickness,
                FontSize = fontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontFamily = digitalClockFont
            };
            hoursLabel.MouseDown += Hours_MouseDown;
            grid.Children.Add(hoursLabel);
            Grid.SetColumn(hoursLabel, 0);
            Grid.SetRow(hoursLabel, 0);
            colonLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = ":",
                BorderThickness = thickness,
                FontSize = fontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontFamily = digitalClockFont
            };
            grid.Children.Add(colonLabel);
            Grid.SetColumn(colonLabel, 1);
            Grid.SetRow(colonLabel, 0);
            minutesLabel = new Label
            {
                Foreground = Brushes.Red,
                Background = Brushes.Black,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                Content = "00",
                BorderThickness = thickness,
                FontSize = fontSize,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                FontFamily = digitalClockFont
            };
            minutesLabel.MouseDown += Minutes_MouseDown;
            grid.Children.Add(minutesLabel);
            Grid.SetColumn(minutesLabel, 2);
            Grid.SetRow(minutesLabel, 0);
        }

        private void AsyncTimer_Tick(object sender, EventArgs e)
        {
            //Sätter tid enbart om timer inte körs
            if (!timer.IsRunning)
            {
                dateTimeNow = DateTime.Now;
                hoursLabel.Content = DateTime.Now.ToString("HH");
                minutesLabel.Content = DateTime.Now.ToString("mm");
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
                soundEffect.Load();
                soundEffect.Play();
                timer.Reset();
                timer.SetTime(0, 11);
                timer.Start();
                timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("ss");
                timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ff");
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
        }

        private void Start_Timers()
        {
            switch (Settings.trainingType)
            {
                //Räkna upp till 60 med minuter och sekunder
                case 0:
                    //Räkna upp till 60min
                    timer.Reset();
                    timer.SetTime(60, 0);
                    timer.Start();
                    timer.TimeChanged += () => hoursLabel.Content = timer.stopWatch.Elapsed.ToString("mm");
                    timer.TimeChanged += () => minutesLabel.Content = timer.stopWatch.Elapsed.ToString("ss");
                    timer.CountDownFinished += timer.Dispose;
                    break;

                case 1:
                    //Intervall träning
                    if (Settings.intervals >= 0)
                    {
                        timer.Reset();
                        timer.SetTime(Settings.minutes, Settings.seconds);
                        timer.Start();
                        timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("mm");
                        timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ss");
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
                    //Räkna ner från inställd tid
                    timer.Reset();
                    timer.SetTime(Settings.minutes, Settings.seconds);

                    timer.Start();
                    timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("mm");
                    timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.CountDownFinished += timer.Dispose;
                    break;

                case 3:
                    //Tabata
                    minutesLabel.Foreground = Brushes.Red;
                    hoursLabel.Foreground = Brushes.Red;
                    colonLabel.Foreground = Brushes.Red;
                    timer.Reset();
                    timer.SetTime(0, 20);
                    timer.Start();
                    timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ff");
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
                    if (Settings.intervals % 6 == 0)
                    {
                        minutesLabel.Foreground = Brushes.Green;
                        hoursLabel.Foreground = Brushes.Green;
                        colonLabel.Foreground = Brushes.Green;
                    }
                    else if (Settings.intervals % 2 == 0)
                    {
                        minutesLabel.Foreground = Brushes.Yellow;
                        hoursLabel.Foreground = Brushes.Yellow;
                        colonLabel.Foreground = Brushes.Yellow;
                    }
                    else
                    {
                        minutesLabel.Foreground = Brushes.Red;
                        hoursLabel.Foreground = Brushes.Red;
                        colonLabel.Foreground = Brushes.Red;
                    }
                    if (Settings.intervals >= 0)
                    {
                        timer.Reset();
                        timer.SetTime(1, 5);
                        timer.Start();
                        timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("mm");
                        timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ss");
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
                    //10 sekunders vila för tabata
                    minutesLabel.Foreground = Brushes.Green;
                    hoursLabel.Foreground = Brushes.Green;
                    colonLabel.Foreground = Brushes.Green;
                    timer.Reset();
                    timer.SetTime(0, 10);
                    timer.Start();
                    timer.TimeChanged += () => hoursLabel.Content = timer.TimeLeft.ToString("ss");
                    timer.TimeChanged += () => minutesLabel.Content = timer.TimeLeft.ToString("ff");
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