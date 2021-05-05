using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Report
    {

        public int Id { get; set; }

        public String Title { get; set; }

        public String Content { get; set; }

        public DateTime fromDate { get; set; }

        public DateTime toDate { get; set; }

        public int? UserId { get; set; }

    }
}