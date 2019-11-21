using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorVacation.Shared;

namespace BlazorVacation.Client.Pages.Vacations
{
    public partial class AlradySubmittedVacations
    {
        List<Vacation>? Vacations;

        protected override async Task OnInitializedAsync()
        {
            int thisYear = DateTime.Today.Year;
            Vacations = await Http.GetJsonAsync<List<Vacation>>($"api/Vacations/GetSubmittedVacations?year={thisYear}");
        }
    }
}
