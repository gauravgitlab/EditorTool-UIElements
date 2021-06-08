using System.Collections.Generic;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;
using UnityEngine;

public class UIElementEditorWindow<TWindow> : EditorWindow where TWindow : UIElementEditorWindow<TWindow>
{
    public class Config
    {
        public string Title;
        public string Layout;
        public List<string> Styles = new List<string>();

        public float minWidth = 700;
        public float minHeight = 400;
    }

    protected Config config;
    public string PanelName => config.Title;

    protected virtual void BuildConfig()
    {
        config = new Config
        {
            Title = "",
            Layout = "",
            minWidth = 700,
            minHeight = 400
        };
        config.Styles.Add(EditorUIUtilities.GetResourcePath("Styles/BaseEditorStyle.uss"));
    }

    public static TWindow CreateWindowEditor()
    {
        TWindow wnd = GetWindow<TWindow>();
        return wnd;
    }

    public void OnEnable()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        BuildConfig();

        minSize = new UnityEngine.Vector2(config.minWidth, config.minHeight);
        titleContent = new UnityEngine.GUIContent(config.Title);

        // uxml
        if (!string.IsNullOrEmpty(config.Layout))
        {
            var visualTree = EditorUIUtilities.LoadVisualTreeAsset(config.Layout);
            visualTree.CloneTree(rootVisualElement);
        }

        // styles
        foreach (var style in config.Styles)
        {
            rootVisualElement.styleSheets.Add(EditorUIUtilities.LoadStyleSheet(style));
        }
    }

    public virtual void Save(string dir, object toSave, string file)
    {
        if(Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }

        //CleanDirectory(dir);
        SaveToJson(toSave, dir + file);
    }

    protected virtual T Load<T>(string dir, string file)
    {
        if (Directory.Exists(dir))
            return LoadFromJson<T>(dir + file);

        return default;
    }

    protected void CleanDirectory(string dir, bool backupDeleted = true)
    {
        List<string> files = new List<string>(Directory.GetFiles(dir));
        if(files.Count > 0)
        {
            string deletedDir = dir + "Deleted~/" + System.DateTime.UtcNow.Ticks.ToString() + "/";
            files.ForEach((string file) =>
            {
                if(file.Contains(".meta") || backupDeleted == false)
                {
                    File.Delete(file);
                }
                else
                {
                    if(Directory.Exists(deletedDir) == false)
                    {
                        Directory.CreateDirectory(deletedDir);
                    }
                    File.Move(file, file.Replace(dir, deletedDir));
                }
            });
        }
    }

    protected static void SaveToJson(object objectToSerialize, string filePath, bool autoExtention = true, bool useConvert = true)
    {
        if (Path.GetExtension(filePath) != ".json" && autoExtention)
            filePath += ".json";

        //var settings = new JsonSerializerSettings();
        //settings.TypeNameHandling = TypeNameHandling.Objects;

        if (!string.IsNullOrEmpty(filePath))
        {
            string jsonString = useConvert ? JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented) : JsonUtility.ToJson(objectToSerialize);
            File.WriteAllText(filePath, jsonString);
        }
    }

    protected static T LoadFromJson<T>(string filePath, bool autoExtention = true, bool useConvert = true)
    {
        if (Path.GetExtension(filePath) != ".json" && autoExtention)
            filePath += ".json";

        //var settings = new JsonSerializerSettings();
        //settings.TypeNameHandling = TypeNameHandling.Objects;

        if (!string.IsNullOrEmpty(filePath))
        {
            string jsonString = File.ReadAllText(filePath);
            if (useConvert)
                return JsonConvert.DeserializeObject<T>(jsonString);
            else
                return JsonUtility.FromJson<T>(jsonString);
        }

        return default;
    }
}
