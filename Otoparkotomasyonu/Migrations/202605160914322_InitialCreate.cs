namespace Otoparkotomasyonu.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aracs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Aracsahibi = c.String(),
                        Telefon = c.Int(nullable: false),
                        tarih = c.Int(nullable: false),
                        Plaka = c.String(),
                        Aracgiris = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Aracs");
        }
    }
}
