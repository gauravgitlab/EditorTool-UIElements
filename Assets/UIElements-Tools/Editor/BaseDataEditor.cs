using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class BaseDataEditor<TEditor, TEntry, TEntryEditorData> : UIElementEditorWindow<TEditor>
    where TEditor : BaseDataEditor<TEditor, TEntry, TEntryEditorData>
    where TEntry : BaseDataEditorEntry<TEntryEditorData>
    where TEntryEditorData : BaseDataEditorData, new()
{
    protected static string AssetDataPath = "Assets/Data/" + typeof(TEditor).Name + "/";
    protected static string AssetDataName = typeof(TEditor).Name;

    public virtual string WindowTitle => typeof(TEditor).Name;
    public virtual Vector2 WindowMinSize { get { return new Vector2(1000, 700); } }

    protected virtual string Layout => EditorUIUtilities.GetResourcePath("Layouts/BaseEditorLayout.uxml");
    protected virtual List<string> Styles { get { return null; } }

    protected ScrollView entryList;
    protected List<TEntry> entries = new List<TEntry>();
    protected TEntry selected = null;

    protected VisualElement generalRoot;

    public BaseDataEditor() { }

    protected class SaveData
    {
        public List<TEntryEditorData> entries = new List<TEntryEditorData>();
    }

    protected static void InternalShowWindow()
    {
        TEditor wnd = GetWindow<TEditor>();
        wnd.minSize = wnd.WindowMinSize;
        wnd.titleContent = new GUIContent(wnd.WindowTitle);
        wnd.Show();
    }

    protected override void BuildConfig()
    {
        base.BuildConfig();
        config.Layout = Layout;
        if (Styles != null)
            config.Styles.AddRange(Styles);
    }

    protected override void Setup()
    {
        base.Setup();

        entryList = rootVisualElement.Q<ScrollView>("entry_list");
        generalRoot = rootVisualElement.Q<VisualElement>("entry_data");

        var saveBtn = rootVisualElement.Q<ToolbarButton>("save_button");
        saveBtn.clickable.clicked += () =>
        {
            SaveEntryData();
        };

        var exportBtn = rootVisualElement.Q<ToolbarButton>("export_button");
        exportBtn.clickable.clicked += () =>
        {
            Export();
        };

        var addBtn = rootVisualElement.Q<ToolbarButton>("add_button");
        addBtn.clickable.clicked += () =>
        {
            AddEntry();
            ApplySorting();
        };

        var copyBtn = rootVisualElement.Q<ToolbarButton>("copy_button");
        copyBtn.clickable.clicked += () =>
        {
            CopyEntry();
            ApplySorting();
        };

        var deleteBtn = rootVisualElement.Q<ToolbarButton>("delete_button");
        deleteBtn.clickable.clicked += () =>
        {
            DeleteEntry();
        };

        // loading save data
        var loadedData = Load<SaveData>(AssetDataPath, AssetDataName);
        if (loadedData != null)
        {
            foreach (TEntryEditorData data in loadedData.entries)
            {
                //ProcessLoadedEntry(data);
                AddEntry(data);
            }
        }

        ApplySorting();
    }

    protected virtual void SaveEntryData()
    {
        SaveData data = new SaveData();
        foreach (var entry in entries)
        {
            data.entries.Add(entry.data);
        }

        Save(AssetDataPath, data, AssetDataName);
    }

    protected virtual void Export()
    {
        
    }

    protected virtual void AddEntry(TEntryEditorData entryData = null)
    {
        if (entryData == null)
            entryData = CreateNewEntryData();

        TEntry entry = CreateEditorEntry(entryData);
        entry.SortUp += () => ShiftEntry(entry, true);
        entry.SortDown += () => ShiftEntry(entry, false);
        entry.RegisterCallback<MouseUpEvent>(e => SelectEntry(entry));
        entries.Add(entry);
    }

    protected virtual TEntryEditorData CreateNewEntryData()
    {
        return new TEntryEditorData();
    }

    protected virtual TEntry CreateEditorEntry(TEntryEditorData entryData)
    {
        return (TEntry)System.Activator.CreateInstance(typeof(TEntry), entryData);
    }

    protected virtual void SelectEntry(TEntry entry)
    {
        if (selected != null)
            selected.Deselect();

        selected = entry;
        selected.Select();

        generalRoot.Clear();
        generalRoot.Add(selected.generalElement);
    }

    protected virtual void CopyEntry()
    {
        if (selected == null)
            return;

        AddEntry();
    }

    protected virtual void DeleteEntry()
    {
        if (selected == null) return;

        entries.Remove(selected);
        selected.Delete();

        generalRoot.Clear();

        selected = null;
        ApplySorting();
    }

    protected virtual void ApplySorting()
    {
        entryList.Clear();
        foreach (var entry in entries)
            entryList.Add(entry);
    }

    public void ShiftEntry(TEntry entry, bool higher = true)
    {
        if (entries.Contains(entry) == false) return;
        int index = entries.FindIndex(x => x == entry);
        if(higher == true && index > 0)
        {
            entries.RemoveAt(index);
            entries.Insert(index - 1, entry);
        }
        else if(higher == false && index < entries.Count - 1)
        {
            entries.RemoveAt(index);
            entries.Insert(index + 1, entry);
        }

        ApplySorting();
    }
}
