using System;

namespace MemoryGame.Models
{
    public class User
    {
        public string Username { get; set; }
        public string ProfileImagePath { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }

        public User(string username, string profileImagePath)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username));
            ProfileImagePath = profileImagePath ?? throw new ArgumentNullException(nameof(profileImagePath));
            GamesPlayed = 0;
            GamesWon = 0;
        }

        // Parameterless constructor for JSON deserialization
        public User()
        {
            Username = string.Empty;
            ProfileImagePath = string.Empty;
            GamesPlayed = 0;
            GamesWon = 0;
        }
    }
} 