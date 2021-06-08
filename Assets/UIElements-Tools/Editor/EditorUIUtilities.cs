

using UnityEditor;
using UnityEngine.UIElements;

public static class EditorUIUtilities 
{
    private const string EditorFolderPath = "Assets/UIElements-Tools/Editor/";

    public static string GetResourcePath(string resourcePath)
    {
        return (EditorFolderPath + resourcePath);
    }

    public static VisualTreeAsset LoadVisualTreeAsset(string path)
    {
        return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
    }

    public static StyleSheet LoadStyleSheet(string path)
    {
        return AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
    }
}
