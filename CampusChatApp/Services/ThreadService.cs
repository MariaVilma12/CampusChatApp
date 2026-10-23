using CampusChat.Data;
using CampusChat.Models;
using ThreadModel = CampusChat.Models.Thread;

namespace CampusChat.Services;

public class ThreadService
{
    /// <summary>
    /// Returns every thread.
    /// </summary>
    public IReadOnlyList<ThreadModel> GetAllThreads()
    {
        return SeedData.Threads;
    }

    /// <summary>
    /// Returns one thread.
    /// Returns null if the thread does not exist.
    /// </summary>
    public ThreadModel? GetThread(int id)
    {
        return SeedData.Threads.FirstOrDefault(t => t.Id == id);
    }

    /// <summary>
    /// Searches thread names.
    /// Returns an empty list if nothing matches.
    /// </summary>
    public IReadOnlyList<ThreadModel> SearchThreads(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return SeedData.Threads;

        return SeedData.Threads
            .Where(t => t.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
