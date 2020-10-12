using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPcalc
{
    class ipv4
    {
        private Byte _octet1;
        private Byte _octet2;
        private Byte _octet3;
        private Byte _octet4;
        private String _octetBin1;
        private String _octetBin2;
        private String _octetBin3;
        private String _octetBin4;
        private Byte _cidr;

        // Constructeurs
        public ipv4()
        {
            _octet1 = 0;
            _octet2 = 0;
            _octet3 = 0;
            _octet4 = 0;
            _octetBin1 = "0";
            _octetBin2 = "0";
            _octetBin3 = "0";
            _octetBin4 = "0";
            _cidr = 0;
        }
        public ipv4(Byte Octet1, Byte Octet2, Byte Octet3, Byte Octet4) : this()
        {
            _octet1 = Octet1;
            _octet2 = Octet2;
            _octet3 = Octet3;
            _octet4 = Octet4;
        }
      
        // Accesseurs en lecture pour les octets décimaux
        public Byte GetOctet1()
        {
            return _octet1;
        }
        public Byte GetOctet2()
        {
            return _octet2;
        }
        public Byte GetOctet3()
        {
            return _octet3;
        }
        public Byte GetOctet4()
        {
            return _octet4;
        }
        // Accesseurs en lecture pour les octets binaires
        public String GetOctetBin1()
        {
            return _octetBin1;
        }
        public String GetOctetBin2()
        {
            return _octetBin2;
        }
        public String GetOctetBin3()
        {
            return _octetBin3;
        }
        public String GetOctetBin4()
        {
            return _octetBin4;
        }
        // Accesseur en lecture pour le CIDR
        public Byte GetCIDR()
        {
            return _cidr;
        }

        // Accesseurs en écriture pour les octets décimaux
        public void SetOctet1(Byte Octet1)
        {
            _octet1 = Octet1;
        }
        public void SetOctet2(Byte Octet2)
        {
            _octet2 = Octet2;
        }
        public void SetOctet3(Byte Octet3)
        {
            _octet3 = Octet3;
        }
        public void SetOctet4(Byte Octet4)
        {
            _octet4 = Octet4;
        }
        // Accesseurs en écriture pour les octets binaires
        public void SetOctetBin1(String Octet1)
        {
            _octetBin1 = Octet1;
        }
        public void SetOctetBin2(String Octet2)
        {
            _octetBin2 = Octet2;
        }
        public void SetOctetBin3(String Octet3)
        {
            _octetBin3 = Octet3;
        }
        public void SetOctetBin4(String Octet4)
        {
            _octetBin4 = Octet4;
        }
        // Accesseur en écriture pour le CIDR
        public void SetCIDR(Byte CIDR)
        {
            _cidr = CIDR;
        }
        // Méthodes
        public override String ToString()
        {
            String maChaine;
            maChaine = String.Concat(_octet1, ".", _octet2, ".", _octet3, ".", _octet4);
            return maChaine;
        }
        public String ToStringBin()
        {
            String maChaine;
            maChaine = String.Concat(_octetBin1, " . ", _octetBin2, " . ", _octetBin3, " . ", _octetBin4);
            return maChaine;
        }
        public void VersBinaire()
        {
            SetOctetBin1(Convert.ToString(GetOctet1(), 2).PadLeft(8, '0'));
            SetOctetBin2(Convert.ToString(GetOctet2(), 2).PadLeft(8, '0'));
            SetOctetBin3(Convert.ToString(GetOctet3(), 2).PadLeft(8, '0'));
            SetOctetBin4(Convert.ToString(GetOctet4(), 2).PadLeft(8, '0'));
        }
        public ipv4 AdresseReseau(ipv4 unMasque) // Méthode à appliquer à une adresse IP
        {
            ipv4 adrReseau = new ipv4();
            Byte octet1 = (Byte)(unMasque.GetOctet1() & _octet1);
            Byte octet2 = (Byte)(unMasque.GetOctet2() & _octet2);
            Byte octet3 = (Byte)(unMasque.GetOctet3() & _octet3);
            Byte octet4 = (Byte)(unMasque.GetOctet4() & _octet4);
            adrReseau.SetOctet1(octet1);
            adrReseau.SetOctet2(octet2);
            adrReseau.SetOctet3(octet3);
            adrReseau.SetOctet4(octet4);
            adrReseau.VersBinaire();
            unMasque.DonneCIDR();
            adrReseau.SetCIDR(unMasque.GetCIDR());
            return adrReseau;
        }
        public ipv4 AdresseDiffusion(ipv4 uneAdresseReseau)
        {
            ipv4 adrDiffusion = new ipv4();
            String chaineAdresseReseau = String.Concat(uneAdresseReseau.GetOctetBin1(), uneAdresseReseau.GetOctetBin2(), uneAdresseReseau.GetOctetBin3(), uneAdresseReseau.GetOctetBin4());
            int nbBitsReseau = uneAdresseReseau.GetCIDR();
            String binAdrDiffusion, binDiffBitsReseau;
            binDiffBitsReseau = chaineAdresseReseau.Substring(0, nbBitsReseau);
            binAdrDiffusion = binDiffBitsReseau.PadRight(32, '1');
            adrDiffusion.SetOctet1(Convert.ToByte(binAdrDiffusion.Substring(0, 8), 2));
            adrDiffusion.SetOctet2(Convert.ToByte(binAdrDiffusion.Substring(8, 8), 2));
            adrDiffusion.SetOctet3(Convert.ToByte(binAdrDiffusion.Substring(16, 8), 2));
            adrDiffusion.SetOctet4(Convert.ToByte(binAdrDiffusion.Substring(24, 8), 2));
            adrDiffusion.VersBinaire();
            return adrDiffusion;
        }
        public ipv4 AdressePremierHote(ipv4 uneAdresseReseau)
        {
            ipv4 adrPremierHote = new ipv4();
            String chaineAdresseReseau = String.Concat(uneAdresseReseau.GetOctetBin1(), uneAdresseReseau.GetOctetBin2(), uneAdresseReseau.GetOctetBin3(), uneAdresseReseau.GetOctetBin4());
            int nbBitsReseau = uneAdresseReseau.GetCIDR();
            String binAdrPremierHote, binPremierHoteBitsReseau;
            binPremierHoteBitsReseau = chaineAdresseReseau.Substring(0, nbBitsReseau);
            binAdrPremierHote = binPremierHoteBitsReseau.PadRight(31, '0').PadRight(32, '1');
            adrPremierHote.SetOctet1(Convert.ToByte(binAdrPremierHote.Substring(0, 8), 2));
            adrPremierHote.SetOctet2(Convert.ToByte(binAdrPremierHote.Substring(8, 8), 2));
            adrPremierHote.SetOctet3(Convert.ToByte(binAdrPremierHote.Substring(16, 8), 2));
            adrPremierHote.SetOctet4(Convert.ToByte(binAdrPremierHote.Substring(24, 8), 2));
            adrPremierHote.VersBinaire();
            return adrPremierHote;
        }
        public ipv4 AdresseDernierHote(ipv4 uneAdresseReseau)
        {
            ipv4 adrDernierHote = new ipv4();
            String chaineAdresseReseau = String.Concat(uneAdresseReseau.GetOctetBin1(), uneAdresseReseau.GetOctetBin2(), uneAdresseReseau.GetOctetBin3(), uneAdresseReseau.GetOctetBin4());
            int nbBitsReseau = uneAdresseReseau.GetCIDR();
            String binAdrDernierHote, binDernierHoteBitsReseau;
            binDernierHoteBitsReseau = chaineAdresseReseau.Substring(0, nbBitsReseau);
            binAdrDernierHote = binDernierHoteBitsReseau.PadRight(31, '1').PadRight(32, '0');
            adrDernierHote.SetOctet1(Convert.ToByte(binAdrDernierHote.Substring(0, 8), 2));
            adrDernierHote.SetOctet2(Convert.ToByte(binAdrDernierHote.Substring(8, 8), 2));
            adrDernierHote.SetOctet3(Convert.ToByte(binAdrDernierHote.Substring(16, 8), 2));
            adrDernierHote.SetOctet4(Convert.ToByte(binAdrDernierHote.Substring(24, 8), 2));
            adrDernierHote.VersBinaire();
            return adrDernierHote;
        }
        public Double NombreAdresses()
        {
            var nbAdresses = Math.Pow(2, 32-_cidr);
            return nbAdresses;
        }
        private void DonneCIDR() // à utiliser avec une instance masque de la classe ipv4
        {
            String chaineMasque = String.Concat(_octetBin1, _octetBin2, _octetBin3, _octetBin4);
            Byte nbBitsReseau = 0;
            foreach (var bit in chaineMasque)
            {
                if (bit == '1')
                {
                    nbBitsReseau++;
                }
            }
            SetCIDR(nbBitsReseau);
        }
        public Char DonneClasse() // à utiliser avec l'adresse réseau
        {
            Char Classe;
            String PremierOctetAdresseReseau = _octetBin1;
            if (_octetBin1.Substring(0, 1) == "0")
                Classe = 'A';
            else if (_octetBin1.Substring(0, 2) == "10")
                Classe = 'B';
            else if (_octetBin1.Substring(0, 3) == "110")
                Classe = 'C';
            else if (_octetBin1.Substring(0, 4) == "1110")
                Classe = 'D';
            else
                Classe = 'E';
            return Classe;   
        }
        public String PriveOuPublic() // à utiliser avec l'adresse réseau
        {
            String PriveOuPublic;
            if (_octet1 == 10)
                PriveOuPublic = "privée";
            else if (_octet1 == 172 && _octet2 < 32)
                PriveOuPublic = "privée";
            else if (_octet1 == 192 && _octet2 == 168)
                PriveOuPublic = "privée";
            else if (_octet1 == 127 || _octet1 == 0)
                PriveOuPublic = "réservée";
            else if (_octet1 > 223)
                PriveOuPublic = "réservée";
            else
                PriveOuPublic = "publique";
            return PriveOuPublic;
        }
    }
}
