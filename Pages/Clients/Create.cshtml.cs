using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static MyStore.Pages.Clients.IndexModel;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        //create a new variable of type clientinfo
        //public ClientInfo clientInfo = new ClientInfo();
        public ClientInfo clientInfo = new ClientInfo();


        //global variable that will save the error message
        public String errorMessage = "";

        //global variable that will allows to return the success message
        public String successMessage = "";
        public void OnGet()
        {
        }

        //in this method we can read the data of the form and we can put this data
        //into clientInfo object
        public void OnPost()
        {
            //we can fill the name from the request
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 || 
                clientInfo.phone.Length == 0|| clientInfo.address.Length == 0)
            {
                errorMessage= "All the fields are required";
                return;
            }
            //save the data into database
            try
            {
                String connectionString = "Data Source=.\\MSSQLSERVER01;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " + "(name, email, phone, address) VALUES " + "(@name, @email, @phone, @address);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name);
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery();


                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = "";
            clientInfo.address = "";
            successMessage = "New Client Added Correctly";

           // Response.Redirect("/Clients/Index");
        }
    }
}
