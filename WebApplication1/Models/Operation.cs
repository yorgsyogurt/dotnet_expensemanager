using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Operation
    {
        public int Id { get; set; }

        public String name { get; set; }

        public int? fromAccountId { get; set; }

        public int? toAccountId { get; set; }

        public int? UserId { get; set; }

        public int amount { get; set; }

        public DateTime creationDate { get; set; }

        public String type { get; set; }
    }
}