namespace vendeglo_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    //ez a migr�ci�s l�p�s �res, mert csak a reverse nav prop-t csin�ltuk meg, ami nem okozza az adatb�zis strukt�ra v�ltoz�s�t
    public partial class reversenavigationpropertyCategoryMenuItems : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
        }
    }
}
