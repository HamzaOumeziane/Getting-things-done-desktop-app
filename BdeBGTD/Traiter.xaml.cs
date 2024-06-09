using GTD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Shapes;
//using ClassesAffaire;
using System.Xml.Linq;

namespace BdeBGTD
{
    /// <summary>
    /// Interaction logic for Traiter.xaml
    /// </summary>
    public partial class Traiter : Window
    {
       // creer les commandes 
        public static RoutedCommand PoubelleCmd = new RoutedCommand();
        public static RoutedCommand IncuberCmd = new RoutedCommand();
        public static RoutedCommand PlanifierActionCmd = new RoutedCommand();
        public static RoutedCommand ActionRapideCmd = new RoutedCommand();
        public static RoutedCommand Retour = new RoutedCommand();

        // creer les listes
        ObservableCollection<ElementGTD> maListe;
        ObservableCollection<ElementGTD> mesActions;
        ObservableCollection<ElementGTD> mesSuivis;
        List<ElementGTD> mesAutresElements;
        // creer l'element 
        ElementGTD unElement;
        int elementCourant = 0;
        // creer le gestionnaire
        GestionnaireGTD unGestionnaire;
       


        public Traiter(GestionnaireGTD _gestionnaire)
        {
            unElement = new ElementGTD();
            
            InitializeComponent();
            // associer les listes et le gestionnaire
            unGestionnaire = _gestionnaire;
            maListe = unGestionnaire.ListeEntrees;
            mesActions = unGestionnaire.ListeActions;
            mesSuivis = unGestionnaire.ListeSuivis;
            mesAutresElements = unGestionnaire.AutresElements;

            // faire le datacontext pour la l'élément courant de la liste d'entrées
            DataContext = unGestionnaire.ListeEntrees[elementCourant];

          

            
        }

        // can execute et executed pour incuber
        private void IncuberCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void IncuberCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            unElement = maListe[elementCourant];

            CalendrierSuivi suivi = new CalendrierSuivi(unElement);
            suivi.Owner = this;
            suivi.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            suivi.ShowDialog();

            mesSuivis.Add(unElement);

            maListe.Remove(unElement);


            if (elementCourant < maListe.Count)
            {
                DataContext = maListe[elementCourant];
            }
            else
            {
                Close();
            }
           
        }

        // can execute et executed pour action rapide
        private void ActionRapideCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = true;
        }

        private void ActionRapide_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            unElement = maListe[elementCourant];

            mesAutresElements.Add(unElement);
            unElement.Statut = "Archive";
            unElement.DateRappel = null;

            maListe.Remove(unElement);

            if (elementCourant < maListe.Count)
            {
                DataContext = maListe[elementCourant];
            }
            else
            {
                Close();
            }
        }


        // can execute et executed pour planifier action
        private void PlanifierActionCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute= true;
        }

        private void PlanifierActionCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            unElement = maListe[elementCourant];

            CalendrierPlanif planification = new CalendrierPlanif(unElement);
            planification.Owner = this;
            planification.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            planification.ShowDialog();

            if(unElement.DateRappel == DateOnly.FromDateTime(DateTime.Now))
            {
                mesActions.Add(unElement);
            }
            else
            {
                mesAutresElements.Add(unElement);
            }
            

            maListe.Remove(unElement);


            if (elementCourant < maListe.Count)
            {
                DataContext = maListe[elementCourant];
            }
            else
            {
                Close();
            }
        }


        // can execute et executed pour la poubelle
        private void PoubelleCmd_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PoubelleCmd_Executed(object sender, ExecutedRoutedEventArgs e)
        {

            maListe.Remove(maListe[elementCourant]);
            if(elementCourant < maListe.Count)
            {
                DataContext = maListe[elementCourant];
            }
            else
            {
                Close();
            }
            
            
        }




        // can execute et executed pour retour
        private void Retour_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Retour_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close() ;
        }
    }
}
