
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using System.Linq;

namespace GWG.UsoUIElements.Utilities
{
    /// <summary>
    /// A static utility class that provides helper methods and extensions for working with Unity's UI Elements system.
    /// This class offers functionality for field validation styling, LINQ-style query operations, element creation, manipulation, and traversal.
    /// </summary>
    /// <remarks>
    /// The UsoUiHelper class serves as the central utility hub for the Uso UI Elements system, providing:
    /// - Field validation status management through CSS class manipulation
    /// - LINQ-style extension methods for UQueryBuilder operations
    /// - Fluent API extensions for creating and styling visual elements
    /// - Convenience methods for common UI element operations like visibility toggling and event handling
    /// - Hierarchical traversal methods for finding elements within the visual tree
    ///
    /// All methods are designed to follow fluent interface patterns where applicable, allowing for method chaining.
    /// The class integrates with the broader Uso UI validation system through standardized CSS class naming conventions.
    /// </remarks>
    public static class UsoUiHelper
    {
        /// <summary>
        /// A list of CSS class names used for field validation status styling.
        /// These classes correspond to different validation states and are used for visual feedback.
        /// </summary>
        /// <remarks>
        /// The list contains predefined CSS class names for all supported field validation states:
        /// - uso-field-label--error: Applied when a field has validation errors
        /// - uso-field-label--warning: Applied when a field has validation warnings
        /// - uso-field-label--success: Applied when a field passes validation successfully
        /// - uso-field-label--info: Applied when a field displays informational status
        /// - uso-field-label--disabled: Applied when a field is disabled
        ///
        /// These classes should have corresponding styles defined in the project's CSS to provide appropriate visual feedback.
        /// </remarks>
        public static List<string> FieldValidationClasses = new List<string>()
        {
            "uso-field-label--error",
            "uso-field-label--warning",
            "uso-field-label--success",
            "uso-field-label--info",
            "uso-field-label--disabled"
        };

        /// <summary>
        /// Sets the field validation status for a visual element by applying the appropriate CSS class.
        /// Removes any existing validation classes and applies the class corresponding to the specified status.
        /// </summary>
        /// <param name="element">The VisualElement to apply the status styling to.</param>
        /// <param name="fieldStatus">The field status type to apply.</param>
        /// <remarks>
        /// This method first removes all validation classes from the element to ensure a clean state,
        /// then applies the appropriate class based on the fieldStatus parameter. The Default status
        /// results in no additional classes being applied, allowing the element to use its base styling.
        ///
        /// The method is central to the Uso UI validation system and ensures consistent visual feedback
        /// across all form elements that implement field validation.
        /// </remarks>
        public static void SetFieldStatus(VisualElement element, FieldStatusTypes fieldStatus)
        {
            foreach (string className in FieldValidationClasses)
            {
                element.RemoveFromClassList(className);
            }

            switch (fieldStatus)
            {
                case FieldStatusTypes.Error:
                    element.AddToClassList("uso-field-label--error");
                    break;
                case FieldStatusTypes.Warning:
                    element.AddToClassList("uso-field-label--warning");
                    break;
                case FieldStatusTypes.Success:
                    element.AddToClassList("uso-field-label--success");
                    break;
                case FieldStatusTypes.Info:
                    element.AddToClassList("uso-field-label--info");
                    break;
                case FieldStatusTypes.Disabled:
                    element.AddToClassList("uso-field-label--disabled");
                    break;
                case FieldStatusTypes.Default:
                default:
                    break;
            }
        }

        /// <summary>
        /// Extends UQueryBuilder with LINQ-style OrderBy functionality to sort elements in ascending order according to a key.
        /// Converts the query to a list and applies standard LINQ OrderBy with the provided key selector and comparer.
        /// </summary>
        /// <typeparam name="T">The type of VisualElement being queried.</typeparam>
        /// <typeparam name="TKey">The type of the key used for sorting.</typeparam>
        /// <param name="query">The UQueryBuilder containing the elements to be sorted.</param>
        /// <param name="keySelector">A function to extract a sort key from each element.</param>
        /// <param name="default">The Comparer to use for comparing keys.</param>
        /// <returns>An ordered sequence of elements sorted by the specified key.</returns>
        /// <remarks>
        /// This extension method bridges the gap between Unity's UQueryBuilder system and standard LINQ operations.
        /// It materializes the query results into a list before applying the ordering operation.
        /// The method allows for custom comparison logic through the comparer parameter.
        /// </remarks>
        public static IEnumerable<T> OrderBy<T, TKey>(this UQueryBuilder<T> query, Func<T, TKey> keySelector, Comparer<TKey> @default)
            where T : VisualElement
        {
            return query.ToList().OrderBy(keySelector, @default);
        }

        /// <summary>
        /// Extends UQueryBuilder with functionality to sort elements by a numeric key in ascending order.
        /// This is a specialized version of OrderBy optimized for numeric sorting operations.
        /// </summary>
        /// <typeparam name="T">The type of VisualElement being queried.</typeparam>
        /// <param name="query">The UQueryBuilder containing the elements to be sorted.</param>
        /// <param name="keySelector">A function to extract a numeric key from each element.</param>
        /// <returns>An ordered sequence of elements sorted by the numeric key in ascending order.</returns>
        /// <remarks>
        /// This method provides a convenient way to sort UI elements by numeric properties such as positions,
        /// sizes, or custom numeric attributes. It uses the default float comparer for consistent sorting behavior.
        /// Common use cases include sorting elements by their layout positions or custom numeric data attributes.
        /// </remarks>
        public static IEnumerable<T> SortByNumericValue<T>(this UQueryBuilder<T> query, Func<T, float> keySelector)
            where T : VisualElement
        {
            return query.OrderBy(keySelector, Comparer<float>.Default);
        }

        /// <summary>
        /// Extends UQueryBuilder with LINQ-style FirstOrDefault functionality to return the first element or a default value.
        /// Converts the query to a list and returns the first element, or null if the sequence is empty.
        /// </summary>
        /// <typeparam name="T">The type of VisualElement being queried.</typeparam>
        /// <param name="query">The UQueryBuilder containing the elements to search.</param>
        /// <returns>The first element in the sequence, or null if no elements are found.</returns>
        /// <remarks>
        /// This extension method provides safe access to the first element in a query result without throwing exceptions
        /// when the sequence is empty. It's particularly useful for optional element lookups where the absence of an element
        /// is a valid scenario that should be handled gracefully.
        /// </remarks>
        public static T FirstOrDefault<T>(this UQueryBuilder<T> query)
            where T : VisualElement
        {
            return query.ToList().FirstOrDefault();
        }

        /// <summary>
        /// Extends UQueryBuilder with LINQ-style Count functionality to count elements that satisfy a specified condition.
        /// Converts the query to a list and applies the predicate function to count matching elements.
        /// </summary>
        /// <typeparam name="T">The type of VisualElement being queried.</typeparam>
        /// <param name="query">The UQueryBuilder containing the sequence of elements to be processed.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>The number of elements that satisfy the condition specified by the predicate.</returns>
        /// <remarks>
        /// This method enables conditional counting operations on UI element queries, useful for validation scenarios,
        /// state analysis, or determining the presence of elements with specific characteristics.
        /// The predicate function is applied to each element, and only elements that return true are counted.
        /// </remarks>
        public static int CountWhere<T>(this UQueryBuilder<T> query, Func<T, bool> predicate)
            where T : VisualElement
        {
            return query.ToList().Count(predicate);
        }

        /// <summary>
        /// Creates a new child VisualElement, applies optional CSS classes, and adds it to the specified parent element.
        /// This method provides a fluent interface for rapid UI construction.
        /// </summary>
        /// <param name="parent">The parent VisualElement to add the new child to.</param>
        /// <param name="classes">Optional array of CSS class names to apply to the new child element.</param>
        /// <returns>The newly created child VisualElement.</returns>
        /// <remarks>
        /// This method streamlines the common pattern of creating, styling, and parenting UI elements in a single operation.
        /// It uses the fluent interface pattern to allow for readable and concise UI construction code.
        /// The method applies classes and parents the element in one atomic operation for convenience.
        /// </remarks>
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Creates a new child element of the specified type, applies optional CSS classes, and adds it to the specified parent element.
        /// This generic method allows creation of specific VisualElement types with fluent interface support.
        /// </summary>
        /// <typeparam name="T">The specific type of VisualElement to create, must have a parameterless constructor.</typeparam>
        /// <param name="parent">The parent VisualElement to add the new child to.</param>
        /// <param name="classes">Optional array of CSS class names to apply to the new child element.</param>
        /// <returns>The newly created child element of type T.</returns>
        /// <remarks>
        /// This generic version of CreateChild allows for creating specific UI element types (like Button, Label, TextField, etc.)
        /// while maintaining the fluent interface pattern. It's particularly useful when you need to manipulate the created
        /// element further and want to maintain the specific type rather than working with the base VisualElement type.
        /// The return type is the specific element type, allowing immediate access to type-specific properties and methods.
        /// </remarks>
        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        /// <summary>
        /// Adds the specified child element to a parent element and returns the child for method chaining.
        /// This method enables fluent interface patterns for element hierarchy construction.
        /// </summary>
        /// <typeparam name="T">The type of the child VisualElement.</typeparam>
        /// <param name="child">The child element to be added to the parent.</param>
        /// <param name="parent">The parent VisualElement to add the child to.</param>
        /// <returns>The child element, allowing for continued method chaining.</returns>
        /// <remarks>
        /// This extension method supports fluent interface patterns by allowing element creation and parenting
        /// to be chained with other operations. It's designed to work seamlessly with other fluent methods
        /// in this class to enable readable and concise UI construction code.
        /// </remarks>
        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        /// <summary>
        /// Adds multiple CSS classes to a VisualElement and returns the element for method chaining.
        /// This method provides a fluent interface for applying multiple CSS classes in a single operation.
        /// </summary>
        /// <typeparam name="T">The type of the VisualElement.</typeparam>
        /// <param name="visualElement">The VisualElement to add classes to.</param>
        /// <param name="classes">Array of CSS class names to add to the element.</param>
        /// <returns>The VisualElement with applied classes, allowing for continued method chaining.</returns>
        /// <remarks>
        /// This method iterates through the provided class names and applies each non-null and non-empty class
        /// to the element. It supports the fluent interface pattern for streamlined element styling during creation.
        /// Null or empty class names are safely ignored without throwing exceptions.
        /// </remarks>
        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (string cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    visualElement.AddToClassList(cls);
                }
            }
            return visualElement;
        }

        /// <summary>
        /// Adds a single CSS class to a VisualElement with null-checking for safety.
        /// This method provides a safe way to add CSS classes with proper error handling.
        /// </summary>
        /// <param name="ele">The VisualElement to add the class to.</param>
        /// <param name="className">The CSS class name to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method provides explicit null-checking and clear error messages for debugging purposes.
        /// It's designed for scenarios where you need guaranteed safety and clear error reporting.
        /// </remarks>
        public static void AddClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.AddToClassList(className);
        }

        /// <summary>
        /// Removes a CSS class from a VisualElement with null-checking for safety.
        /// This method provides a safe way to remove CSS classes with proper error handling.
        /// </summary>
        /// <param name="ele">The VisualElement to remove the class from.</param>
        /// <param name="className">The CSS class name to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method provides explicit null-checking and clear error messages for debugging purposes.
        /// It safely removes the specified class if it exists, with no effect if the class is not present.
        /// </remarks>
        public static void RemoveClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.RemoveFromClassList(className);
        }

        /// <summary>
        /// Toggles a CSS class on a VisualElement (adds if not present, removes if present) with null-checking for safety.
        /// This method provides a convenient way to toggle CSS classes for state changes.
        /// </summary>
        /// <param name="ele">The VisualElement to toggle the class on.</param>
        /// <param name="className">The CSS class name to toggle.</param>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method is particularly useful for implementing toggle behaviors, hover effects, or state changes
        /// where a CSS class represents a binary state. It provides explicit null-checking for safety.
        /// </remarks>
        public static void ToggleClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.ToggleInClassList(className);
        }

        /// <summary>
        /// Adds a click event listener to a VisualElement with null-checking for safety.
        /// This method provides a convenient way to register click event handlers.
        /// </summary>
        /// <param name="ele">The VisualElement to add the click listener to.</param>
        /// <param name="callback">The event callback to execute when the element is clicked.</param>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method simplifies the common pattern of adding click event handlers to UI elements.
        /// It provides explicit null-checking and a more readable method name than the generic RegisterCallback.
        /// </remarks>
        public static void AddClickListener(this VisualElement ele, EventCallback<ClickEvent> callback)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.RegisterCallback(callback);
        }

        /// <summary>
        /// Determines whether a VisualElement is currently visible by checking its display style.
        /// Returns true if the element's display style is not set to None.
        /// </summary>
        /// <param name="ele">The VisualElement to check for visibility.</param>
        /// <returns>True if the element is visible (display is not None); otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method checks the resolved display style to determine actual visibility state.
        /// It's useful for conditional logic that depends on element visibility without directly checking CSS properties.
        /// Note that this only checks the display property and doesn't account for other factors like opacity or parent visibility.
        /// </remarks>
        public static bool IsVisible(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            return ele.resolvedStyle.display != DisplayStyle.None;
        }

        /// <summary>
        /// Toggles the visibility of a VisualElement between visible (Flex) and hidden (None) display states.
        /// This method provides a convenient way to show/hide elements with a single method call.
        /// </summary>
        /// <param name="ele">The VisualElement to toggle visibility for.</param>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method toggles between DisplayStyle.None (hidden) and DisplayStyle.Flex (visible).
        /// It's commonly used for implementing show/hide functionality, collapsible sections, or modal dialogs.
        /// The method preserves the element's layout properties when visible by using Flex display mode.
        /// </remarks>
        public static void ToggleVisibility(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.style.display = ele.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>
        /// Adds a manipulator to a VisualElement and returns the element for method chaining.
        /// This method enables fluent interface patterns for adding interaction manipulators.
        /// </summary>
        /// <typeparam name="T">The type of the VisualElement.</typeparam>
        /// <param name="visualElement">The VisualElement to add the manipulator to.</param>
        /// <param name="manipulator">The IManipulator to add to the element.</param>
        /// <returns>The VisualElement with the added manipulator, allowing for continued method chaining.</returns>
        /// <remarks>
        /// This method supports fluent interface patterns by allowing manipulator addition to be chained
        /// with other element configuration operations. Manipulators handle complex interactions like dragging,
        /// resizing, or custom gesture recognition.
        /// </remarks>
        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement
        {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }

        /// <summary>
        /// Recursively traverses up the visual hierarchy to find and return the root element of the document.
        /// This method climbs the parent chain until it reaches an element with no parent.
        /// </summary>
        /// <param name="ele">The starting VisualElement to begin the traversal from.</param>
        /// <returns>The root VisualElement of the document hierarchy.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <remarks>
        /// This method uses recursion to traverse up the visual tree until it finds the root element.
        /// It's useful for accessing document-level properties or performing operations that require
        /// knowledge of the complete hierarchy context. The root element is typically the UIDocument's root visual element.
        /// </remarks>
        public static VisualElement GetDocumentRoot(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            return (ele.parent == null ? ele : ele.parent.GetDocumentRoot());
        }

        /// <summary>
        /// Recursively searches for a child element with the specified name within the visual hierarchy.
        /// This method performs a depth-first search through all descendants of the starting element.
        /// </summary>
        /// <param name="ele">The starting VisualElement to begin the search from.</param>
        /// <param name="name">The name of the child element to find.</param>
        /// <returns>The first child VisualElement with the matching name, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the element parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the name parameter is null or empty.</exception>
        /// <remarks>
        /// This method performs a recursive depth-first search through the entire visual tree starting from the specified element.
        /// It checks immediate children first, then recursively searches within each child's hierarchy.
        /// The search returns the first matching element found, so element names should be unique within the search scope for predictable results.
        /// This method is useful for finding elements by their programmatically assigned names when direct references are not available.
        /// </remarks>
        public static VisualElement FindChildByName(this VisualElement ele, string name)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            foreach (var child in ele.Children())
            {
                if (child.name == name)
                    return child;

                var result = child.FindChildByName(name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}