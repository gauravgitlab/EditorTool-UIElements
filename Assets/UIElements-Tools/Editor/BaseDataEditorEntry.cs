using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class BaseDataEditorEntry<TEntryEditorData> : LayoutVisualElement
    where TEntryEditorData : BaseDataEditorData
{
    protected Label nameLabel;
    public VisualElement generalElement = new VisualElement();

    public Action SortUp;
    public Action SortDown;

    public TEntryEditorData data;
    protected virtual List<object> ObjectsToGenerateUI { get { return new List<object>(); } }

    public BaseDataEditorEntry(TEntryEditorData _data) : base(false)
    {
        data = _data;
        Setup();
    }

    public override void OnDemandInit()
    {
        base.OnDemandInit();

        // this one is setting up UI for heading area
        SetupUIFields();
    }

    protected virtual void SetupUIFields()
    {
        Dictionary<string, object> dynamicFields = BuildDynamicFileds();
        foreach (var obj in ObjectsToGenerateUI)
            EditorUIController.BuildUI(obj, generalElement, dynamicFields);
    }

    protected virtual Dictionary<string, object> BuildDynamicFileds()
    {
        return null;
    }

    protected override void BuildConfig()
    {
        base.BuildConfig();
        config.Layout = EditorUIUtilities.GetResourcePath("Layouts/BaseEntryLayout.uxml");
    }

    protected override void Setup()
    {
        base.Setup();

        nameLabel = this.Q<Label>("name");
        nameLabel.text = $"OK_{UnityEngine.Random.Range(1, 1000)}";

        Button up = this.Q<Button>("sort_up");
        up.clickable.clicked += () => SortUp?.Invoke();

        Button down = this.Q<Button>("sort_down");
        down.clickable.clicked += () => SortDown?.Invoke();
    }

    public virtual void Delete()
    {

    }
}
