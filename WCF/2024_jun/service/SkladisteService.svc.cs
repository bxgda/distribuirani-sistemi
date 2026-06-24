using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace WCF_Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SkladisteService : ISkladiste
    {
        static List<Zakup> zakupi = new List<Zakup>();

        public List<Skladiste> AktivnaSkladistaVlasnika(string jmbg)
        {
            return zakupi
                .Where(z =>
                    z.Vlasnik.JMBG == jmbg
                    && z.Skladiste.PocetakZakupa <= DateTime.Now
                    && z.Skladiste.KrajZakupa >= DateTime.Now
                )
                .Select(z => z.Skladiste)
                .ToList();
        }

        public List<SkladisteIstorija> SvaSkladistaSaIstorijom()
        {
            return zakupi
                .GroupBy(z => z.Skladiste.IdSkladista)
                .Select(g => new SkladisteIstorija { IdSkladista = g.Key, Zakupi = g.ToList() })
                .ToList();
        }

        public List<Vlasnik> VlasniciAktivnihSkladista()
        {
            return zakupi
                .Where(z =>
                    z.Skladiste.PocetakZakupa <= DateTime.Now
                    && z.Skladiste.KrajZakupa >= DateTime.Now
                )
                .Select(z => z.Vlasnik)
                .Distinct()
                .ToList();
        }

        public void ZakupiSkladiste(Skladiste skladiste, Vlasnik vlasnik)
        {
            zakupi.Add(new Zakup { Skladiste = skladiste, Vlasnik = vlasnik });
        }
    }
}
