using System.IO;
using UnityEditor;
using UnityEngine;

public class LineCounter
{
    [MenuItem("Tools/CountLines")]
    private static void CountLines()
    {
        string rootPath = Application.dataPath;

        int totalLines = CountLinesInSolution(Path.Combine(rootPath, "ApeCharacter"));
        Debug.Log($"Общее количество строк кода в проекте: {totalLines}");
    }

    private static int CountLinesInSolution(string rootPath)
    {
        if (!Directory.Exists(rootPath))
        {
            Debug.LogError("Указанная папка не найдена.");
            return 0;
        }

        int totalLines = 0;
            
        string[] csFiles = Directory.GetFiles(rootPath, "*.cs", SearchOption.AllDirectories);

        foreach (var file in csFiles)
        {
            try
            {
                int fileLines = File.ReadAllLines(file).Length;
                totalLines += fileLines;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Ошибка при чтении файла {file}: {ex.Message}");
            }
        }

        return totalLines;
    }
}
