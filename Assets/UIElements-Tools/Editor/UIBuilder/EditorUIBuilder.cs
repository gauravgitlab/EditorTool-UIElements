

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UIElements;

public abstract class EditorUIBuilder
{
    public const string FieldElement = "FieldElement";
    public const string ElementLabel = "ElementLabel";
    public const string RootElement = "RootElement";

    public virtual bool IsMatch(FieldInfo fieldInfo)
    {
        return false;
    }

    public virtual void Process(FieldInfo fieldInfo, object sourceObject, VisualElement root)
    {

    }

    protected static bool IsAttributeDefined<T>(FieldInfo fieldInfo) where T : Attribute
    {
        return IsAttributeDefined(fieldInfo, typeof(T));
    }

    protected static bool IsAttributeDefined(FieldInfo fieldInfo, Type type)
    {
        return fieldInfo.IsDefined(type) || fieldInfo.FieldType.IsDefined(type);
    }

    protected static T GetAttribute<T>(FieldInfo fieldInfo) where T : Attribute
    {
        return GetAttribute<T>(fieldInfo, typeof(T));
    }

    protected static T GetAttribute<T>(FieldInfo fieldInfo, Type type) where T : Attribute
    {
        if (fieldInfo.IsDefined(type))
        {
            return fieldInfo.GetCustomAttribute<T>();
        }

        if (fieldInfo.FieldType.IsDefined(type))
        {
            return fieldInfo.FieldType.GetCustomAttribute<T>();
        }

        return null;
    } 
}

public abstract class EditorUIBuilderCompType<T> : EditorUIBuilder
{
    protected readonly Type CompType = typeof(T);

    public override bool IsMatch(FieldInfo fieldInfo)
    {
        return fieldInfo.FieldType == CompType;
    }
}

public abstract class EditorUIBuilderCompAttributeType<T> : EditorUIBuilder
{
    protected readonly Type CompType = typeof(T);

    public override bool IsMatch(FieldInfo fieldInfo)
    {
        return IsAttributeDefined(fieldInfo, CompType);
    }
}


public abstract class EditorUIBuilderCompGenericValueType : EditorUIBuilder
{
    protected readonly Type CompType;

    public EditorUIBuilderCompGenericValueType(Type type)
    {
        CompType = type;
    }

    public override bool IsMatch(FieldInfo fieldInfo)
    {
        if (fieldInfo.FieldType.IsGenericType)
        {
            Type genericDef = fieldInfo.FieldType.GetGenericTypeDefinition();
            return genericDef == CompType;
        }

        return false;
    }
}
