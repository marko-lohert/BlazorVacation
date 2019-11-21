using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorVacation.Shared;

namespace BlazorVacation.Client.Pages.Reports
{
    public partial class WhoIsOnVacation
    {
        List<(Employee employee, Vacation vacation)>? EmployeesVacations { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EmployeesVacations = await Http.GetJsonAsync<List<(Employee, Vacation)>>($"api/Vacations/WhoIsOnVacation?range=7");
        }
    }
}
