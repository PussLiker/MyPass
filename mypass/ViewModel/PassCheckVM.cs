using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using mypass.Model;
using mypass.Utilities;

namespace mypass.ViewModel
{
    internal class PassCheckVM : ViewModelBase
    {
       
        private string _password;
        private string _strengthResult;
        private ObservableCollection<string> _feedback;
        private int _strengthProgress;

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string StrengthResult
        {
            get => _strengthResult;
            set
            {
                if (_strengthResult != value)
                {
                    _strengthResult = value;
                    OnPropertyChanged(nameof(StrengthResult));
                }
            }
        }

        public ObservableCollection<string> Feedback
        {
            get => _feedback;
            set
            {
                if (_feedback != value)
                {
                    _feedback = value;
                    OnPropertyChanged(nameof(Feedback));
                }
            }
        }

        public int StrengthProgress
        {
            get => _strengthProgress;
            set
            {
                if (_strengthProgress != value)
                {
                    _strengthProgress = value;
                    OnPropertyChanged(nameof(StrengthProgress));
                }
            }
        }

        public ICommand CheckPasswordCommand { get; }

        public PassCheckVM()
        {

            Feedback = new ObservableCollection<string>();
            CheckPasswordCommand = new RelayCommand(_ => CheckPassword());
        }

        private void CheckPassword()
        {

            // Проверка силы пароля
            var (strength, feedback) = PasswordValidator.CheckPasswordStrength(Password);

            StrengthProgress = strength == PasswordValidator.PasswordStrength.Strong ? 100 : strength == PasswordValidator.PasswordStrength.Medium ?
                50: strength == PasswordValidator.PasswordStrength.Weak ? 25 : 0;
            StrengthResult = $"Уровень надёжности: {strength}";

            // Добавляем отзывы о пароле
            Feedback.Clear();
            foreach (var item in feedback)
            {
                Feedback.Add(item);
            }
        }
    }
}
