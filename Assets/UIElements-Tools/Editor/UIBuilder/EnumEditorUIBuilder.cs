

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class EnumEditorUIBuilder : EditorUIBuilder
{
    public override bool IsMatch(FieldInfo fieldInfo)
    {
        return fieldInfo.FieldType.IsEnum;
    }

    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        var typedEnum = (Enum)Convert.ChangeType(Enum.Parse(fieldInfo.FieldType, Enum.GetValues(fieldInfo.FieldType).GetValue(0).ToString()), typeof(Enum));

        EnumField field = new EnumField(fieldInfo.Name, typedEnum)
        {
            value = (Enum)fieldInfo.GetValue(sourceObject)
        };
        field.RegisterValueChangedCallback((ChangeEvent<Enum> e) => fieldInfo.SetValue(sourceObject, field.value));
        field.labelElement.AddToClassList("label_min_width");
        SetFieldElement(ref currentElementData, field);
    }
}
