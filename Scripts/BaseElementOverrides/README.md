# Base Element Overrides {#base-element-overrides}
This system uses the underlying UIElements built into the Unity UI Toolkit system and overrides them with additional functionality.

- Each item in this folder uses the same name as the base component with the Uso prefix added.
- Each file implements the IUsoUiElement interface that sets up the validation and other base functions
- All items use the same pattern and offer validation indicators and data binding methods

## Constructors
- Uso[Element] (string fieldname)
- Uso[Element] (string fieldname, out string newFieldName)
- Uso[Element] (string fieldname, string fieldtext(if applicable))
- Uso[Element] (string fieldname, string fieldtext(if applicable), out string newFieldName)
- Uso[Element] (string fieldname, string fieldtext(if applicable), string bindingPath, BindingMode bindingMode)
- Uso[Element] (string fieldname, string fieldtext(if applicable), string bindingPath, BindingMode bindingMode, out string newFieldName)
- Uso[Element] (string fieldname, string bindingPath, BindingMode bindingMode)
- Uso[Element] (string fieldname, string bindingPath, BindingMode bindingMode, out string newFieldName)

## Methods
- SetFieldStatus(FieldStatusTypes fieldStatus)
- ShowFieldStatus(bool status)
- ApplyBinding([elementProperty], [bindingPath], [bindingMode])
- GetParentLineItem()
- GetParentForm()


Check out the 
[UsoUIElements Scripting Docs](https://www.garage-ware.com/uso-ui-elements/namespaceGWG_1_1UsoUIElements.html) on our website for more information.
