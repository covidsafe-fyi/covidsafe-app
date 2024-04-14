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
                searchName = fullName.Substring(parens + 1, fullName.Length - parens - 2);
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