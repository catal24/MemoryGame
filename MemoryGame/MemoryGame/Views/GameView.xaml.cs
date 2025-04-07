using System.Windows;
using MemoryGame.Models;
using MemoryGame.ViewModels;

namespace MemoryGame.Views
{
    public partial class GameView : Window
    {
        public GameView(User currentUser)
        {
            InitializeComponent();
            DataContext = new GameViewModel(currentUser);
        }
    }
} 