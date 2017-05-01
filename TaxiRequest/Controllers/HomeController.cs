using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace TaxiRequest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string Name,string Phone)
        {
            if (Name != null && Phone != null)
              SetData(Name, Phone);
         
            return View();
        }
     
        private bool SetData(string name,string phone)
        {
            string IP = "";

            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            string Country = null;

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            IP = addr[1].ToString();

            //Initializing a new xml document object to begin reading the xml file returned
            XmlDocument doc = new XmlDocument();
            doc.Load("http://www.freegeoip.net/xml");
            XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");
            IP = "" + nodeLstCity[0].InnerText + IP;
            Country = doc.InnerText.ToString();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=localhost; Initial Catalog=taxiData; Integrated Security=True";
                conn.Open();
                string cheqForClient=$"Select * from Client where 'Phone' = '{phone}'";


                using (SqlCommand Cheqcommand = new SqlCommand(cheqForClient))
                {
                    int i = 0;
                    Cheqcommand.Connection = conn;
                    SqlDataReader reader= Cheqcommand.ExecuteReader();
                    while(reader.Read())
                    {
                        i++;
                    }
                    if (i<= 0)
                    {
                        string AddNewClient = $"Insert into Client Values('{IP}','{Country}','{name}','{phone}')";
                        using (SqlCommand AddClientcommand = new SqlCommand(AddNewClient))
                        {
                            AddClientcommand.Connection = conn;
                            AddClientcommand.ExecuteNonQuery();
                        }
                    }
                    string AddOrder = $"Insert into OrderTemp Values($'{IP}','{Country}','{name}','{phone}')";
                    using (SqlCommand AddClientcommand = new SqlCommand(AddOrder))
                    {
                        AddClientcommand.Connection = conn;
                        AddClientcommand.ExecuteNonQuery();

                        return true;
                    }
                }
                
                
                    



            }
     
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}