using System;

namespace Final_Project.Data
{
    // Abstract class representing an entity with assigned tasks
    public abstract class TaskAssignable
    {
        public string Task1 { get; set; }

        // Abstract method to be implemented by derived classes
        public abstract string GetTaskDetails();
    }

    // Interface representing a schedule
    public interface ISchedule
    {
        DateOnly Date { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        string GetScheduleDetails();
    }

    // Schedule1 class inheriting from TaskAssignable and implementing ISchedule
    public class Schedule1 : TaskAssignable, ISchedule
    {
        public DateOnly Date { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public Schedule1(DateOnly date, int id, string first_name, string last_name, string task1)
        {
            this.Date = date;
            this.Id = id;
            this.Name = $"{first_name} {last_name}";
            this.Task1 = task1;
        }

        public override string GetTaskDetails()
        {
            return $"Task: {Task1}";
        }

        public string GetScheduleDetails()
        {
            return $"{Id}, {Name}, {Date}, {GetTaskDetails()}";
        }
    }
}
