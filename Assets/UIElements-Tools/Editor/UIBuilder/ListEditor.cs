using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class ListEditor<T> : LayoutVisualElement
{
    private string elementName;
    private List<T> sourceListValue;
    private VisualElement elements;
    private string[] alternatingFormatting;
    private int formattingIndex = 0;
    private Dictionary<string, object> dynamicFields;

    public ListEditor() : base(false) { }

    public override void ExternalSetup(params object[] param)
    {
        elementName = param[0] as string;
        sourceListValue = param[1] as List<T>;
        alternatingFormatting = param[2] as string[];
        dynamicFields = param[3] as Dictionary<string, object>;
        Setup();
    }

    protected override void Setup()
    {
        base.Setup();
        AddToClassList("fill_column");

        Label label = new Label(elementName);
        label.AddToClassList("fixed_width_150");
        Add(label);

        VisualElement indented = new VisualElement();
        indented.AddToClassList("fill_column");
        indented.AddToClassList("padding_left_16");

        elements = new VisualElement();
        elements.AddToClassList("fill_column");
        indented.Add(elements);

        if(sourceListValue != null)
        {
            foreach (var element in sourceListValue)
                AddItem(element, true);
        }

        Button addBtn = new Button();
        addBtn.text = "Add";
        addBtn.clickable.clicked += () =>
        {
            T newItem;
            if (typeof(T) == typeof(string))
                newItem = (T)(object)"";
            else
                newItem = Activator.CreateInstance<T>();
            AddItem(newItem);
        };
        indented.Add(addBtn);

        Add(indented);
    }

    private void AddItem(T newItem, bool fromSrc = false)
    {
        if(fromSrc == false)
        {
            if (sourceListValue == null)
                sourceListValue = new List<T>();

            sourceListValue.Add(newItem);
        }

        ListElementEditor<T> ee = new ListElementEditor<T>(newItem, dynamicFields);
        elements.Add(ee);

        ee.OnDelete += DeleteItem;
        ee.OnValueChanged += OnListValueChanged;
        ee.OnShiftUp += ShiftUp;
        ee.OnShiftDown += ShiftDown;

        //add formating
        if(alternatingFormatting != null)
        {
            string format = alternatingFormatting[formattingIndex];
            formattingIndex += 1;
            if (formattingIndex >= alternatingFormatting.Length)
                formattingIndex = 0;

            ee.AddToClassList(format);
        }
    }

    private void DeleteItem(ListElementEditor<T> host)
    {
        elements.Remove(host);
        RebuildSrcList();
    }

    private void ShiftUp(ListElementEditor<T> host)
    {
        int index = elements.IndexOf(host);
        if(index > 0)
        {
            elements.Insert(index - 1, host);
            RebuildSrcList();
        }
    }

    private void ShiftDown(ListElementEditor<T> host)
    {
        int index = elements.IndexOf(host);
        if (index < elements.childCount - 1)
        {
            elements.Insert(index + 1, host);
            RebuildSrcList();
        }
    }

    private void RebuildSrcList()
    {
        List<T> newList = new List<T>();
        foreach(ListElementEditor<T> element in elements.Children())
        {
            newList.Add(element.data.sourceValue);
        }

        ChangeEvent<object> e = ChangeEvent<object>.GetPooled(sourceListValue, newList);
        sourceListValue = newList;
        OnValueChanged?.Invoke(e);
    }

    private void OnListValueChanged(ChangeEvent<T> e)
    {
        if(e.previousValue.Equals(e.newValue) == false)
        {
            RebuildSrcList();
        }
    }

}

public class ListElementEditor<T> : LayoutVisualElement
{
    public class Data
    {
        public T sourceValue;
    }

    public Data data = new Data();
    public Action<ListElementEditor<T>> OnDelete;
    public Action<ListElementEditor<T>> OnShiftUp;
    public Action<ListElementEditor<T>> OnShiftDown;

    public new Action<ChangeEvent<T>> OnValueChanged;

    private Dictionary<string, object> DynamicFields;

    public ListElementEditor(T value, Dictionary<string, object> dynamicFields) : base(false)
    {
        data.sourceValue = value;
        DynamicFields = dynamicFields;

        Setup();
    }

    protected override void BuildConfig()
    {
        base.BuildConfig();
        config.Layout = EditorUIUtilities.GetResourcePath("Layouts/ListElementEditorLayout.uxml");
    }

    protected override void Setup()
    {
        base.Setup();
        AddToClassList("fill_row");

        VisualElement container = this.Q<VisualElement>("entry_container");

        Button deleteBtn = this.Q<Button>("delete");
        deleteBtn.clickable.clicked += () => { OnDelete?.Invoke(this); };

        Button upBtn = this.Q<Button>("sort_up");
        upBtn.clickable.clicked += () => { OnShiftUp?.Invoke(this); };

        Button downBtn = this.Q<Button>("sort_down");
        downBtn.clickable.clicked += () => { OnShiftDown?.Invoke(this); };

        EditorUIController.BuildUI(data, container, DynamicFields);
    }
}