using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

class Program
{
    // IMPORTANT: Configure your SQL Server connection string here.
    // E.g., "Data Source=.;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;"
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

    /// <summary>
    /// Executes one or more DDL (Data Definition Language) SQL commands (e.g., CREATE, ALTER, DROP for functions/procedures).
    /// Each string in the provided array is executed as a separate command/batch.
    /// </summary>
    /// <param name="commandName">A descriptive name for the overall operation being executed.</param>
    /// <param name="sqlCommands">An array of SQL command strings to execute. Each string will be sent as a separate command.</param>
    static void ExecuteDdlCommand(string commandName, string[] sqlCommands)
    {
        Console.WriteLine($"\n--- Executing {commandName} ---");
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");

                foreach (string sqlCommand in sqlCommands)
                {
                    if (string.IsNullOrWhiteSpace(sqlCommand))
                        continue;

                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        command.CommandTimeout = 30; // 30 seconds timeout (optional but good practice)

                        command.ExecuteNonQuery();
                        // Print the first line of the executed command for logging purposes
                        Console.WriteLine($"  - Part of {commandName} executed: '{sqlCommand.Trim().Split('\n')[0].Trim()}'...");
                    }
                }
                Console.WriteLine($"{commandName} completed all parts successfully.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error during {commandName}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during {commandName}: {ex.Message}");
        }
    }

    /// <summary>
    /// Executes the fn_CalculateAnnualSalary scalar function for a specific employee
    /// and prints their annual salary.
    /// </summary>
    /// <param name="employeeId">The ID of the employee to get the annual salary for.</param>
    static void ExecuteScalarFunctionForEmployee(int employeeId)
    {
        Console.WriteLine($"\nRetrieving annual salary for EmployeeID: {employeeId}...");
        try
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Connection opened successfully.");

                // SQL query to call the scalar function
                // We need to first get the employee's salary to pass it to the function.
                // This can be done in one query using a subquery or JOIN.
                // A simpler way for a specific employee ID is to select the salary first,
                // then pass it to the function. Or, include the function call directly in SELECT.
                string sqlQuery = $@"
                    SELECT
                        E.FirstName,
                        E.LastName,
                        E.Salary,
                        dbo.fn_CalculateAnnualSalary(E.Salary) AS AnnualSalary
                    FROM
                        Employees AS E
                    WHERE
                        E.EmployeeID = @EmployeeID;";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            decimal monthlySalary = reader.GetDecimal(reader.GetOrdinal("Salary"));
                            decimal annualSalary = reader.GetDecimal(reader.GetOrdinal("AnnualSalary"));

                            Console.WriteLine($"Employee: {firstName} {lastName} (ID: {employeeId})");
                            Console.WriteLine($"Monthly Salary: {monthlySalary:C}");
                            Console.WriteLine($"Annual Salary: {annualSalary:C}");
                        }
                        else
                        {
                            Console.WriteLine($"No employee found with EmployeeID: {employeeId}");
                        }
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error while executing function for EmployeeID {employeeId}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred for EmployeeID {employeeId}: {ex.Message}");
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 7: Return Data from a Scalar Function ---");

        // --- Step 1: Create the prerequisite scalar function (from Exercise 1 of Functions PDF) ---
        string[] createFunctionSqlCommands = new string[]
        {
            // Drop function if it exists to ensure a clean creation
            @"
            IF OBJECT_ID('fn_CalculateAnnualSalary', 'FN') IS NOT NULL
                DROP FUNCTION fn_CalculateAnnualSalary;
            ",
            // Create the scalar function
            @"
            CREATE FUNCTION fn_CalculateAnnualSalary
            (
                @Salary DECIMAL(10,2)
            )
            RETURNS DECIMAL(10,2)
            AS
            BEGIN
                RETURN @Salary * 12;
            END;
            "
        };
        ExecuteDdlCommand("Create fn_CalculateAnnualSalary", createFunctionSqlCommands);

        // --- Step 2: Execute Exercise 7's goal ---
        Console.WriteLine("\n--- Executing Exercise 7: Return Data from a Scalar Function ---");
        // Goal: Return the annual salary for a specific employee using 'fn_CalculateAnnualSalary'.
        // Steps: Execute the 'fn_CalculateAnnualSalary function for an employee with 'EmployeeID = 1'.
        ExecuteScalarFunctionForEmployee(1); // This call is now after the method definition

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }
}