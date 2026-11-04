using CampusChatApp.Data;

namespace CampusChatApp.Services;

public static class AppStateBootstrap
{
    public static void Initialize()
    {
        _ = AppStore.Users;
        _ = AppStore.Threads;
        _ = AppStore.Chats;
    }
}
