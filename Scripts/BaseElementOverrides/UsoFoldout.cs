using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom foldout control that extends Unity's Foldout with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding, and custom styling through CSS classes.
    /// The control includes a header toggle element that can be styled independently and supports various constructor overloads
    /// for different initialization scenarios. The foldout is configured with flex-shrink: 0 by default to maintain its size.
    /// </remarks>
    [UxmlElement]
    public partial class UsoFoldout : Foldout, IUsoUiElement
    {
        /// <summary>
        /// CSS class name applied to the foldout header for independent styling of the toggle element.
        /// </summary>
        private const string ElementHeaderStylesheet = "uso-foldout-header";

        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoFoldout instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-foldout";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the expanded/collapsed state.
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
        /// This method sets up the basic USO framework integration for the control and configures the header toggle.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
            style.flexShrink = 0;
            Header = this.Q<Toggle>();
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

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

        /// <summary>
        /// Reference to the toggle element that serves as the foldout header.
        /// This toggle controls the expanded/collapsed state and can be styled independently.
        /// </summary>
        public Toggle Header;

        /// <summary>
        /// Private field for storing custom header stylesheet information.
        /// Used for applying specific styling to the foldout header element.
        /// </summary>
        private string _elementHeaderStylesheet;

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with default settings.
        /// Creates a basic foldout with USO framework integration enabled.
        /// </summary>
        public UsoFoldout() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with the specified header text.
        /// Creates a foldout with a custom display text for the header toggle.
        /// </summary>
        /// <param name="headerText">The text to display in the foldout header.</param>
        public UsoFoldout(string headerText) : base()
        {
            InitElement();
            text = headerText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with the specified field name and initial state.
        /// Creates a foldout with custom identification and sets its expanded/collapsed state.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="state">True to initialize the foldout as expanded; false for collapsed.</param>
        public UsoFoldout(string fieldName, bool state) : base()
        {
            InitElement(fieldName);
            value = state;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with field name, initial state, and returns a reference.
        /// Provides custom identification and state configuration with an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="state">True to initialize the foldout as expanded; false for collapsed.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created field.</param>
        public UsoFoldout(string fieldName, bool state, out UsoFoldout newField) : base()
        {
            InitElement(fieldName);
            value = state;
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with field name and header text.
        /// Combines custom identification with display text configuration for the header.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="headerText">The text to display in the foldout header.</param>
        public UsoFoldout(string fieldName, string headerText) : base()
        {
            InitElement(fieldName);
            text = headerText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with field name, header text, and returns a reference.
        /// Provides custom identification and header configuration with an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="headerText">The text to display in the foldout header.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created field.</param>
        public UsoFoldout(string fieldName, string headerText, out UsoFoldout newField) : base()
        {
            InitElement(fieldName);
            text = headerText;
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with field name, header text, and initial state.
        /// Creates a fully configured foldout with custom identification, display text, and expansion state.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="headerText">The text to display in the foldout header.</param>
        /// <param name="state">True to initialize the foldout as expanded; false for collapsed.</param>
        public UsoFoldout(string fieldName, string headerText, bool state) : base()
        {
            InitElement(fieldName);
            value = state;
            text = headerText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoFoldout class with full configuration and returns a reference.
        /// Provides complete foldout setup with custom identification, header text, state, and immediate access to the created Instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this field element.</param>
        /// <param name="headerText">The text to display in the foldout header.</param>
        /// <param name="state">True to initialize the foldout as expanded; false for collapsed.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created field.</param>
        public UsoFoldout(string fieldName, string headerText, bool state, out UsoFoldout newField) : base()
        {
            InitElement(fieldName);
            value = state;
            text = headerText;
            newField = this;
        }

        /// <summary>
        /// Finalizer for the UsoFoldout class.
        /// Currently empty but reserved for future cleanup operations if needed.
        /// </summary>
        ~UsoFoldout()
        {

        }


    }
}