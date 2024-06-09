using GTD;
using System;
using System.Collections.Generic;
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

namespace BdeBGTD
{
    /// <summary>
    /// Interaction logic for lesActions.xaml
    /// </summary>
    public partial class lesActions : Window
    {
        public static RoutedCommand terminer = new RoutedCommand();
        public static RoutedCommand poursuivre = new RoutedCommand();

        ElementGTD unElement;
        GestionnaireGTD unGestionnaire;

        public lesActions(ElementGTD elem, GestionnaireGTD _gestionnaire)
        {
            InitializeComponent();

            unElement = elem;
            unGestionnaire = _gestionnaire;
            DataContext = unElement;
        }

        private void terminer_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void terminer_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            unElement.Statut = "Archive";
            unElement.DateRappel = null;
            unGestionnaire.AutresElements.Add(unElement);
            unGestionnaire.ListeActions.Remove(unElement);

            Close();
        }

        private void poursuivre_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void poursuivre_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DataContext = unElement;
            Close();
        }
    }
}
