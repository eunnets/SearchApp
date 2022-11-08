using System.Text.RegularExpressions;

namespace SearchApp.Lib.Utilities
{
    public static class HtmlParser
    {
        public const string genericUrlPattern = "[A-Za-z0-9]+\\.+[A-Za-z0-9.]+$?";

        public static List<string> GetUrls(
            string html, 
            string tagName, 
            string attributeName,
            string attributeValue)
        {
            List<string> urls = new List<string>();
            string pattern = $"<{tagName} {attributeName}=\"{attributeValue}\">(({genericUrlPattern}).*?)</{tagName}>";
            Regex regex = new Regex(pattern);

            var matches = regex.Matches(html);
            for (int index = 0; index < matches.Count; index++)
            {
                urls.Add(matches[index].Groups[2].Value);
            }

            return urls;
        }

        public static string GetAttributeValue(
            string html,
            string tagName,
            string attributeName,
            string urlInElement = "")
        {
            string urlPattern = string.IsNullOrEmpty(urlInElement) ? genericUrlPattern : urlInElement;
            string pattern = $"<{tagName} {attributeName}=\"([^\"]*)\">(({urlPattern}).*?)</{tagName}>";
            Regex regex = new Regex(pattern);

            var matches = regex.Matches(html);
            string attributeValue = string.Empty;

            if (matches.Count > 0)
            {
                attributeValue = matches[0].Groups[1].Value;
            }

            return attributeValue;
        }
    }
}
