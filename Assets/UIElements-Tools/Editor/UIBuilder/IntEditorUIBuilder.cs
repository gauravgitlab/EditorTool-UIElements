using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class IntEditorUIBuilder : EditorUIBuilderCompType<int>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        var field = new IntegerField(fieldInfo.Name)
        {
            value = (int)fieldInfo.GetValue(sourceObject)
        };

        field.RegisterValueChangedCallback((ChangeEvent<int> e) => fieldInfo.SetValue(sourceObject, field.value));
        field.labelElement.AddToClassList("label_min_width");
        SetFieldElement(ref currentElementData, field);
    }


}
