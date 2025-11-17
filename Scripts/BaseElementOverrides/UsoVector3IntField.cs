
using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom Vector3Int field control that extends Unity's Vector3IntField with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system for three-dimensional integer vector input.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for Vector3Int values, and custom styling through CSS classes.
    /// The control is specifically designed for inputting and editing three-dimensional integer vectors commonly used in game development
    /// for positions, coordinates, grid locations, and other spatial data that requires whole number precision.
    /// The control supports various constructor overloads for different initialization scenarios including labeling and data binding configuration,
    /// making it suitable for both standalone use and integration within complex data-bound UI systems.
    /// </remarks>
    [UxmlElement]
    public partial class UsoVector3IntField : Vector3IntField, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoVector3IntField instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-vector-3-int-field";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the Vector3Int value of the field.
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
        /// Initializes a new instance of the UsoVector3IntField class with default settings.
        /// Creates a Vector3Int input field with USO framework integration enabled and no initial label.
        /// </summary>
        public UsoVector3IntField() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new instance of the UsoVector3IntField class with the specified label text.
        /// Creates a Vector3Int input field with custom label text for user interface clarity.
        /// </summary>
        /// <param name="fieldLabel">The label text to display alongside the Vector3Int input field.</param>
        public UsoVector3IntField(string fieldLabel) : base(fieldLabel)
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new instance of the UsoVector3IntField class with field name and data binding configuration.
        /// Creates a Vector3Int input field with custom identification and automatic data binding for value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this Vector3Int field element.</param>
        /// <param name="fieldBindPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindMode">The binding mode that controls data flow between source and target.</param>
        public UsoVector3IntField(string fieldName, string fieldBindPath, BindingMode fieldBindMode) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoVector3IntField class with field name, data binding, and returns a reference.
        /// Creates a Vector3Int input field with custom identification, automatic data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this Vector3Int field element.</param>
        /// <param name="fieldBindPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created Vector3Int field.</param>
        public UsoVector3IntField(string fieldName, string fieldBindPath, BindingMode fieldBindMode, out UsoVector3IntField newField) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoVector3IntField class with field name, label text, and data binding configuration.
        /// Creates a fully configured Vector3Int input field with custom identification, display label, and automatic data binding for value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this Vector3Int field element.</param>
        /// <param name="fieldLabel">The label text to display alongside the Vector3Int input field.</param>
        /// <param name="fieldBindPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindMode">The binding mode that controls data flow between source and target.</param>
        public UsoVector3IntField(string fieldName, string fieldLabel, string fieldBindPath, BindingMode fieldBindMode) : base(fieldLabel)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoVector3IntField class with complete configuration and returns a reference.
        /// Creates a fully configured Vector3Int input field with custom identification, display label, automatic data binding, and immediate access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this Vector3Int field element.</param>
        /// <param name="fieldLabel">The label text to display alongside the Vector3Int input field.</param>
        /// <param name="fieldBindPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created Vector3Int field.</param>
        public UsoVector3IntField(string fieldName, string fieldLabel, string fieldBindPath, BindingMode fieldBindMode, out UsoVector3IntField newField) : base(fieldLabel)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
            newField = this;
        }
    }
}