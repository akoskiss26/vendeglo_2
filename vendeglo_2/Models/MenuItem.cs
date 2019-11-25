using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace vendeglo_2.Models
{
    public class MenuItem
    {
        public int Id { get; set; }   //ez lesz a PK
        public string Name { get; set; }
        public string Description { get; set; }
    }
}