using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class EditorUIController
{
    public static EditorUIController Instance = null;

    //public Dictionary<string, object> DynamicFields = null;
    public List<EditorUIBuilder> UIBuilders = new List<EditorUIBuilder>();

    private void Init()
    {
        UIBuilders = new List<EditorUIBuilder>();
        var uiBuilderTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsSubclassOf(typeof(EditorUIBuilder)) && !p.IsAbstract)
            .OrderBy(p => p.Name);

        foreach (var uiBuilderType in uiBuilderTypes)
        {
            //Debug.LogError("= " + uiBuilderType.FullName);
            EditorUIBuilder uiBuilder = (EditorUIBuilder)Activator.CreateInstance(uiBuilderType);
            if (uiBuilder == null)
            {
                Debug.LogError("UIBuilder not found");
                continue;
            }

            UIBuilders.Add(uiBuilder);
        }
    }

    public static void BuildUI(object sourceObject, VisualElement root)
    {
        if (Instance == null)
        { 
            Instance = new EditorUIController();
            Instance.Init();
        }

        Instance.GenerateFieldElements(sourceObject, root);
    }

    public void GenerateFieldElements(object sourceObject, VisualElement root)
    {
        if (sourceObject == null)
        {
            Debug.LogError("Can not generate Field Elements for null object");
            return;
        }

        var fields = sourceObject.GetType().GetFields();

        foreach(FieldInfo fieldInfo in fields) {
            GenerateFieldElement(fieldInfo, sourceObject, root);
        }
    }

    public void GenerateFieldElement(FieldInfo fieldInfo, object sourceObject, VisualElement root)
    {
        //Dictionary<string, object> currentElementData = new Dictionary<string, object>();
        //currentElementData.Add(EditorUIBuilder.RootElement, root);

        var uiBuilder = UIBuilders.Find(builder => builder.IsMatch(fieldInfo));
        if (uiBuilder != null)
            uiBuilder.Process(fieldInfo, sourceObject, root);

        //if (currentElementData.ContainsKey(EditorUIBuilder.FieldElement))
          //  root?.Add((VisualElement)currentElementData[EditorUIBuilder.FieldElement]);
    }
}
