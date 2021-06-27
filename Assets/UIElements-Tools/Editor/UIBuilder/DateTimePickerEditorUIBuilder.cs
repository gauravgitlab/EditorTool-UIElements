using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public class DateTimePickerEditorUIBuilder : EditorUIBuilderCompAttributeType<DateTimePickerAttribute>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, VisualElement root)
    {
        DateTimePickerAttribute attribute = GetAttribute<DateTimePickerAttribute>(fieldInfo, CompType);
        var field = new DateTimePickerEditor(fieldInfo.Name, attribute.Level);

        field.SetDateTimeString(fieldInfo.GetValue(sourceObject).ToString());
        field.OnChange += (ChangeEvent<string> e) =>
        {
            fieldInfo.SetValue(sourceObject, e.newValue);
        };

        root.Add(field);
    }
}
