using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom row layout container element that extends Unity's VisualElement with USO UI framework functionality and horizontal layout configuration.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system for structured horizontal content organization.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding capabilities, and custom styling through CSS classes.
    /// The control is specifically designed as a horizontal layout container with automatic flex direction configuration set to row layout.
    /// It features automatic full-width styling (100% width) to provide consistent horizontal layout behavior across the application.
    /// Row elements are commonly used for creating horizontal form layouts, toolbar arrangements, and side-by-side content presentations
    /// that require consistent spacing, alignment, and validation management throughout the USO framework.
    /// The control has field status functionality disabled by default, focusing on its role as a structural layout container.
    /// </remarks>
    [UxmlElement]
    public partial class UsoRowElement : VisualElement, IUsoUiElement
    {
        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoRowElement instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-row-element";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to empty string as row elements typically manage child element layout rather than direct values.
        /// </summary>
        private const string DefaultBindProp = "";

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
        /// <value>True if field status functionality is enabled; otherwise, false. Default is false for row elements.</value>
        /// <remarks>
        /// Row elements have field status functionality disabled by default since they primarily serve as layout containers
        /// rather than interactive or validatable content elements.
        /// </remarks>
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
        private bool _fieldStatusEnabled = false;

        /// <summary>
        /// Applies data binding to the specified property of this control using Unity's data binding system.
        /// Configures the binding with the provided path and mode for automatic data synchronization.
        /// </summary>
        /// <param name="fieldBindingProp">The property name on this control to bind to.</param>
        /// <param name="fieldBindingPath">The path to the data source property to bind from.</param>
        /// <param name="fieldBindingMode">The binding mode that determines how data flows between source and target.</param>
        /// <exception cref="Exception">Thrown when binding setup fails. Original exception is preserved and re-thrown.</exception>
        /// <remarks>
        /// While row elements primarily handle layout, this method enables binding to container-level properties
        /// such as visibility, styling characteristics, or other layout-related attributes.
        /// </remarks>
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

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

        /// <summary>
        /// Initializes a new Instance of the UsoRowElement class with default settings.
        /// Creates a horizontal layout container with USO framework integration and automatic row configuration.
        /// </summary>
        public UsoRowElement() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoRowElement class with the specified field name.
        /// Creates a horizontal layout container with custom identification and full-width styling configuration.
        /// </summary>
        /// <param name="fieldName">The name to assign to this row element.</param>
        /// <remarks>
        /// This constructor specifically sets the width to 100% in addition to the standard initialization,
        /// ensuring consistent full-width behavior for named row elements.
        /// </remarks>
        public UsoRowElement(string fieldName) : base()
        {
            InitElement(fieldName);
            style.width = new StyleLength(Length.Percent(100));
        }

        /// <summary>
        /// Initializes a new Instance of the UsoRowElement class with field name and returns a reference.
        /// Creates a horizontal layout container with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this row element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created row element.</param>
        public UsoRowElement(string fieldName, out UsoRowElement newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies horizontal layout configuration.
        /// This method sets up the basic USO framework integration and configures the element for row-based layout behavior.
        /// </summary>
        /// <param name="fieldName">The name to assign to the element. Default is empty string if not specified.</param>
        /// <remarks>
        /// The initialization process includes setting the flex direction to row for horizontal layout,
        /// configuring full-width styling (100% width), applying appropriate CSS classes, and enabling
        /// field status functionality according to the default settings. The 'new' keyword is used to
        /// hide any inherited InitElement method and provide row-specific initialization behavior.
        /// </remarks>
        public new void InitElement(string fieldName = "")
        {
            name = fieldName;
            style.flexDirection = FlexDirection.Row;
            style.width = new StyleLength(Length.Percent(100));
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }
    }
}