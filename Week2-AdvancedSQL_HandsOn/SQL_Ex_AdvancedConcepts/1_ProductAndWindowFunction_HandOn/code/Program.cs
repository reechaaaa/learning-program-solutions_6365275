using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

// Define a simple Product class to hold our data
public class Product
{
    public int ProductID { get; set; }
    public string ProductName { get; set; } = null!; // Initialize to non-nullable string for CS8618 warning fix
    public string Category { get; set; } = null!;    // Initialize to non-nullable string for CS8618 warning fix
    public decimal Price { get; set; }
    public int Rank { get; set; } // For the calculated rank
}

class Program
{
    // Make connectionString static if you want to use it across static methods
    private static string _connectionString = "Data Source=.;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;"; // Moved to class level

    static void Main(string[] args)
    {
        // --- IMPORTANT: CONFIGURE YOUR SQL SERVER CONNECTION STRING ---
        // You can find your server name in SSMS (when you connect or by running SELECT @@SERVERNAME)
        // Common examples:
        // "Data Source=.;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;" (for default local instance)
        // "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;" (for localdb)
        // "Data Source=YOUR_MACHINE_NAME;Initial Catalog=ProductDB;Integrated Security=True;Encrypt=False;"
        // The connection string is now defined at the class level as a static field.
        // No need to redeclare it here unless you want to make it mutable per run.

        // --- SQL Queries for different ranking functions ---
        string rowNumberQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    ROW_NUMBER() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";

        string rankQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";

        string denseRankQuery = @"
            SELECT
                ProductID,
                ProductName,
                Category,
                Price,
                CalculatedRank
            FROM (
                SELECT
                    ProductID,
                    ProductName,
                    Category,
                    Price,
                    DENSE_RANK() OVER (PARTITION BY Category ORDER BY Price DESC) AS CalculatedRank
                FROM
                    Products
            ) AS RankedProducts
            WHERE CalculatedRank <= 3
            ORDER BY Category, CalculatedRank;";


        // --- Execute each ranking query ---
        ExecuteAndPrintResults("ROW_NUMBER()", rowNumberQuery);
        ExecuteAndPrintResults("RANK()", rankQuery);
        ExecuteAndPrintResults("DENSE_RANK()", denseRankQuery);

        Console.WriteLine("\n--- All ranking queries executed. Press any key to exit. ---");
        Console.ReadKey();
    } // This is the closing brace for the Main method

    // --- Function to execute a query and print results ---
    // This method MUST be outside the Main method but inside the Program class.
    static void ExecuteAndPrintResults(string queryName, string sqlQuery)
    {
        List<Product> products = new List<Product>(); // Local list for each call
        Console.WriteLine($"\n--- {queryName} (Top 3 Most Expensive Products per Category) ---");

        try
        {
            // Use the static _connectionString field
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Console.WriteLine($"Connected to SQL Server for {queryName}."); // Uncomment for debugging connection

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Rank = (int)reader.GetInt64(reader.GetOrdinal("CalculatedRank"))
                            });
                        }
                    }
                }
            }

            // Print the results from the list
            if (products.Count > 0)
            {
                Console.WriteLine("{0,-12} {1,-28} {2,-18} {3,-12} {4,-8}", "Product ID", "Product Name", "Category", "Price", "Rank");
                Console.WriteLine("----------------------------------------------------------------------------------");
                foreach (var product in products)
                {
                    Console.WriteLine("{0,-12} {1,-28} {2,-18} {3,-12:C} {4,-8}",
                        product.ProductID, product.ProductName, product.Category, product.Price, product.Rank);
                }
            }
            else
            {
                Console.WriteLine("No products found or query returned no results.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Error during {queryName}: {ex.Message}");
            Console.WriteLine($"Error Code: {ex.Number}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred during {queryName}: {ex.Message}");
        }
    } // This is the closing brace for the ExecuteAndPrintResults method
} // This is the closing brace for the Program class