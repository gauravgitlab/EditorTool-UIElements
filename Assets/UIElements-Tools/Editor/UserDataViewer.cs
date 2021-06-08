using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UserDataViewer : UserSaveDataEditorBase<UserDataViewer, UserData>
{
    [MenuItem("Editors/Save Viewer/User Data")]
    public static void ShowWindow()
    {
        InternalShowWindow();
    }
}

public class GameSettingDataViewer : UserSaveDataEditorBase<GameSettingDataViewer, GameSettingsData>
{
    [MenuItem("Editors/Save Viewer/Game Settings Data")]
    public static void ShowWindow()
    {
        InternalShowWindow();
    }
}
