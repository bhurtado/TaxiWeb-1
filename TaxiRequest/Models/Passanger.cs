using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiRequest.Models
{
    public class Passanger
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Latitude { get; set; }
        public decimal longitude { get; set; }
    }
}