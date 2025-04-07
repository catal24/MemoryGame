using System;
using System.Collections.Generic;

namespace MemoryGame.Models
{
    public class GameState
    {
        public string Username { get; set; } = string.Empty;
        public string SelectedCategory { get; set; } = string.Empty;
        public List<TileState> Tiles { get; set; } = new();
        public int BoardRows { get; set; }
        public int BoardColumns { get; set; }
        public TimeSpan TimeRemaining { get; set; }
        public DateTime SaveTime { get; set; }
    }

    public class TileState
    {
        public int Id { get; set; }
        public string FrontImage { get; set; } = string.Empty;
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }
    }
} 