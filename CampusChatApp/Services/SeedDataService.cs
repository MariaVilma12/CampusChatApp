using CampusChat.Data;

namespace CampusChat.Services;

public class SeedDataService
{
    public static void Initialize()
    {
        _ = SeedData.Users;
        _ = SeedData.Threads;
    }
}
