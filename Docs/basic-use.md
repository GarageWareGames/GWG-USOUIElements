# Basic Usage & Element Overrides {#usoui-basic-use}

[TOC]

## Drop-In Replacement Pattern
Every standard element in Unity's `UnityEngine.UIElements` has a corresponding `Uso` counterpart in `GWG.UsoUIElements`.

### Instant Construction with Callbacks
Standard UI Toolkit requires multiple lines to create a button, set its text, assign a name, add a tooltip, and register a click event. In UsoUIElements, this is a single, fluent line:

```csharp
using UnityEngine.UIElements;
using GWG.UsoUIElements;

// Single-line button creation with name, callback, and out-variable
var submitBtn = new UsoButton("SubmitButton", OnSubmitClicked, out UsoButton btnRef)
{
    text = "Submit Application",
    tooltip = "Submits all current form data for validation."
};
root.Add(submitBtn);
```

---

## Available Base Overrides
| Native Element | UsoUIElements Override | Enhancements |
|---|---|---|
| `Button` | `UsoButton` | Inline click actions, out-variable references, automatic tooltip wiring. |
| `Label` | `UsoLabel` | Inline text assignment, status indicators, dynamic formatting. |
| `TextField` | `UsoTextField` | Live validation badges, placeholder hints, delayed change callbacks. |
| `Toggle` | `UsoToggle` | Inline change delegates, custom check styling. |
| `Slider` / `SliderInt` | `UsoSlider` / `UsoSliderInt` | Value display labels, min/max bounds formatting. |
| `ListView` | `UsoListView` | Streamlined item binding delegates and dynamic re-population. |
| `Foldout` | `UsoFoldout` | Saved collapse states, header status icons. |

Proceed to [Simplified Data Binding](@ref usoui-data-binding).

any available property in the element.
~~~
newElement.ApplyBinding([elementProperty],[bindingPath], [bindingMode]);
~~~

There are also several other constructors for each of the elements that
allow adding text, or other related elements settings, but the one
shown above is the most basic way of creating and binding the element at
creation time.<br>
For details on available Contructors see the documentation for the element type.

### Examples
Using a <code>UsoLabel</code> as an example:<br>
~~~
new UsoLabel([elementName], [labelText], [labelType], [bindingPath], [bindingMode]);
-OR-
UsoLabel exampleLabel = new UsoLabel("exampleLabel", "Example Label", 
LabelType.Default, "exampleBindingPath", BindingMode.TwoTarget);
~~~
The code above would bind the default bind property (for a label it is the
.text property) to a property in the data source object named
"exampleBindingPath" in READ ONLY mode.<br>
Valid Binding Modes are:
* ToSource  - <i>Write Only</i>
* ToTarget  - <i>Read Only</i>
* TwoWay    - <i>Read/Write</i>
* TwoWayOneTime - <i>Read Only with no updates after first read</i>


