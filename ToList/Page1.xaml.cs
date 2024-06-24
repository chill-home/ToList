using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToList.servises;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private TimeSpan _selectedTime;
        private DateTime _selectedDate;

        public Action<TaskItem> TaskAdded;

        public Page1(DateTime selectedDate)
        {
            InitializeComponent();
            _selectedTime = new TimeSpan(7, 35, 0);
            _selectedDate = selectedDate;
            SelectedDateLabel.Text = $"Задача для {_selectedDate.ToString("dd.MM.yyyy")}";
            UpdateTimeLabel();
        }

        private void OnUpButtonClicked(object sender, EventArgs e)
        {
            // Добавляем 5 минут
            _selectedTime = _selectedTime.Add(new TimeSpan(0, 5, 0));
            UpdateTimeLabel();
        }

        private void OnDownButtonClicked(object sender, EventArgs e)
        {
            // Убираем 5 минут, если текущее время больше или равно 5 минут
            if (_selectedTime.TotalMinutes >= 5)
            {
                _selectedTime = _selectedTime.Subtract(new TimeSpan(0, 5, 0));
            }
            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            // Обновляем текст метки времени
            TimeLabel.Text = _selectedTime.ToString(@"hh\:mm");
        }

        private async void CreateTaskClicked(object sender, EventArgs e)
        {
            var task = new TaskItem
            {
                TaskName = TaskNameEntry.Text,
                Description = TaskDescriptionEntry.Text,
                Date = _selectedDate,
                Time = _selectedTime
            };

            await App.Database.SaveTaskAsync(task);

            // Показать уведомление и вернуться на главную страницу
            await DisplayAlert("Задача добавлена!", $"Задача для {_selectedDate.ToString("dd.MM.yyyy")} добавлена.", "OK");
            await Navigation.PopAsync();
        }
    }

}