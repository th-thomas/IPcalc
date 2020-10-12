using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IPcalc
{
    class PrincipalIP
    {
        static public void Main(string[] args)
        // Programme : IPcalc
        // Auteur : Thibault THOMAS
        // Objectif : à partir d'une adresse IP et d'un masque, donner les caractéristiques du réseau.
        // Date de création : 12 décembre 2019 (v0.1 -Alpha-)
        // v0.4 -Beta- (8 février 2020) :
        //      * Affichage en couleurs
        //      * Ajout des adresses binaires au côté des adresses en décimal pointé.
        //      * Affiche les bits réseau et les bits hôtes
        //      * Ajout des attributs "Réservé" pour les adresses de classe D et E, ainsi que pour les adresses en 127
        //      * Prend désormais correctement en compte le CIDR si rentré avec le caractère '/'.
        // v0.3 -Beta- (6 janvier 2020) :
        //      * Correction d'erreurs dans les boucles de saisie
        // v0.2 -Beta- (27 décembre 2019) :
        //      * Ajout des caractéristiques Privé ou Public et de la Classe.
        //      * Le programme autorise maintenant la saisie d'un CIDR ou d'un sans passer par un menu
        //          demandant à l'utilisateur quel type il saisira (1 Masque décimal pointé 2 CIDR)
        //      * Correction d'une erreur dans le code qui permettait à l'utilisateur de saisir un masque
        //          incorrect (exemple : 255.255.255.1). 

        {
            ipv4 ip, masque, adresseReseau, adresseDiffusion, adressePremierhote, adresseDernierHote;
            Double nbHotes;
            String chaineHotes, saisieContinuer;
            Boolean afficher, continuer;
            String versionDuProgramme = "v0.4";

            AfficherEnBleu($"================== IP Calc {versionDuProgramme} ==================\n\n");
            do
            {
                continuer = true;
                ip = SaisieIPv4();
                masque = SaisieMasque();
                Console.Clear();
                ip.VersBinaire();
                masque.VersBinaire();
                adresseReseau = ip.AdresseReseau(masque);
                Char Classe = ip.DonneClasse();
                adresseDiffusion = ip.AdresseDiffusion(adresseReseau);
                adressePremierhote = ip.AdressePremierHote(adresseReseau);
                adresseDernierHote = ip.AdresseDernierHote(adresseReseau);
                var nbAdresses = adresseReseau.NombreAdresses();
                var PriveOuPublic = adresseReseau.PriveOuPublic();
                afficher = true;
                if (nbAdresses < 4)
                {
                    nbHotes = 0;
                    chaineHotes = "hôte";
                    afficher = false;
                }
                else
                {
                    nbHotes = nbAdresses - 2;
                    chaineHotes = "hôtes";
                }
                
                Console.Write("Adresse IP :".PadRight(26));
                AfficherEnVert(ip.ToString().PadRight(19));
                AfficherEnBleu(ip.ToStringBin() + "\n");
                Console.Write("Masque de sous-réseau :".PadRight(26));
                AfficherEnVert(masque.ToString().PadRight(19));
                AfficherEnBleu(masque.ToStringBin() + "\n");
                Console.Write("".PadRight(26));
                AfficherEnVert($"(/{adresseReseau.GetCIDR()})\n\n");          
                Console.Write("Adresse réseau :".PadRight(26));
                AfficherEnVert(adresseReseau.ToString().PadRight(19));
                switch (adresseReseau.DonneClasse())
                {
                    case 'A':
                        AfficherEnMagenta(adresseReseau.ToStringBin().Substring(0,1));
                        AfficherEnBleu(adresseReseau.ToStringBin().Substring(1));
                        break;
                    case 'B':
                        AfficherEnMagenta(adresseReseau.ToStringBin().Substring(0, 2));
                        AfficherEnBleu(adresseReseau.ToStringBin().Substring(2));
                        break;
                    case 'C':
                        AfficherEnMagenta(adresseReseau.ToStringBin().Substring(0, 3));
                        AfficherEnBleu(adresseReseau.ToStringBin().Substring(3));
                        break;
                    case 'D':
                        AfficherEnMagenta(adresseReseau.ToStringBin().Substring(0, 4));
                        AfficherEnBleu(adresseReseau.ToStringBin().Substring(4));
                        break;
                    default:
                        AfficherEnMagenta(adresseReseau.ToStringBin().Substring(0, 5));
                        AfficherEnBleu(adresseReseau.ToStringBin().Substring(5));
                        break;
                }
                AfficherEnMagenta($"  Classe {adresseReseau.DonneClasse()}");
                Console.Write($"  Adr. {PriveOuPublic}\n");
                StringBuilder chaineHotesReseau = new StringBuilder();
                for (Byte i = 0; i < 32; i++)
                {
                    if (i != 0 && i != 31 && (i + 1) % 8 == 0)
                    {
                        if (i < adresseReseau.GetCIDR())
                        {
                            chaineHotesReseau.Append("n");
                            chaineHotesReseau.Append(" . ");
                        }
                        else
                        {
                            chaineHotesReseau.Append("h");
                            chaineHotesReseau.Append(" . ");
                        }
                    }
                    else
                    {
                        if (i < adresseReseau.GetCIDR())
                            chaineHotesReseau.Append("n");
                        else
                            chaineHotesReseau.Append("h");
                    }
                }
                Console.Write("".PadRight(45));
                AfficherEnJaune($"{chaineHotesReseau}\n");
                Console.Write("".PadRight(45) + "(n = bits réseau, h = bits hôtes)\n");
                Console.Write("Adresses sur le réseau :".PadRight(26));
                AfficherEnVert($"{nbAdresses}".PadRight(19) + "(" + nbHotes + " " + chaineHotes + ")\n");
                if (afficher)
                {
                    Console.Write("Adresse de diffusion :".PadRight(26));
                    AfficherEnVert(adresseDiffusion.ToString().PadRight(19));
                    AfficherEnBleu(adresseDiffusion.ToStringBin() + "\n");
                    Console.Write("Adresse du premier hôte :".PadRight(26));
                    AfficherEnVert(adressePremierhote.ToString().PadRight(19));
                    AfficherEnBleu(adressePremierhote.ToStringBin() + "\n");
                    Console.Write("Adresse du dernier hôte :".PadRight(26));
                    AfficherEnVert(adresseDernierHote.ToString().PadRight(19));
                    AfficherEnBleu(adresseDernierHote.ToStringBin() + "\n");
                }
                else
                {
                    Console.Write("Adresse de diffusion :".PadRight(26));
                    AfficherEnVert("N/A\n");
                    Console.Write("Adresse du premier hôte :".PadRight(26));
                    AfficherEnVert("N/A\n");
                    Console.Write("Adresse du dernier hôte :".PadRight(26));
                    AfficherEnVert("N/A\n");
                }
                
                Console.WriteLine("\n");
                Console.WriteLine("Continuer ? (oui/non)");
                saisieContinuer = SaisieEnVert();
                while (saisieContinuer != "oui" && saisieContinuer != "non")
                {
                    SaisieInvalide(saisieContinuer + " n'est pas une réponse valide.");
                    Console.WriteLine("Continuer ? (oui/non)");
                    saisieContinuer = SaisieEnVert();
                } 
                if (saisieContinuer == "non")
                {
                    continuer = false;
                    Console.WriteLine("\nAu revoir !");
                    Console.WriteLine($"### IPcalc {versionDuProgramme} Copyright 2019-2020 Thibault THOMAS ###");
                    Thread.Sleep(2000);
                }
                else
                    Console.Clear();
            } while (continuer);
        }
        static private ipv4 SaisieIPv4()
        {
            String saisie;
            String[] abcd;
            String elementA, elementB, elementC, elementD;
            String[] separateur = new string[] { "." };
            ipv4 IPsaisie = new ipv4();
            Boolean formatValide = true;
            do
            {
                formatValide = true;
                Console.Write("Adresse (hôte ou réseau) :\t");
                saisie = SaisieEnVert();
                
                try
                {
                    abcd = saisie.Split(separateur, 4, StringSplitOptions.None);
                    elementA = abcd[0];
                    elementB = abcd[1];
                    elementC = abcd[2];
                    elementD = abcd[3];
                    IPsaisie = new ipv4(Byte.Parse(elementA), Byte.Parse(elementB), Byte.Parse(elementC), Byte.Parse(elementD));
                }
                catch (FormatException)
                {
                    formatValide = false;
                }
                catch (IndexOutOfRangeException)
                {
                    formatValide = false;
                }
                catch (OverflowException)
                {
                    formatValide = false;
                }
                finally
                {
                    if (!formatValide)
                        SaisieInvalide();
                }
            } while (!formatValide);
            return IPsaisie;
        }
        static private ipv4 SaisieMasque()
        {
            String saisie;
            String[] abcd;
            String elementA, elementB, elementC, elementD;
            String[] separateur = new string[] { "." };
            ipv4 MasqueSaisi = new ipv4();
            Byte unCIDR;
            Boolean formatValide;
            Boolean TypeEstCIDR = false;

            do // Cette boucle fait saisir à l'utilisateur un masque dans un format valide (CIDR ou décimal pointé)
            {
                formatValide = true;
                Console.Write("Masque / CIDR :\t\t\t");
                saisie = SaisieEnVert();
                int index = saisie.IndexOf('/');
                if (index == 0)
                    saisie = saisie.Substring(1);
                if (Byte.TryParse(saisie, out unCIDR) && unCIDR < 33) // L'utilisateur saisit directement un CIDR (nombre compris entre 0 et 32)
                    TypeEstCIDR = true;
                else // L'utilisateur rentre un masque en notation décimale pointée... ou tout autre chose
                {
                    try
                    {
                        abcd = saisie.Split(separateur, 4, StringSplitOptions.None);
                        elementA = abcd[0];
                        elementB = abcd[1];
                        elementC = abcd[2];
                        elementD = abcd[3];
                        MasqueSaisi = new ipv4(Byte.Parse(elementA), Byte.Parse(elementB), Byte.Parse(elementC), Byte.Parse(elementD));
                    }
                    catch (FormatException)
                    {
                        formatValide = false;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        formatValide = false;
                    }
                    catch (OverflowException)
                    {
                        formatValide = false;
                    }
                    finally
                    {
                        if (formatValide)
                        {
                            Int32 resteOctetA, resteOctetB, resteOctetC, resteOctetD;
                            resteOctetA = 256 - MasqueSaisi.GetOctet1();
                            resteOctetB = 256 - MasqueSaisi.GetOctet2();
                            resteOctetC = 256 - MasqueSaisi.GetOctet3();
                            resteOctetD = 256 - MasqueSaisi.GetOctet4();

                            if (!(EstPuissanceDeDeux(resteOctetA) && EstPuissanceDeDeux(resteOctetB) && EstPuissanceDeDeux(resteOctetC) && EstPuissanceDeDeux(resteOctetD)))
                                formatValide = false;
                        }
                    }
                }
                if (TypeEstCIDR) // Si l'utilisateur a rentré un CIDR
                {
                    StringBuilder unMasqueBinaire = new StringBuilder();
                    for (Byte i = 0; i < 32; i++)
                    {
                        if (i != 0 && i != 31 && (i + 1) % 8 == 0)
                        {
                            if (i < unCIDR)
                            {
                                unMasqueBinaire.Append("1");
                                unMasqueBinaire.Append(".");
                            }
                            else
                            {
                                unMasqueBinaire.Append("0");
                                unMasqueBinaire.Append(".");
                            }
                        }
                        else
                        {
                            if (i < unCIDR)
                                unMasqueBinaire.Append("1");
                            else
                                unMasqueBinaire.Append("0");
                        }
                    }
                    abcd = unMasqueBinaire.ToString().Split(separateur, 8, StringSplitOptions.None);
                    elementA = Convert.ToByte(abcd[0], 2).ToString();
                    elementB = Convert.ToByte(abcd[1], 2).ToString();
                    elementC = Convert.ToByte(abcd[2], 2).ToString();
                    elementD = Convert.ToByte(abcd[3], 2).ToString();
                    MasqueSaisi = new ipv4(Byte.Parse(elementA), Byte.Parse(elementB), Byte.Parse(elementC), Byte.Parse(elementD));
                }
                if (!formatValide)
                    SaisieInvalide();
            } while (!formatValide);
            return MasqueSaisi;
        }
        static private Boolean EstPuissanceDeDeux(Int32 x)
        {
            return (x != 0) && ((x & (x - 1)) == 0);
        }
        static private void SaisieInvalide()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("Votre saisie est invalide !");
            Console.ResetColor();
            Console.WriteLine();
        }
        static private void SaisieInvalide(string chaine)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(chaine);
            Console.ResetColor();
            Console.WriteLine();
        }
        static private String SaisieEnVert()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            String saisie = Console.ReadLine().ToLower();
            Console.ResetColor();
            return saisie;
        }
        static private void AfficherEnVert(string uneChaine)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(uneChaine);
            Console.ResetColor();
        }
        static private void AfficherEnMagenta(string uneChaine)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(uneChaine);
            Console.ResetColor();
        }
        static private void AfficherEnBleu(string uneChaine)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(uneChaine);
            Console.ResetColor();
        }
        static private void AfficherEnJaune(string uneChaine)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(uneChaine);
            Console.ResetColor();
        }
    }
}
