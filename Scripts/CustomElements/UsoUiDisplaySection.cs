
using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom UI display section container element that extends Unity's VisualElement with USO UI framework functionality and specialized content display capabilities.
    /// Provides enhanced styling, field validation, data binding capabilities, theme integration, and data source management for structured content presentation within the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding capabilities, and custom styling through CSS classes.
    /// The control serves as a specialized container for display-oriented content sections, featuring automatic theme loading
    /// and flexible layout configuration optimized for content presentation scenarios. It includes comprehensive data source
    /// management capabilities with methods for connecting and disconnecting data objects for dynamic content scenarios.
    /// Display sections are commonly used for organizing related content, creating structured layouts, and providing
    /// consistent presentation containers throughout USO applications. The control automatically loads the USO theme stylesheet
    /// and configures flexible growth properties suitable for adaptive content display requirements.
    /// The control has field status functionality disabled by default, focusing on its role as a presentation container.
    /// </remarks>
    [UxmlElement]
    public partial class UsoUiDisplaySection : VisualElement, IUsoUiElement
    {


#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS stylesheet name applied to all UsoUiDisplaySection instances for styling purposes.
        /// Uses "uso-display-section" for consistent section-based styling throughout the application.
        /// </summary>
        private const string ElementStylesheet = "uso-display-section";

        /// <summary>
        /// CSS class name applied to all UsoUiDisplaySection instances for styling purposes.
        /// Uses "uso-display-section" to maintain consistency with the stylesheet configuration.
        /// </summary>
        private const string ElementClass = "uso-display-section";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to empty string as display sections typically manage child content rather than direct values.
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
        /// <value>True if field status functionality is enabled; otherwise, false. Default is false for display sections.</value>
        /// <remarks>
        /// Display sections have field status functionality disabled by default since they primarily serve
        /// as content presentation containers rather than interactive or validatable elements.
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
        /// Initializes the USO UI element with the specified field name and applies comprehensive styling and theme configuration.
        /// This method sets up the basic USO framework integration including theme loading, flexible layout, and styling application.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// The initialization process includes setting flexible growth properties for adaptive sizing, loading the USO theme
        /// stylesheet from resources for consistent styling, and applying appropriate CSS classes for display section presentation.
        /// The method ensures proper integration with the USO framework's theming and styling system.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            style.flexGrow = 1;
            AddToClassList(ElementStylesheet);
            name = fieldName;

            ThemeStyleSheet usoTheme = Resources.Load<ThemeStyleSheet>("UsoUiElements/UsoUiElementsTheme");
            if (usoTheme != null)
            {
                styleSheets.Add(usoTheme);
            }
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
        /// <remarks>
        /// While display sections primarily manage content presentation, this method enables binding to container-level
        /// properties such as visibility, styling characteristics, or other display-related attributes.
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

#endregion

        /// <summary>
        /// Initializes a new instance of the UsoUiDisplaySection class with default settings.
        /// Creates a display section container with USO framework integration, theme loading, and flexible layout configuration.
        /// </summary>
        public UsoUiDisplaySection() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new instance of the UsoUiDisplaySection class with the specified field name.
        /// Creates a display section container with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this display section element.</param>
        public UsoUiDisplaySection(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoUiDisplaySection class with field name and returns a reference.
        /// Creates a display section container with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this display section element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created display section.</param>
        public UsoUiDisplaySection(string fieldName, out UsoUiDisplaySection newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoUiDisplaySection class with field name and initial data source.
        /// Creates a display section container with custom identification and establishes initial data binding relationships.
        /// </summary>
        /// <param name="fieldName">The name to assign to this display section element.</param>
        /// <param name="fieldDatasource">The Unity Object to use as the initial data source for display content binding.</param>
        public UsoUiDisplaySection(string fieldName, Object fieldDatasource) : base()
        {
            InitElement(fieldName);
            dataSource = fieldDatasource;
        }

        /// <summary>
        /// Initializes a new instance of the UsoUiDisplaySection class with field name, initial data source, and returns a reference.
        /// Creates a display section container with custom identification, establishes initial data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this display section element.</param>
        /// <param name="fieldDatasource">The Unity Object to use as the initial data source for display content binding.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created display section.</param>
        public UsoUiDisplaySection(string fieldName, Object fieldDatasource, out UsoUiDisplaySection newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            dataSource = fieldDatasource;
        }

        /// <summary>
        /// Updates the display section's data source, establishing or clearing data binding relationships for dynamic content management.
        /// Manages the connection and disconnection of data sources with proper binding cleanup when necessary.
        /// </summary>
        /// <param name="fieldDatasource">The Unity Object to use as the new data source. Pass null to disconnect the current data source.</param>
        /// <remarks>
        /// When a non-null data source is provided, the method establishes the binding relationship for content display.
        /// When null is provided, it clears all existing bindings and sets the data source to null, enabling clean
        /// disconnection of data relationships. This method enables dynamic data source management for scenarios where
        /// display content context needs to change during runtime, such as switching between different data objects
        /// or clearing content for reset operations.
        /// </remarks>
        public void UpdateDatasource(Object fieldDatasource = null)
        {
            if (fieldDatasource != null)
            {
                dataSource = fieldDatasource;
            }
            else
            {
                ClearBindings();
                dataSource = null;
            }
        }
    }
}