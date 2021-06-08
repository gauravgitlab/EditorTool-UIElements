
using UnityEditor;

public class ArenaDataEditor : BaseDataEditor<ArenaDataEditor, ArenaEditorEntry, ArenaEditorEntry_Data>
{
    [MenuItem("Editors/Arena Editor")]
    public static void ShowWindow()
    {
        InternalShowWindow();
    }

    protected override ArenaEditorEntry_Data CreateNewEntryData()
    {
        return new ArenaEditorEntry_Data()
        {
            arenaEntry = new ArenaEditorEntry_Data.ArenaEntry()
            {
                ArenaType = eArena.BlackStone,
                Name = "BlackStone"
            }
        };
    }
}
