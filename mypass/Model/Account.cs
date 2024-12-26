using System.ComponentModel;

namespace mypass.Model
{
    internal class Account : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                if (_isPasswordVisible != value)
                {
                    _isPasswordVisible = value;
                    OnPropertyChanged(nameof(IsPasswordVisible));
                }
            }
        }

        public void OpenLink()
        {
            Model.OpenLink.Open(Username);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
