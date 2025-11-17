using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom label control that extends Unity's Label with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for text content, and custom styling through CSS classes.
    /// The control includes support for different label types (Header, SubHeader, Title, Subtitle, Description, Default)
    /// that automatically apply appropriate CSS classes for consistent visual hierarchy throughout the application.
    /// </remarks>
    [UxmlElement]
    public partial class UsoLabel : Label, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoLabel instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-label";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'text' property which controls the displayed text content.
        /// </summary>
        private const string DefaultBindProp = "text";

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
        /// Initializes a new instance of the UsoLabel class with default settings.
        /// Creates an empty label with USO framework integration and default label type styling.
        /// </summary>
        public UsoLabel() : base()
        {
            InitElement(null, LabelType.Default);
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with the specified text.
        /// Creates a label with custom text content and default label type styling.
        /// </summary>
        /// <param name="fieldLabelText">The text content to display in the label.</param>
        public UsoLabel(string fieldLabelText) : base(fieldLabelText)
        {
            InitElement(null, LabelType.Default);
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with text and label type.
        /// Creates a label with custom text content and specific visual styling based on the label type.
        /// </summary>
        /// <param name="fieldLabelText">The text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        public UsoLabel(string fieldLabelText, LabelType fieldLabelType) : base(fieldLabelText)
        {
            InitElement(null, fieldLabelType);
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with text, label type, and returns a reference.
        /// Creates a label with custom text, styling, and provides an out parameter for immediate access to the created instance.
        /// </summary>
        /// <param name="fieldLabelText">The text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created label.</param>
        public UsoLabel(string fieldLabelText, LabelType fieldLabelType, out UsoLabel newField) : base(fieldLabelText)
        {
            InitElement(null, fieldLabelType);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with field name, text, and label type.
        /// Creates a label with custom identification, text content, and specific visual styling.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelText">The text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with field name, text, label type, and returns a reference.
        /// Creates a fully configured label and provides an out parameter for immediate access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelText">The text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created label.</param>
        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, out UsoLabel newField) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with complete configuration including data binding.
        /// Creates a label with custom identification, text, styling, and automatic data binding for dynamic content updates.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelText">The initial text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        /// <param name="bindingPath">The path to the data source property for automatic text binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target. Default is ToTarget.</param>
        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, string bindingPath, BindingMode fieldBindingMode = BindingMode.ToTarget) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType, bindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoLabel class with complete configuration, data binding, and reference output.
        /// Creates a fully configured label with automatic data binding and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelText">The initial text content to display in the label.</param>
        /// <param name="fieldLabelType">The label type that determines visual styling and hierarchy level.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic text binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created label.</param>
        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, string fieldBindingPath, BindingMode fieldBindingMode, out UsoLabel newField) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
            newField = this;
        }

        /// <summary>
        /// Private initialization method that combines element setup with data binding configuration.
        /// This overload provides convenient setup for constructors that need both element initialization and binding.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelText">The label type that determines visual styling and hierarchy level.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic text binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        private void InitElement(string fieldName, LabelType fieldLabelType, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            InitElement(fieldName, fieldLabelType);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Private initialization method that sets up the label with USO framework integration and applies type-specific styling.
        /// This method configures the basic element properties and adds appropriate CSS classes based on the label type.
        /// </summary>
        /// <param name="fieldName">The name to assign to this label element.</param>
        /// <param name="fieldLabelType">The label type that determines which CSS class to apply for styling.</param>
        /// <remarks>
        /// Label type CSS class mapping:
        /// - Header → "uso-label--header"
        /// - SubHeader → "uso-label--subheader"
        /// - Title → "uso-label--title"
        /// - Subtitle → "uso-label--subtitle"
        /// - Description → "uso-label--description"
        /// - Default → No additional class applied
        /// </remarks>
        private void InitElement(string fieldName, LabelType fieldLabelType)
        {
            InitElement(fieldName);
            switch (fieldLabelType)
            {
                case LabelType.Header:
                    AddToClassList("uso-label--header");
                    break;
                case LabelType.SubHeader:
                    AddToClassList("uso-label--subheader");
                    break;
                case LabelType.Title:
                    AddToClassList("uso-label--title");
                    break;
                case LabelType.Subtitle:
                    AddToClassList("uso-label--subtitle");
                    break;
                case LabelType.Description:
                    AddToClassList("uso-label--description");
                    break;
                case LabelType.Default:
                default:
                    break;
            }
        }

    }
}