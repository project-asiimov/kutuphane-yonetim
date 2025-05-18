using System;
using System.Collections.Generic;
using System.Text;

namespace KutuphaneYonetimSistemiApp
{
    public class Kitap
    {
        public int KitapId { get; set; }
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public bool Durum { get; set; }

        public Kitap(int kitapId, string ad, string yazar)
        {
            KitapId = kitapId;
            Ad = ad;
            Yazar = yazar;
            Durum = true;
        }

        public void DurumGuncelle(bool yeniDurum)
        {
            Durum = yeniDurum;
        }

        public override string ToString()
        {
            return $"Kitap ID: {KitapId}, Ad: {Ad}, Yazar: {Yazar}, Durum: {(Durum ? "Müsait" : "Ödünçte")}";
        }
    }
    public class Uye
    {
        public int UyeId { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }

        public Uye(int uyeId, string ad, string soyad)
        {
            UyeId = uyeId;
            Ad = ad;
            Soyad = soyad;
        }

        public override string ToString()
        {
            return $"Üye ID: {UyeId}, Ad: {Ad}, Soyad: {Soyad}";
        }
    }
    public class Odunc
    {
        public int OduncId { get; set; }
        public Uye OduncAlan { get; set; }
        public Kitap OduncAlinanKitap { get; set; }
        public DateTime OduncAlimTarihi { get; set; }
        public DateTime? IadeTarihi { get; set; }

        public Odunc(int oduncId, Uye uye, Kitap kitap)
        {
            OduncId = oduncId;
            OduncAlan = uye;
            OduncAlinanKitap = kitap;
            OduncAlimTarihi = DateTime.Now;
            IadeTarihi = null;
        }

        public override string ToString()
        {
            string durum = IadeTarihi == null ? "Aktif" : $"İade Edildi ({IadeTarihi.Value})";
            return $"Ödünç ID: {OduncId}, {OduncAlan}, {OduncAlinanKitap}, Durum: {durum}";
        }
    }
    public class KutuphaneYonetimSistemi
    {
        public List<Kitap> KitapListesi { get; set; }
        public List<Uye> UyeListesi { get; set; }
        public List<Odunc> OduncListesi { get; set; }
        private int nextOduncId = 1;

        public KutuphaneYonetimSistemi()
        {
            KitapListesi = new List<Kitap>();
            UyeListesi = new List<Uye>();
            OduncListesi = new List<Odunc>();
        }
        public void OduncAl(int uyeId, int kitapId)
        {
            Uye uye = UyeListesi.Find(u => u.UyeId == uyeId);
            if (uye == null)
            {
                Console.WriteLine("Üye bulunamadı.");
                return;
            }

            Kitap kitap = KitapListesi.Find(k => k.KitapId == kitapId);
            if (kitap == null)
            {
                Console.WriteLine("Kitap bulunamadı.");
                return;
            }
            if (!kitap.Durum)
            {
                Console.WriteLine("Bu kitap şu anda ödünçte.");
                return;
            }

            Odunc odunc = new Odunc(nextOduncId++, uye, kitap);
            OduncListesi.Add(odunc);
            kitap.DurumGuncelle(false);

            Console.WriteLine("Ödünç alma işlemi başarılı:");
            Console.WriteLine(odunc);
        }
        public void IadeEt(int oduncId)
        {
            Odunc odunc = OduncListesi.Find(o => o.OduncId == oduncId);
            if (odunc == null)
            {
                Console.WriteLine("Ödünç işlemi bulunamadı.");
                return;
            }
            if (odunc.IadeTarihi != null)
            {
                Console.WriteLine("Bu ödünç işlemi zaten iade edilmiş.");
                return;
            }

            odunc.IadeTarihi = DateTime.Now;
            odunc.OduncAlinanKitap.DurumGuncelle(true);

            Console.WriteLine("Kitap iade edildi:");
            Console.WriteLine(odunc);
        }
        public void OduncBilgisi()
        {
            if (OduncListesi.Count == 0)
            {
                Console.WriteLine("Ödünç işlemleri geçmişi boş.");
                return;
            }

            foreach (var odunc in OduncListesi)
            {
                Console.WriteLine(odunc);
            }
        }
        public void KitapEkle(Kitap kitap)
        {
            KitapListesi.Add(kitap);
        }
        public void UyeEkle(Uye uye)
        {
            UyeListesi.Add(uye);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            PerformLogin();
            KutuphaneYonetimSistemi sistem = new KutuphaneYonetimSistemi();

            sistem.KitapEkle(new Kitap(1, "Suç ve Ceza", "Dostoyevski"));
            sistem.KitapEkle(new Kitap(2, "1984", "George Orwell"));
            sistem.KitapEkle(new Kitap(3, "Kürk Mantolu Madonna", "Sabahattin Ali"));
            sistem.KitapEkle(new Kitap(4, "Araba Sevdası", "Ahmet Doğruyol"));
            sistem.KitapEkle(new Kitap(5, "Bir Delinin Anı Defteri", "Nikolay Gogol"));
            sistem.KitapEkle(new Kitap(6, "Küçük Prens", "Eylül Küpçük"));

            sistem.UyeEkle(new Uye(1, "Ahmet", "Doğruyol"));
            sistem.UyeEkle(new Uye(2, "Aylin", "Bozkurt"));
            sistem.UyeEkle(new Uye(3, "Batuhan", "Acar"));
            sistem.UyeEkle(new Uye(4, "Yağız", "Çelik"));

            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n--- Kütüphane Yönetim Sistemi ---");
                Console.WriteLine("1. Kitap listesini görüntüle");
                Console.WriteLine("2. Üye listesini görüntüle");
                Console.WriteLine("3. Ödünç alma işlemi yap");
                Console.WriteLine("4. Kitap iade et");
                Console.WriteLine("5. Tüm ödünç işlemleri geçmişini görüntüle");
                Console.WriteLine("6. Çıkış");
                Console.Write("Seçiminiz: ");
                string secim = Console.ReadLine();
                Console.WriteLine();

                switch (secim)
                {
                    case "1":
                        Console.WriteLine("Kitap Listesi:");
                        foreach (var kitap in sistem.KitapListesi)
                        {
                            Console.WriteLine(kitap);
                        }
                        break;
                    case "2":
                        Console.WriteLine("Üye Listesi:");
                        foreach (var uye in sistem.UyeListesi)
                        {
                            Console.WriteLine(uye);
                        }
                        break;
                    case "3":
                        Console.Write("Ödünç alma işlemi için Üye ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int uyeId))
                        {
                            Console.WriteLine("Geçersiz Üye ID.");
                            break;
                        }
                        Console.Write("Ödünç alınacak Kitap ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int kitapId))
                        {
                            Console.WriteLine("Geçersiz Kitap ID.");
                            break;
                        }
                        sistem.OduncAl(uyeId, kitapId);
                        break;
                    case "4":
                        Console.Write("İade edilecek Ödünç ID giriniz: ");
                        if (!int.TryParse(Console.ReadLine(), out int oduncId))
                        {
                            Console.WriteLine("Geçersiz Ödünç ID.");
                            break;
                        }
                        sistem.IadeEt(oduncId);
                        break;
                    case "5":
                        Console.WriteLine("Ödünç İşlemleri Geçmişi:");
                        sistem.OduncBilgisi();
                        break;
                    case "6":
                        Console.WriteLine("Sistemden çıkılıyor...");
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }

                Console.WriteLine("\nDevam etmek için bir tuşa basınız...");
                Console.ReadKey();
            }
        }
        static void PerformLogin()
        {
            int maxAttempts = 3;
            int attempt = 0;
            bool isAuthenticated = false;

            while (attempt < maxAttempts && !isAuthenticated)
            {
                Console.Clear();
                WriteHeader();

                Console.Write("Kullanıcı Adı: ");
                string username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Kullanıcı adı boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                Console.Write("Parola: ");
                string password = ReadPassword();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Parola boş olamaz. Tekrar deneyiniz.\n");
                    attempt++;
                    Console.WriteLine("Devam etmek için bir tuşa basınız...");
                    Console.ReadKey();
                    continue;
                }

                if (username.Equals("supme", StringComparison.OrdinalIgnoreCase) && password == "supme123")
                {
                    isAuthenticated = true;
                    Console.WriteLine("\nYönetici olarak giriş başarılı.");
                    Console.WriteLine("\nGiriş işlemi tamamlandı. Ana menüye yönlendiriliyorsunuz...");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("\nGiriş bilgileri hatalı. Lütfen tekrar deneyiniz.\n");
                    attempt++;
                    if (attempt < maxAttempts)
                    {
                        Console.WriteLine($"Kalan deneme hakkınız: {maxAttempts - attempt}\n");
                        Console.WriteLine("Devam etmek için bir tuşa basınız...");
                        Console.ReadKey();
                    }
                }
            }

            if (!isAuthenticated)
            {
                Console.WriteLine("Çok fazla hatalı giriş denemesi. Program sonlandırılıyor.");
                Environment.Exit(0);
            }
        }
        static void WriteHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("***********************************************");
            Console.WriteLine("         Kütüphane Yönetim Sistemi           ");
            Console.WriteLine("***********************************************\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password.Remove(password.Length - 1, 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    Console.Write("*");
                    password.Append(key.KeyChar);
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }
}