using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace mypass.Model
{
    internal class OpenLink
    {
        public static void Open(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            // Проверяем, содержит ли ссылка схему (http:// или https://)
            if (!Regex.IsMatch(url, @"^https?://", RegexOptions.IgnoreCase))
            {
                url = "https://" + url;
            }

            // Проверяем, является ли ссылка валидным URL
            Uri uriResult;
            if (Uri.TryCreate(url, UriKind.Absolute, out uriResult) &&
                (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                // Дополнительная проверка на то, что в URL есть доменное имя и оно не является просто строкой
                if (Regex.IsMatch(url, @"^(https?://)?([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,6}(/[\w-]*)*$"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                else
                {
                    Debug.WriteLine("Неправильный URL (не содержит доменное имя): " + url);
                }
            }
            else
            {
                Debug.WriteLine("Неправильный URL: " + url);
            }
        }
    }
}
