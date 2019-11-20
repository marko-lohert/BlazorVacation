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
    }
}
