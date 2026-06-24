using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WCF_Service
{
    [ServiceContract]
    public interface ISkladiste
    {
        [OperationContract]
        void ZakupiSkladiste(Skladiste skladiste, Vlasnik vlasnik);

        [OperationContract]
        List<Skladiste> AktivnaSkladistaVlasnika(string jmbg);

        [OperationContract]
        List<Vlasnik> VlasniciAktivnihSkladista();

        [OperationContract]
        List<SkladisteIstorija> SvaSkladistaSaIstorijom();
    }

    [DataContract]
    public class Vlasnik
    {
        [DataMember]
        public string Ime { get; set; }

        [DataMember]
        public string Prezime { get; set; }

        [DataMember]
        public string JMBG { get; set; }
    }

    [DataContract]
    public class Skladiste
    {
        [DataMember]
        public int IdSkladista { get; set; }

        [DataMember]
        public DateTime PocetakZakupa { get; set; }

        [DataMember]
        public DateTime? KrajZakupa { get; set; }

        [DataMember]
        public double Cena { get; set; }
    }

    [DataContract]
    public class Zakup
    {
        [DataMember]
        public Vlasnik Vlasnik { get; set; }

        [DataMember]
        public Skladiste Skladiste { get; set; }
    }

    [DataContract]
    public class SkladisteIstorija
    {
        [DataMember]
        public int IdSkladista { get; set; }

        [DataMember]
        public List<Zakup> Zakupi { get; set; }
    }
}
