
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// Defines the contract for all USO UI framework elements, providing a standardized interface for field validation,
    /// data binding, styling, and framework integration capabilities.
    /// </summary>
    /// <remarks>
    /// This interface serves as the foundation for the USO UI framework, ensuring consistent behavior across all custom UI controls.
    /// It establishes common functionality including field status management, data binding capabilities, CSS class manipulation,
    /// and element initialization. All USO UI controls should implement this interface to maintain framework compatibility
    /// and provide users with a unified API for interaction and configuration.
    /// The interface supports various UI scenarios including form validation, dynamic styling, and automated data synchronization.
    /// </remarks>
    public interface IUsoUiElement
    {
        /// <summary>
        /// Gets a value indicating whether field status/validation functionality is enabled for this element.
        /// When enabled, the element can display validation states and apply appropriate styling classes.
        /// </summary>
        /// <value>True if field status functionality is active; otherwise, false.</value>
        /// <remarks>
        /// This property controls whether the element participates in the USO validation and status system.
        /// When disabled, validation-related CSS classes are removed and status indicators are hidden.
        /// </remarks>
        bool FieldStatusEnabled { get; }

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback of the element.
        /// The status type automatically influences the element's appearance through CSS class modifications.
        /// </summary>
        /// <value>A FieldStatusTypes value indicating the current validation or display state.</value>
        /// <remarks>
        /// This property reflects the element's current status in the validation system and triggers appropriate
        /// visual changes through the UsoUiHelper.SetFieldStatus method implementation.
        /// </remarks>
        FieldStatusTypes FieldStatus { get; }

        /// <summary>
        /// Applies data binding configuration to the specified property of this element using Unity's data binding system.
        /// Enables automatic synchronization between the UI element and data source properties.
        /// </summary>
        /// <param name="fieldBindingProp">The name of the property on this element to bind to the data source.</param>
        /// <param name="fieldBindingPath">The property path in the data source to bind from.</param>
        /// <param name="fieldBindingMode">The binding mode that controls the direction and behavior of data flow.</param>
        /// <remarks>
        /// This method provides a standardized way to establish data binding relationships across all USO UI elements.
        /// The default implementation is empty, allowing individual controls to override with specific binding logic.
        /// Binding modes typically include ToTarget (one-way to UI), ToSource (one-way from UI), and TwoWay (bidirectional).
        /// </remarks>
        void ApplyBinding(string fieldBindingProp, string fieldBindingPath, BindingMode fieldBindingMode) { }

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies framework-specific configuration.
        /// Sets up the element for integration with the USO framework including styling, validation, and identification.
        /// </summary>
        /// <param name="fieldName">The name to assign to this element for identification and binding purposes.</param>
        /// <remarks>
        /// This method serves as the primary initialization point for USO framework integration. It typically includes
        /// setting the element name, applying CSS classes, configuring field status functionality, and any other
        /// framework-specific setup required for proper operation. The default implementation is empty, allowing
        /// individual controls to provide their specific initialization logic.
        /// </remarks>
        void InitElement(string fieldName) { }

        /// <summary>
        /// Removes the specified CSS class name from this element's class list.
        /// This method is inherited from Unity's VisualElement and provides dynamic styling capabilities.
        /// </summary>
        /// <param name="className">The CSS class name to remove from the element.</param>
        /// <remarks>
        /// This method allows runtime modification of element styling by removing CSS classes.
        /// It's commonly used in conjunction with field status changes and conditional styling scenarios.
        /// </remarks>
        void RemoveFromClassList(string className);

        /// <summary>
        /// Adds the specified CSS class name to this element's class list.
        /// This method is inherited from Unity's VisualElement and enables dynamic styling modifications.
        /// </summary>
        /// <param name="className">The CSS class name to add to the element.</param>
        /// <remarks>
        /// This method enables runtime styling changes by adding CSS classes to elements.
        /// It's frequently used for applying validation states, themes, and conditional styling based on element state or user interaction.
        /// </remarks>
        void AddToClassList(string className);

        /// <summary>
        /// Updates the element's field status type, which affects its visual appearance and validation state.
        /// The status change is typically reflected through CSS class modifications and visual indicators.
        /// </summary>
        /// <param name="fieldStatus">The new FieldStatusTypes value to apply to this element.</param>
        /// <remarks>
        /// This method provides a standardized way to update element validation states across the USO framework.
        /// Implementation typically involves updating the FieldStatus property and triggering appropriate visual changes
        /// through the UsoUiHelper.SetFieldStatus utility method.
        /// </remarks>
        void SetFieldStatus(FieldStatusTypes fieldStatus);

        /// <summary>
        /// Controls the visibility and functionality of the field status/validation system for this element.
        /// When disabled, removes validation-related styling and status indicators from the element.
        /// </summary>
        /// <param name="status">True to enable field status functionality; false to disable it.</param>
        /// <remarks>
        /// This method allows dynamic control over whether an element participates in the validation system.
        /// When disabled, validation CSS classes are typically removed and status indicators are hidden,
        /// allowing elements to display in a neutral state without validation feedback.
        /// </remarks>
        void ShowFieldStatus(bool status);

        /// <summary>
        /// Removes all data binding configurations from this element.
        /// This method is inherited from Unity's VisualElement and provides cleanup functionality for data bindings.
        /// </summary>
        /// <remarks>
        /// This method is useful for cleaning up data binding relationships when elements are being disposed
        /// or when binding configurations need to be reset. It helps prevent memory leaks and ensures proper
        /// cleanup of binding resources in dynamic UI scenarios.
        /// </remarks>
        void ClearBindings();


        void ClearField();
    }

    /// <summary>
    /// Enumeration defining the types of UI elements supported by the USO framework.
    /// Used for element categorization, factory methods, and type-specific behavior configuration.
    /// </summary>
    /// <remarks>
    /// This enumeration provides a way to categorize and identify different types of UI elements within the USO framework.
    /// It can be used for factory pattern implementations, configuration systems, and type-specific processing logic.
    /// The enumeration covers the primary interactive and display elements commonly used in USO applications.
    /// </remarks>
    public enum ElementType
    {
        /// <summary>
        /// Represents a button element for user interaction and command execution.
        /// </summary>
        Button,

        /// <summary>
        /// Represents a toggle/checkbox element for boolean input and state management.
        /// </summary>
        Toggle,

        /// <summary>
        /// Represents a floating-point slider element for numeric range input.
        /// </summary>
        Slider,

        /// <summary>
        /// Represents an integer slider element for whole number range input.
        /// </summary>
        SliderInt,

        /// <summary>
        /// Represents a label element for text display and information presentation.
        /// </summary>
        Label,

        /// <summary>
        /// Represents a help box element for displaying informational messages and guidance.
        /// </summary>
        HelpBox,

        /// <summary>
        /// Represents a foldout element for collapsible content organization.
        /// </summary>
        Foldout,

        /// <summary>
        /// Represents a window element for modal dialogs and popup interfaces.
        /// </summary>
        Window,
    }

    /// <summary>
    /// Enumeration defining the visual hierarchy and styling types for label elements within the USO framework.
    /// Used to apply appropriate CSS classes and establish consistent typography throughout the application.
    /// </summary>
    /// <remarks>
    /// This enumeration provides a standardized way to categorize label elements based on their semantic importance
    /// and visual hierarchy. Each type corresponds to specific CSS styling that maintains consistent typography
    /// and visual hierarchy throughout USO applications. The types range from prominent headers to descriptive text,
    /// allowing for clear information architecture and user interface organization.
    /// </remarks>
    public enum LabelType
    {
        /// <summary>
        /// Default label styling with no additional hierarchical emphasis.
        /// Used for standard text labels and general content display.
        /// </summary>
        Default,

        /// <summary>
        /// Primary header styling for top-level section titles and main headings.
        /// Typically the largest and most prominent text styling in the hierarchy.
        /// </summary>
        Header,

        /// <summary>
        /// Secondary header styling for subsection titles and secondary headings.
        /// Smaller than Header but more prominent than other text types.
        /// </summary>
        SubHeader,

        /// <summary>
        /// Title styling for important content titles and significant labels.
        /// Used for content that needs emphasis but is below header level.
        /// </summary>
        Title,

        /// <summary>
        /// Subtitle styling for secondary titles and supporting information headers.
        /// Provides moderate emphasis for organizational content.
        /// </summary>
        Subtitle,

        /// <summary>
        /// Description styling for explanatory text and detailed information.
        /// Typically used for help text, instructions, and supplementary content.
        /// </summary>
        Description,
    }

    /// <summary>
    /// Enumeration defining the validation and status states available for USO UI elements.
    /// Used to communicate field validation results, user feedback, and element states through visual indicators.
    /// </summary>
    /// <remarks>
    /// This enumeration provides a standardized way to represent different states and validation results across
    /// all USO UI elements. Each status type triggers specific visual styling through CSS classes and may include
    /// color changes, icons, or other visual indicators. The status system enables consistent user feedback
    /// for form validation, operation results, and contextual information throughout USO applications.
    /// Status changes are typically applied through the SetFieldStatus method and reflected automatically in the UI.
    /// </remarks>
    public enum FieldStatusTypes
    {
        /// <summary>
        /// Default state with no special status indication.
        /// Used for normal operation when no validation or status feedback is needed.
        /// </summary>
        Default,

        /// <summary>
        /// Error state indicating validation failure or operation errors.
        /// Typically displayed with red styling and error indicators.
        /// </summary>
        Error,

        /// <summary>
        /// Warning state indicating potential issues or cautionary information.
        /// Typically displayed with yellow/amber styling and warning indicators.
        /// </summary>
        Warning,

        /// <summary>
        /// Success state indicating successful validation or completed operations.
        /// Typically displayed with green styling and success indicators.
        /// </summary>
        Success,

        /// <summary>
        /// Informational state for providing contextual information or guidance.
        /// Typically displayed with blue styling and information indicators.
        /// </summary>
        Info,

        /// <summary>
        /// Disabled state indicating the element is inactive or unavailable.
        /// Typically displayed with muted styling and disabled visual cues.
        /// </summary>
        Disabled
    }


}