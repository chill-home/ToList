using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ToList.servises;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotification;
namespace ToList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
            LoadTodayTasks();
        }

        private async void LoadTodayTasks()
        {
            try
            {
                // Получаем текущую дату
                var today = DateTime.Now.Date;
                Debug.WriteLine($"[LoadTodayTasks] Loading tasks for date: {today}");

                // Загружаем задачи из базы данных
                var tasks = await App.Database.GetTasksByDateAsync(today);
                if (tasks == null)
                {
                    Debug.WriteLine("[LoadTodayTasks] No tasks were returned from the database.");
                    tasks = new List<TaskItem>(); // Создаем пустой список для предотвращения дальнейших ошибок
                }
                else
                {
                    Debug.WriteLine($"[LoadTodayTasks] {tasks.Count} tasks were loaded from the database.");
                }

                // Очищаем StackLayout перед добавлением новых элементов
                TasksStackLayout.Children.Clear();

                // Если задач на сегодня нет
                if (!tasks.Any())
                {
                    var noTasksLabel = new Label
                    {
                        Text = "No tasks for today.",
                        Style = (Style)Resources["TaskLabelStyle"]
                    };

                    TasksStackLayout.Children.Add(noTasksLabel);
                    return;
                }

                // Добавляем задачи в StackLayout
                foreach (var task in tasks)
                {
                    string formattedTime = null;

                    // Проверка на null и обработка task.Time как строки
                    if (task.Time != null)
                    {
                        var timeString = task.Time.ToString();

                        // Попытка парсинга строки в DateTime
                        if (DateTime.TryParse(timeString, out DateTime dateTime))
                        {
                            formattedTime = dateTime.ToString("HH:mm");
                        }
                        // Попытка парсинга строки в TimeSpan
                        else if (TimeSpan.TryParse(timeString, out TimeSpan timeSpan))
                        {
                            formattedTime = timeSpan.ToString(@"hh\:mm");
                        }
                        
                    }
                    else
                    {
                        Debug.WriteLine($"[LoadTodayTasks] Task {task.TaskName} has a null Time.");
                    }

                    var taskLabel = new Label
                    {
                        Text = formattedTime != null
                            ? $"{task.TaskName}: {task.Description} : {formattedTime}"
                            : $"{task.TaskName}: {task.Description} (No time specified)",
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
                await DisplayAlert("Error", $"Failed to load tasks: {ex.Message}", "OK");
            }
        }
        private async void MainPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }
    }
}