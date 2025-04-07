using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MemoryGame.Commands;
using MemoryGame.Models;
using MemoryGame.Services;
using System.Windows;
using MemoryGame.Views;

namespace MemoryGame.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private User? _selectedUser;
        private string _newUsername = string.Empty;
        private string _newUserImagePath = string.Empty;

        public LoginViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>(_userService.GetUsers());

            AddUserCommand = new RelayCommand(AddUser, CanAddUser);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            PlayCommand = new RelayCommand(Play, CanPlay);
            BrowseImageCommand = new RelayCommand(BrowseImage);
        }

        public ObservableCollection<User> Users { get; }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                SetProperty(ref _selectedUser, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                SetProperty(ref _newUsername, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string NewUserImagePath
        {
            get => _newUserImagePath;
            set
            {
                SetProperty(ref _newUserImagePath, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand BrowseImageCommand { get; }

        private bool CanAddUser(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(NewUsername) && 
                   !string.IsNullOrWhiteSpace(NewUserImagePath);
        }

        private void AddUser(object? parameter)
        {
            var newUser = new User(NewUsername, NewUserImagePath);
            _userService.AddUser(newUser);
            Users.Add(newUser);
            NewUsername = string.Empty;
            NewUserImagePath = string.Empty;
        }

        private bool CanDeleteUser(object? parameter)
        {
            return SelectedUser != null;
        }

        private void DeleteUser(object? parameter)
        {
            if (SelectedUser != null)
            {
                _userService.DeleteUser(SelectedUser);
                Users.Remove(SelectedUser);
            }
        }

        private bool CanPlay(object? parameter)
        {
            return SelectedUser != null;
        }

        private void Play(object? parameter)
        {
            if (SelectedUser != null)
            {
                var gameView = new GameView(SelectedUser);
                gameView.Show();
                if (Application.Current.MainWindow is Window mainWindow)
                {
                    mainWindow.Close();
                }
                Application.Current.MainWindow = gameView;
            }
        }

        private void BrowseImage(object? parameter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif)|*.jpg;*.jpeg;*.png;*.gif"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                NewUserImagePath = openFileDialog.FileName;
            }
        }
    }
} 