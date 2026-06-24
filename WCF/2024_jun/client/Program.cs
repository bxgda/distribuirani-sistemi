using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCF_Client.ServiceReference1;

namespace WCF_Client
{
    internal class Program
    {
        static void Main()
        {
            var proxy = new SkladisteClient();

            // Zakup 1
            proxy.ZakupiSkladiste(
                new Skladiste
                {
                    IdSkladista = 1,
                    PocetakZakupa = DateTime.Now,
                    KrajZakupa = DateTime.Now.AddMonths(6),
                    Cena = 500,
                },
                new Vlasnik
                {
                    Ime = "Petar",
                    Prezime = "Petrovic",
                    JMBG = "1234567890123",
                }
            );

            // Zakup 2 — isti vlasnik, drugo skladiste
            proxy.ZakupiSkladiste(
                new Skladiste
                {
                    IdSkladista = 2,
                    PocetakZakupa = DateTime.Now,
                    KrajZakupa = DateTime.Now.AddMonths(3),
                    Cena = 300,
                },
                new Vlasnik
                {
                    Ime = "Petar",
                    Prezime = "Petrovic",
                    JMBG = "1234567890123",
                }
            );

            // Zakup 3 — drugi vlasnik
            proxy.ZakupiSkladiste(
                new Skladiste
                {
                    IdSkladista = 1,
                    PocetakZakupa = DateTime.Now.AddMonths(7),
                    KrajZakupa = DateTime.Now.AddMonths(12),
                    Cena = 600,
                },
                new Vlasnik
                {
                    Ime = "Marko",
                    Prezime = "Markovic",
                    JMBG = "9876543210987",
                }
            );

            // Aktivna skladista vlasnika
            Console.WriteLine("=== Aktivna skladista Petra ===");
            var aktivna = proxy.AktivnaSkladistaVlasnika("1234567890123");
            foreach (var s in aktivna)
                Console.WriteLine($"  ID: {s.IdSkladista}, Cena: {s.Cena}, Do: {s.KrajZakupa:d}");

            // Vlasnici aktivnih skladista
            Console.WriteLine("\n=== Vlasnici aktivnih skladista ===");
            var vlasnici = proxy.VlasniciAktivnihSkladista();
            foreach (var v in vlasnici)
                Console.WriteLine($"  {v.Ime} {v.Prezime} ({v.JMBG})");

            // Sva skladista sa istorijom
            Console.WriteLine("\n=== Istorija svih skladista ===");
            var istorija = proxy.SvaSkladistaSaIstorijom();
            foreach (var si in istorija)
            {
                Console.WriteLine($"--- Skladiste {si.IdSkladista} ---");
                foreach (var z in si.Zakupi)
                    Console.WriteLine(
                        $"  {z.Vlasnik.Ime} {z.Vlasnik.Prezime}: {z.Skladiste.PocetakZakupa:d} - {z.Skladiste.KrajZakupa:d}"
                    );
            }

            Console.ReadKey();
        }
    }
}
