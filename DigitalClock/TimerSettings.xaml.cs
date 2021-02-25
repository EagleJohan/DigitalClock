using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DigitalClock
{
    /// <summary>
    /// Interaction logic for TimerSettings.xaml
    /// </summary>
    public partial class TimerSettings : Window
    {
        public int intervallReps { get; set; }
        public ComboBox timerType;
        public TextBox minutesTextBox;
        public TextBox secondsTextBox;
        public TextBox intervalTextBox;

        public TimerSettings()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            //Window settings
            Title = "Settings";
            Height = 400;
            Width = 600;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Topmost = true;
            //scrolling
            ScrollViewer root = new ScrollViewer();
            root.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            root.VerticalContentAlignment = VerticalAlignment.Center;
            root.HorizontalContentAlignment = HorizontalAlignment.Center;
            Content = root;

            //Main grid
            Grid grid = new Grid();
            root.Content = grid;
            grid.Margin = new Thickness(5);
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { });
            grid.ColumnDefinitions.Add(new ColumnDefinition { });
            //Titel för inställningar
            Label title = new Label
            {
                Content = "Inställningar",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35
            };
            grid.Children.Add(title);
            //Spara och stäng knapp
            Button stäng = new Button
            {
                Content = "Stäng",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 25
            };
            grid.Children.Add(stäng);
            Grid.SetColumn(stäng, 1);
            stäng.Click += delegate { Close(); };
            //Combobox för att välja träningstyp och en itemsource
            //Itemsource i lista
            List<string> träningsTyper = new List<string> { "Räkna upp till 60", "Intervallträning", "Räkna ner", "Tabata", "Fight Gone Bad" };
            //Comboboxen timertype
            timerType = new ComboBox
            {
                ItemsSource = träningsTyper,
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35,
                MaxHeight = 70,
                SelectedIndex = Settings.trainingType
            };
            timerType.SelectionChanged += TimerType_SelectionChanged;
            grid.Children.Add(timerType);
            Grid.SetRow(timerType, 1);
            Grid.SetColumnSpan(timerType, 2);
            //Labels på vänstra sidan
            #region
            Label minutesLabel = new Label
            {
                Content = "Minuter:",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35
            };
            grid.Children.Add(minutesLabel);
            Grid.SetRow(minutesLabel, 2);
            Label secondsLabel = new Label
            {
                Content = "Sekunder:",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35
            };
            grid.Children.Add(secondsLabel);
            Grid.SetRow(secondsLabel, 3);
            Label intervalLabel = new Label
            {
                Content = "Antal:",
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35
            };
            grid.Children.Add(intervalLabel);
            Grid.SetRow(intervalLabel, 4);
            #endregion
            //Textbox för input på högra sidan
            minutesTextBox = new TextBox
            {
                Text = Settings.minutes.ToString(),
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35,
                IsEnabled = false
            };
            minutesTextBox.KeyDown += Integers_KeyDown;
            minutesTextBox.TextChanged += MinutesTextBox_TextChanged;
            grid.Children.Add(minutesTextBox);
            Grid.SetColumn(minutesTextBox, 1);
            Grid.SetRow(minutesTextBox, 2);
            secondsTextBox = new TextBox
            {
                Text = Settings.seconds.ToString(),
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35,
                IsEnabled = false
            };
            secondsTextBox.KeyDown += Integers_KeyDown;
            secondsTextBox.TextChanged += SecondsTextBox_TextChanged;
            grid.Children.Add(secondsTextBox);
            Grid.SetColumn(secondsTextBox, 1);
            Grid.SetRow(secondsTextBox, 3);
            intervalTextBox = new TextBox
            {
                Text = Settings.intervals.ToString(),
                Margin = new Thickness(5),
                Padding = new Thickness(5),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = 35,
                IsEnabled = false
            };
            intervalTextBox.KeyDown += Integers_KeyDown;
            intervalTextBox.TextChanged += IntervalTextBox_TextChanged;
            grid.Children.Add(intervalTextBox);
            Grid.SetColumn(intervalTextBox, 1);
            Grid.SetRow(intervalTextBox, 4);
        }

        private void TimerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Settings.trainingType = timerType.SelectedIndex;
            switch (timerType.SelectedIndex) 
            {
                case 0:
                    minutesTextBox.IsEnabled = false;
                    secondsTextBox.IsEnabled = false;
                    intervalTextBox.IsEnabled = false;
                    break;
                case 1:
                    intervalTextBox.IsEnabled = true;
                    minutesTextBox.IsEnabled = true;
                    secondsTextBox.IsEnabled = true;
                    break;
                case 2:
                    intervalTextBox.IsEnabled = false;
                    minutesTextBox.IsEnabled = true;
                    secondsTextBox.IsEnabled = true;
                    break;
                case 3:
                    minutesTextBox.IsEnabled = false;
                    secondsTextBox.IsEnabled = false;
                    intervalTextBox.IsEnabled = true;
                    break;
                case 4:
                    intervalTextBox.Text = "22";
                    intervalTextBox.IsEnabled = true;
                    minutesTextBox.Text = "1";
                    minutesTextBox.IsEnabled = false;
                    secondsTextBox.Text = "5";
                    secondsTextBox.IsEnabled = false;
                    break;
            }
        }

        private void Integers_KeyDown(object sender, KeyEventArgs e)
        {
            var digitkeys = e.Key >= Key.D0 && e.Key <= Key.D9;
            var numbpadKeys = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9;
            var modifiedKey = e.KeyboardDevice.Modifiers == ModifierKeys.None;
            if (modifiedKey && (digitkeys || numbpadKeys))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void IntervalTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(intervalTextBox.Text, out int result);
            if (intervalTextBox.Text.Length > 0 && result < 0)
            {
                intervalTextBox.Text = "0";
            }
            else
            {
                Settings.intervals = result;
            }
        }

        private void SecondsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(secondsTextBox.Text, out int result);
            if (secondsTextBox.Text.Length > 0 && result < 0)
            {
                secondsTextBox.Text = "0";
            }
            else
            {
                Settings.seconds = result;
            }
        }

        private void MinutesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(minutesTextBox.Text, out int result);
            if (minutesTextBox.Text.Length > 0 && result < 0)
            {
                minutesTextBox.Text = "0";
            }
            else
            {
                Settings.minutes = result;
            }
        }
    }
}