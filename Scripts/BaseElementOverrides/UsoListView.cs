using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom list view control that extends Unity's ListView with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for collection sources, and custom styling through CSS classes.
    /// The control is pre-configured with common list view settings including dynamic height virtualization, add/remove functionality,
    /// reordering capabilities, alternating row backgrounds, and comprehensive user interaction features.
    /// It also provides virtual methods for customizing header, footer, and empty state elements.
    /// </remarks>
    [UxmlElement]
    public partial class UsoListView : ListView, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoListView instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-list-view";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'itemsSource' property which controls the collection data displayed in the list.
        /// </summary>
        private const string DefaultBindProp = "itemsSource";

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
        /// Initializes a new instance of the UsoListView class with default settings.
        /// Creates a list view with USO framework integration and comprehensive default configuration.
        /// </summary>
        public UsoListView() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with the specified field name.
        /// Creates a list view with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        public UsoListView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with field name and returns a reference.
        /// Creates a list view with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created list view.</param>
        public UsoListView(string fieldName, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with field name and data binding configuration.
        /// Creates a list view with custom identification and automatic data binding for collection synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        /// <param name="fieldBindingPath">The path to the data source collection property for automatic binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        public UsoListView(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with field name, data binding, and returns a reference.
        /// Creates a list view with custom identification, automatic data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        /// <param name="fieldBindingPath">The path to the data source collection property for automatic binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created list view.</param>
        public UsoListView(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with field name, header text, and data binding.
        /// Creates a fully configured list view with custom identification, header title, and automatic data binding.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        /// <param name="headerText">The text to display in the list view header.</param>
        /// <param name="fieldBindingPath">The path to the data source collection property for automatic binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        public UsoListView(string fieldName, string headerText, string fieldBindingPath, BindingMode fieldBindingMode) : base()
        {
            InitElement(fieldName);
            headerTitle = headerText;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes a new instance of the UsoListView class with complete configuration and returns a reference.
        /// Creates a fully configured list view with custom identification, header title, automatic data binding, and immediate access to the created instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this list view element.</param>
        /// <param name="headerText">The text to display in the list view header.</param>
        /// <param name="fieldBindingPath">The path to the data source collection property for automatic binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created list view.</param>
        public UsoListView(string fieldName, string headerText, string fieldBindingPath, BindingMode fieldBindingMode, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            headerTitle = headerText;
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies comprehensive default configuration.
        /// This method sets up the basic USO framework integration and configures all list view properties for optimal user experience.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// Default configuration includes:
        /// - Dynamic height virtualization for performance
        /// - Collection size display and add/remove functionality
        /// - Foldout header and footer visibility
        /// - Animated reordering with single selection
        /// - Alternating row backgrounds and horizontal scrolling
        /// - Flexible layout with grow/shrink properties
        /// - Custom empty state element factory
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            showBoundCollectionSize = true;
            allowAdd = true;
            allowRemove = true;
            showAddRemoveFooter = true;
            showFoldoutHeader = true;
            reorderable = true;
            reorderMode = ListViewReorderMode.Animated;
            showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            horizontalScrollingEnabled = true;
            selectionType = SelectionType.Single;
            style.flexGrow = 1;
            style.flexShrink = 0;
            makeNoneElement = MakeNoneElement;
            AddToClassList(ElementClass);
        }

        /// <summary>
        /// Creates a visual element to display when the list view has no data items.
        /// This virtual method can be overridden to customize the empty state appearance and messaging.
        /// </summary>
        /// <returns>A VisualElement containing the empty state UI, typically including informational text and styling.</returns>
        /// <remarks>
        /// The default implementation creates a UsoVisualElement with a HelpBox containing user guidance
        /// about using the Add button to create new items. The element includes the "uso-list-view__no-data" CSS class for styling.
        /// </remarks>
        public virtual VisualElement MakeNoneElement()
        {
            var content = new UsoVisualElement();
            content.AddToClassList("uso-list-view__no-data");
            content.Add(new HelpBox("No Data Entered.\nUse the Add button at the bottom to create a new item", HelpBoxMessageType.Info));
            return content;
        }

        /// <summary>
        /// Creates a visual element to use as a list view header with the specified label text.
        /// This virtual method can be overridden to customize the header appearance and layout.
        /// </summary>
        /// <param name="labelText">The text content to display in the header element.</param>
        /// <returns>A VisualElement configured as a header, typically a Label with title styling.</returns>
        /// <remarks>
        /// The default implementation creates a Label with the "uso-label--title" CSS class for consistent
        /// title-level styling throughout the application.
        /// </remarks>
        public virtual VisualElement MakeHeader(string labelText)
        {
            Label content = new Label();
            content.AddToClassList("uso-label--title");
            content.text = labelText;
            return content;
        }

        /// <summary>
        /// Creates a visual element to use as a list view footer with the specified label text.
        /// This virtual method can be overridden to customize the footer appearance and layout.
        /// </summary>
        /// <param name="labelText">The text content to display in the footer element.</param>
        /// <returns>A VisualElement configured as a footer, typically a Label with subtitle styling.</returns>
        /// <remarks>
        /// The default implementation creates a Label with the "uso-label--subtitle" CSS class for consistent
        /// subtitle-level styling throughout the application.
        /// </remarks>
        public virtual VisualElement MakeFooter(string labelText)
        {
            Label content = new Label();
            content.AddToClassList("uso-label--subtitle");
            content.text = labelText;
            return content;
        }
    }
}