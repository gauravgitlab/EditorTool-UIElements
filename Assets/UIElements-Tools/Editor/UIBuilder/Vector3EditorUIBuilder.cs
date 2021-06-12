using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class Vector3EditorUIBuilder : EditorUIBuilderCompType<Vector3>
{
    public override void Process(FieldInfo fieldInfo, object sourceObject, ref Dictionary<string, object> currentElementData)
    {
        Vector3Field field = new Vector3Field(fieldInfo.Name)
        {
            value = (Vector3)fieldInfo.GetValue(sourceObject)
        };

        field.RegisterValueChangedCallback((ChangeEvent<Vector3> e) => fieldInfo.SetValue(sourceObject, field.value));
        field.labelElement.AddToClassList("label_min_width");

        SetFieldElement(ref currentElementData, field);
    }
}
