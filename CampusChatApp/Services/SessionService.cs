namespace CampusChatApp.Services;

public class SessionService
{
    public string CurrentUserName { get; set; } = "Campus Student";

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(CurrentUserName);

    public void SignIn(string userName)
    {
        CurrentUserName = userName;
    }

    public void SignOut()
    {
        CurrentUserName = string.Empty;
    }
}
