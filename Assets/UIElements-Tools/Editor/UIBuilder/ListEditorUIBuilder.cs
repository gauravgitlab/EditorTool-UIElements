using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public class ListEditorUIBuilder : EditorUIBuilderCompGenericValueType
{
    public ListEditorUIBuilder() : base(typeof(List<>))
    {

    }

    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        Type generic = typeof(ListEditor<>);
        Type[] typeArgs = fieldInfo.FieldType.GetGenericArguments();
        Type constructed = generic.MakeGenericType(typeArgs);

        // color alternate format
        string[] altFmt = null;
        if (IsAttributeDefined<AlternatingFormattingAttribute>(fieldInfo))
        {
            altFmt = GetAttribute<AlternatingFormattingAttribute>(fieldInfo).Formatting;
        }

        LayoutVisualElement field = Activator.CreateInstance(constructed) as LayoutVisualElement;
        field.ExternalSetup(fieldInfo.Name, fieldInfo.GetValue(sourceObject), altFmt, null);
        field.OnValueChanged += (ChangeEvent<object> e) =>
        {
            object target = sourceObject;
            object newVal = e.newValue;
            fieldInfo.SetValue(target, newVal);
        };

        SetFieldElement(ref currentElementData, field);
    }

}
