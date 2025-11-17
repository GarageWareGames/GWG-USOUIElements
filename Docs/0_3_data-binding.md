# Data Binding {#data-binding}

## Overview
Data binding allows you to bind a property in a data source object to a 
property in a target object. In the standard UI Elements package this is done 
by:
* Creating the element
* Setting the datasource (if needed)
* Setting the binding path

While this is simple, it can be a little verbose and VERY repetitive as you 
must do all these steps for every element.

In UsoUIElements we have built this <i>(and several other repetitive tasks)</i> 
into constructor methods for evey field so that data binding can be done at 
creation time or later in a loop if needed.

## Syntax
All elements support data binding and can be setup at creation time and will 
apply the default bindpath.<br>
All elements have the same signiture structure for simplicity
```
newElement([newElementName], [bindingPath], [bindingMode]);
```
When binding AFTER creation you can use the ApplyBinding() method to bind to 
any available property in the element.
~~~
newElement.ApplyBinding([elementProperty],[bindingPath], [bindingMode]);
~~~

There are also several other constructors for each of the elements that 
allow adding text, or other related elements settings, but the one 
shown above is the most basic way of creating and binding the element at 
creation time.<br>
For details on available Contructors see the documentation for the element type.

## Examples
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

