
using System;
using System.Collections.Generic;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom line item container element that extends Unity's VisualElement with USO UI framework functionality and hierarchical organization capabilities.
    /// Provides enhanced styling, field validation, data binding capabilities, validation state management, and integration with the USO UI system for structured content organization.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding capabilities, and custom styling through CSS classes.
    /// The control serves as a fundamental organizational unit within the USO framework, designed for creating structured layouts
    /// with hierarchical relationships between form elements and content sections. It includes validation state tracking through
    /// the IsValidated property and provides methods for querying child line item relationships within the UI hierarchy.
    /// Line items are commonly used as containers for form rows, content sections, and organizational groupings that require
    /// consistent styling and validation management throughout the application. The control enables nested line item structures
    /// for complex UI organizations while maintaining framework consistency and parent-child relationship management.
    /// </remarks>
    [UxmlElement]
    public partial class UsoLineItem : VisualElement, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoLineItem instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-line-item";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to empty string as line items typically manage child element bindings rather than direct values.
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
        /// <value>True if field status functionality is enabled; otherwise, false. Default is true for line items.</value>
        /// <remarks>
        /// Unlike some other USO container elements, line items have field status functionality enabled by default
        /// since they commonly participate in validation scenarios and form organization.
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
        /// <remarks>
        /// While line items typically manage child element bindings, this method enables binding to container-level
        /// properties such as visibility, validation states, or other organizational characteristics.
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
        /// <remarks>
        /// This method enables hierarchical line item relationships, allowing child line items to access
        /// their parent containers for validation coordination, styling inheritance, and organizational purposes.
        /// </remarks>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

        /// <summary>
        /// Gets or sets a value indicating whether this line item has passed validation checks.
        /// This property is used for tracking validation state within forms and structured layouts.
        /// </summary>
        /// <value>True if the line item and its contents have been validated successfully; otherwise, false.</value>
        /// <remarks>
        /// This property provides a way to track validation state at the line item level, enabling
        /// form validation systems to coordinate validation results across hierarchical content structures.
        /// It can be used in conjunction with field status indicators to provide comprehensive validation feedback.
        /// </remarks>
        public bool IsValidated { get; set; }

        /// <summary>
        /// Initializes a new instance of the UsoLineItem class with default settings.
        /// Creates a line item container with USO framework integration and validation functionality enabled.
        /// </summary>
        public UsoLineItem()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new instance of the UsoLineItem class with the specified field name.
        /// Creates a line item container with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this line item element.</param>
        public UsoLineItem(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Retrieves a list of direct child UsoLineItem elements within this line item's hierarchy.
        /// This method provides access to nested line item structures for validation coordination and hierarchical management.
        /// </summary>
        /// <returns>A List&lt;UsoLineItem&gt; containing all direct child line items, excluding this line item itself.</returns>
        /// <remarks>
        /// This method performs a query to find all child UsoLineItem elements and filters out the current instance
        /// to prevent self-inclusion in the results. It's useful for implementing hierarchical validation systems,
        /// cascading style applications, and organizational operations that need to work with nested line item structures.
        /// The method only returns direct children and does not perform recursive searches through deeper nested levels.
        /// </remarks>
        public List<UsoLineItem> GetChildLineItems()
        {
            var childList = this.Query().Children<UsoLineItem>().ToList();
            var filteredList = new List<UsoLineItem>();
            foreach (var child in childList)
            {
                if (child == this) continue;
                filteredList.Add(child);
            }

            return filteredList;
        }
    }
}