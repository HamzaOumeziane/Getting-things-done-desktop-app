using GTD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
//using ClassesAffaire;
using System.Data.Common;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using System.Reflection.Metadata;
using System.Diagnostics.Contracts;


namespace BdeBGTD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Creation des commandes
        public static RoutedCommand AProposCmd = new RoutedCommand();

        public static RoutedCommand QuitterCmd = new RoutedCommand();

        public static RoutedCommand AjouterEntreeCmd = new RoutedCommand();

        public static RoutedCommand TraiterCmd = new RoutedCommand();

        public static RoutedCommand AllerProchainJour = new RoutedCommand();


        // creation de la date courante qui signifie la date d'aujourd'hui
        public DateOnly date_courante = DateOnly.FromDateTime(DateTime.Now);

        // création du gestionnaire qui contient les 4 listes : une liste d'entrée, une liste des actions, une liste des suivis et une liste
        // qui contient les prochaines actions et les archives
        private GestionnaireGTD _gestionnaire;
        private string _pathFichier;
        // un char pour séparer les répertoires
        private char DIR_SEPARATOR = Path.DirectorySeparatorChar;
        private int nbEntrees;
        

        public MainWindow()
        {
            InitializeComponent();

            // Faire les raccourcis : 
            // Ctrl Q pour sauvegarder et quitter
            QuitterCmd.InputGestures.Add(new KeyGesture(Key.Q, ModifierKeys.Control));
            // Ctrl A pour ajouter une entrée
            AjouterEntreeCmd.InputGestures.Add(new KeyGesture(Key.A, ModifierKeys.Control));
            // Ctrl T pour traiter les entrées
            TraiterCmd.InputGestures.Add(new KeyGesture(Key.T, ModifierKeys.Control));


            nbEntrees = 0;

            //On associe la date courante avec le texte où on affiche la date courante
            DateCourante.Text = date_courante.ToString();

            _gestionnaire = new GestionnaireGTD();

            // le chemin vers le fichier
            _pathFichier = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + DIR_SEPARATOR +
                          "Fichiers-3GP" + DIR_SEPARATOR + "bdeb_gtd.xml";

            // appeler la méthode pour charger le fichier Xml
            ChargerFichierXml();

            // Associer la textBox des boites d'entrées avec la liste des entrées
            BoiteEntrees.ItemsSource = _gestionnaire.ListeEntrees;
            // Associer la textBox des Prochianes actions avec la liste des actions
            ProchainesActions.ItemsSource = _gestionnaire.ListeActions;
            // Associer la textBox des Éléments de suivi avec la liste des suivis
            SystemeSuivi.ItemsSource = _gestionnaire.ListeSuivis;

        }

        // une méthode qui charge le fichier XMl
        private void ChargerFichierXml()
        {
            // créer le document XML
            XmlDocument document = new XmlDocument();
            // On charge dans ce document le fichier
            document.Load(_pathFichier);
            // créer la racine
            XmlElement racine = document.DocumentElement;
            // créer les éléments de la liste
            XmlNodeList lesElementsXML = racine.GetElementsByTagName("element_gtd");

            // Pour chaque élément 
            foreach (XmlElement unElement in lesElementsXML)
            {
                ElementGTD elem = new ElementGTD(unElement);
                
                switch (elem.Statut)
                {
                    // si son statut est Entrée on l'ajoute à la liste des entrées
                    case "Entree":
                        _gestionnaire.ListeEntrees.Add(elem);
                        break;
                    // si son statut est Action
                    case "Action":
                        // si sa date de rappel dépasse la date courante
                        if(elem.DateRappel > date_courante)
                        {
                            // on l'ajoute dans la liste des actions
                            _gestionnaire.ListeActions.Add(elem);
                        }
                        else
                        {
                            // sinon on l'ajoute dans la 4eme liste (prochaines actions)
                            _gestionnaire.AutresElements.Add(elem);
                        }
                        
                        break;
                    // si son statut est Suivi on l'ajoute dans la liste des suivis
                    case "Suivi":
                        _gestionnaire.ListeSuivis.Add(elem);
                        break;
                    default:
                        break;

                }
            }

        }

        // faire le can execute et executed pour le À Propos
        private void AProposCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AProposCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Bdeb GTD \n" +
                            "Version 1.0 \n" +
                            "Auteur: Hamza Oumeziane");
        }

        // faire le can execute et executed pour Quitter
        private void QuitterCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void QuitterCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SauvegarderXml();
            Close();
        }

        // faire le can execute et executed pour ajouter une entrée
        private void AjouterEntreeCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AjouterEntreeCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // on crée une nouvelle fenetre qui sert à afficher pour ajouter un élément
            AjoutEntree fenetreEntree = new AjoutEntree(_gestionnaire.ListeEntrees);
            fenetreEntree.Owner = this;
            fenetreEntree.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreEntree.ShowDialog();
            

        }

        // faire le can execute et executed pour traiter une commande
        private void TraiterCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if(_gestionnaire.ListeEntrees.Count > 0)
            {
              e.CanExecute = true;
            }
        }

        private void TraiterCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //On crée une nouvelle fenetre qui sert à afficher pour traiter les éléments
            Traiter fenetreTraiter = new Traiter(_gestionnaire);
            fenetreTraiter.Owner = this;
            fenetreTraiter.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            fenetreTraiter.ShowDialog();
        }


        // Faire le can execute et executed pour le bouton + (aller au prochain jour)
        private void AllerProchainJour_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AllerProchainJour_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // créer une liste qui sert à enlever les elements de la liste des actions
            List<ElementGTD> poubelleActions = new List<ElementGTD>();
            // créer une qui liste sert à enlever les elements de la liste des suivis
            List<ElementGTD> poubelleSuivis = new List<ElementGTD>();

            //int proActions = _gestionnaire.AutresElements.Count;
            //int suivi = _gestionnaire.ListeSuivis.Count;
            DateOnly nouvelleDate = date_courante.AddDays(1);
            DateCourante.Text = nouvelleDate.ToString();
            date_courante = nouvelleDate;
            // a chaque qu'on ajoute un jour, on parcourt tous les éléments des suivis, si 
            // la date de rappel pour un élément égale à la date courante, on met l'élément dans les entrées
            foreach (ElementGTD elem in _gestionnaire.ListeSuivis)
            {
                if (elem.DateRappel == date_courante)
                {
                    poubelleSuivis.Add(elem);
                    _gestionnaire.ListeEntrees.Add(elem);
                    elem.Statut = "Entree";
                    elem.DateRappel = null;
                }

            }

            // on enleve de la liste des suivis les éléemnts qui sont dans les entrées
            foreach(ElementGTD elem in poubelleSuivis)
            {
                _gestionnaire.ListeSuivis.Remove(elem);
            }


            // a chaque qu'on ajoute un jour, on parcourt tous les éléments des prochaines actions, si 
            // la date de rappel pour un élément égale à la date courante, on met l'élément dans les actions
            foreach (ElementGTD elem in _gestionnaire.AutresElements)
            {
                if(elem.DateRappel == date_courante)
                {
                    poubelleActions.Add(elem);
                    _gestionnaire.ListeActions.Add(elem);
                }
            }

            // On enleve de la liste des prochaines actions les éléments qui sont dans les actions
            foreach (ElementGTD elem in poubelleActions)
            {
                _gestionnaire.AutresElements.Remove(elem);
            }


        }


        // La méthode qui sert à sauvegarder le fichier XML
        private void SauvegarderXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement racine = doc.CreateElement("element_gtd");
            doc.AppendChild(racine);

            // pour chaque élément de la liste des entrées, on l'ajoute à la racine
            foreach (ElementGTD entree in _gestionnaire.ListeEntrees)
            {
                racine.AppendChild(entree.VersXML(doc));
            }

            // pour chaque élément de la liste des actions, on l'ajoute à la racine
            foreach (ElementGTD actions in _gestionnaire.ListeActions)
            {
                racine.AppendChild(actions.VersXML(doc));
            }

            // pour chaque élément de la liste des suivis, on l'ajoute à la racine
            foreach (ElementGTD suivi in _gestionnaire.ListeSuivis)
            {
                racine.AppendChild(suivi.VersXML(doc));
            }

            // pour chaque élément de 4eme liste, on l'ajoute à la racine
            foreach (ElementGTD prochains in _gestionnaire.AutresElements)
            {
                racine.AppendChild(prochains.VersXML(doc));
            }

            // on sauvegarde le document dans le fichier 
            doc.Save(_pathFichier);
        }

        private void ProchainesActions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Créer un élément qui est l'élément qu'on a double cliqué  
            ElementGTD unElement = (ElementGTD)ProchainesActions.SelectedItem;
            // Créer la fenetre pour afficher l'action
            lesActions fenetreAction = new lesActions(unElement, _gestionnaire);
            fenetreAction.Owner = this;
            fenetreAction.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            fenetreAction.ShowDialog();



        }
    }
}
