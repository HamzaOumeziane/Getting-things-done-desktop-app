using System.Xml;

namespace GTD
{
    public class ElementGTD
    {
        // Créer les attributs de l'éléments qui sont : 
        // le nom comme une chaine de caractere
        public string Nom { get; set; }
        // la description
        public string Description { get; set; }
        // la date rappel
        public DateOnly? DateRappel { get; set; }
        // et son statut
        public string Statut { get; set; }

       
        
        public ElementGTD(XmlElement element)
        {
            Nom = element.GetAttribute("nom");
            Description = element.InnerText;
            Statut = element.GetAttribute("statut");

            // si la date rappel n'est pas vide dans le fichier, on la divise en 3 : annee, mois et jour
            if(element.GetAttribute("dateRappel") != "")
            {
                string[] laDate = element.GetAttribute("dateRappel").Split("-");
                int.TryParse(laDate[0], out int annee);
                int.TryParse(laDate[1], out int mois);
                int.TryParse(laDate[2], out int jour);
                DateRappel = new DateOnly(annee, mois, jour);
            }
            else
            {
                DateRappel = null;
            }
            
        }

        // lorsqu'on crée un élément on met son statut automatiquement a entrée
        public ElementGTD() 
        {
            Statut = "Entree";
        }

        

        // méthode pour prendre l'élément et le mettre dans le XML
        public XmlElement VersXML(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("element_gtd");
            element.SetAttribute("nom", Nom);
            element.SetAttribute("statut", Statut);
            element.SetAttribute("dateRappel", DateRappel.ToString());
            element.InnerText = Description;

            return element;

        }

        // méthode ToString
        public override string ToString()
        {
            if(Statut == "Suivi")
            {
                return $"{Nom} ({DateRappel.ToString()})";
            }
            else
            {
                return Nom;
            }
            
        }



    }



}