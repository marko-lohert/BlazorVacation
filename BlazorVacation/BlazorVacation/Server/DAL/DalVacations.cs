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


            using SqlConnection connection = new SqlConnection(ConnectionString);
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

        public int GetTotalAssignedVacationDays(int year)
        {
            string cmdText = $@"SELECT EMPLOYEE.TOTAL_ASSIGNED_VACATION_DAYS
                                FROM EMPLOYEE
                                WHERE FIRST_NAME = '{CurrentUser.FirstName}' AND
	                                  LAST_NAME = '{CurrentUser.LastName}';";


            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmdText, connection);

            SqlDataReader reader = sqlCommand.ExecuteReader();

            int totalAssignedVacationDays = 0;
            while (reader.Read())
            {
                totalAssignedVacationDays = (int)reader["TOTAL_ASSIGNED_VACATION_DAYS"];
            }

            reader.Close();
            connection.Close();

            return totalAssignedVacationDays;
        }

        public void AddNewVacation(Vacation newVacation)
        {
            int approved = newVacation.Approved ? 1 : 0;
            int setUpOutOfOfficeEmail = newVacation.SetUpOutOfOfficeEmail ? 1 : 0;

            string cmdText = $@"INSERT INTO VACATION (FROM_DATE, TILL_DATE, NOTE, DURATION, APPROVED, SETUP_OUT_OF_OFFICE_EMAIL)
                                VALUES ('{newVacation.FromDate.ToString("yyyy-MM-dd")}', '{newVacation.TillDate.ToString("yyyy-MM-dd")}', '{newVacation.Note}', {newVacation.Duration}, {approved}, {setUpOutOfOfficeEmail});
                                INSERT INTO EMPLOYEE_VACATION (EMPLOYEE_ID, VACATION_ID)
                                VALUES ((SELECT ID FROM EMPLOYEE WHERE EMPLOYEE.FIRST_NAME = '{CurrentUser.FirstName}' AND EMPLOYEE.LAST_NAME = '{CurrentUser.LastName}'), (SELECT MAX(ID) FROM VACATION));";

            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand sqlCommand = new SqlCommand(cmdText, connection);

            int rowsInserted = sqlCommand.ExecuteNonQuery();

            connection.Close();
        }

        public List<(Employee, Vacation)> GetVacationsCompanyNextDays(int range)
        {
            List<(Employee, Vacation)> listEmployeeVacation = new List<(Employee, Vacation)>();

            string cmdText = $@"SELECT VACATION.FROM_DATE,
                                       VACATION.TILL_DATE,
                                       VACATION.DURATION,                                       
                                       VACATION.NOTE,                                       
                                       VACATION.APPROVED,                                       
                                       VACATION.SETUP_OUT_OF_OFFICE_EMAIL,                                       
                                       EMPLOYEE.*
                                FROM VACATION
                                INNER JOIN EMPLOYEE_VACATION ON EMPLOYEE_VACATION.VACATION_ID = VACATION.ID
                                INNER JOIN EMPLOYEE ON EMPLOYEE.ID = EMPLOYEE_VACATION.EMPLOYEE_ID
                                WHERE VACATION.FROM_DATE <= CAST(DATEADD(day, {range}, SYSDATETIME()) AS DATE)
                                    AND VACATION.FROM_DATE >= CAST(SYSDATETIME() AS DATE)
                                ORDER BY VACATION.FROM_DATE;";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand sqlCommand = new SqlCommand(cmdText, connection);

                SqlDataReader reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    listEmployeeVacation.Add
                    (
                        (
                            new Employee
                            {
                                FirstName = reader["FIRST_NAME"] as string,
                                LastName = reader["LAST_NAME"] as string,
                                TotalAssignedVacationDays = (int)reader["TOTAL_ASSIGNED_VACATION_DAYS"]
                            }
                            ,
                            new Vacation
                            {
                                FromDate = (DateTime)reader[0],
                                TillDate = (DateTime)reader[1],
                                Duration = (int)reader[2],
                                Note = !reader.IsDBNull(3) ? reader[3] as string : null,
                                Approved = (bool)reader[4],
                                SetUpOutOfOfficeEmail = !reader.IsDBNull(5) ? (bool)reader[5] : false
                            }
                        )
                    );
                }

                reader.Close();
                connection.Close();

                return listEmployeeVacation;
            }
        }

        private (string FirstName, string LastName) CurrentUser = ("Marko", "Lohert");

        private string ConnectionString = "Data Source = .;Initial Catalog = BlazorVacation;Integrated Security=true";
    }
}
