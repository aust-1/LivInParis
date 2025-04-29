namespace LivInParisRoussilleTeynier.UI;

public class StatsPage : Page
{
    public override void Display()
    {
        var menu = new ScrollingMenu(
            "Module Statistiques",
            choices:
            [
                "Nombre de livraisons par cuisinier",
                "Commandes sur une période",
                "Moyenne des prix des commandes",
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
                case 0:
                    var tv0 = new TableView(lines: Repository.Chef.GetDeliveryCountByChef(20));
                    Window.AddElement(tv0);
                    Window.Render();
                    Thread.Sleep(2000);
                    Window.RemoveElement(tv0);
                    break;
                case 1:
                    var tv1 = new TableView(
                        lines: Repository.OrderLine.GetOrdersByPeriod(
                            20,
                            DateTime.MinValue,
                            DateTime.MaxValue
                        )
                    );
                    Window.AddElement(tv1);
                    Window.Render();
                    Thread.Sleep(2000);
                    Window.RemoveElement(tv1);
                    break;
                case 2:
                    var tv2 = new Prompt(Repository.OrderLine.GetAverageOrderPrice().ToString());
                    Window.AddElement(tv2);
                    Window.Render();
                    Thread.Sleep(2000);
                    Window.RemoveElement(tv2);
                    break;
            }
        }
    }
}
