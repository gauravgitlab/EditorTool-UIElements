using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class BoolEditorUIBuilder : EditorUIBuilderCompType<bool>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        var field = new Toggle(fieldInfo.Name)
        {
            value = (bool)fieldInfo.GetValue(sourceObject)
        };

        field.RegisterValueChangedCallback((ChangeEvent<bool> e) => fieldInfo.SetValue(sourceObject, field.value));
        field.labelElement.AddToClassList("lable_min_width");

        SetFieldElement(ref currentElementData, field);
    }
}
