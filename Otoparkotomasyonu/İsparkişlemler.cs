using Otoparkotomasyonu.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Otoparkotomasyonu
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        OtoparkOtomasyonu baglan = new OtoparkOtomasyonu();
        string sonHesaplananPlaka = ""; // en üste, baglan'ın yanına ekle
        private void button1_Click(object sender, EventArgs e)
        {
            int mevcutAracSayisi = baglan.Araclar.Count(x => x.CikisZamani == null);
            if (mevcutAracSayisi >= 8)
            {
                MessageBox.Show("Otopark tamamen doludur! Maksimum 8 araç sınırına ulaşıldı.", "Kapasite Dolu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string plaka = textBox4.Text.Trim().ToUpper();
            Regex plakaRegex = new Regex(@"^(0[1-9]|[1-7][0-9]|8[0-1])[A-Z]{1,3}[0-9]{1,4}$");
            if (!plakaRegex.IsMatch(plaka))
            {
                MessageBox.Show("Geçersiz plaka formatı! Örnek: 34EBT591", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Arac yeniArac = new Arac();
            yeniArac.Aracsahibi = textBox1.Text;
            yeniArac.Telefon = maskedTextBox1.Text;
            yeniArac.tarih = Convert.ToDateTime(maskedTextBox2.Text);
            yeniArac.Plaka = plaka;
            string[] saatParts = maskedTextBox3.Text.Split(':');
            yeniArac.Aracgiris = Convert.ToInt32(saatParts[0]) + Convert.ToInt32(saatParts[1]) / 60.0;

            baglan.Araclar.Add(yeniArac);
            baglan.SaveChanges();

            // ComboBox'ları güncelle
            baglan = new OtoparkOtomasyonu();
            var tumListe = baglan.Araclar.ToList();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach (Arac a in tumListe.Where(x => x.CikisZamani == null))
            {
                if (a.Aracsahibi != null) comboBox1.Items.Add(a.Aracsahibi);
                if (a.Plaka != null) comboBox2.Items.Add(a.Plaka);
            }

            MessageBox.Show("Araç başarıyla kaydedildi!");
        }
        private void YenileVeDoldur()
        {
            baglan = new OtoparkOtomasyonu();
            var tumListe = baglan.Araclar.ToList();

            var aktifListe = tumListe.Where(x => x.CikisZamani == null)
                .Select(x => new {
                    x.Id,
                    x.Aracsahibi,
                    x.Telefon,
                    x.tarih,
                    x.Plaka,
                    Aracgiris = TimeSpan.FromHours(x.Aracgiris).ToString(@"hh\:mm")
                }).ToList();
            dataGridView1.DataSource = aktifListe;

            var cikisListe = tumListe
                .Where(x => x.CikisZamani != null && x.Tutar != null)
                .Select(x => new {
                    x.Aracsahibi,
                    x.Plaka,
                    CikisZamani = x.CikisZamani.Value.ToString("dd.MM.yyyy HH:mm"),
                    Tutar = Math.Round(x.Tutar.Value, 2).ToString("0.00") + " TL"
                }).ToList();
            dataGridView2.DataSource = cikisListe;

            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach (Arac a in tumListe.Where(x => x.CikisZamani == null))
            {
                if (a.Aracsahibi != null) comboBox1.Items.Add(a.Aracsahibi);
                if (a.Plaka != null) comboBox2.Items.Add(a.Plaka);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            baglan = new OtoparkOtomasyonu();
            var aktifListe = baglan.Araclar
                .Where(x => x.CikisZamani == null)
                .ToList()
                .Select(x => new {
                    x.Id,
                    x.Aracsahibi,
                    x.Telefon,
                    x.tarih,
                    x.Plaka,
                    Aracgiris = TimeSpan.FromHours(x.Aracgiris).ToString(@"hh\:mm")
                }).ToList();

            dataGridView1.DataSource = aktifListe;
            dataGridView1.Visible = true;
            dataGridView2.Visible = false;
        }

        int arananId;
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            int arananId = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            Arac bulunan = baglan.Araclar.Where(x => x.Id == arananId).FirstOrDefault();
            if (bulunan != null)
            {
                string plaka = textBox4.Text.Trim().ToUpper();
                Regex plakaRegex = new Regex(@"^(0[1-9]|[1-7][0-9]|8[0-1])[A-Z]{1,3}[0-9]{1,4}$");
                if (!plakaRegex.IsMatch(plaka))
                {
                    MessageBox.Show("Geçersiz plaka formatı! Örnek: 34EBT591", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bulunan.Aracsahibi = textBox1.Text;
                maskedTextBox1.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
                bulunan.Telefon = maskedTextBox1.Text;
                bulunan.tarih = Convert.ToDateTime(maskedTextBox2.Text);
                bulunan.Plaka = plaka;
                string[] saatParts = maskedTextBox3.Text.Split(':');
                bulunan.Aracgiris = Convert.ToInt32(saatParts[0]) + Convert.ToInt32(saatParts[1]) / 60.0;
                baglan.SaveChanges();
                YenileVeDoldur();
                MessageBox.Show("Araç bilgileri başarıyla güncellendi!");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Visible = false;
            dataGridView2.DataSource = null;
            dataGridView2.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {

            using (OtoparkOtomasyonu dene = new OtoparkOtomasyonu())
            {
                if (dene.Database == null)
                {
                    dene.Database.Create();
                    MessageBox.Show("Veritabanı oluşturuldu", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Veritabanı mevcut, bağlandınız.");
                }
            }
            button5.Enabled = false;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            baglan.Database.CreateIfNotExists();
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            YenileVeDoldur();

        }

        private void button6_Click(object sender, EventArgs e) // Ücret Hesapla
        {
            if (comboBox2.SelectedItem == null || comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Lütfen hem araç sahibini hem de plakayı seçin!");
                return;
            }

            string secilenPlaka = comboBox2.SelectedItem.ToString();
            string secilenSahip = comboBox1.SelectedItem.ToString();

            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                // Plaka + araç sahibi AYNI kayda ait olmalı
                Arac cikisYapacakArac = db.Araclar.FirstOrDefault(x =>
                    x.Plaka == secilenPlaka &&
                    x.Aracsahibi == secilenSahip &&
                    x.CikisZamani == null);

                if (cikisYapacakArac == null)
                {
                    MessageBox.Show("Seçilen plaka ile araç sahibi eşleşmiyor! Çıkış yapılamaz.",
                        "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    double girisSaat = cikisYapacakArac.Aracgiris;
                    string[] cikisParts = maskedTextBox4.Text.Split(':');
                    double cikisSaat = Convert.ToInt32(cikisParts[0]) + Convert.ToInt32(cikisParts[1]) / 60.0;

                    double toplamSaat = cikisSaat - girisSaat;
                    if (toplamSaat <= 0)
                    {
                        MessageBox.Show("Çıkış saati, giriş saatinden küçük veya eşit olamaz!");
                        return;
                    }

                    double saatlikUcret = 25.0;
                    double toplamTutar = Math.Round(toplamSaat * saatlikUcret, 2);

                    sonHesaplananPlaka = secilenPlaka;
                    textBox6.Text = toplamTutar.ToString("0.00") + " TL";

                    // Datagrid'e özet göster
                    var ozet = new[]
                    {
            new {
                AracSahibi = cikisYapacakArac.Aracsahibi,
                Plaka = secilenPlaka,
                GirisSaati = TimeSpan.FromHours(girisSaat).ToString(@"hh\:mm"),
                CikisSaati = TimeSpan.FromHours(cikisSaat).ToString(@"hh\:mm"),
                ToplamSaat = Math.Round(toplamSaat, 2).ToString("0.00") + " sa",
                Tutar = toplamTutar.ToString("0.00") + " TL"
            }
        };

                    dataGridView2.DataSource = ozet.ToList();
                    dataGridView2.Visible = true;
                }
                catch (Exception)
                {
                    MessageBox.Show("Lütfen çıkış saatini doğru formatta giriniz! (Örn: 15:00)");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e) // Öde
        {
            if (string.IsNullOrEmpty(sonHesaplananPlaka))
            {
                MessageBox.Show("Lütfen önce ücret hesaplaması yapın!");
                return;
            }

            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                Arac cikisArac = db.Araclar.FirstOrDefault(x => x.Plaka == sonHesaplananPlaka && x.CikisZamani == null);
                if (cikisArac == null)
                {
                    MessageBox.Show("Lütfen önce ücret hesaplaması yapın!");
                    return;
                }

                string[] cikisParts = maskedTextBox4.Text.Split(':');
                double cikisSaat = Convert.ToInt32(cikisParts[0]) + Convert.ToInt32(cikisParts[1]) / 60.0;
                double toplamSaat = cikisSaat - cikisArac.Aracgiris;
                double toplamTutar = Math.Round(toplamSaat * 25.0, 2);

                cikisArac.CikisZamani = DateTime.Today.AddHours(cikisSaat);
                cikisArac.Tutar = toplamTutar;
                db.SaveChanges();

                MessageBox.Show("Ödeme başarıyla alındı, aracın otoparktan çıkışı yapıldı!", "Çıkış Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                sonHesaplananPlaka = "";
                textBox6.Clear();
                YenileVeDoldur();
            }
        }

       

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox4.Text = "";
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();

        }

        private void button9_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            maskedTextBox4.Clear();
            textBox6.Text = "";
            sonHesaplananPlaka = "";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Lütfen plaka giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string arananPlaka = textBox2.Text.Trim().ToUpper();

            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var bulunanArac = db.Araclar
                    .Where(x => x.Plaka == arananPlaka)
                    .ToList()
                    .Select(x => new {
                        x.Id,
                        x.Aracsahibi,
                        x.Telefon,
                        Tarih = x.tarih.ToString("dd.MM.yyyy"),
                        x.Plaka,
                        Aracgiris = TimeSpan.FromHours(x.Aracgiris).ToString(@"hh\:mm"),
                        CikisZamani = x.CikisZamani.HasValue ? x.CikisZamani.Value.ToString("dd.MM.yyyy HH:mm") : "Hâlâ parkta",
                        Tutar = x.Tutar.HasValue ? Math.Round(x.Tutar.Value, 2).ToString("0.00") + " TL" : "-"
                    }).ToList();

                if (bulunanArac.Count == 0)
                {
                    MessageBox.Show("Bu plakaya ait araç bulunamadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dataGridView3.DataSource = null;
                    return;
                }

                dataGridView3.DataSource = bulunanArac;
                dataGridView3.Visible = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {

            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var cikisListe = db.Araclar
                    .Where(x => x.CikisZamani != null && x.Tutar != null)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Araç Sahibi\tPlaka\tÇıkış Zamanı\tTutar");
                foreach (var a in cikisListe)
                {
                    sb.AppendLine($"{a.Aracsahibi}\t{a.Plaka}\t{a.CikisZamani.Value:dd.MM.yyyy HH:mm}\t{Math.Round(a.Tutar.Value, 2):0.00} TL");
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Dosyası|*.xls";
                sfd.FileName = "CikisYapanAraclar";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Dosya kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var aktifListe = db.Araclar
                    .Where(x => x.CikisZamani == null)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Id\tAraç Sahibi\tTelefon\tTarih\tPlaka\tGiriş Saati");
                foreach (var a in aktifListe)
                {
                    sb.AppendLine($"{a.Id}\t{a.Aracsahibi}\t{a.Telefon}\t{a.tarih:dd.MM.yyyy}\t{a.Plaka}\t{TimeSpan.FromHours(a.Aracgiris):hh\\:mm}");
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Dosyası|*.xls";
                sfd.FileName = "ParkHalindekiAraclar";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Dosya kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var bugun = DateTime.Today;
                var gunlukListe = db.Araclar
                    .Where(x => x.CikisZamani != null && x.Tutar != null)
                    .ToList()
                    .Where(x => x.CikisZamani.Value.Date == bugun)
                    .ToList();

                double toplamCiro = gunlukListe.Sum(x => x.Tutar.Value);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"========== GÜNLÜK CİRO RAPORU ==========");
                sb.AppendLine($"Tarih: {bugun:dd.MM.yyyy}");
                sb.AppendLine();
                sb.AppendLine("Araç Sahibi | Plaka | Çıkış Saati | Tutar");
                sb.AppendLine("------------------------------------------");
                foreach (var a in gunlukListe)
                {
                    sb.AppendLine($"{a.Aracsahibi} | {a.Plaka} | {a.CikisZamani.Value:HH:mm} | {Math.Round(a.Tutar.Value, 2):0.00} TL");
                }
                sb.AppendLine("------------------------------------------");
                sb.AppendLine($"TOPLAM CİRO: {Math.Round(toplamCiro, 2):0.00} TL");

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Metin Dosyası|*.txt";
                sfd.FileName = $"GunlukCiro_{bugun:dd_MM_yyyy}";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Günlük ciro raporu kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            button11_Click(sender, e);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var eskiListe = db.Araclar
                    .ToList()
                    .Select(x => new {
                        x.Id,
                        x.Aracsahibi,
                        x.Telefon,
                        Tarih = x.tarih.ToString("dd.MM.yyyy"),
                        x.Plaka,
                        Aracgiris = TimeSpan.FromHours(x.Aracgiris).ToString(@"hh\:mm"),
                        CikisZamani = x.CikisZamani.HasValue ? x.CikisZamani.Value.ToString("dd.MM.yyyy HH:mm") : "Hâlâ parkta",
                        Tutar = x.Tutar.HasValue ? Math.Round(x.Tutar.Value, 2).ToString("0.00") + " TL" : "-"
                    }).ToList();

                dataGridView1.DataSource = eskiListe;
                dataGridView1.Visible = true;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            using (OtoparkOtomasyonu db = new OtoparkOtomasyonu())
            {
                var cikisListe = db.Araclar
                    .Where(x => x.CikisZamani != null && x.Tutar != null)
                    .ToList()
                    .Select(x => new {
                        x.Aracsahibi,
                        x.Plaka,
                        CikisZamani = x.CikisZamani.Value.ToString("dd.MM.yyyy HH:mm"),
                        Tutar = Math.Round(x.Tutar.Value, 2).ToString("0.00") + " TL"
                    }).ToList();

                dataGridView2.DataSource = cikisListe;
                dataGridView2.Visible = true;
            }
        }
    }
}
    

