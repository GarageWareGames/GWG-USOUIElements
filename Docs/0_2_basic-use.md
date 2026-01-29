# Basic Use {#basic-use}

## Using the asset
Using the asset is very straight forward, all elements in the package will 
work as just as there base elements do so you simply add the 'Uso' 
prefix to any existing element that has been enabled in the package.

You can see all the currently available base element overrides in the
<code>BaseElementOverrides</code> folder.

### Using the Field Validation Indicators
Validation indicatios are enabled by default on form type fields such as 
<code>Label</code> and <code>TextField</code> elements, among others.

You can set the type of indicator by setting the FieldStatus property of the 
element like so:
```
myTextField.SetFieldStatus = FieldStatus.Error;
```
[FieldStatus Enum](@ref FieldStatus)

You can see the current status with the FieldStatus property like so:
```
myTextfield.FieldStatus
```

You can Enable/Disable the indicator with the <code>ShowFieldStatus</code> property.
```
myTextField.ShowFieldStatus = false/true;
```

