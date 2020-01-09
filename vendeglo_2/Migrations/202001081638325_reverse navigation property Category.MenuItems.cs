namespace vendeglo_2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    //ez a migrációs lépés üres, mert csak a reverse nav prop-t csináltuk meg, ami nem okozza az adatbázis struktúra változását
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
