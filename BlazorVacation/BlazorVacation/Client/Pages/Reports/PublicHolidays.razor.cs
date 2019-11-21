using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorVacation.Shared;

namespace BlazorVacation.Client.Pages.Reports
{
    public partial class PublicHolidays
    {
        [Parameter]
        public string? Year { get; set; } // string? instead of int because Year is used as parameter in routing.

        List<Holiday>? Holidays;
        static List<int> ListYears = Enumerable.Range(2019, 3).ToList<int>();
        public string? SelectedYear { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(Year))
                Year = DateTime.Now.Year.ToString();

            Holidays = await Http.GetJsonAsync<List<Holiday>>($"api/Holidays/GetHolidays?year={Year}");

            SelectedYear = Year;
        }

        async Task ChangeYear()
        {
            Holidays = await Http.GetJsonAsync<List<Holiday>>($"api/Holidays/GetHolidays?year={Year}");
            this.StateHasChanged();
        }
    }
}
