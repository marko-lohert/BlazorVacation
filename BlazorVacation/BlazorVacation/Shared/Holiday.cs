using System;

namespace BlazorVacation.Shared
{
    public class Holiday
    {
        public DateTime FromDate { get; set; }
        public DateTime TillDate { get; set; }
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
