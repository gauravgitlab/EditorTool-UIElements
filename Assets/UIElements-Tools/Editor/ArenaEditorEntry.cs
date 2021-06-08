using System;
using System.Collections.Generic;

[Serializable]
public class ArenaEditorEntry_Data : BaseDataEditorData
{
    [Serializable]
    public class ArenaEntry
    {
        public string Name;
        public eArena ArenaType;
        public List<eEnemyType> Zombies;

        [AlternatingFormatting(Formatting = new string[] {"color_green_bg", "color_orange_bg"})]
        public List<ChapterLevelEditor> Levels = new List<ChapterLevelEditor>();
    }

    [Serializable]
    [AutoEditorData]
    public class ChapterLevelEditor
    {
        public int LevelID;
        public string LevelName;

        [TimeSpanPicker(LevelMax = TimeSpanPickerAttribute.EditableLevel.Minutes)]
        public double BestTime;

        [DateTimePicker(Level = DateTimePickerEditor.EditableLevel.Hours)]
        public string AvailableFrom = string.Empty;
    }

    public ArenaEntry arenaEntry;
}

public class ArenaEditorEntry : BaseDataEditorEntry<ArenaEditorEntry_Data>
{
    public ArenaEditorEntry(ArenaEditorEntry_Data data) : base(data)
    {
    }

    protected override List<object> ObjectsToGenerateUI
    {
        get
        {
            return new List<object> { data.arenaEntry };
        }
    }
}
