using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows;

namespace MemoryGame.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        private static BitmapImage? _defaultImage;

        private static BitmapImage DefaultImage
        {
            get
            {
                if (_defaultImage == null)
                {
                    try
                    {
                        _defaultImage = new BitmapImage();
                        _defaultImage.BeginInit();
                        _defaultImage.UriSource = new Uri("pack://application:,,,/MemoryGame;component/Resources/Images/back.png", UriKind.Absolute);
                        _defaultImage.CacheOption = BitmapCacheOption.OnLoad;
                        _defaultImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        _defaultImage.EndInit();
                        _defaultImage.Freeze();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to load default image: {ex.Message}");
                        return CreateEmptyImage();
                    }
                }
                return _defaultImage;
            }
        }

        private static BitmapImage CreateEmptyImage()
        {
            var emptyImage = new BitmapImage();
            emptyImage.BeginInit();
            emptyImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            emptyImage.CacheOption = BitmapCacheOption.OnLoad;
            emptyImage.EndInit();
            emptyImage.Freeze();
            return emptyImage;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string imagePath || string.IsNullOrEmpty(imagePath))
                return DefaultImage;

            try
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load image {imagePath}: {ex.Message}");
                return DefaultImage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 