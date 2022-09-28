using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static MyStore.Pages.Clients.IndexModel;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients

{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
      
        public string errorMessage = "";
        public string successMessage = "";


        public void OnGet()
        {
            //in this method we have to read the id of the client and we need to fill the 
            //clientInfo object which will be displayed into the page
            //so read the id we havbe to provide the name of the parameter which is the id
            String id = Request.Query["id"];

            try
            {
                //connect to the database and fill the clientinfo object
                String connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";

                //Connecting to SQL (import using System.Data.SqlClient library top)
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //ClientInfo clientInfo = new ClientInfo();    
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.name = reader.GetString(1);
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

            }
        }

        public void OnPost()
        {

            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            //Console.WriteLine("this is bs" + clientInfo.name);

            if (clientInfo.id.Length == 0 || clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
                clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
             {
               errorMessage = "All the fields are required";
                return;
            }
            try
            {
                String connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients " +
                        "SET name=@name, email=@email, phone=@phone, address=@address" +
                        " WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);

                        command.ExecuteNonQuery();

                    }
                }



            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");

        }
    }
}
