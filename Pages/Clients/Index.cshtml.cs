using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        //we need to fill this list in the onget method


        //it get executed when we access to the page using the http get method. 
        //in this method we need to access to the database and read the data from the clients table
        public void OnGet()
        {
            try
            { //in the try we need to connect to the database
                String connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";

                //Connecting to SQL (import using System.Data.SqlClient library top)
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // create a sql query that allows us to read data form the clients table
                    String sql = "SELECT * FROM clients";

                    //create a sql command which will allows to execute the sql query
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        //execute this command to obtain the sql data reader
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            //using while loop we can read the data form the table 
                            while (reader.Read())
                            {
                                //save the data into clientinfo object
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.created_at = reader.GetDateTime(5).ToString();
                                
                                // adding the clientInfo object to our list 
                                listClients.Add(clientInfo);
                            }
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }

        }
        //we have to create a class called client info that will store the data of only one client
        //from the database

         public class ClientInfo
        { //this class will hold only one client info 
          //to store the all the clients data we need to create a list 

            public string id;
            public string name;
            public string email;
            public string phone;
            public string address;
            public string created_at;
        }



    }
}





