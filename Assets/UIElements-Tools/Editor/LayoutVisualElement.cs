using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class LayoutVisualElement : VisualElement
{
    protected bool IsInitialised = false;

    public class Config
    {
        public string Layout;
        public List<string> Styles = new List<string>();
    }

    protected Config config;
    public Action<ChangeEvent<object>> OnValueChanged;

    public LayoutVisualElement(bool triggerSetup = true)
    {
        if (triggerSetup)
            Setup();
    }

    protected virtual void BuildConfig()
    {
        config = new Config() { Layout = "" };
        config.Styles.Add(EditorUIUtilities.GetResourcePath("Styles/BaseEditorStyle.uss"));
    }

    protected virtual void Setup()
    {
        BuildConfig();

        // uxml
        if (!string.IsNullOrEmpty(config.Layout))
        {
            var visualTree = EditorUIUtilities.LoadVisualTreeAsset(config.Layout);
            visualTree.CloneTree(this);
        }

        // styles
        foreach(var style in config.Styles)
        {
            styleSheets.Add(EditorUIUtilities.LoadStyleSheet(style));
        }
    }

    public virtual void ExternalSetup(params object[] param)
    {

    }

    public virtual void Select()
    {
        this.AddToClassList("selected");
        if (!IsInitialised)
            OnDemandInit();
    }

    public virtual void Deselect()
    {
        this.RemoveFromClassList("selected");
    }

    public virtual void OnDemandInit()
    {
        IsInitialised = true;
    }
}
