using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string saveDirectory = Application.dataPath + "/Saves/";

    public static void Init()
    {
        // check if save directory exists
        if (!Directory.Exists(saveDirectory))
        {
            // if not, create it
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public static void Save(string saveString)
    {
        File.WriteAllText(saveDirectory + "save.txt", saveString);

    }

    public static string Load()
    {
        if (File.Exists(saveDirectory + "save.txt"))
        {
            string saveString = File.ReadAllText(saveDirectory + "save.txt");
            return saveString;
        }
        else
        {
            return null;
        }
    }

}
