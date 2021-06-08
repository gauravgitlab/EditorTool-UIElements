using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DateTimePickerEditor : LayoutVisualElement
{
    public enum EditableLevel
    {
        Years,
        Months,
        Days,
        Hours,
        Minutes,
        Seconds
    }

    public enum Months
    {
        January,
        Febuary,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    private readonly string elementName;
    private readonly EditableLevel level;

    public Action<ChangeEvent<string>> OnChange;

    private TextField compiled;

    private PopupField<int> years;
    private EnumField months;
    private VisualElement daysRoot;
    private PopupField<int> days;
    private PopupField<int> hours;
    private PopupField<int> minutes;
    private PopupField<int> seconds;

    public DateTimePickerEditor(string name, EditableLevel _level) : base(false)
    {
        elementName = name;
        level = _level;
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

        years = new PopupField<int>(GetYears(), 4);
        years.AddToClassList("fill_row");
        years.AddToClassList("flex_basis_1");
        years.RegisterValueChangedCallback((ChangeEvent<int> e) =>
        {
            CompileDateTime();
        });
        inputArea.Add(years);

        if (level >= EditableLevel.Months)
        {
            months = new EnumField(Months.January);
            months.AddToClassList("fill_row");
            months.AddToClassList("flex_basis_1");
            months.RegisterValueChangedCallback((ChangeEvent<Enum> e) =>
            {
                UpdateDaysDropdown();
                CompileDateTime();
            });
            inputArea.Add(months);
        }

        if (level >= EditableLevel.Days)
        {
            daysRoot = new VisualElement();
            daysRoot.AddToClassList("fill_row");
            daysRoot.AddToClassList("flex_basis_1");
            //DateTime.DaysInMonth()
            UpdateDaysDropdown();
            inputArea.Add(daysRoot);
        }

        if (level >= EditableLevel.Hours)
        {
            List<int> listH = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            hours = new PopupField<int>(listH, 0);
            hours.AddToClassList("fill_row");
            hours.AddToClassList("flex_basis_1");
            hours.RegisterValueChangedCallback((ChangeEvent<int> e) =>
            {
                CompileDateTime();
            });
            inputArea.Add(hours);
        }

        if (level >= EditableLevel.Minutes)
        {
            List<int> listM = new List<int>();
            for (int i = 0; i < 60; ++i)
            {
                listM.Add(i);
            }
            minutes = new PopupField<int>(listM, 0);
            minutes.AddToClassList("fill_row");
            minutes.AddToClassList("flex_basis_1");
            minutes.RegisterValueChangedCallback((ChangeEvent<int> e) =>
            {
                CompileDateTime();
            });
            inputArea.Add(minutes);
        }

        if (level >= EditableLevel.Seconds)
        {
            List<int> listS = new List<int>();
            for (int i = 0; i < 60; ++i)
            {
                listS.Add(i);
            }
            seconds = new PopupField<int>(listS, 0);
            seconds.AddToClassList("fill_row");
            seconds.AddToClassList("flex_basis_1");
            seconds.RegisterValueChangedCallback((ChangeEvent<int> e) =>
            {
                CompileDateTime();
            });
            inputArea.Add(seconds);
        }

        compiled = new TextField();
        compiled.AddToClassList("fill_row");
        compiled.AddToClassList("flex_basis_1");
        compiled.AddToClassList("fill_grow_2");
        compiled.RegisterCallback<FocusOutEvent>((FocusOutEvent e) =>
        {
            SetDateTimeString(compiled.value, true);
        });
        inputArea.Add(compiled);
    }

    private List<int> GetDays()
    {
        List<int> retVal = new List<int>();
        int end = DateTime.DaysInMonth(years.value, (int)(Months)months.value + 1);
        for (int i = 0; i < end; i++)
        {
            retVal.Add(1 + i);
        }
        return retVal;
    }

    private List<int> GetYears()
    {
        List<int> years = new List<int>();
        int start = DateTime.UtcNow.Year - 4;
        for (int i = 0; i < 15; i++)
        {
            years.Add(start + i);
        }

        return years;
    }

    private void UpdateDaysDropdown()
    {
        int index = 0;
        List<int> listD = GetDays();
        if (days != null)
        {
            if (days.userData != null && (int)days.userData == listD.Count)
                return;

            days.RemoveFromHierarchy();
            index = days.index;
            days = null;
        }

        days = new PopupField<int>(listD, index < listD.Count ? index : 0);
        days.AddToClassList("fill_row");
        days.AddToClassList("flex_basis_1");
        days.userData = listD.Count;
        days.RegisterValueChangedCallback((ChangeEvent<int> e2) =>
        {
            CompileDateTime();
        });
        daysRoot.Add(days);
    }

    public void CompileDateTime()
    {
        DateTime dateTime = new DateTime();
        dateTime = dateTime.AddYears(years.value - 1);

        if(level >= EditableLevel.Months)
        {
            int temp = (int)(Months)months.value;
            dateTime = dateTime.AddMonths(temp);
        }

        if(level >= EditableLevel.Days)
        {
            dateTime = dateTime.AddDays(days.value - 1);
        }

        if(level >= EditableLevel.Hours)
        {
            dateTime = dateTime.AddHours(hours.value);
        }

        if (level >= EditableLevel.Minutes)
        {
            dateTime = dateTime.AddMinutes(minutes.value);
        }

        if (level >= EditableLevel.Seconds)
        {
            dateTime = dateTime.AddSeconds(seconds.value);
        }

        string newVal = dateTime.ToString("u");
        if(newVal != compiled.value)
        {
            ChangeEvent<string> changeEvent = ChangeEvent<string>.GetPooled(compiled.value, newVal);
            compiled.value = newVal;

            OnChange?.Invoke(changeEvent);
        }
    }

    public void SetDateTimeString(string value, bool viaInternal = false)
    {
        if (DateTime.TryParse(value, out DateTime dateTime))
        {
            dateTime = dateTime.ToUniversalTime();
            string newVal = dateTime.ToString("u");
            if (newVal != compiled.value || viaInternal)
            {
                years.value = dateTime.Year;
                if (months != null)
                    months.value = (Months)(dateTime.Month - 1);
                if (days != null)
                {
                    UpdateDaysDropdown();
                    days.value = dateTime.Day;
                }
                if (hours != null)
                    hours.value = dateTime.Hour;

                ChangeEvent<string> changeEvent = ChangeEvent<string>.GetPooled(compiled.value, newVal);
                compiled.value = newVal;

                OnChange?.Invoke(changeEvent);
            }
        }
    }
}
