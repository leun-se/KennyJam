using System.Collections.Generic;

public static class LevelProgress
{
    private static HashSet<string> completedLevels = new HashSet<string>();

    public static void MarkCompleted(string levelName)
    {
        completedLevels.Add(levelName);
    }

    public static bool IsCompleted(string levelName)
    {
        return completedLevels.Contains(levelName);
    }
}