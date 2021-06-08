using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public class AutoEditorDataEditorUIBuilder : EditorUIBuilderCompAttributeType<AutoEditorDataAttribute>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        VisualElement fieldRoot = new VisualElement();
        EditorUIController.Instance.GenerateFieldElements(fieldInfo.GetValue(sourceObject), fieldRoot, EditorUIController.Instance.DynamicFields);
        SetFieldElement(ref currentElementData, fieldRoot);
    }
}
