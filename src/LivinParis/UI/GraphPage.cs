namespace LivinParisRoussilleTeynier.UI;

public class GraphPage : Page
{
    public override void Display()
    {
        var menu = new ScrollingMenu(
            "Module Graph",
            choices:
            [
                "Exporter JSON/XML",
                "Visualiser graphe",
                "Coloration Welsh-Powell",
                "Couverture minimale Chu-Liu/Edmonds",
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
                case 0: /* ExportService.ExportToJsonXml(...) */
                    break;
                case 1: /* Visualization.ShowGraph(...) */
                    break;
                case 2: /* GraphAlgorithms.ColorGraph() */
                    break;
                case 3: /* GraphAlgorithms.FindArborescenceChuLiu() */
                    break;
            }
        }
    }
}
