using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartApartmentDataCore.Models
{
    public class PropertiesViewModel:PageModel
    {       
            public int propertyID { get; set; }
            public string name { get; set; }
            public string formerName { get; set; }
            public string streetAddress { get; set; }
            public string city { get; set; }
            public string market { get; set; }
            public string state { get; set; }
            public float lat { get; set; }
            public float lng { get; set; }
            public int mgmtID { get; set; }
       
    }
}
