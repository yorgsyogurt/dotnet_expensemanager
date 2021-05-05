using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Debt
    {

        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? YourId { get; set; }

        public int iOwe { get; set; }

        public int youOwe { get; set; }

    }
}