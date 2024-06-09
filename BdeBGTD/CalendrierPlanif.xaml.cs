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
    /// Interaction logic for CalendrierPlanif.xaml
    /// </summary>
    public partial class CalendrierPlanif : Window
    {
        // créer un élément
        ElementGTD element;
        public CalendrierPlanif(ElementGTD elem)
        {
            InitializeComponent();
            // Associer l'élément avec lequel on veut planifier
            element = elem;
        }

        // Créer une méthode pour double click la date
        private void Calendar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Calendar calendrier = sender as Calendar;
            // lorsqu'on double click une date, on prend cette date et on l'associe à la date rappel de l'élément,
            // et puis on modifie le statut de l'élément et on le met comme Action
            if (calendrier.SelectedDate.HasValue)
            {
                DateTime date = calendrier.SelectedDate.Value;
                element.DateRappel = DateOnly.Parse(date.ToShortDateString());
                element.Statut = "Action";
            }

            Close();
        }
    }
}
