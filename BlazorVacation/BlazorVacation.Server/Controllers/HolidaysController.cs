using System;
using System.Collections.Generic;
using BlazorVacation.Server.Dal;
using BlazorVacation.Server.DAL;
using BlazorVacation.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorVacation.Server.Controllers
{
    [Route("api/[controller]")]
    public class HolidaysController : Controller
    {
        [HttpGet("[action]")]
        public List<Holiday> GetHolidays(int year)
        {
            if (year == default)
                year = DateTime.Today.Year;

            var dal = new DalHolidays();
            return dal.GetHolidays(year);
        }        
    }
}
