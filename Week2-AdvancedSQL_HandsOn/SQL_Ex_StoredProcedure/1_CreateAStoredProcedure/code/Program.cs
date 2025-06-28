using System;
using System.Data;
using Microsoft.Data.SqlClient;

class Program
{
    // IMPORTANT: Configure your SQL Server connection string here.
    // Replace "Data Source=.;" with your actual SQL Server instance name if different.
    // Examples:
    // "Data Source=.;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;" (for default local instance)
    // "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;" (for localdb)
    // "Data Source=YOUR_MACHINE_NAME;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;"
    private static string _connectionString = "Data Source=RICHA;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;";

    static void Main(string[] args)
    {
        Console.WriteLine("--- Exercise 1: Creating a Stored Procedure ---");

        // SQL commands for creating the stored procedure.
        // Each string in this array will be executed as a separate batch.
        string[] createSpSqlCommands = new string[]
        {
            // Command 1: Drop the procedure if it exists.
            // This must be a separate batch from CREATE PROCEDURE.
            @"
            IF OBJECT_ID('sp_GetEmployeesByDepartment', 'P') IS NOT NULL
                DROP PROCEDURE sp_GetEmployeesByDepartment;
            ",
            // Command 2: Create the stored procedure.
            // This must be the first statement in its own batch.
            @"
            CREATE PROCEDURE sp_GetEmployeesByDepartment
                @DepartmentID INT
            AS
            BEGIN
                SELECT
                    EmployeeID,
                    FirstName,
                    LastName,
                    DepartmentID,
                    Salary,
                    JoinDate
                FROM
                    Employees
                WHERE
                    DepartmentID = @DepartmentID;
            END;
            "
        };

        // Execute the commands to create the stored procedure
        ExecuteDdlCommand("Create sp_GetEmployeesByDepartment", createSpSqlCommands);

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    /// <summary>
    /// Executes one or more DDL (Data Definition Language) SQL commands (e.g., CREATE, ALTER, DROP).
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
                    // Skip empty or whitespace-only lines
                    if (string.IsNullOrWhiteSpace(sqlCommand))
                        continue;

                    using (SqlCommand command = new SqlCommand(sqlCommand, connection))
                    {
                        // Set a command timeout to prevent indefinite waiting
                        command.CommandTimeout = 30; // 30 seconds

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
}