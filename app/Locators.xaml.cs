namespace locator;

public partial class Locators : ContentPage
{
    public Locators()
    {
        InitializeComponent();
    }

    private async void ShowLocator(object sender, EventArgs e)
    {
        Button? b = sender as Button;
        if (b != null)
        {
            var fullName = b.Text;
            string searchName;
            var parens = b.Text.IndexOf('(');
            if (parens > -1)
            {
                searchName = fullName.Substring(0, parens - 1).Trim();
            }
            else
            {
                searchName = fullName;
            }

            searchName = searchName.ToLowerInvariant();

            await Shell.Current.GoToAsync("///locator?name=" + searchName + "&fullName=" + fullName);
        }
    }
}