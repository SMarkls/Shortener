using System.Text.RegularExpressions;

namespace StatisticService.Utils;

public static class BrowserParser
{
    public static string GetBrowserName(string? userAgent)
    {
        var browser = "another";
        if (userAgent is null)
        {
            return browser;
        }

        browser = Regex.IsMatch(userAgent, "ucbrowser", RegexOptions.IgnoreCase) ? "UCBrowser" : browser;
        browser = Regex.IsMatch(userAgent, "edg", RegexOptions.IgnoreCase) ? "Edge" : browser;
        browser = Regex.IsMatch(userAgent, "googlebot", RegexOptions.IgnoreCase) ? "GoogleBot" : browser;
        browser = Regex.IsMatch(userAgent, "chromium", RegexOptions.IgnoreCase) ? "Chromium" : browser;
        browser = Regex.IsMatch(userAgent, "firefox|fxios", RegexOptions.IgnoreCase) && !Regex.IsMatch(userAgent, "seamonkey", RegexOptions.IgnoreCase) ? "Firefox" : browser;
        browser = Regex.IsMatch(userAgent, "; msie|trident", RegexOptions.IgnoreCase) && !Regex.IsMatch(userAgent, "ucbrowser", RegexOptions.IgnoreCase) ? "IE" : browser;
        browser = Regex.IsMatch(userAgent, "chrome|crios", RegexOptions.IgnoreCase) &&
                  !Regex.IsMatch(userAgent, "opr|opera|chromium|edg|ucbrowser|googlebot", RegexOptions.IgnoreCase)
            ? "Chrome"
            : browser;
        browser = Regex.IsMatch(userAgent, "safari", RegexOptions.IgnoreCase) &&
                  !Regex.IsMatch(userAgent, "chromium|edg|ucbrowser|chrome|crios|opr|opera|fxios|firefox", RegexOptions.IgnoreCase)
            ? "Safari"
            : browser;
        browser = Regex.IsMatch(userAgent, "opr|opera", RegexOptions.IgnoreCase) ? "Opera" : browser;

        return browser;
    }
}
