using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mypass.Model;
<<<<<<< Updated upstream
=======
using mypass.Utilities;
using mypass.View;
using System.Windows.Controls;
>>>>>>> Stashed changes

namespace mypass.ViewModel
{
    internal class PassGenVM : Utilities.ViewModelBase
    {
        private readonly PageModel _pageModel;
<<<<<<< Updated upstream

        public PassGenVM()
        {
            _pageModel = new PageModel();
        }
=======
        public ICommand CopyTextCommand { get; }
        public ICommand PasswordGenerateCommand { get; }
        
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

        public PassGenVM()
        {
            // Создаем команду
            CopyTextCommand = new RelayCommand(CopyTextToClipboard);
            PasswordGenerateCommand = new RelayCommand(SetPass);
            _pageModel = new PageModel();
            
        }
        private void SetPass(object sender)
        {
            // Пример инициализации текста
            Text = PasswordGeneration.PasswordGenerate(10, true, false, true, false, "!");
            
        }
        private void CopyTextToClipboard(object text)
        {
            if (!string.IsNullOrWhiteSpace(text?.ToString()))
            {
                Clipboard.SetText(text.ToString());
            }
        }

>>>>>>> Stashed changes
    }
}
