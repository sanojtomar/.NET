using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Please enter your first name: ");
            string firstName = Console.ReadLine();

            Console.Write("Please enter your last name: ");
            string lastName = Console.ReadLine();

            DoWork doWork = new DoWork();
            IList<string> listItems = doWork.Process(firstName, lastName);
            foreach (var item in listItems)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }

    public class DoWork
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\All Files\SideHustle\Assignments\ABN_AMRO\Implementation2\Data\ApplicationDB.mdf"";Integrated Security=True";
        public IList<string> Process(string firstName, string lastName)
        {
            return CallDatabase(firstName, lastName);
        }

        private IList<string> CallDatabase(string firstName, string lastName)
        {
            IList<string> items = new List<string>();

            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand("GenerateOutput", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@firstName", firstName);
                cmd.Parameters.AddWithValue("@lastName", lastName);
                connection.Open();
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    items.Add(reader["item"].ToString());
                }
                connection.Close();
            }

            return items;
        }
    }

}
