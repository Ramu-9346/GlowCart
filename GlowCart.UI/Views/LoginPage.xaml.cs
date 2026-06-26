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
            // Handle login logic here
            await DisplayAlert("Login", "Login button clicked", "OK");
        }
    }
}