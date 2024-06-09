using System.Collections.ObjectModel;

namespace GTD
{
    public class GestionnaireGTD
    {
        // créer l'indice courant
        int _indiceCourant;

        // créer les listes nécessaires
        // liste d'entrées
        public ObservableCollection<ElementGTD> ListeEntrees { get; private set; }
        // liste d'Actions
        public ObservableCollection<ElementGTD> ListeActions { get; private set; }
        // liste des suivis
        public ObservableCollection<ElementGTD> ListeSuivis { get; private set; }
        // liste des autres éléments (prochaines actions et les archives)
        public List<ElementGTD> AutresElements { get; private set; }

        public GestionnaireGTD()
        {
            ListeEntrees = new ObservableCollection<ElementGTD>();
            ListeActions = new ObservableCollection<ElementGTD>();
            ListeSuivis = new ObservableCollection<ElementGTD>();
            AutresElements = new List<ElementGTD> { };
        }

        public bool ProchainExiste
        {
            get => _indiceCourant < ListeEntrees.Count - 1;
        }

        public void AllerAuProchain()
        {
            if(ProchainExiste)
            {
                _indiceCourant++;
            }
        }


    }
}
