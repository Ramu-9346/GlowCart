using System;
using Xamarin.Forms;

namespace GlowCart.Mobile.Views
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
            await DisplayAlert("Login", "Login button clicked", "OK");
        }
    }
}