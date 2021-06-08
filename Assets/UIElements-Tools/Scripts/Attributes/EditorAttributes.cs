using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEditorDataAttribute : Attribute
{ }

public class AlternatingFormattingAttribute : Attribute
{
    public string[] Formatting;
}
