using System.Collections.Generic;

namespace BlazorVacation.Shared
{
    public class Employee
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string FirstName { get; set; }
        public string LastName { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public int TotalAssignedVacationDays { get; set; }
        public List<Vacation>? Vacations { get; set; }
    }
}
