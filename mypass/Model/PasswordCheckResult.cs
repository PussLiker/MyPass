using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    internal class PasswordCheckResult
    {
        public string Status { get; set; }  // Статус (например, "weak", "strong")
        public string Color { get; set; }    // Цвет или код для отображения на фронте (например, зеленый для сильного пароля)
    }
}
