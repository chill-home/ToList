using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToList.servises
{
    public class TaskItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
