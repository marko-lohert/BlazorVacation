using System;

namespace BlazorVacation.Shared
{
    public class Vacation
    {
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
        public string Note { get; set; }
        public int Duration { get; set; }
        public bool Approved { get; set; }
        public bool SetUpOutOfOfficeEmail { get; set; }
    }
}
