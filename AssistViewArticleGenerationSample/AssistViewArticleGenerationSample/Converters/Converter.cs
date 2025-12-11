using System;
using System.Globalization;

namespace AssistViewArticleGenerationSample.Converters
{
    /// <summary>
    /// Wraps plain HTML content with a base template that sets typography and line-height for better readability.
    /// </summary>
    public sealed class HtmlWrapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var content = value as string ?? string.Empty;
            var template = "<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">" +
                           "<style>body{font-family:-apple-system,Roboto,'Segoe UI',Arial,sans-serif;color:#1C1B1F;font-size:16px;line-height:1.6;margin:0;}" +
                           "p{margin:0 0 12px;} li{margin:0 0 8px;} h1,h2,h3{margin:16px 0 8px;} pre,code{background:#F6F6F6;border-radius:6px;padding:2px 4px;} ul,ol{padding-left:22px;}</style></head>" +
                           "<body>" + content + "</body></html>";
            return template;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a boolean into a GridLength, expanding to a fixed width when true and Star otherwise.
    /// </summary>
    public sealed class BoolToGridLengthConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b && b ? new GridLength(300) : new GridLength(1, GridUnitType.Star);
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a boolean into a GridLength, returning Star when true and 0 when false.
    /// </summary>
    public sealed class BoolToGridLengthConverterInverse : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b && b ? new GridLength(1, GridUnitType.Star) : new GridLength(0);
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Inverts a boolean value.
    /// </summary>
    public sealed class InverseBoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b ? !b : value;
        }
    }

    /// <summary>
    /// Maps a boolean to one of two strings provided via parameter in the form "FalseText|TrueText".
    /// </summary>
    public sealed class BoolConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string strParam)
            {
                var parts = strParam.Split('|');
                if (parts.Length == 2)
                {
                    return boolValue ? parts[1] : parts[0];
                }
            }
            return value;
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Returns true when the bound string is not null or empty; otherwise false.
    /// </summary>
    public sealed class IsNotNullOrEmptyConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is string s && !string.IsNullOrEmpty(s);
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
