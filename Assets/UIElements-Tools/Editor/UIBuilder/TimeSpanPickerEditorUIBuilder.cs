using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeSpanPickerEditorUIBuilder : EditorUIBuilderCompAttributeType<TimeSpanPickerAttribute>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        TimeSpanPickerAttribute attribute = GetAttribute<TimeSpanPickerAttribute>(fieldInfo, CompType);

        var field = new TimeSpanPickerEditor(fieldInfo.Name, attribute.LevelMax, attribute.LevelMin);
        field.SetTimespan((double)fieldInfo.GetValue(sourceObject));
        field.OnChange += (ChangeEvent<double> e) =>
        {
            fieldInfo.SetValue(sourceObject, e.newValue);
        };

        SetFieldElement(ref currentElementData, field);
    }
}
