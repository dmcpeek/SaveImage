using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SaveImage
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Server=localhost;Database=paphiopedilum;uid=root;Pwd=password"; // Replace with your MySQL connection string

            Console.Write("Enter the ImageID: ");
            int imageId;
            if (!int.TryParse(Console.ReadLine(), out imageId))
            {
                Console.WriteLine("Invalid ImageID!");
                return;
            }

            // Establish the database connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Create a parameterized SQL query to retrieve the image and filename from the database
                string sql = "SELECT Image, FileName FROM images WHERE ImageID = @ImageID";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ImageID", imageId);

                    using (MySqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            byte[] imageBytes = (byte[])reader["Image"];
                            string fileName = (string)reader["FileName"];

                            Console.Write("Enter the output directory path: ");
                            string outputDirectory = Console.ReadLine();

                            // Combine the output directory and file name to create the full output path
                            string outputPath = Path.Combine(outputDirectory, fileName);

                            // Write the image bytes to the specified output path
                            File.WriteAllBytes(outputPath, imageBytes);

                            Console.WriteLine("Image saved successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Image not found.");
                        }
                    }
                }
            }
        }
    }
}