using System;

public class TimeSpanPickerAttribute : Attribute
{
    public enum EditableLevel
    {
        Weeks,
        Days,
        Hours,
        Minutes,
        Seconds,
        Max
    }

    public EditableLevel LevelMax = EditableLevel.Weeks;
    public EditableLevel LevelMin = EditableLevel.Seconds;
}
