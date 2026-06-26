using System;
using Xamarin.Forms;

namespace GlowCart.UI.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            loginButton.Clicked += OnLoginButtonClicked;
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // Add logic to handle login
            bool isAuthenticated = await AuthenticationService.LoginAsync(emailEntry.Text, passwordEntry.Text);

            if (isAuthenticated)
            {
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid login credentials.", "OK");
            }
        }
    }
}