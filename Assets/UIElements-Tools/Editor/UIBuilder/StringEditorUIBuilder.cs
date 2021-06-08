
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public class StringEditorUIBuilder : EditorUIBuilderCompType<string>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        TextField field = new TextField(fieldInfo.Name)
        {
            value = (string)fieldInfo.GetValue(sourceObject)
        };

        field.RegisterValueChangedCallback((ChangeEvent<string> e) => fieldInfo.SetValue(sourceObject, field.value));
        field.labelElement.AddToClassList("label_min_width");

        SetFieldElement(ref currentElementData, field);
    }        
}
