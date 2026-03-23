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
            // Logic to handle login
            if (string.IsNullOrEmpty(emailEntry.Text) || string.IsNullOrEmpty(passwordEntry.Text))
            {
                await DisplayAlert("Error", "Please enter both email and password.", "OK");
                return;
            }

            // Call to backend service for authentication
            bool isAuthenticated = await AuthService.AuthenticateUser(emailEntry.Text, passwordEntry.Text);

            if (isAuthenticated)
            {
                await Navigation.PushAsync(new HomePage());
            }
            else
            {
                await DisplayAlert("Error", "Invalid email or password.", "OK");
            }
        }
    }
}