using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mypass.Model
{
    internal class CommonPasswordsList
    {
        public static readonly HashSet<string> Passwords = new HashSet<string>
        {
         "123456", "123456789", "qwerty", "password", "12345", "qwerty123", "password123",
        "12345678", "111111", "123123", "abc123", "1234567", "000000", "iloveyou",
        "1234", "welcome", "admin", "monkey", "login", "1234567890", "letmein",
        "dragon", "football", "sunshine", "princess", "qwertyuiop", "asdfghjkl",
        "1qaz2wsx", "654321", "superman", "1q2w3e", "hello", "passw0rd", "123qwe",
        "zxcvbnm", "zaq12wsx", "starwars", "master", "shadow", "password1", "abc",
        "baseball", "trustno1", "whatever", "michael", "hunter", "batman", "test",
        "flower", "freedom", "123abc", "soccer", "letmein123", "qazwsx", "love",
        "admin123", "google", "987654321", "1a2b3c", "abc1234", "p@ssword", "pa$$w0rd",
        "123456a", "7777777", "6543210", "q1w2e3r4t5", "zaqxsw", "asdf1234", "1q2w3e4r",
        "pass1234", "admin1234", "user123", "1234abcd", "qwertyui", "iloveu", "pass123",
        "welcome1", "admin1", "1q2w3e4r5t", "a1b2c3d4", "asdfgh", "zxcvbn", "98765",
        "zaqwsx", "12345678910", "test1234", "1a2b3c4d", "adminadmin", "password1234",
        "p@ssw0rd123", "qwerty12345", "123qweasd", "asdfgh123", "qazwsxedc", "1234qwer",
        "87654321", "1qazxsw2", "zxcvbn123", "q1w2e3r4t", "passw0rd123", "userpass",
        "123456789a", "11111111", "222222", "333333", "444444", "555555", "666666",
        "888888", "999999", "00000000", "abcd123456", "passwordpassword", "letmein1",
        "123321", "password1", "iloveyou1", "qwertyqwerty", "superman123", "football1",
        "123qweasd", "123abc123", "qwerty1", "qwerty1234", "123qwe123", "iloveyou123",
        "abc12345", "qwerty12345", "welcome123", "flower123", "qwerty!123", "iloveyou1!",
        "1qazxsw2", "sunshine123", "1q2w3e4r5t", "test123", "letmein1!", "1234abcd1",
        "love123", "1qaz2wsx123", "password321", "12345abc", "happy123", "welcome!123",
        "starwars1", "qwertyabc", "iloveyou1234", "superman12", "123qwert", "dragon123",
        "qwerty1q2", "1234!@#", "mypassword", "testpassword", "qwerty12", "iloveyou2",
        "admin12345", "abc123abc", "12345!@#", "qwertyqwe", "qwerty123!@", "123qwe",
        "sunshine1", "password12", "login123", "hello123", "iloveyou321", "flower12",
        "shadow123", "dragon1", "love1234", "letmein1234", "qwerty654", "zxcvbnm123",
        "1qaz2wsxq", "admin!123", "qwertyabc1", "mypassword1", "1qazxswq", "1234pass",
        "trustno123", "111111q", "test12345", "abcdef123", "letmein12", "password3210",
        "qwerty111", "password123q", "football12", "abc123abc1", "sunshine12", "letmeinq",
        "password123456", "adminadmin1", "testpassword1", "qwertyqwe123", "monkey1234",
        "shadow1234", "dragon1234", "iloveyou12345", "123456abcd", "12345!qwerty",
        "adminpassword", "qwerty678", "123password", "starwars123", "12345xyz", "iloveyou111",
        "password12345", "superman12345", "qwerty777", "abc1234abc", "letmein!1234", "qwerty@123",
        "superman1234", "passwordpassword123", "abcdefg123", "qwertyqwerty1", "123abc321",
        "123!@#qwert", "flower12q", "qwertyapple", "michael123", "hunter123", "passwordabc",
        "qwerty!abc", "12345qwe", "passwordqwe", "test!12345", "iloveyouq", "abcdpassword",
        "1234567abc", "asdfgh1234", "monkey!123", "letmein!1", "mypass123", "1qazxsw1",
        "123456!abc", "iloveyou123123", "password123abc", "qwerty12abc", "sunshine@123",
        "testpass1", "flower!123", "starwarsabc", "qwertyzxcv", "123456abc!", "passwordletmein",
        "qwerty123q", "supermanpass", "zxcvbnm!123", "adminqwerty", "test@1234", "sunshine1abc",
        "qwerty12345abc", "monkey123q", "1q2w3e4r5t123", "letmein12345", "password@123",
        "superman123abc", "123456abcd1", "dragon123q", "adminqwerty123", "iloveyou123abc",
        "123qwertyabc", "qwertyqwe12345", "passwordabc123", "superman@123", "passwordletmein1",
        "qwertyapple1", "superman123abc", "password1234q", "1234567pass", "sunshine1234abc","пароль",
        "пароль123", "йцукен", "йцукен123", "парольйцукен", "дракон", "дракон123",
        "админ", "админ123", "цветок", "цветок123", "люблю", "люблю123", "солнце", "солнце123",
        "любимый", "любимый123", "моялюбовь", "моялюбовь123", "йцукенйцукен", "мойпароль",
        "мойпароль123", "привет", "привет123", "дракондракон", "люблюсолнце", "пароль1",
        "пароль12345", "красота", "осень", "зима", "весна", "лето", "дом123", "работа",
        "работа123", "семья", "семья123", "друг", "друг123", "приветики", "пароль999",
        "счастье", "любовь", "радость", "успех", "добро", "здоровье", "улыбка", "парольдобро",
        "день", "ночь", "паспорт", "паспорт123", "код123", "кодпароль", "любовь123",
        "ромашка", "ромашка123", "пушистик", "пушистик123", "котик", "котик123", "собачка",
        "собачка123", "птичка", "птичка123", "домдом", "работаосень", "солнцезима",
        "доброта", "котенок", "котенок123", "малыш", "малыш123", "солнышко", "солнышко123",
        "звезда", "звезда123", "ромашкаромашка", "дружба", "дружба123", "дети", "дети123",
        "праздник", "праздник123", "родина", "родина123", "паспорт999", "люблютебя",
        "люблютебя123", "романтика", "романтика123", "красота123", "природа", "природа123",
        "зеленый", "зеленый123", "море", "море123", "река", "река123", "гора", "гора123",
        "земля", "земля123", "космос", "космос123", "галактика", "галактика123", "сказка",
        "сказка123", "сказочный", "сказочный123", "другдетей", "любимыйкот", "мойдруг",
        "мойдруг123", "летосолнце", "осеннийдождь", "зимнийснег", "весенняяроса"
        };
    }
}
