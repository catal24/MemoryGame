using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using System.Linq;
using MemoryGame.Commands;
using MemoryGame.Models;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using System.Text.Json;

namespace MemoryGame.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly DispatcherTimer _timer;
        private readonly User _currentUser;
        private ObservableCollection<Tile> _tiles;
        private int _boardRows;
        private int _boardColumns;
        private string _selectedCategory;
        private TimeSpan _timeRemaining;
        private TimeSpan _timeLimit;
        private int _customRows;
        private int _customColumns;
        private Tile? _firstSelectedTile;
        private bool _canSelectTile = true;

        public GameViewModel(User currentUser)
        {
            _currentUser = currentUser;
            _tiles = new ObservableCollection<Tile>();
            _boardRows = 4;
            _boardColumns = 4;
            _selectedCategory = "Category1";
            _timeLimit = TimeSpan.FromMinutes(5);
            _timeRemaining = _timeLimit;
            _customRows = 4;
            _customColumns = 4;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            // Initialize commands
            SelectCategoryCommand = new RelayCommand(SelectCategory);
            NewGameCommand = new RelayCommand(NewGame);
            OpenGameCommand = new RelayCommand(OpenGame);
            SaveGameCommand = new RelayCommand(SaveGame);
            ShowStatisticsCommand = new RelayCommand(ShowStatistics);
            ExitCommand = new RelayCommand(Exit);
            SetBoardSizeCommand = new RelayCommand(SetBoardSize);
            SetCustomBoardSizeCommand = new RelayCommand(SetCustomBoardSize, CanSetCustomBoardSize);
            SetTimeLimitCommand = new RelayCommand(SetTimeLimit);
            TileClickCommand = new RelayCommand(TileClick);
            ShowAboutCommand = new RelayCommand(ShowAbout);
        }

        public ObservableCollection<Tile> Tiles
        {
            get => _tiles;
            set => SetProperty(ref _tiles, value);
        }

        public int BoardRows
        {
            get => _boardRows;
            set => SetProperty(ref _boardRows, value);
        }

        public int BoardColumns
        {
            get => _boardColumns;
            set => SetProperty(ref _boardColumns, value);
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }

        public string TimeRemaining => _timeRemaining.ToString(@"mm\:ss");

        public int CustomRows
        {
            get => _customRows;
            set => SetProperty(ref _customRows, value);
        }

        public int CustomColumns
        {
            get => _customColumns;
            set => SetProperty(ref _customColumns, value);
        }

        public ICommand SelectCategoryCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand ShowStatisticsCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SetBoardSizeCommand { get; }
        public ICommand SetCustomBoardSizeCommand { get; }
        public ICommand SetTimeLimitCommand { get; }
        public ICommand TileClickCommand { get; }
        public ICommand ShowAboutCommand { get; }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _timeRemaining = _timeRemaining.Subtract(TimeSpan.FromSeconds(1));
            OnPropertyChanged(nameof(TimeRemaining));

            if (_timeRemaining <= TimeSpan.Zero)
            {
                _timer.Stop();
                MessageBox.Show("Time's up! Game Over!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
                // TODO: Save statistics
            }
        }

        private void SelectCategory(object? parameter)
        {
            if (parameter is string category)
            {
                SelectedCategory = $"Category{category}";
                // Reinitialize game with new category if tiles exist
                if (Tiles.Any())
                {
                    InitializeGame();
                }
            }
        }

        private void NewGame(object? parameter)
        {
            InitializeGame();
            _timer.Start();
        }

        private void InitializeGame()
        {
            _timeRemaining = _timeLimit;
            OnPropertyChanged(nameof(TimeRemaining));

            // Create pairs of tiles
            var totalTiles = BoardRows * BoardColumns;
            var pairs = totalTiles / 2;
            var tiles = new ObservableCollection<Tile>();

            // Define image paths
            var backImage = "pack://application:,,,/MemoryGame;component/Resources/Images/back.png";
            var matchedImage = "pack://application:,,,/MemoryGame;component/Resources/Images/matched.png";

            for (int i = 0; i < pairs; i++)
            {
                // Use modulo to cycle through available images if we have fewer images than pairs
                var imageIndex = i % 8; // Assuming we have at least 8 images per category
                var frontImage = $"pack://application:,,,/MemoryGame;component/Resources/Images/{SelectedCategory}/image{imageIndex}.png";

                // Create two tiles with the same image
                tiles.Add(new Tile(i, frontImage, backImage, matchedImage));
                tiles.Add(new Tile(i, frontImage, backImage, matchedImage));
            }

            // Shuffle tiles
            var random = new Random();
            Tiles = new ObservableCollection<Tile>(tiles.OrderBy(x => random.Next()));
        }

        private void OpenGame(object? parameter)
        {
            try
            {
                // Look for user's save file
                var saveFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "games", $"{_currentUser.Username}_save.json");
                
                if (!File.Exists(saveFilePath))
                {
                    MessageBox.Show("No saved game found.", "Load Game", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Load and deserialize the game state
                var json = File.ReadAllText(saveFilePath);
                var gameState = JsonSerializer.Deserialize<GameState>(json);

                if (gameState == null || gameState.Username != _currentUser.Username)
                {
                    MessageBox.Show("Invalid save file.", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Restore game state
                _timeRemaining = gameState.TimeRemaining;
                BoardRows = gameState.BoardRows;
                BoardColumns = gameState.BoardColumns;
                SelectedCategory = gameState.SelectedCategory;

                var tiles = new ObservableCollection<Tile>();
                var backImage = "pack://application:,,,/MemoryGame;component/Resources/Images/back.png";
                var matchedImage = "pack://application:,,,/MemoryGame;component/Resources/Images/matched.png";

                foreach (var tileState in gameState.Tiles)
                {
                    var tile = new Tile(tileState.Id, tileState.FrontImage, backImage, matchedImage)
                    {
                        IsFlipped = tileState.IsFlipped,
                        IsMatched = tileState.IsMatched
                    };
                    tiles.Add(tile);
                }

                Tiles = tiles;
                OnPropertyChanged(nameof(TimeRemaining));

                // Start timer if game is not finished
                if (!Tiles.All(t => t.IsMatched) && _timeRemaining > TimeSpan.Zero)
                {
                    _timer.Start();
                }

                MessageBox.Show("Game loaded successfully!", "Load Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading game: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveGame(object? parameter)
        {
            // Create game state
            var gameState = new GameState
            {
                Username = _currentUser.Username,
                SelectedCategory = SelectedCategory,
                BoardRows = BoardRows,
                BoardColumns = BoardColumns,
                TimeRemaining = _timeRemaining,
                SaveTime = DateTime.Now,
                Tiles = Tiles.Select(t => new TileState
                {
                    Id = t.Id,
                    FrontImage = t.FrontImage,
                    IsFlipped = t.IsFlipped,
                    IsMatched = t.IsMatched
                }).ToList()
            };

            try
            {
                // Create the games directory if it doesn't exist
                var gamesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "games");
                Directory.CreateDirectory(gamesDirectory);

                // Save to user-specific file (overwriting previous save)
                var saveFilePath = Path.Combine(gamesDirectory, $"{_currentUser.Username}_save.json");
                var json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(saveFilePath, json);

                MessageBox.Show("Game saved successfully!", "Save Game", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving game: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStatistics(object? parameter)
        {
            // TODO: Show statistics window
        }

        private void Exit(object? parameter)
        {
            _timer.Stop();
            if (Application.Current.MainWindow is Window mainWindow)
            {
                mainWindow.Close();
            }
        }

        private void SetBoardSize(object? parameter)
        {
            if (parameter as string == "standard")
            {
                BoardRows = 4;
                BoardColumns = 4;
            }
        }

        private void SetCustomBoardSize(object? parameter)
        {
            if (CustomRows >= 2 && CustomRows <= 6 && CustomColumns >= 2 && CustomColumns <= 6)
            {
                BoardRows = CustomRows;
                BoardColumns = CustomColumns;
            }
        }

        private bool CanSetCustomBoardSize(object? parameter)
        {
            return CustomRows >= 2 && CustomRows <= 6 && CustomColumns >= 2 && CustomColumns <= 6;
        }

        private void SetTimeLimit(object? parameter)
        {
            // TODO: Implement time limit setting
        }

        private void TileClick(object? parameter)
        {
            if (!_canSelectTile || parameter is not Tile clickedTile || clickedTile.IsMatched || clickedTile.IsFlipped)
                return;

            clickedTile.IsFlipped = true;

            if (_firstSelectedTile == null)
            {
                _firstSelectedTile = clickedTile;
            }
            else
            {
                _canSelectTile = false;
                if (_firstSelectedTile.Id == clickedTile.Id)
                {
                    // Match found
                    _firstSelectedTile.IsMatched = true;
                    clickedTile.IsMatched = true;
                    _firstSelectedTile = null;
                    _canSelectTile = true;

                    // Check if game is won
                    if (Tiles.All(t => t.IsMatched))
                    {
                        _timer.Stop();
                        MessageBox.Show("Congratulations! You won!", "Game Won", MessageBoxButton.OK, MessageBoxImage.Information);
                        // TODO: Save statistics
                    }
                }
                else
                {
                    // No match
                    var firstTile = _firstSelectedTile;
                    _firstSelectedTile = null;

                    // Delay before flipping back
                    var delayTimer = new DispatcherTimer
                    {
                        Interval = TimeSpan.FromSeconds(1)
                    };

                    delayTimer.Tick += (s, e) =>
                    {
                        firstTile.IsFlipped = false;
                        clickedTile.IsFlipped = false;
                        _canSelectTile = true;
                        delayTimer.Stop();
                    };

                    delayTimer.Start();
                }
            }
        }

        private void ShowAbout(object? parameter)
        {
            MessageBox.Show(
                "Memory Game\n\n" +
                "Student: Balan Catalin Ioan\n" +
                "Email: catalin-ioan.balan@student.unitbv.ro\n" +
                "Group: 10LF231\n" +
                "Specialization: Computer Science",
                "About",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }
} 