using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Account
    {

        public int Id { get; set; }

        public String type { get; set; }

        public String name { get; set; }

        public String currency { get; set; }

        public int balance { get; set; }

        public int? UserId { get; set; }

        public DateTime creationDate { get; set; }

        public Account(int a, String b, String c, String d, int e, int f)
        {
            Id = a;
            type = b;
            name = c;
            currency = d;
            balance = e;
            UserId = f;

        }

        public Account()
        {

        }

    }
}