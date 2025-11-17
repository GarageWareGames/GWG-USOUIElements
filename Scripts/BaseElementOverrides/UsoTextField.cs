using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom text field control that extends Unity's TextField with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for string values, and custom styling through CSS classes.
    /// The control includes additional functionality such as data clearing capabilities and supports various constructor overloads
    /// for different initialization scenarios including labeling, data binding configuration, and custom data source assignment.
    /// This is one of the most commonly used input controls in the USO framework for text-based user input.
    /// </remarks>
    [UxmlElement]
    public partial class UsoTextField : TextField, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoTextField instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-text-field";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the text content of the field.
        /// </summary>
        private const string DefaultBindProp = "value";

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback.
        /// This property is automatically reflected in the UI through CSS class modifications.
        /// </summary>
        /// <value>The current FieldStatusTypes value indicating the field's validation state.</value>
        [UxmlAttribute]
        public FieldStatusTypes FieldStatus
        {
            get
            {
                return _fieldStatus;
            }
            private set
            {
                _fieldStatus = value;
                UsoUiHelper.SetFieldStatus(this, value);
            }
        }
        private FieldStatusTypes _fieldStatus;

        /// <summary>
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is true.</value>
        [UxmlAttribute]
        public bool FieldStatusEnabled
        {
            get
            {
                return _fieldStatusEnabled;
            }

            private set
            {
                _fieldStatusEnabled = value;
                if (value)
                {
                    AddToClassList(ElementValidationClass);
                }
                else
                {
                    RemoveFromClassList(ElementValidationClass);
                }
            }
        }
        private bool _fieldStatusEnabled = true;

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        /// <summary>
        /// Applies data binding to the specified property of this control using Unity's data binding system.
        /// Configures the binding with the provided path and mode for automatic data synchronization.
        /// </summary>
        /// <param name="fieldBindingProp">The property name on this control to bind to.</param>
        /// <param name="fieldBindingPath">The path to the data source property to bind from.</param>
        /// <param name="fieldBindingMode">The binding mode that determines how data flows between source and target.</param>
        /// <exception cref="Exception">Thrown when binding setup fails. Original exception is preserved and re-thrown.</exception>
        public void ApplyBinding(string fieldBindingProp, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            try
            {
                SetBinding(fieldBindingProp, new DataBinding()
                {
                    dataSourcePath = new PropertyPath(fieldBindingPath),
                    bindingMode = fieldBindingMode
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Updates the field's status type, which affects its visual appearance and validation state.
        /// The status change is automatically reflected in the UI through the FieldStatus property.
        /// </summary>
        /// <param name="fieldStatus">The new field status type to apply.</param>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        /// <summary>
        /// Controls the visibility and functionality of the field status/validation system.
        /// When disabled, removes validation-related styling from the control.
        /// </summary>
        /// <param name="status">True to enable field status functionality; false to disable it.</param>
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        /// <summary>
        /// Clears the text field's content by setting the value to an empty string.
        /// This method provides a convenient way to reset the field's data programmatically.
        /// </summary>
        public void ClearData()
        {
            value = string.Empty;
        }

        /// <summary>
        /// Retrieves the first ancestor UsoLineItem control in the visual tree hierarchy.
        /// This is useful for accessing parent container functionality and maintaining proper UI structure.
        /// </summary>
        /// <returns>The parent UsoLineItem if found; otherwise, null.</returns>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name, label text, and returns a reference.
        /// Creates a text field with custom identification, display label, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="labelText">The label text to display alongside the text field control.</param>
        /// <param name="newFieldName">Output parameter that receives a reference to the newly created text field.</param>
        public UsoTextField(string fieldName, string labelText, out UsoTextField newFieldName) : base(labelText)
        {
            InitElement(fieldName, out newFieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name and optional label text.
        /// Creates a text field with custom identification and optional display labeling for user interface clarity.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="labelText">Optional label text to display alongside the text field control. Default is null.</param>
        public UsoTextField(string fieldName, string labelText = null) : base(labelText)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name, label text, data binding, and returns a reference.
        /// Creates a fully configured text field with custom identification, display label, automatic data binding, and immediate access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="labelText">The label text to display alongside the text field control.</param>
        /// <param name="bindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newFieldName">Output parameter that receives a reference to the newly created text field.</param>
        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, out UsoTextField newFieldName) : base(labelText)
        {
            InitElement(fieldName, bindingPath, bindingMode, out newFieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name, data binding configuration, and returns a reference.
        /// Creates a text field with custom identification, automatic data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="bindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newFieldName">Output parameter that receives a reference to the newly created text field.</param>
        public UsoTextField(string fieldName, string bindingPath, BindingMode bindingMode, out UsoTextField newFieldName) : base()
        {
            InitElement(fieldName, bindingPath, bindingMode, out newFieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name and data binding configuration.
        /// Creates a text field with custom identification and automatic data binding for value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="bindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        public UsoTextField(string fieldName, string bindingPath, BindingMode bindingMode) : base()
        {
            InitElement(fieldName, bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with field name, label text, and data binding configuration.
        /// Creates a fully configured text field with custom identification, display label, and automatic data binding for value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="labelText">The label text to display alongside the text field control.</param>
        /// <param name="bindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode) : base(labelText)
        {
            InitElement(fieldName, bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with complete configuration including custom data source.
        /// Creates a fully configured text field with custom identification, display label, automatic data binding, and explicit data source assignment.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="labelText">The label text to display alongside the text field control.</param>
        /// <param name="bindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="fieldDatasource">The Unity Object to use as the data source for binding operations.</param>
        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, Object fieldDatasource) : base(labelText)
        {
            InitElement(fieldName);
            dataSource = fieldDatasource;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        /// <summary>
        /// Private initialization method that combines element setup with data binding configuration and reference output.
        /// This overload provides convenient setup for constructors that need element initialization, binding, and immediate reference access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newFieldName">Output parameter that receives a reference to the newly created text field.</param>
        private void InitElement(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode, out UsoTextField newFieldName)
        {
            InitElement(fieldName, out newFieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Private initialization method that combines element setup with data binding configuration.
        /// This overload provides convenient setup for constructors that need both element initialization and binding.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        private void InitElement(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Private initialization method that sets up the element and provides a reference through an out parameter.
        /// This overload provides convenient setup for constructors that need immediate reference access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this text field element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created text field.</param>
        private void InitElement(string fieldName, out UsoTextField newField)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoTextField class with default settings.
        /// Creates an empty text field with USO framework integration enabled.
        /// </summary>
        public UsoTextField()
        {
            InitElement();
        }
    }
}