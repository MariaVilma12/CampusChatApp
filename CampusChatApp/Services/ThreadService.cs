using CampusChatApp.Models;

namespace CampusChatApp.Services;

public class ThreadService
{
    private readonly List<CampusChatApp.Models.Thread> _threads = new();

    public IReadOnlyList<CampusChatApp.Models.Thread> GetThreads() => _threads;

    public CampusChatApp.Models.Thread AddThread(CampusChatApp.Models.Thread thread)
    {
        thread.Id = _threads.Count == 0 ? 1 : _threads.Max(t => t.Id) + 1;
        _threads.Add(thread);
        return thread;
    }

    public CampusChatApp.Models.Thread? GetThreadById(int id) => _threads.FirstOrDefault(t => t.Id == id);
}
