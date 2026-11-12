using CampusChatApp.Models;
using ThreadModel = CampusChatApp.Models.Thread;

namespace CampusChatApp.Data;

public static class AppStore
{
    public static List<User> Users = new();

    public static List<ThreadModel> Threads = new();

    public static List<DirectChat> Chats = new();

}