namespace LivInParisRoussilleTeynier.UI;

public class AccountPage : Page
{
    public override void Display()
    {
        var menu = new ScrollingMenu(
            "Module Client",
            choices: ["Ajouter un compte", "Supprimer un compte", "Retour"]
        );

        Window.AddElement(menu);
        Window.ActivateElement(menu);
        var response = menu.GetResponse();
        Window.RemoveElement(menu);
        Window.Render();

        if (response?.Status != Status.Selected)
            return;

        switch (response.Value)
        {
            case 0:
                AjouterComptePage();
                break;
            case 1:
                SupprimerAccountPage();
                break;
        }
    }

    private void AjouterComptePage()
    {
        Prompt emailPrompt = new("Email :");
        Prompt passwordPrompt = new("Mot de passe :");

        List<Prompt> prompts = [emailPrompt, passwordPrompt];

        List<string> promptResponses = [];

        foreach (var prompt in prompts)
        {
            Window.AddElement(prompt);
            Window.ActivateElement(prompt);
            var response = prompt.GetResponse();
            Window.RemoveElement(prompt);
            promptResponses.Add(response?.Value ?? string.Empty);
        }

        Repository.Account.Create(null, promptResponses[0], promptResponses[1]);
        Repository.Customer.Create(null, 0m, LoyaltyRank.classic, false);
        Window.AddElement(new Text(["Client ajouté avec succès."]));
        Window.Render();
        Thread.Sleep(1000);
    }

    private void SupprimerAccountPage()
    {
        Prompt emailPrompt = new("Email du client à supprimer :");
        Window.AddElement(emailPrompt);
        Window.ActivateElement(emailPrompt);
        var response = emailPrompt.GetResponse();
        Window.RemoveElement(emailPrompt);

        if (response?.Status == Status.Selected)
        {
            var email = response.Value;
            Repository.Account.Delete(email);
            Window.AddElement(new Text(["Compte supprimé."]));

            Window.Render();
            Thread.Sleep(1000);
        }
    }
}
