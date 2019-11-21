using System;
using System.Collections.Generic;
using BlazorVacation.Server.Dal;
using BlazorVacation.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorVacation.Server.Controllers
{
    [Route("api/[controller]")]
    public class VacationsController : Controller
    {
        [HttpGet("[action]")]
        public List<Vacation> GetSubmittedVacations(int year)
        {
            var dal = new DalVacations();
            return dal.GetSubmittedVacationsCurrentUser(year);
        }

        [HttpGet("[action]")]
        public int GetTotalAssignedVacationDays(int year)
        {
            var dal = new DalVacations();
            return dal.GetTotalAssignedVacationDays(year);
        }

        [HttpPut("[action]")]
        public void AddNewVacation([FromBody]Vacation newVacation)
        {
            var dal = new DalVacations();
            dal.AddNewVacation(newVacation);
        }

        [HttpGet("[action]")]
        public List<(Employee employee, Vacation vacation)> WhoIsOnVacation(int range)
        {
            if (range == default)
                range = 7;

            var dal = new DalVacations();
            return dal.GetVacationsCompanyNextDays(range);
        }
    }
}
