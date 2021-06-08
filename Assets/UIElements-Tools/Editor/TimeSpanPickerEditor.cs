using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class TimeSpanPickerEditor : LayoutVisualElement
{
    private readonly string elementName;
    private readonly TimeSpanPickerAttribute.EditableLevel levelMax;
    private readonly TimeSpanPickerAttribute.EditableLevel levelMin;

    public Action<ChangeEvent<double>> OnChange;
    private double previousValue = 0;

    private Dictionary<TimeSpanPickerAttribute.EditableLevel, IntegerField> fields = new Dictionary<TimeSpanPickerAttribute.EditableLevel, IntegerField>();
    private Dictionary<TimeSpanPickerAttribute.EditableLevel, double> levelInSeconds = new Dictionary<TimeSpanPickerAttribute.EditableLevel, double>
    {
        {TimeSpanPickerAttribute.EditableLevel.Seconds, 1 },
        {TimeSpanPickerAttribute.EditableLevel.Minutes, 60 },
        {TimeSpanPickerAttribute.EditableLevel.Hours, 3600 },
        {TimeSpanPickerAttribute.EditableLevel.Days, 86400 },
        {TimeSpanPickerAttribute.EditableLevel.Weeks, 604800 }
    };

    public TimeSpanPickerEditor(string name, TimeSpanPickerAttribute.EditableLevel _levelMax, TimeSpanPickerAttribute.EditableLevel _levelMin) : base(false)
    {
        elementName = name;
        levelMax = _levelMax;
        levelMin = _levelMin;
        Setup();
    }

    protected override void Setup()
    {
        base.Setup();
        AddToClassList("fill_row");

        Label label = new Label(elementName);
        label.AddToClassList("fixed_width_150");
        Add(label);

        VisualElement inputArea = new VisualElement();
        inputArea.AddToClassList("fill_row");
        Add(inputArea);

        for (TimeSpanPickerAttribute.EditableLevel level = TimeSpanPickerAttribute.EditableLevel.Weeks; level < TimeSpanPickerAttribute.EditableLevel.Max; ++level)
        {
            if (level >= levelMax && level <= levelMin)
            {
                IntegerField field = new IntegerField(level.ToString());
                field.AddToClassList("fill_row");
                field.AddToClassList("flex_basis_1");
                field.labelElement.AddToClassList("min_width_75");
                field.value = 0;
                field.RegisterValueChangedCallback((ChangeEvent<int> e) =>
                {
                    CompileTimespan();
                });
                inputArea.Add(field);
                fields.Add(level, field);
            }
        }
    }

    public void CompileTimespan()
    {
        double span = 0;
        foreach(var field in fields)
        {
            span += field.Value.value * levelInSeconds[field.Key];
        }

        if(span != previousValue)
        {
            ChangeEvent<double> changeEvent = ChangeEvent<double>.GetPooled(previousValue, span);
            previousValue = span;
            OnChange?.Invoke(changeEvent);
        }
    }

    public void SetTimespan(double value, bool viaInternal = false)
    {
        if (value != previousValue || viaInternal)
        {
            double workingValue = value;
            for (TimeSpanPickerAttribute.EditableLevel level = TimeSpanPickerAttribute.EditableLevel.Weeks; level < TimeSpanPickerAttribute.EditableLevel.Max; ++level)
            {
                if (fields.ContainsKey(level))
                {
                    double remainder = workingValue % levelInSeconds[level];
                    workingValue -= remainder;

                    fields[level].value = (int)(workingValue / levelInSeconds[level]);

                    workingValue = remainder;
                }
            }

            ChangeEvent<double> changeEvent = ChangeEvent<double>.GetPooled(previousValue, value);
            previousValue = value;

            OnChange?.Invoke(changeEvent);
        }
    }
}
