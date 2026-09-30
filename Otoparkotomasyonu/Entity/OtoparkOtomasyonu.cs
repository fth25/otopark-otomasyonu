using System;
using System.Data.Entity;

namespace Otoparkotomasyonu.Entity
{
    public class OtoparkOtomasyonu : DbContext
    {
        // Your context has been configured to use a 'OtoparkOtomasyonu' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Otoparkotomasyonu.Entity.OtoparkOtomasyonu' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'OtoparkOtomasyonu' 
        // connection string in the application configuration file.
        public OtoparkOtomasyonu() : base("name=Arac")
        {
            Database.SetInitializer<OtoparkOtomasyonu>(null);
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Arac> Araclar { get; set; }
    }

    public class Arac
    {
        public int Id { get; set; }
        public string Aracsahibi { get; set; }
        public string Telefon { get; set; }
        public DateTime tarih { get; set; }
        public string Plaka { get; set; }
        public double Aracgiris { get; set; }
        public DateTime? CikisZamani { get; set; }
        public double? Tutar { get; set; }
    }
}