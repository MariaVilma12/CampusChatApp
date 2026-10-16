namespace CampusChatApp.Helpers;

public static class DateHelper
{
    public static string FormatRelativeTime(DateTime date)
    {
        var elapsed = DateTime.UtcNow - date;

        if (elapsed.TotalMinutes < 1)
            return "just now";

        if (elapsed.TotalHours < 1)
            return $"{(int)elapsed.TotalMinutes} minutes ago";

        if (elapsed.TotalDays < 1)
            return $"{(int)elapsed.TotalHours} hours ago";

        return $"{(int)elapsed.TotalDays} days ago";
    }
}
