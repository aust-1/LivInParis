namespace LivinParisRoussilleTeynier.UI;

/// <summary>
/// Main menu page for navigating between application modules.
/// </summary>
public class MainMenuPage : Page
{
    public override void Display()
    {
        Title title = new("LivinParis");
        Header header = new(
            "By Eliott ROUSSILLE and François TEYNIER",
            "LivinParis - Main Menu",
            "TD E"
        );
        Footer footer = new("[ESC] Quit", "[Z | \u2191] Up, [S | \u2193] Down", "[ENTER] Select");

        Window.AddElement(title, header, footer);
        Window.Render();

        while (true)
        {
            var menu = new ScrollingMenu(
                "Liv'in Paris - Menu Principal",
                choices:
                [
                    "1. Comptes",
                    "2. Cuisiniers",
                    "3. Commandes",
                    "4. Statistiques",
                    "5. Fonctions de Graphe",
                    "Quitter",
                ]
            );

            Window.AddElement(menu);
            Window.ActivateElement(menu);
            var response = menu.GetResponse();
            Window.RemoveElement(menu);
            Window.Render();

            if (response?.Status != Status.Selected)
                break;

            switch (response.Value)
            {
                case 0:
                    new AccountPage().Display();
                    break;
                case 1:
                    new ChefPage().Display();
                    break;
                case 2:
                    new OrderPage().Display();
                    break;
                case 3:
                    new StatsPage().Display();
                    break;
                case 4:
                    new GraphPage().Display();
                    break;
                default:
                    return;
            }
        }
    }
}
