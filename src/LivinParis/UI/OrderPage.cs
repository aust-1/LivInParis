namespace LivinParisRoussilleTeynier.UI;

public class OrderPage : Page
{
    public override void Display()
    {
        var service = Repository.OrderLine;
        var menu = new ScrollingMenu(
            "Module Commandes",
            choices:
            [
                "Créer une commande",
                "Modifier une commande",
                "Afficher le prix d'une commande",
                "Afficher le chemin de livraison",
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
                case 0: /* service.CreateOrder(...) */
                    break;
                case 1: /* service.ModifyOrder(...) */
                    break;
                case 2: /* service.GetTotalPrice(...) */
                    break;
                case 3: /* Appel à GraphAlgorithms.GetPartialGraphByBellmanFord(...) */
                    break;
            }
        }
    }
}
