using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MemoryGame.Models
{
    public class Tile : INotifyPropertyChanged
    {
        private string _frontImage;
        private string _backImage;
        private string _matchedImage;
        private bool _isFlipped;
        private bool _isMatched;
        private int _id;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public Tile(int id, string frontImage, string backImage, string matchedImage)
        {
            _id = id;
            _frontImage = frontImage;
            _backImage = backImage;
            _matchedImage = matchedImage;
            _isFlipped = false;
            _isMatched = false;
        }

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string FrontImage
        {
            get => _frontImage;
            set => SetProperty(ref _frontImage, value);
        }

        public string BackImage
        {
            get => _backImage;
            set => SetProperty(ref _backImage, value);
        }

        public string MatchedImage
        {
            get => _matchedImage;
            set => SetProperty(ref _matchedImage, value);
        }

        public bool IsFlipped
        {
            get => _isFlipped;
            set
            {
                if (SetProperty(ref _isFlipped, value))
                {
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        public bool IsMatched
        {
            get => _isMatched;
            set
            {
                if (SetProperty(ref _isMatched, value))
                {
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        public string CurrentImage => IsMatched ? MatchedImage : (IsFlipped ? FrontImage : BackImage);
    }
} 