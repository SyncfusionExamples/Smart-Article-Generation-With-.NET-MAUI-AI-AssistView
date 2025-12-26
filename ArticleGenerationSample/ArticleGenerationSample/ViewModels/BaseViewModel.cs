using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArticleGenerationSample.ViewModels
{
    /// <summary>
    /// Base ViewModel class implementing INotifyPropertyChanged
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields

        private bool _isBusy = false;
        private string _title = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the ViewModel is busy
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raise property changed event
        /// </summary>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }
}
