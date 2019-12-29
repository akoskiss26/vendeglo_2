using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace vendeglo_2.Models
{
    public class MenuItem
    {
        public int Id { get; set; }   //ez lesz a PK, kötelező, mivel int és Id nevű, ebből automatikusan PK lesz
        /// <summary>
        /// Name mezőt kötelező kitölteni (System.ComponentModel.DataAnnotations névteret betölteni)
        /// hogy később indexelni lehessen, nem lehet korlátlan hosszúságú, ezért korlátozzuk 200 karakterre
        /// </summary>
        [Required] 
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        /// <summary>
        /// Ez az osztály az adott menüelem kategóriáját (előétel, főétel)
        /// tartalmazza
        /// Navigációs property: egy másik táblában levő adatot tölt be
        /// </summary>
        public Category Category { get; set; }
    }


}