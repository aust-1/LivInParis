namespace LivInParisRoussilleTeynier.UI;

public class ChefPage : Page
{
    public override void Display()
    {
        var service = Repository.Chef;
        var menu = new ScrollingMenu(
            "Module Cuisinier",
            choices:
            [
                "Ajouter / Supprimer / Modifier",
                "Afficher clients servis",
                "Afficher plats par fréquence",
                "Afficher plat du jour",
                "Retour",
            ]
        );

        Window.AddElement(menu);
        Window.ActivateElement(menu);
        var response = menu.GetResponse();
        Window.RemoveElement(menu);
        Window.Render();

        if (response?.Status == Status.Selected)
        {
            switch (response.Value)
            {
                case 1: /* service.GetClientsServedByChef(...) */
                    break;
                case 2: /* service.GetDishFrequencies(...) */
                    break;
                case 3: /* service.GetTodayDish(...) */
                    break;
            }
        }
    }
}
