using CampusChatApp.Data;

namespace CampusChatApp.Services;

public class SeedDataService
{
    public static void Initialize()
    {
        _ = SeedData.Users;
        _ = SeedData.Threads;
    }
}
