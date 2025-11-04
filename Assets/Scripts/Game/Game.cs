public static class Game
{
    public static IGameManager Manager { get; private set; }

    public static void RegisterManager(IGameManager manager)
    {
        Manager = manager;
    }
}