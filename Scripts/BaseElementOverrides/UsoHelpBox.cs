using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom help box control that extends Unity's HelpBox with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators that automatically map to HelpBox message types, automatic data binding,
    /// and custom styling through CSS classes. The control is configured with flex-shrink: 0 by default to maintain its size.
    /// Field status types are automatically converted to appropriate HelpBox message types for visual consistency.
    /// </remarks>
    [UxmlElement]
    public partial class UsoHelpBox : HelpBox, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoHelpBox instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-help-box";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'text' property which controls the displayed message content.
        /// </summary>
        private const string DefaultBindProp = "text";

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback.
        /// This property is automatically reflected in the UI through CSS class modifications and message type changes.
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
        /// Initializes a new Instance of the UsoHelpBox class with the specified message and help type.
        /// Creates a help box with custom content and visual styling based on the message type.
        /// </summary>
        /// <param name="message">The text message to display in the help box.</param>
        /// <param name="helpType">The type of help box that determines its visual appearance and icon. Default is None.</param>
        public UsoHelpBox(string message, HelpBoxMessageType helpType = HelpBoxMessageType.None) : base(message, helpType)
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with message, help type, and returns a reference.
        /// Creates a help box with custom content and provides an out parameter for immediate access to the created Instance.
        /// </summary>
        /// <param name="message">The text message to display in the help box.</param>
        /// <param name="helpType">The type of help box that determines its visual appearance and icon.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created help box.</param>
        public UsoHelpBox(string message, HelpBoxMessageType helpType, out UsoHelpBox newField) : base(message, helpType)
        {
            InitElement();
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with the specified field name.
        /// Creates an empty help box with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this help box element.</param>
        public UsoHelpBox(string fieldName)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with field name and returns a reference.
        /// Creates an empty help box with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this help box element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created help box.</param>
        public UsoHelpBox(string fieldName, out UsoHelpBox newField)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with field name, message, and help type.
        /// Creates a fully configured help box with custom identification, content, and visual styling.
        /// </summary>
        /// <param name="fieldName">The name to assign to this help box element.</param>
        /// <param name="message">The text message to display in the help box.</param>
        /// <param name="helpType">The type of help box that determines its visual appearance and icon.</param>
        public UsoHelpBox(string fieldName, string message, HelpBoxMessageType helpType)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with field name, message, help type, and returns a reference.
        /// Creates a fully configured help box and provides an out parameter for immediate access to the created Instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this help box element.</param>
        /// <param name="message">The text message to display in the help box.</param>
        /// <param name="helpType">The type of help box that determines its visual appearance and icon.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created help box.</param>
        public UsoHelpBox(string fieldName, string message, HelpBoxMessageType helpType, out UsoHelpBox newField)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoHelpBox class with default settings.
        /// Creates an empty help box with USO framework integration enabled.
        /// </summary>
        public UsoHelpBox()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control and configures layout properties.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            style.flexShrink = 0;
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
        /// Updates the field's status type, which affects its visual appearance and automatically maps to the appropriate HelpBox message type.
        /// The status change is automatically reflected in the UI through both the FieldStatus property and the HelpBox message type.
        /// </summary>
        /// <param name="fieldStatus">The new field status type to apply. Maps to corresponding HelpBoxMessageType values.</param>
        /// <remarks>
        /// Status mapping:
        /// - Error → HelpBoxMessageType.Error
        /// - Warning → HelpBoxMessageType.Warning
        /// - Info → HelpBoxMessageType.Info
        /// - All other values → HelpBoxMessageType.None
        /// </remarks>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
            if (fieldStatus == FieldStatusTypes.Error)
            {
                messageType = HelpBoxMessageType.Error;
                return;
            }

            if (fieldStatus == FieldStatusTypes.Warning)
            {
                messageType = HelpBoxMessageType.Warning;
                return;
            }

            if (fieldStatus == FieldStatusTypes.Info)
            {
                messageType = HelpBoxMessageType.Info;
                return;
            }
            messageType = HelpBoxMessageType.None;
        }

    }
}