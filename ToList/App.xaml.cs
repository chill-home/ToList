using System;
using System.IO;
using ToList.servises;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToList
{
    public partial class App : Application
    {
        static TaskDatabase database;
        public static TaskDatabase Database
        {
            get
            {
                if (database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Tasks.db3");
                    database = new TaskDatabase(dbPath);
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Page2());
        }
    }
}