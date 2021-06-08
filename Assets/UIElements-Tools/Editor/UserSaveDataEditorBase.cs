using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class UserSaveDataEditorBase<TWindow, TData> : UIElementEditorWindow<UserSaveDataEditorBase<TWindow, TData>>
    where TWindow : UserSaveDataEditorBase<TWindow, TData>
    where TData : new()
{
    protected static string AssetDataPath = "Assets/SaveData/";
    protected static string AssetDataName
    {
        get
        {
            if (typeof(TData) == typeof(GameSettingsData))
                return Utilities.Md5Sum("game_settings_data");
            else if (typeof(TData) == typeof(UserData))
                return Utilities.Md5Sum("user_data");

            return "error";
        }
    }

    protected string WindowTitle = typeof(TWindow).Name;
    protected Vector2 WindowMinSize = new Vector2(1000, 700);

    protected static void InternalShowWindow()
    {
        TWindow wnd = GetWindow<TWindow>();
        wnd.minSize = wnd.WindowMinSize;
        wnd.titleContent = new GUIContent(wnd.WindowTitle);
        wnd.Show();
    }

    private ScrollView dataRoot;

    protected TData dataStructData;

    protected override void BuildConfig()
    {
        base.BuildConfig();
        config.Layout = EditorUIUtilities.GetResourcePath("Layouts/UserDataEditorLayout.uxml");
        config.Styles.Add(EditorUIUtilities.GetResourcePath("Styles/UserDataEditorStyles.uss"));
    }

    protected override void Setup()
    {
        base.Setup();

        dataRoot = rootVisualElement.Q<ScrollView>("config_info");

        LoadData();

        EditorUIController.BuildUI(dataStructData, dataRoot);

        var saveBtn = rootVisualElement.Q<ToolbarButton>("save_button");
        saveBtn.clickable.clicked += () =>
        {
            SaveData();
        };
    }

    private void SaveData()
    {
        Save(AssetDataPath, dataStructData, AssetDataName);
    }

    private void LoadData()
    {
        dataStructData = Load<TData>(AssetDataPath, AssetDataName);
        if (dataStructData == null)
            dataStructData = new TData();
    }

    protected override T Load<T>(string dir, string file)
    {
        if (Directory.Exists(dir))
            return LoadFromJson<T>(dir + file, false, false);

        return default;
    }

    public override void Save(string dir, object toSave, string file)
    {
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }

        SaveToJson(toSave, dir + file, false, false);
    }
}
