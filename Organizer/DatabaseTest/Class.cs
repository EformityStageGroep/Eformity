using Microsoft.Data.SqlClient;

namespace DatabaseConnection
{
    class InsertDatabaseTest
    {
        public void Insert()
        {
            // Connection string to your SQL Server database
            string connectionString = Organizer.Properties.Resources.ConnectionString;

            // Prompt the user to enter priority
            Console.WriteLine("Enter priority (low/medium/high):");
            string priority = Console.ReadLine();

            try
            {
                // Establish connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // SQL command to insert the priority into the database
                    string sql = "INSERT INTO Priorities (PriorityValue) VALUES (@Priority)";

                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameters to the command to avoid SQL injection
                        command.Parameters.AddWithValue("@Priority", priority);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the insertion was successful
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Priority inserted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Priority insertion failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine(); // Keep the console window open
        }
    }
    class FetchDatabaseTest
    {
        public void Fetch()
        {
            string connectionString = Organizer.Properties.Resources.ConnectionString;

            Console.WriteLine("Enter ID to fetch:");
            int idToFetch = int.Parse(Console.ReadLine());

            try
            {
                // Establish connection to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // SQL command to select data from the database for a specific ID
                    string sql = "SELECT * FROM Priorities WHERE ID = @ID";

                    // Create a SqlCommand object
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Add parameter for ID
                        command.Parameters.AddWithValue("@ID", idToFetch);

                        // Execute the command
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.HasRows)
                            {
                                // Iterate through the rows
                                while (reader.Read())
                                {
                                    // Access data using column names or indices
                                    int id = reader.GetInt32(reader.GetOrdinal("ID"));
                                    string priorityValue = reader.GetString(reader.GetOrdinal("PriorityValue"));

                                    // Process the fetched data, e.g., print it
                                    Console.WriteLine($"ID: {id}, PriorityValue: {priorityValue}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No data found for the specified ID.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
    }
}
