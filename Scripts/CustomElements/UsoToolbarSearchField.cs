
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom toolbar search field control that extends Unity's BindableElement with specialized search functionality and USO UI framework integration.
    /// Provides enhanced search capabilities, value change notifications, integrated clear functionality, and horizontal layout optimization for toolbar contexts.
    /// </summary>
    /// <remarks>
    /// This control implements the INotifyValueChanged&lt;string&gt; interface to provide standardized value change notification behavior.
    /// It combines a UsoTextField for text input with a UsoToolbarButton for clear functionality, creating a complete search experience.
    /// The control is specifically designed for toolbar integration with flexible layout properties, automatic sizing, and centered alignment.
    /// It features automatic value synchronization between the internal text field and the search field's value property, ensuring
    /// consistent state management and change notifications. The integrated clear button provides immediate search reset capability
    /// with appropriate styling and click handling for enhanced user experience within toolbar contexts.
    /// The control maintains USO framework styling consistency while providing specialized search field behavior.
    /// </remarks>
    public class UsoToolbarSearchField : BindableElement, INotifyValueChanged<string>
    {
        /// <summary>
        /// Private backing field for the search field's current value.
        /// Used to track the current search text and coordinate value changes between internal components.
        /// </summary>
        private string _value;

        /// <summary>
        /// Gets or sets the internal UsoTextField component that provides the text input functionality.
        /// This text field handles the actual text input, editing, and user interaction for the search field.
        /// </summary>
        /// <value>The UsoTextField Instance used for text input operations.</value>
        /// <remarks>
        /// This property provides access to the underlying text field component for advanced configuration,
        /// event handling, or styling operations that may be needed beyond the standard search field functionality.
        /// </remarks>
        public UsoTextField textfield
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current search value of the toolbar search field.
        /// Setting this property updates the internal text field without triggering change notifications.
        /// </summary>
        /// <value>The current search text as a string.</value>
        /// <remarks>
        /// This property provides the primary interface for getting and setting the search field's value.
        /// When setting the value, it uses SetValueWithoutNotify to prevent recursive change notifications
        /// while ensuring the internal text field displays the correct value.
        /// </remarks>
        public string value
        {
            get
            {
                return _value;
            }
            set
            {
                textfield.SetValueWithoutNotify(value);
            }
        }

        /// <summary>
        /// Sets the search field's value without triggering value change notifications.
        /// This method provides direct value assignment for scenarios where change events should not be fired.
        /// </summary>
        /// <param name="newValue">The new search value to assign to the field.</param>
        /// <remarks>
        /// This method is useful for programmatic value updates where change notifications are not desired,
        /// such as during initialization, data loading, or when responding to external value changes.
        /// It ensures both the internal backing field and the text field display the correct value.
        /// </remarks>
        public void SetValueWithoutNotify(string newValue)
        {
            _value = newValue;
            textfield.value = _value;
        }

        /// <summary>
        /// Explicit implementation of the INotifyValueChanged&lt;string&gt;.value property.
        /// Provides standardized value access for Unity's binding and notification systems.
        /// </summary>
        /// <value>The current search text value for binding and notification purposes.</value>
        /// <remarks>
        /// This explicit interface implementation ensures compatibility with Unity's value change notification system
        /// while maintaining the public value property for direct access. The getter returns the current value,
        /// while the setter uses SetValueWithoutNotify to prevent notification loops.
        /// </remarks>
        string INotifyValueChanged<string>.value
        {
            get
            {
                return _value;
            }
            set
            {
                textfield.SetValueWithoutNotify(value);
            }
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbarSearchField class with integrated text field and clear button functionality.
        /// Creates a complete search interface with horizontal layout, flexible sizing, and automatic value synchronization.
        /// </summary>
        /// <remarks>
        /// The constructor performs comprehensive setup including:
        /// - Horizontal row layout configuration with flexible growth and shrinkage
        /// - Center alignment for optimal toolbar presentation
        /// - UsoTextField creation and configuration with flexible sizing
        /// - Value change callback registration for automatic synchronization
        /// - Clear button creation with fixed sizing and styling
        /// - Clear button click handling for immediate search reset functionality
        /// The resulting control provides a complete search experience optimized for toolbar contexts.
        /// </remarks>
        public UsoToolbarSearchField()
        {
            style.flexDirection = FlexDirection.Row;
            style.flexGrow = 1;
            style.flexShrink = 1;
            style.alignItems = Align.Center;
            textfield = new UsoTextField();
            textfield.style.flexGrow = 1;
            textfield.style.flexShrink = 1;
            textfield.style.marginRight = 0;

            Add(textfield);
            textfield.RegisterValueChangedCallback(evt =>
            {
                _value = evt.newValue;
                this.value = _value;
            });

            // add a clear button
            UsoToolbarButton clearButton = new UsoToolbarButton
            {
                style =
                {
                    maxWidth = 20,
                    maxHeight = 20,
                    flexShrink = 0,
                    flexGrow = 0,
                    marginLeft = 0
                },
                text = "X"
            };
            clearButton.AddToClassList("clear-button");
            clearButton.clickable.clicked += () =>
            {
                value = "";
            };
            Add(clearButton);
        }

    }
}