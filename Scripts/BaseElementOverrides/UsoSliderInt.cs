using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom integer slider control that extends Unity's SliderInt with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for integer values, and custom styling through CSS classes.
    /// The control is pre-configured with a default range of 0 to 100, but can be customized through the standard Unity SliderInt properties.
    /// The control supports various constructor overloads for different initialization scenarios including labeling and data binding configuration.
    /// Note that this control shares the same CSS class as UsoSlider ("uso-slider") for consistent styling between float and integer sliders.
    /// </remarks>
    [UxmlElement]
    public partial class UsoSliderInt : SliderInt, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoSliderInt instances for styling purposes.
        /// Uses the same class as UsoSlider to maintain consistent styling between float and integer sliders.
        /// </summary>
        private const string ElementClass = "uso-slider";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the current slider position/value.
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
        /// Initializes a new instance of the UsoSliderInt class with default settings.
        /// Creates an integer slider with USO framework integration and default range configuration (0 to 100).
        /// </summary>
        public UsoSliderInt() : base()
        {
            InitElement(null);
        }

        /// <summary>
        /// Initializes a new instance of the UsoSliderInt class with the specified field name.
        /// Creates an integer slider with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slider element.</param>
        public UsoSliderInt(string fieldName) : base()
        {
            InitElement(fieldName);
            name = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the UsoSliderInt class with field name and label text.
        /// Creates an integer slider with custom identification and display label for user interface clarity.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slider element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slider control.</param>
        public UsoSliderInt(string fieldName, string fieldLabelText) : base(fieldLabelText)
        {
            InitElement(fieldName);
            name = fieldName;
        }

        /// <summary>
        /// Initializes a new instance of the UsoSliderInt class with field name, label text, and returns a reference.
        /// Creates an integer slider with custom identification, display label, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slider element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slider control.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created slider.</param>
        public UsoSliderInt(string fieldName, string fieldLabelText, out UsoSliderInt newField) : base(fieldLabelText)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoSliderInt class with field name, label text, and data binding configuration.
        /// Creates a fully configured integer slider with custom identification, display label, and automatic data binding for value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slider element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slider control.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        public UsoSliderInt(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabelText)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoSliderInt class with complete configuration and returns a reference.
        /// Creates a fully configured integer slider with custom identification, display label, automatic data binding, and immediate access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slider element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slider control.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created slider.</param>
        public UsoSliderInt(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode, out UsoSliderInt newField) : base(fieldLabelText)
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies default integer slider configuration.
        /// This method sets up the basic USO framework integration and configures the slider's default range and styling.
        /// </summary>
        /// <param name="fieldName">The name to assign to the element. Can be null for unnamed elements.</param>
        /// <remarks>
        /// Default configuration includes:
        /// - Range from 0 to 100 (lowValue = 0, highValue = 100)
        /// - USO CSS class for consistent styling with other slider controls
        /// - Field status functionality enabled
        /// The integer range (0-100) is more suitable for typical integer input scenarios compared to the float slider's 0-1 range.
        /// </remarks>
        public void InitElement(string fieldName)
        {
            name = fieldName;
            lowValue = 0;
            highValue = 100;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }
    }
}
