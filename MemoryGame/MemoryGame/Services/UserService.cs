using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using MemoryGame.Models;

namespace MemoryGame.Services
{
    public class UserService
    {
        private static UserService? _instance;
        private readonly string _usersFilePath;
        private List<User> _users;

        private UserService()
        {
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            Directory.CreateDirectory(dataDirectory); // Create Data directory if it doesn't exist
            _usersFilePath = Path.Combine(dataDirectory, "users.json");
            _users = LoadUsers();
        }

        public static UserService Instance
        {
            get
            {
                _instance ??= new UserService();
                return _instance;
            }
        }

        public List<User> GetUsers()
        {
            return _users;
        }

        public User? GetUser(string username)
        {
            return _users.Find(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            SaveUsers();
        }

        public void UpdateUser(User user)
        {
            var existingUser = _users.Find(u => u.Username == user.Username);
            if (existingUser != null)
            {
                existingUser.GamesPlayed = user.GamesPlayed;
                existingUser.GamesWon = user.GamesWon;
                existingUser.ProfileImagePath = user.ProfileImagePath;
                SaveUsers();
            }
        }

        public void DeleteUser(User user)
        {
            // Delete user's saved game if it exists
            var gamesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "games");
            var saveFilePath = Path.Combine(gamesDirectory, $"{user.Username}_save.json");
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            // Remove user from list and save changes
            _users.Remove(user);
            SaveUsers();
        }

        private List<User> LoadUsers()
        {
            if (File.Exists(_usersFilePath))
            {
                string json = File.ReadAllText(_usersFilePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            return new List<User>();
        }

        public void SaveUsers()
        {
            string json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_usersFilePath, json);
        }
    }
} 