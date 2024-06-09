using GTD;

namespace Tests
{
    public class TestGestionnaireGTD
    {
        private GestionnaireGTD _gestionnaireGTD;

        [SetUp]
        public void Setup()
        {
            _gestionnaireGTD = new GestionnaireGTD();
        }

        // Premier test de l'énoncé
        [Test]
        public void TestActionPosterieurePasDansListe()
        {
            //ARRANGE
            ElementGTD elem = new ElementGTD();
            DateOnly date_courante = DateOnly.FromDateTime(DateTime.Now);
            _gestionnaireGTD.AutresElements.Add(elem);
            
            
            //ACT
            elem.DateRappel = DateOnly.FromDateTime(DateTime.Now.AddDays(-1));
            if (elem.DateRappel == date_courante)
            {
                _gestionnaireGTD.ListeActions.Add(elem);
                _gestionnaireGTD.AutresElements.Remove(elem);
            }


            //ASSERT

            Assert.IsEmpty(_gestionnaireGTD.ListeActions);
        }

        // Deuxième test de l'énoncé
        [Test]
        public void TestActionVientDansProchaineAction()
        {
            //ARRANGE
            ElementGTD elem = new ElementGTD();
            DateOnly date_courante = DateOnly.FromDateTime(DateTime.Now);
            _gestionnaireGTD.AutresElements.Add(elem);

            //ACT
            _gestionnaireGTD.ListeActions.Clear();
            elem.DateRappel = DateOnly.FromDateTime(DateTime.Now);

            if (elem.DateRappel == date_courante)
            {
                _gestionnaireGTD.ListeActions.Add(elem);
                _gestionnaireGTD.AutresElements.Remove(elem);
            }


            //ASSERT
            Assert.AreSame(_gestionnaireGTD.ListeActions[0], elem);
        }

        // Troisième test de l'énoncé
        [Test]
        public void TestSuiviPasseAEntree()
        {

            //ARRANGE
            ElementGTD elem = new ElementGTD();
            DateOnly date_courante = DateOnly.FromDateTime(DateTime.Now);
            _gestionnaireGTD.ListeSuivis.Add(elem);

            //ACT
            _gestionnaireGTD.ListeSuivis.Clear();
            elem.DateRappel = DateOnly.FromDateTime(DateTime.Now);

            if (elem.DateRappel == date_courante)
            {
                _gestionnaireGTD.ListeEntrees.Add(elem);
                _gestionnaireGTD.ListeSuivis.Remove(elem);
            }


            //ASSERT
            Assert.AreSame(_gestionnaireGTD.ListeEntrees[0], elem);
        }
    }
}