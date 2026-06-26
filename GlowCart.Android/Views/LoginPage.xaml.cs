using System;
using Xamarin.Forms;

namespace GlowCart.Android.Views
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
            // Logic for handling login
            await DisplayAlert("Login", "Login button clicked", "OK");
        }
    }
}