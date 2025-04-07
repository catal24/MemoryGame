using System.Windows;
using MemoryGame.ViewModels;

namespace MemoryGame.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
} 