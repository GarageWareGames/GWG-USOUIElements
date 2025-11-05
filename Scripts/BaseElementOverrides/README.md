# Base Elements Overrides
This system uses the underlying UIELements built into the Unity UI Toolkit system and overrides them with additional functionality.

- Each item in this folder iuses the same name as the base component with the Uso prefix added.
- Each file impliments the IUsoUiElement interface that sets up the validation and other base functions
- All items use the same pattern and offer validation indicators and data binding methods

## Constructors
- Uso[Object](string fieldname)
- Uso[Object](string fieldname, out string newFieldName)
- Uso[Object](string fieldname, string fieldtext(if applicaple))
- Uso[Object](string fieldname, string fieldtext(if applicaple), out string newFieldName)
- Uso[Object](string fieldname, string fieldtext(if applicaple), string bindingPath, BindingMode bindingMode)
- Uso[Object](string fieldname, string fieldtext(if applicaple), string bindingPath, BindingMode bindingMode, out string newFieldName)
- Uso[Object](string fieldname, string bindingPath, BindingMode bindingModee)
- Uso[Object](string fieldname, string bindingPath, BindingMode bindingMode, out string newFieldName)

## Methods
- SetFieldStatus(FieldStatusTypes fieldStatus)
- ShowFieldStatus(bool status)
