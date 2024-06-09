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
    /// Interaction logic for CalendrierSuivi.xaml
    /// </summary>
    public partial class CalendrierSuivi : Window
    {
        // Faire les memes instructions qu'on a fait avec CalendrierPlanif 


        ElementGTD element;

        public CalendrierSuivi(ElementGTD elem)
        {
            InitializeComponent();
            element = elem;
        }

        private void AnnulerPlanif(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AnnulerPlanif(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }


        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Calendar calendrier = sender as Calendar;

            if (calendrier.SelectedDate.HasValue)
            {
                DateTime date = calendrier.SelectedDate.Value;
                element.DateRappel = DateOnly.Parse(date.ToShortDateString());
                // on modifie le statut de l'élément comme étant Suivi
                element.Statut = "Suivi";
            }

            Close();
        }
    }
}
