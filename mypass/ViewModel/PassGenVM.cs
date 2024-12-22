using mypass.Model;
using mypass.Utilities;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace mypass.ViewModel
{
    internal class PassGenVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
        private CancellationTokenSource _cancellationTokenSource;
        public ICommand CopyTextCommand { get; }
        public ICommand PasswordGenerateCommand { get; }
        public ICommand ExitCommand { get; }

        private string _text;
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(Text)); // Уведомляем View об изменении
                }
            }
        }


        private Brush _textColor = Brushes.White;
        public Brush TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    OnPropertyChanged(nameof(TextColor));
                }
            }
        }

        public ICommand IncreasePasswordLengthCommand { get; }
        public ICommand DecreasePasswordLengthCommand { get; }

        private int _passwordLength = 16;
        public int PasswordLength
        {
            get => _passwordLength;
            set
            {
                if (_passwordLength != value && value > 3 && value <= 99) // Ограничение 4-99
                {
                    _passwordLength = value;
                    OnPropertyChanged(nameof(PasswordLength));
                }
            }
        }
        private bool _includeLowercase = true;
        public bool IncludeLowercase
        {
            get => _includeLowercase;
            set
            {
                if (_includeLowercase != value)
                {
                    _includeLowercase = value;
                    OnPropertyChanged(nameof(IncludeLowercase));
                }
            }
        }

        private bool _includeUppercase = true;
        public bool IncludeUppercase
        {
            get => _includeUppercase;
            set
            {
                if (_includeUppercase != value)
                {
                    _includeUppercase = value;
                    OnPropertyChanged(nameof(IncludeUppercase));
                }
            }
        }

        private bool _includeDigits = true;
        public bool IncludeDigits
        {
            get => _includeDigits;
            set
            {
                if (_includeDigits != value)
                {
                    _includeDigits = value;
                    OnPropertyChanged(nameof(IncludeDigits));
                }
            }
        }

        private bool _includeSpecialCharacters = true;
        public bool IncludeSpecialCharacters
        {
            get => _includeSpecialCharacters;
            set
            {
                if (_includeSpecialCharacters != value)
                {
                    _includeSpecialCharacters = value;
                    OnPropertyChanged(nameof(IncludeSpecialCharacters));
                }
            }
        }

        private string _specialCharacters = "~!@#$%^&*+-/.,\\{}[]();:?<>\"'_";
        public string SpecialCharacters
        {
            get => _specialCharacters;
            set
            {
                if (_specialCharacters != value)
                {
                    _specialCharacters = value;
                    OnPropertyChanged(nameof(SpecialCharacters));
                }
            }
        }



        public PassGenVM()
        {
            // Создаем команду
            CopyTextCommand = new RelayCommand(CopyTextToClipboard);
            PasswordGenerateCommand = new RelayCommand(SetPass);
            ExitCommand = new RelayCommand(Exit);
            _pageModel = new PageModel();

            // Пример инициализации текста
            PasswordGenerateCommand = new RelayCommand(ExecutePasswordAnimation);
            ExecutePasswordAnimation(new object());

            IncreasePasswordLengthCommand = new RelayCommand(_ => PasswordLength++, _ => PasswordLength < 99);
            DecreasePasswordLengthCommand = new RelayCommand(_ => PasswordLength--, _ => PasswordLength > 4);


        }
        private void Exit(object s)
        {
            if (s is Window w)
            { w?.Close(); }
        }
        private void SetPass(object sender)
        {
            // Пример инициализации текста
            Text = PasswordGeneration.PasswordGenerate(10, true, false, true, false, "!");

        }

        private async void ExecutePasswordAnimation(object obj)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await AnimatePasswordGeneration(_cancellationTokenSource.Token);
            }
            catch (TaskCanceledException)
            {

            }
        }

        private async Task AnimatePasswordGeneration(CancellationToken cancellationToken)
        {


            for (int i = 0; i < 6; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                // Генерация случайного промежуточного пароля
                Text = PasswordGeneration.PasswordGenerate(PasswordLength, IncludeUppercase, IncludeLowercase, IncludeDigits, IncludeSpecialCharacters, SpecialCharacters);
                TextColor = Brushes.DarkGray;

                // Имитируем задержки между сменой паролей
                await Task.Delay(50, cancellationToken);
            }

            // Финальный белый пароль
            Text = PasswordGeneration.PasswordGenerate(PasswordLength, IncludeUppercase, IncludeLowercase, IncludeDigits, IncludeSpecialCharacters, SpecialCharacters);
            TextColor = Brushes.White;
        }


        private void CopyTextToClipboard(object text)
        {
            if (!string.IsNullOrWhiteSpace(text?.ToString()))
            {
                Clipboard.SetText(text.ToString());
            }
        }

    }


}


