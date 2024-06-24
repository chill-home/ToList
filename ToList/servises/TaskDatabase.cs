using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ToList.servises
{
    public class TaskDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public TaskDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<TaskItem>().Wait();
        }

        public Task<List<TaskItem>> GetTasksAsync()
        {
            return _database.Table<TaskItem>().ToListAsync();
        }

        
        public Task<List<TaskItem>> GetTasksByDateAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            return _database.Table<TaskItem>()
                            .Where(t => t.Date >= startOfDay && t.Date <= endOfDay)
                            .ToListAsync();
        }

        public Task<int> SaveTaskAsync(TaskItem task)
        {
            if (task.Id != 0)
            {
                return _database.UpdateAsync(task);
            }
            else
            {
                return _database.InsertAsync(task);
            }
        }

        public Task<int> DeleteTaskAsync(TaskItem task)
        {
            return _database.DeleteAsync(task);
        }
    }

}
