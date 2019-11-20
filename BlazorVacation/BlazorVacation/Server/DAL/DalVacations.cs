using BlazorVacation.Shared;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace BlazorVacation.Server.Dal
{
    public class DalVacations
    {
        public List<Vacation> GetSubmittedVacationsCurrentUser(int year)
        {
            List<Vacation> listVacation = new List<Vacation>();

            string cmdText = $@"SELECT VACATION.*
                                FROM VACATION
                                INNER JOIN EMPLOYEE_VACATION ON EMPLOYEE_VACATION.VACATION_ID = VACATION.ID
                                INNER JOIN EMPLOYEE ON EMPLOYEE.ID = EMPLOYEE_VACATION.EMPLOYEE_ID
                                WHERE EMPLOYEE.FIRST_NAME = '{CurrentUser.FirstName}' AND
	                                  EMPLOYEE.LAST_NAME = '{CurrentUser.LastName}' AND
                                      (YEAR(VACATION.FROM_DATE) = {year} OR
	                                   YEAR(VACATION.TILL_DATE) = {year});";


            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    listVacation.Add
                    (
                        new Vacation
                        {
                            FromDate = (DateTime)reader["FROM_DATE"],
                            TillDate = (DateTime)reader["TILL_DATE"],
                            Duration = (int)reader["DURATION"],
                            Note = !reader.IsDBNull(3) ? reader["NOTE"] as string : null,
                            Approved = (bool)reader["APPROVED"],
                            SetUpOutOfOfficeEmail = !reader.IsDBNull(5) ? (bool)reader["SETUP_OUT_OF_OFFICE_EMAIL"] : false
                        }
                    );
                }

                reader.Close();
                connection.Close();

                return listVacation;
            }
        }
   
        private (string FirstName, string LastName) CurrentUser = ("Marko", "Lohert");

        private string ConnectionString = "Data Source = .;Initial Catalog = BlazorVacation;Integrated Security=true";
    }
}
