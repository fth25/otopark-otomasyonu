namespace Otoparkotomasyonu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OtoparkGuncelleme : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Aracs", "Telefon", c => c.String());
            AlterColumn("dbo.Aracs", "tarih", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Aracs", "tarih", c => c.Int(nullable: false));
            AlterColumn("dbo.Aracs", "Telefon", c => c.Int(nullable: false));
        }
    }
}
