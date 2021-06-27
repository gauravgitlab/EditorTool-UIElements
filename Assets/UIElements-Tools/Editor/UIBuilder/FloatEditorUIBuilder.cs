using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class FloatEditorUIBuilder : EditorUIBuilderCompType<float>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, VisualElement root)
    {
        var floatField = new FloatField(fieldInfo.Name)
        {
            value = (float)fieldInfo.GetValue(sourceObject)
        };
        floatField.RegisterValueChangedCallback((ChangeEvent<float> e) => fieldInfo.SetValue(sourceObject, floatField.value));
        floatField.labelElement.AddToClassList("label_min_width");

        root.Add(floatField);
    }
}
