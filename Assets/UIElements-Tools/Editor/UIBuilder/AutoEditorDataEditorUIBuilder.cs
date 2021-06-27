using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public class AutoEditorDataEditorUIBuilder : EditorUIBuilderCompAttributeType<AutoEditorDataAttribute>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, VisualElement root)
    {
        VisualElement fieldRoot = new VisualElement();
        EditorUIController.Instance.GenerateFieldElements(fieldInfo.GetValue(sourceObject), fieldRoot);

        root.Add(fieldRoot);
    }
}
