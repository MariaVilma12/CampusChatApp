using CampusChatApp.Data;

namespace CampusChatApp.Services;

public class SeedDataService
{
    public static void Initialize()
    {
        SeedUsers.Initialize();
        SeedThreads.Initialize();
        SeedChats.Initialize();
    }
}
