using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorVacation.Shared;

namespace BlazorVacation.Client.Pages.Vacations
{
    public partial class AddNewVacation
    {
        Vacation NewVacation = new Vacation()
        {
            FromDate = DateTime.Today,
            TillDate = DateTime.Today.AddDays(1)
        };

        string? Message { get; set; }

        List<Holiday>? Holidays;
        int TotalAssignedVacationDays;

        protected override async Task OnInitializedAsync()
        {
            Holidays = await Http.GetJsonAsync<List<Holiday>>("api/Holidays/GetHolidays");
            TotalAssignedVacationDays = await Http.GetJsonAsync<int>("api/Vacations/GetTotalAssignedVacationDays");
        }

        int CalculateDuration()
        {
            if (NewVacation.TillDate >= NewVacation.FromDate)
                return CountTotalDays() - CountNonWorkingDays(NewVacation.FromDate, NewVacation.TillDate, Holidays) + 1;
            else
                return 0;

            int CountTotalDays() => NewVacation.TillDate > NewVacation.FromDate ? (NewVacation.TillDate - NewVacation.FromDate).Days : 0;

            static int CountNonWorkingDays(DateTime startDate, DateTime endDate, List<Holiday>? holidays)
            {
                if (startDate > endDate)
                    return 0;

                var countNonWorkingDays = (from x in Enumerable.Range(0, (endDate - startDate).Days + 1)
                                           select startDate.AddDays(x) into d
                                           where d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday
                                               || (holidays != null && holidays.Exists(h => h.FromDate <= d && h.TillDate >= d))
                                           select d)
                                            .Count();

                return countNonWorkingDays;
            }
        }

        private (bool isValid, string? message) Validate()
        {
            string errorMessage;

            if (NewVacation.TillDate < NewVacation.FromDate)
            {
                errorMessage = @"<div class=""alert alert-danger"">
                        <strong>Till date</strong> must be equal or greater than <strong>from date</strong>.
                     </div>";

                return (false, errorMessage);
            }

            if (NewVacation.TillDate >= NewVacation.FromDate && NewVacation.Duration == 0)
            {
                errorMessage = @"<div class=""alert alert-danger"">
                        Vacation should last at least one day. <em>Weekends and holidays are not counted as vacation days</em>.
                     </div>";

                return (false, errorMessage);
            }

            return (true, null);
        }

        async Task SubmitRequest()
        {
            if (NewVacation.Note == NewVacation.Note?.ToUpper())
            {
                string warningMessage = "It's recommended that a note is not written with all uppercase letters. Are you sure that you would like to continue?";
                object[] parameters = { warningMessage };

                bool continueWithSubmit = await JSRuntime.InvokeAsync<bool>("jsInteropBlazorVacation.ask", parameters);

                if (!continueWithSubmit)
                    return;
            }

            NewVacation.Duration = CalculateDuration();

            var (validVacation, message) = Validate();
            if (!validVacation)
            {
                Message = message;
                return;
            }

            await Http.SendJsonAsync(HttpMethod.Put, "api/Vacations/AddNewVacation", NewVacation);

            Message = @"<div class=""alert alert-success"">
                Vacation request submitted.
            </div>";
        }
    }
}
