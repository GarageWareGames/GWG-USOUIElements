using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom form container element that extends Unity's VisualElement with USO UI framework functionality and specialized form management capabilities.
    /// Provides enhanced styling, field validation, data binding capabilities, form lifecycle events, and integration with the USO UI system for comprehensive form-based user interfaces.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding capabilities, and custom styling through CSS classes.
    /// The control serves as a specialized container for form-based interfaces, providing form lifecycle management through
    /// events for reset, submit, cancel, and data source connection/disconnection operations. It automatically loads and applies
    /// the USO theme stylesheet and configures itself with flexible layout properties suitable for form layouts.
    /// The control includes comprehensive data source management capabilities, allowing dynamic connection and disconnection
    /// of data objects with appropriate event notifications for form state management and data synchronization scenarios.
    /// </remarks>
    [UxmlElement]
    public partial class UsoForm : VisualElement
    {
        public StyleSheet UsoStyleSheet
        {
            get
            {
                return _usoDefaultStyleSheet;
            }
            set
            {
                if(_usoDefaultStyleSheet != null)
                {
                    if (styleSheets.Contains(_usoDefaultStyleSheet))
                    {
                        styleSheets.Remove(_usoDefaultStyleSheet);
                    }
                }
                _usoDefaultStyleSheet = value;

                if (_usoDefaultStyleSheet != null)
                {
                    styleSheets.Add(_usoDefaultStyleSheet);
                }
            }
        }
        private StyleSheet _usoDefaultStyleSheet;

        /// <summary>
        /// Internal event triggered when the form reset operation is requested.
        /// Subscribe to this event to implement custom reset logic for form fields and data.
        /// </summary>
        /// <remarks>
        /// This event provides a centralized mechanism for handling form reset operations across all contained form elements.
        /// Implementers can use this to clear form data, reset validation states, and restore default values.
        /// </remarks>
        internal event Action OnClearForm;

        /// <summary>
        /// Internal event triggered when the form's data source is disconnected or cleared.
        /// Subscribe to this event to handle cleanup operations when data binding relationships are removed.
        /// </summary>
        /// <remarks>
        /// This event notifies subscribers when the data source is being disconnected, allowing for proper
        /// cleanup of binding relationships, UI state management, and resource disposal.
        /// </remarks>
        internal event Action<Object> OnFormDataDisconnection;

        /// <summary>
        /// Internal event triggered when a new data source is connected to the form.
        /// Subscribe to this event to initialize form fields and establish data binding relationships.
        /// </summary>
        /// <remarks>
        /// This event enables subscribers to respond to new data source connections by initializing
        /// form fields, establishing bindings, and configuring the UI to reflect the new data context.
        /// </remarks>
        internal event Action<Object> OnFormDataConnection;

        /// <summary>
        /// Initiates a form reset operation by invoking the OnClearForm event.
        /// This method provides a centralized way to trigger reset logic across all form components.
        /// </summary>
        /// <remarks>
        /// Calling this method will notify all subscribers to the OnClearForm event, allowing them
        /// to implement their specific reset logic such as clearing fields, resetting validation states,
        /// and restoring default values.
        /// </remarks>
        public void ClearForm()
        {
            OnClearForm?.Invoke();
            // find all UsoForm children and reset their values
            foreach (VisualElement child in this.Children())
            {
                if (child is IUsoUiElement element)
                {
                    element.ClearField();
                }
            }
        }

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS stylesheet name applied to all UsoForm instances for styling purposes.
        /// Uses "uso-display-section" for consistent section-based styling.
        /// </summary>
        private const string ElementStylesheet = "uso-display-section";

        /// <summary>
        /// CSS class name applied to all UsoForm instances for styling purposes.
        /// Uses "uso-display-section" to maintain consistency with the stylesheet.
        /// </summary>
        private const string ElementClass = "uso-display-section";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to empty string as form elements typically manage multiple child bindings rather than a single value.
        /// </summary>
        private const string DefaultBindProp = "";

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback.
        /// This property is automatically reflected in the UI through CSS class modifications.
        /// </summary>
        /// <value>The current FieldStatusTypes value indicating the field's validation state.</value>
        [UxmlAttribute]
        public FieldStatusTypes FormStatus
        {
            get
            {
                return _formStatus;
            }
            private set
            {
                _formStatus = value;
                UsoUiHelper.SetFieldStatus(this, value);
            }
        }
        private FieldStatusTypes _formStatus;

        /// <summary>
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is false for form elements.</value>
        /// <remarks>
        /// Form elements have field status functionality disabled by default since they primarily serve
        /// as containers for other form controls that handle their own validation states.
        /// </remarks>
        [UxmlAttribute]
        public bool FormStatusEnabled
        {
            get
            {
                return _formStatusEnabled;
            }

            private set
            {
                _formStatusEnabled = value;
                if (value)
                {
                    AddToClassList(ElementValidationClass);
                }
                else
                {
                    RemoveFromClassList(ElementValidationClass);
                }
                foreach (VisualElement child in this.Children())
                {
                    if (child is IUsoUiElement element)
                    {
                        element.ShowFieldStatus(value);
                    }
                }
            }
        }
        private bool _formStatusEnabled = false;

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes and theme configuration.
        /// This method sets up the basic USO framework integration for the control including theme loading and flexible layout configuration.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// The initialization process includes setting flexible layout properties, loading the USO theme stylesheet
        /// from resources, and applying appropriate CSS classes for consistent form styling throughout the application.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            style.flexGrow = 1;
            AddToClassList(ElementStylesheet);
            name = fieldName;

            if (_usoDefaultStyleSheet == null)
            {
                UsoStyleSheet = Resources.Load<StyleSheet>("UsoUiElements/UsoUiElementsTheme");
            }
            AddToClassList(ElementClass);
            FormStatusEnabled = _formStatusEnabled;
        }

        /// <summary>
        /// Applies data binding to the specified property of this control using Unity's data binding system.
        /// Configures the binding with the provided path and mode for automatic data synchronization.
        /// </summary>
        /// <param name="fieldBindingProp">The property name on this control to bind to.</param>
        /// <param name="fieldBindingPath">The path to the data source property to bind from.</param>
        /// <param name="fieldBindingMode">The binding mode that determines how data flows between source and target.</param>
        /// <exception cref="Exception">Thrown when binding setup fails. Original exception is preserved and re-thrown.</exception>
        /// <remarks>
        /// While form elements typically manage child element bindings rather than direct value bindings,
        /// this method enables binding to form-level properties such as visibility, enabled state, or other container characteristics.
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
        /// <param name="formStatus">The new field status type to apply.</param>
        public void SetFormStatus(FieldStatusTypes formStatus)
        {
            FormStatus = formStatus;
        }

        /// <summary>
        /// Controls the visibility and functionality of the field status/validation system.
        /// When disabled, removes validation-related styling from the control.
        /// </summary>
        /// <param name="status">True to enable field status functionality; false to disable it.</param>
        public void ShowFormStatus(bool status)
        {
            FormStatusEnabled = status;
            // Set status enabled state on all child elements
        }

        /// <summary>
        /// Retrieves the first ancestor UsoLineItem control in the visual tree hierarchy.
        /// This is useful for accessing parent container functionality and maintaining a proper UI structure.
        /// </summary>
        /// <returns>The parent UsoLineItem if found; otherwise, null.</returns>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

#endregion

        /// <summary>
        /// Initializes a new Instance of the UsoForm class with default settings.
        /// Creates a form container with USO framework integration, theme loading, and form lifecycle event management.
        /// </summary>
        public UsoForm() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoForm class with the specified field name.
        /// Creates a form container with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="formName">The name to assign to this form element.</param>
        public UsoForm(string formName) : base()
        {
            InitElement(formName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoForm class with field name and returns a reference.
        /// Creates a form container with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="formName">The name to assign to this form element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created form element.</param>
        public UsoForm(string formName, out UsoForm newField) : base()
        {
            InitElement(formName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoForm class with field name and initial data source.
        /// Creates a form container with custom identification and establishes initial data binding relationships.
        /// </summary>
        /// <param name="formName">The name to assign to this form element.</param>
        /// <param name="fieldDatasource">The Unity Object to use as the initial data source for form binding operations.</param>
        public UsoForm(string formName, Object fieldDatasource) : base()
        {
            InitElement(formName);
            UpdateDatasource(fieldDatasource);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoForm class with field name, initial data source, and returns a reference.
        /// Creates a form container with custom identification, establishes initial data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="formName">The name to assign to this form element.</param>
        /// <param name="fieldDatasource">The Unity Object to use as the initial data source for form binding operations.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created form element.</param>
        public UsoForm(string formName, Object fieldDatasource, out UsoForm newField) : base()
        {
            InitElement(formName);
            newField = this;
            UpdateDatasource(fieldDatasource);
        }

        /// <summary>
        /// Updates the form's data source, establishing or clearing data binding relationships and triggering appropriate lifecycle events.
        /// Manages the connection and disconnection of data sources with proper event notifications for form state management.
        /// </summary>
        /// <param name="fieldDatasource">The Unity Object to use as the new data source. Pass null to disconnect the current data source.</param>
        /// <remarks>
        /// When a non-null data source is provided, the method establishes the binding relationship and invokes the OnFormDataConnection event.
        /// When null is provided, it clears all existing bindings, sets the data source to null, and invokes the OnFormDataDisconnection event.
        /// This method enables dynamic data source management for scenarios where form data context needs to change during runtime.
        /// </remarks>
        public void UpdateDatasource(Object fieldDatasource = null)
        {
            if (fieldDatasource != null)
            {
                dataSource = fieldDatasource;
                OnFormDataConnection?.Invoke(fieldDatasource);
            }
            else
            {
                ClearBindings();
                dataSource = null;
                OnFormDataDisconnection?.Invoke(fieldDatasource);
            }
        }
    }
}