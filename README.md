\# 🚗 Otopark Otomasyonu



C# Windows Forms + Entity Framework 6 ile yazılmış basit otopark yönetim sistemi.



\## Özellikler

\- Araç giriş/çıkış kaydı (maks. 8 araç kapasite)

\- Ücret hesaplama (25 TL/saat)

\- Plaka formatı doğrulama

\- Kullanıcı girişi (demo: ADMIN / 1234)

\- SQL Server üzerinde Entity Framework Code First



\## Kurulum

1\. SQL Server'da `OtoparkDB` veritabanı oluştur

2\. `App.config.example` → `App.config` olarak kopyala

3\. Connection string'i kendi SQL Server'ına göre düzenle

4\. NuGet: `EntityFramework 6.5.2` yüklü olmalı

5\. Package Manager Console'da: `Update-Database`

6\. F5 ile çalıştır



\## Kullanım

\- Giriş: `ADMIN` / `1234`

\- Araç ekle → plaka (örn: 34ABC123), telefon, tarih, giriş saati

\- Çıkış: Araç sahibi + plaka + çıkış saati seç → Hesapla → Öde



\## Teknolojiler

\- C# .NET Framework 4.7.2

\- Windows Forms

\- Entity Framework 6.5.2 (Code First + Migrations)

\- SQL Server



\## Not

Eğitim amaçlı geliştirilmiş demo projedir.

