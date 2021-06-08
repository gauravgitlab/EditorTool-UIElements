using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class FloatEditorUIBuilder : EditorUIBuilderCompType<float>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        var floatField = new FloatField(fieldInfo.Name)
        {
            value = (float)fieldInfo.GetValue(sourceObject)
        };
        floatField.RegisterValueChangedCallback((ChangeEvent<float> e) => fieldInfo.SetValue(sourceObject, floatField.value));
        floatField.labelElement.AddToClassList("label_min_width");
        SetFieldElement(ref currentElementData, floatField);
    }
}
