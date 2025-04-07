using System.Windows;
using MemoryGame.ViewModels;

namespace MemoryGame.Views
{
    public partial class StatisticsView : Window
    {
        public StatisticsView()
        {
            InitializeComponent();
            DataContext = new StatisticsViewModel();
        }
    }
} 