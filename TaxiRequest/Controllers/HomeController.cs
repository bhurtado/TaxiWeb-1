using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TaxiRequest.Models;
namespace TaxiRequest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string Name, string Phone,string latitude,string longitude)
        {
            if (Name != null && Phone != null)
            {
                Passanger passanger = new Passanger();
                passanger.Name = Name;
                passanger.Phone = Phone;
                passanger.Latitude = decimal.Parse(latitude);
                passanger.longitude =decimal.Parse(longitude);
                passanger.OrderStatusId = (int)OrderStatus.New;
                if(SetData(passanger))
                {
                    return View("OrderPage");
                }
            }
               

            return View(); 
        }

        private bool SetData(Passanger passanger)
        {
          

                using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = @"Data Source=localhost; Initial Catalog=taxiData; Integrated Security=True";
                conn.Open();
              
                        string AddNewOrder = $"Insert into Orders Values({passanger.Latitude},{passanger.longitude},'{passanger.Name}','{passanger.Phone}',{passanger.OrderStatusId},'{DateTime.Now}')";
                        using (SqlCommand AddClientcommand = new SqlCommand(AddNewOrder))
                        {
                            AddClientcommand.Connection = conn;
                    if (AddClientcommand.ExecuteNonQuery() > 0)
                    {
                        return true;
                   
                    }
                    else return false;
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