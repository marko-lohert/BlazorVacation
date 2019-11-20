using BlazorVacation.Shared;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace BlazorVacation.Server.DAL
{
    public class DalHolidays
    {
        public List<Holiday> GetHolidays(int year)
        {
            List<Holiday> listHoliday = new List<Holiday>();

            string cmdText = $@"SELECT *
                                FROM HOLIDAY
                                WHERE YEAR(FROM_DATE) = {year} OR
	                                  YEAR(TILL_DATE) = {year};";

            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmdText, connection);

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                listHoliday.Add
                (
                    new Holiday
                    {
                        FromDate = (DateTime)reader["FROM_DATE"],
                        TillDate = (DateTime)reader["TILL_DATE"],
                        Name = (string)reader["Name"]
                    }
                );
            }

            reader.Close();
            connection.Close();

            return listHoliday;
        }

        private string ConnectionString = "Data Source = .;Initial Catalog = BlazorVacation;Integrated Security=true";
    }
}
