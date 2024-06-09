using GTD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using ClassesAffaire;
using System.Xml.Linq;

namespace BdeBGTD
{
    /// <summary>
    /// Interaction logic for AjoutEntree.xaml
    /// </summary>
    public partial class AjoutEntree : Window
    {
        // Créer les commandes 
        public static RoutedCommand AjouterEntree = new RoutedCommand();

        public static RoutedCommand AnnulerEntree = new RoutedCommand();
        //Créer la listbox
        private List<TextBox> _lesTextBox;

        //Créer une liste
        ObservableCollection<ElementGTD> _laListe;

        ElementGTD _element;

        public AjoutEntree(ObservableCollection<ElementGTD> liste)
        {

            _lesTextBox = new List<TextBox>();
            _element = new ElementGTD();
            
            InitializeComponent();

            _lesTextBox.Add(TextBoxNomElement);

            // Faire le datacontext pour l'élément
            DataContext = _element;
            // Associer la liste qu'on a crée avec la liste des entrées
            _laListe = liste;
        }

        private void AjouterEntree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ChampsTextePleins();
        }

        private void AjouterEntree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // on ajoute l'élément a la liste des entrées
            _laListe.Add(_element);

            // on crée un nouvel élément pour permettre d'ajouter un autre élément si on veut
            _element = new ElementGTD();
            DataContext = _element;

            // si la box n'est pas checké on ferme la fenetre
            if ( ! CheckboxFenetreOuverte.IsChecked.Value)
            {
                DialogResult = true;
                Close();
            }
        }

        // une méthode qui permet de savoir si le champ des textes sont pleins (seulement pour le nom)
        private bool ChampsTextePleins()
        {
            bool reponse = true;
            foreach (TextBox textBox in _lesTextBox)
            {
                if (textBox.Text.Equals(""))
                {
                    reponse = false;
                    break;
                }
            }
            return reponse;
        }

        // faire le can execute et executed pour annuler l'entrée
        private void AnnulerEntree_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AnnulerEntree_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }
}
