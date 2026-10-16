namespace CampusChatApp.Helpers;

public static class ValidationHelper
{
    public static bool IsValidEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value.Contains('@') && value.Contains('.');
    }
}
