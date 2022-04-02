
using ChoETL;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data.SqlClient;
using System.Text;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        
       
        [HttpGet(Name = "GetWeatherForecast")]
        public string Get(string? table, int? id)
        {
            
            try
            {
                int i = 0;
                string sql;
                //string[] a1 = new string[5];
                ArrayList a1 = new ArrayList();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.ConnectionString = "Data Source = (localdb)\\mssqllocaldb; Initial Catalog = BrugereItems; Integrated Security = True; Pooling = False";
                //DataContext db = new DataContext(Microsoft.Extensions..ge);

                //builder.DataSource = "(localdb)\\mssqllocaldb";
                builder.DataSource = "azuresamlet.database.windows.net";
                builder.UserID = "flowtek333";
                builder.Password = "Lowbob123";
                builder.InitialCatalog = "customersSample";
                StringBuilder sb = new StringBuilder();



                Console.WriteLine(builder.ConnectionString);

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();


                    if (id == null)
                    {
                        sql = "SELECT * FROM " + table + " bruger";
                    }
                    else
                    {
                        sql = "SELECT * FROM " + table + " WHERE Brugerid = " + id + "FOR JSON PATH, ROOT('Orders')";
                    }

                    var comm = new SqlCommand(sql, connection);

                    using (var parser = new ChoJSONWriter(sb))
                    parser.Write(comm.ExecuteReader());


                   
                }
                return sb.ToString();
            }
            catch (SqlException e)
            {
                return null;
            }
            Console.WriteLine("\nDone. Press enter.");
            Console.ReadLine();
        }
        [HttpPost(Name = "PostWeatherForecast")]
        public void addBruger(string? name)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //builder.ConnectionString = "Data Source = (localdb)\\mssqllocaldb; Initial Catalog = BrugereItems; Integrated Security = True; Pooling = False";
            //DataContext db = new DataContext(Microsoft.Extensions..ge);
            builder.DataSource = "(localdb)\\mssqllocaldb";
            builder.InitialCatalog = "BrugereItems";

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Brugeres (Name)";
                query += " VALUES (@Name)";

                SqlCommand myCommand = new SqlCommand(query, connection);
                myCommand.Parameters.AddWithValue("@Name", name);
                // ... other parameters
                myCommand.ExecuteNonQuery();
            }
        }
    }
}