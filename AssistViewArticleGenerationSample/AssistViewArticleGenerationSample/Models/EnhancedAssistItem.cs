using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssistViewArticleGenerationSample.Models
{
    /// <summary>
    /// Enhanced message item with rating and action support
    /// </summary>
    public class EnhancedAssistItem : INotifyPropertyChanged
    {
        private int _helpfulCount = 0;
        private int _unhelpfulCount = 0;
        private MessageRating _userRating = MessageRating.None;

        /// <summary>
        /// Gets or sets the message ID
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Gets or sets the message text/content
        /// </summary>
        public string MessageText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether this is a user message
        /// </summary>
        public bool IsUserMessage { get; set; }

        /// <summary>
        /// Gets or sets the timestamp
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the HTML-formatted content
        /// </summary>
        public string HtmlContent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of response sections
        /// </summary>
        public List<ResponseSection> Sections { get; set; } = new();

        /// <summary>
        /// Gets or sets the helpful count
        /// </summary>
        public int HelpfulCount
        {
            get => _helpfulCount;
            set
            {
                if (_helpfulCount != value)
                {
                    _helpfulCount = value;
                    OnPropertyChanged(nameof(HelpfulCount));
                }
            }
        }

        /// <summary>
        /// Gets or sets the unhelpful count
        /// </summary>
        public int UnhelpfulCount
        {
            get => _unhelpfulCount;
            set
            {
                if (_unhelpfulCount != value)
                {
                    _unhelpfulCount = value;
                    OnPropertyChanged(nameof(UnhelpfulCount));
                }
            }
        }

        /// <summary>
        /// Gets or sets the user's rating
        /// </summary>
        public MessageRating UserRating
        {
            get => _userRating;
            set
            {
                if (_userRating != value)
                {
                    _userRating = value;
                    OnPropertyChanged(nameof(UserRating));
                }
            }
        }

        /// <summary>
        /// Gets or sets whether actions are visible
        /// </summary>
        public bool ShowActions { get; set; } = true;

        /// <summary>
        /// Gets the formatted timestamp
        /// </summary>
        public string FormattedTime => Timestamp.ToString("hh:mm tt");

        /// <summary>
        /// Gets the helpful percentage
        /// </summary>
        public string HelpfulPercentage
        {
            get
            {
                int total = HelpfulCount + UnhelpfulCount;
                if (total == 0) return "0%";
                int percentage = (HelpfulCount * 100) / total;
                return $"{percentage}%";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Enum for message rating
    /// </summary>
    public enum MessageRating
    {
        None = 0,
        Helpful = 1,
        Unhelpful = 2
    }
}
