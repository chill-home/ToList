using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ToList
{
    public partial class MainPage : ContentPage
    {
        private DateTime _selectedDate;
        public MainPage()
        {
            InitializeComponent();
            CreateCalendar();
        }
        private void CreateCalendar()
        {
            DateTime today = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);
            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            var dayOfWeek = (int)firstDayOfMonth.DayOfWeek;

            for (int i = 0; i < daysInMonth; i++)
            {
                var dayLabel = new Label
                {
                    Text = (i + 1).ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    Style = (Style)Resources["DayLabelStyle"]
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                int day = i + 1;  // захват переменной для использования в лямбда-выражении
                tapGestureRecognizer.Tapped += (s, e) => OnDayTapped(day);
                dayLabel.GestureRecognizers.Add(tapGestureRecognizer);

                CalendarGrid.Children.Add(dayLabel, (dayOfWeek + i) % 7, (dayOfWeek + i) / 7);
            }
        }
        private async void OnDayTapped(int day)
        {
            _selectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, day);
            await DisplayAlert("Date Selected", $"You selected {day}", "OK");

            await LoadTasksForSelectedDate();
        }
        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _selectedDate = e.NewDate;
            await LoadTasksForSelectedDate();
        }
        private async Task LoadTasksForSelectedDate()
        {
            try
            {
                var tasks = await App.Database.GetTasksByDateAsync(_selectedDate);
                TasksStackLayout.Children.Clear();

                if (tasks == null || !tasks.Any())
                {
                    var noTasksLabel = new Label
                    {
                        Text = "No tasks for the selected date.",
                        Style = (Style)Resources["DayLabelStyle"]
                    };

                    TasksStackLayout.Children.Add(noTasksLabel);
                    return;
                }

                foreach (var task in tasks)
                {
                    var taskLabel = new Label
                    {
                        Text = $"{task.TaskName}: {task.Description}:{task.Time}",
                        Style = (Style)Resources["TaskLabelStyle"]
                    };

                    var frame = new Frame
                    {
                        Style = (Style)Resources["TaskFrameStyle"],
                        Content = taskLabel
                    };

                    TasksStackLayout.Children.Add(frame);
                }
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
            }
        }
        private async void OnAddTaskButtonClicked(object sender, EventArgs e)
        {
            if (_selectedDate == default)
            {
                await DisplayAlert("No Date Selected", "Please select a date first.", "OK");
                return;
            }

            await Navigation.PushAsync(new Page1(_selectedDate));
        }

    }   
}
