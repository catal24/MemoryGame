using System.Collections.ObjectModel;
using MemoryGame.Models;
using MemoryGame.Services;

namespace MemoryGame.ViewModels
{
    public class StatisticsViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private ObservableCollection<User> _users;

        public StatisticsViewModel()
        {
            _userService = UserService.Instance;
            _users = new ObservableCollection<User>(_userService.GetUsers());
        }

        public ObservableCollection<User> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }
    }
} 