using System.Collections.Generic;

namespace vendeglo_2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //újabb navigációs property
        //ide felsoroljuk az adott kategóriába tartozó menuItem-eket
        public List<MenuItem> MenuItems { get; set; }

    }
}