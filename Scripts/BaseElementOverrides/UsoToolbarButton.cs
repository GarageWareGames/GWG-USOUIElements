using System;
using GWG.UsoUIElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom toolbar button control that extends Unity's ToolbarButton with USO UI framework functionality.
    /// Provides enhanced styling, field validation, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, custom styling through CSS classes, and various initialization options.
    /// The control is specifically designed for use within toolbar contexts and provides enhanced functionality
    /// beyond the standard Unity ToolbarButton, including action-based initialization and field status management.
    /// This control is commonly used for toolbar actions, commands, and interactive elements within the USO framework.
    /// </remarks>
    [UxmlElement]
    public partial class UsoToolbarButton : ToolbarButton, IUsoUiElement
    {
        /// <summary>
        /// CSS class name applied to all UsoToolbarButton instances for styling purposes.
        /// </summary>
        private const string ElementStylesheet = "uso-toolbar-button";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to "value" for potential future binding scenarios.
        /// </summary>
        private const string DefaultBindProp = "value";

        /// <summary>
        /// Private backing field for the FieldStatusEnabled property.
        /// </summary>
        private bool _fieldStatusEnabled;

        /// <summary>
        /// Private backing field for the FieldStatus property.
        /// </summary>
        private FieldStatusTypes _fieldStatus;

        /// <summary>
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is false.</value>
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

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with default settings.
        /// Creates a toolbar button with USO framework integration enabled.
        /// </summary>
        public UsoToolbarButton()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with the specified field name.
        /// Creates a toolbar button with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        public UsoToolbarButton(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with field name and returns a reference.
        /// Creates a toolbar button with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created toolbar button.</param>
        public UsoToolbarButton(string fieldName, out UsoToolbarButton newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with field name and display text.
        /// Creates a toolbar button with custom identification and text content for user interface clarity.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        /// <param name="fieldText">The text to display on the toolbar button.</param>
        public UsoToolbarButton(string fieldName,string fieldText) : base()
        {
            InitElement(fieldName);
            base.text = fieldText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with field name, display text, and returns a reference.
        /// Creates a toolbar button with custom identification, text content, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        /// <param name="fieldText">The text to display on the toolbar button.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created toolbar button.</param>
        public UsoToolbarButton(string fieldName,string fieldText, out UsoToolbarButton newField) : base()
        {
            InitElement(fieldName);
            base.text = fieldText;
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with a click action callback.
        /// Creates a toolbar button with predefined click behavior and USO framework integration.
        /// </summary>
        /// <param name="btnAction">The action to execute when the toolbar button is clicked.</param>
        public UsoToolbarButton(Action btnAction) : base(btnAction)
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with field name and a click action callback.
        /// Creates a toolbar button with custom identification, predefined click behavior, and USO framework integration.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        /// <param name="btnAction">The action to execute when the toolbar button is clicked.</param>
        public UsoToolbarButton(string fieldName, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarButton class with field name, click action, and returns a reference.
        /// Creates a toolbar button with custom identification, predefined click behavior, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar button element.</param>
        /// <param name="btnAction">The action to execute when the toolbar button is clicked.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created toolbar button.</param>
        public UsoToolbarButton(string fieldName, Action btnAction, out UsoToolbarButton newField) : base(btnAction)
        {
            InitElement(fieldName);
            newField = this;
        }
    }
}