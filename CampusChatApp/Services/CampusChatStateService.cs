namespace CampusChatApp.Services;

public class CampusChatStateService
{
    public event Action? DataChanged;

    public void NotifyChanged()
    {
        DataChanged?.Invoke();
    }
}
