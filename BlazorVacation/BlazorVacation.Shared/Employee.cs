using System.Collections.Generic;

namespace BlazorVacation.Shared
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TotalAssignedVacationDays { get; set; }
        public List<Vacation> Vacations { get; set; }
    }
}
