
using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom slide toggle control that extends Unity's BaseField&lt;bool&gt; with USO UI framework functionality and animated sliding behavior.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system for interactive boolean input with visual sliding animation.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for boolean values, and custom styling through CSS classes.
    /// The control is derived from Unity documentation examples with compatibility modifications for the USO framework.
    /// It features a visual sliding knob animation that provides intuitive feedback for boolean state changes, supporting
    /// multiple interaction methods including mouse clicks, keyboard navigation, and gamepad input. The control requires
    /// a specific USS stylesheet to function properly and automatically loads the appropriate styling resources.
    /// The sliding toggle provides an enhanced user experience compared to standard checkboxes with smooth visual transitions
    /// and responsive interaction handling across different input devices and contexts.
    /// </remarks>
    // Derives from BaseField<bool> base class. Represents a container for its input part.
    [UxmlElement]
    public partial class UsoSlideToggle : BaseField<bool>, IUsoUiElement
    {

        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoSlideToggle instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-slide-toggle";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property which controls the toggle's boolean state.
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

        // In the spirit of the BEM standard, the SlideToggleControl has its own block class and two element classes. It also
        // has a class that represents the enabled state of the toggle.

        /// <summary>
        /// Primary USS class name for the slide toggle control following BEM naming conventions.
        /// </summary>
        private const string USSClassName = "uso-slide-toggle";

        /// <summary>
        /// USS class name for the label element of the slide toggle control.
        /// Reserved for potential future label-specific styling requirements.
        /// </summary>
        private const string LabelUssClassName = "uso-slide-toggle__label"; // Reserved lookup, there are many of these in the docs

        /// <summary>
        /// USS class name for the input background element of the slide toggle control.
        /// </summary>
        private const string InputUssClassName = "uso-slide-toggle__input";

        /// <summary>
        /// USS class name for the sliding knob element within the toggle control.
        /// </summary>
        private const string InputKnobUssClassName = "uso-slide-toggle__input-knob";

        /// <summary>
        /// USS class name applied when the toggle is in the checked/enabled state.
        /// </summary>
        private const string InputCheckedUssClassName = "uso-slide-toggle__input--checked";

        /// <summary>
        /// Visual element representing the input background area of the slide toggle.
        /// </summary>
        VisualElement m_Input;

        /// <summary>
        /// Visual element representing the sliding knob that moves within the toggle.
        /// </summary>
        VisualElement m_Knob;

        /// <summary>
        /// Visual element representing the text label associated with the toggle.
        /// </summary>
        VisualElement m_Label;

        /// <summary>
        /// Initializes a new Instance of the UsoSlideToggle class with default settings.
        /// Creates a slide toggle control with USO framework integration and no initial label text.
        /// </summary>
        /// <remarks>
        /// This default constructor calls the label-based constructor with a null parameter to maintain
        /// consistent initialization behavior while providing a parameterless constructor option.
        /// </remarks>
        public UsoSlideToggle() : this(null) { }

        /// <summary>
        /// Initializes a new Instance of the UsoSlideToggle class with field name and label text.
        /// Creates a slide toggle control with custom identification and display label for user interface clarity.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slide toggle element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slide toggle control.</param>
        public UsoSlideToggle(string fieldName, string fieldLabelText) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoSlideToggle class with field name, label text, and returns a reference.
        /// Creates a slide toggle control with custom identification, display label, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slide toggle element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slide toggle control.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created slide toggle.</param>
        public UsoSlideToggle(string fieldName, string fieldLabelText, out UsoSlideToggle newField) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoSlideToggle class with field name, label text, and data binding configuration.
        /// Creates a fully configured slide toggle control with custom identification, display label, and automatic data binding for boolean value synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this slide toggle element.</param>
        /// <param name="fieldLabelText">The label text to display alongside the slide toggle control.</param>
        /// <param name="fieldBindingPath">The path to the data source property for automatic value binding.</param>
        /// <param name="fieldBindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="usoSlideToggle"></param>
        public UsoSlideToggle(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
            ApplyBinding("value", fieldBindingPath, fieldBindingMode);
        }

        public UsoSlideToggle(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode, out UsoSlideToggle usoSlideToggle) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
            ApplyBinding("value", fieldBindingPath, fieldBindingMode);
            usoSlideToggle = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoSlideToggle class with the specified label text.
        /// Creates a slide toggle control with custom label text for user interface presentation.
        /// </summary>
        /// <param name="label">The label text to display alongside the slide toggle control.</param>
        /// <remarks>
        /// This constructor provides the primary initialization path for slide toggle controls with labels,
        /// setting up the control with appropriate styling and interactive behavior for boolean value management.
        /// </remarks>
        public UsoSlideToggle(string label) : base(label, null)
        {
            InitElement();
        }

        /// <summary>
        /// Initializes the USO UI element with comprehensive slide toggle setup including styling, event handlers, and visual components.
        /// This method sets up the complete slide toggle functionality with USO framework integration and interactive behavior.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// The initialization process includes:
        /// - Loading required USS stylesheet for slide toggle styling
        /// - Setting up visual elements (input background, knob, label)
        /// - Configuring CSS classes following BEM naming conventions
        /// - Registering event handlers for click, keyboard, and navigation input
        /// - Applying Unity Editor-specific styling for inspector compatibility
        /// The control requires the UsoSlideToggle stylesheet to function properly and will attempt to load it from resources.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
            style.flexShrink = 0;
            // This control REQUIRES a specific USS stylesheet to operate.
            StyleSheet styleSheet = Resources.Load<StyleSheet>("UsoUiElements/Stylesheets/UsoSlideToggle");

            /*
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                styleSheet = Resources.Load<StyleSheet>("UsoUiElements/Editor/UsoSlideToggle");
            }
            #endif
            */


            if (styleSheet != null)
            {
                styleSheets.Add(styleSheet);
            }
            // Style the control overall.
            AddToClassList(USSClassName);
            // Get the BaseField's label element and use it as the label for the toggle.'
            m_Label = this.labelElement;

            m_Label?.ClearClassList();
            m_Label?.AddToClassList(LabelUssClassName);
            m_Label?.AddToClassList("uso-slide-toggle__label");

            // Get the BaseField's visual input element and use it as the background of the slide.
            m_Input = this.Q(className: BaseField<bool>.inputUssClassName);
            m_Input.AddToClassList(InputUssClassName);

            // Create a "knob" child element for the background to represent the actual slide of the toggle.
            m_Knob = new();
            m_Knob.AddToClassList(InputKnobUssClassName);
            m_Input.Add(m_Knob);

            // There are three main ways to activate or deactivate the SlideToggleControl. All three event handlers use the
            // static function pattern described in the Custom control best practices.

            // ClickEvent fires when a sequence of pointer down and pointer up actions occurs.
            RegisterCallback<ClickEvent>(evt => OnClick(evt));
            // KeydownEvent fires when the field has focus and a user presses a key.
            RegisterCallback<KeyDownEvent>(evt => OnKeydownEvent(evt));
            // NavigationSubmitEvent detects input from keyboards, gamepads, or other devices at runtime.
            RegisterCallback<NavigationSubmitEvent>(evt => OnSubmit(evt));

            #if UNITY_EDITOR
            // Add the Unity-specific classes to the control to make it match in inspector
            AddToClassList("unity-base-bool-field");
            AddToClassList("unity-bool-field");
            AddToClassList("unity-base-field__aligned");
            m_Label?.AddToClassList("unity-base-text-field__label");
            m_Label?.AddToClassList("unity-text-field__label");
            m_Label?.AddToClassList("unity-property-field__label");
            m_Label?.AddToClassList("uso-slide-toggle__label");
            m_Input.style.marginTop = StyleKeyword.Auto;
            m_Input.style.marginBottom = StyleKeyword.Auto;
            m_Input.style.minHeight = 16;
            #endif
        }

        /// <summary>
        /// Static event handler for click events on the slide toggle control.
        /// Toggles the boolean value when the user clicks on the control and prevents event propagation.
        /// </summary>
        /// <param name="evt">The ClickEvent containing information about the click interaction.</param>
        /// <remarks>
        /// This method follows Unity's static event handler pattern for custom controls to ensure proper
        /// event handling and performance. It safely casts the event target and invokes the toggle operation.
        /// </remarks>
        private static void OnClick(ClickEvent evt)
        {
            var slideToggle = evt.currentTarget as UsoSlideToggle;
            slideToggle?.ToggleValue();

            evt.StopPropagation();
        }

        /// <summary>
        /// Static event handler for navigation submit events on the slide toggle control.
        /// Handles gamepad and other navigation device input to toggle the boolean value.
        /// </summary>
        /// <param name="evt">The NavigationSubmitEvent containing information about the navigation input.</param>
        /// <remarks>
        /// This method provides accessibility and gamepad support for the slide toggle control,
        /// following Unity's static event handler pattern for optimal performance and proper event management.
        /// </remarks>
        private static void OnSubmit(NavigationSubmitEvent evt)
        {
            var slideToggle = evt.currentTarget as UsoSlideToggle;
            slideToggle?.ToggleValue();

            evt.StopPropagation();
        }

        /// <summary>
        /// Static event handler for keyboard events on the slide toggle control.
        /// Toggles the boolean value when specific keys (Enter, Return, Space) are pressed in editor contexts.
        /// </summary>
        /// <param name="evt">The KeyDownEvent containing information about the key press.</param>
        /// <remarks>
        /// This method specifically handles keyboard input in editor contexts, as runtime navigation
        /// is handled by NavigationSubmitEvent. It responds to Enter, Return, and Space key presses
        /// to provide standard keyboard accessibility for toggle operations.
        /// </remarks>
        private static void OnKeydownEvent(KeyDownEvent evt)
        {
            var slideToggle = evt.currentTarget as UsoSlideToggle;

            // NavigationSubmitEvent event already covers keydown events at runtime, so this method shouldn't handle
            // them.
            if (slideToggle?.panel?.contextType == ContextType.Player)
                return;

            // Toggle the value only when the user presses Enter, Return, or Space.
            if (evt.keyCode == KeyCode.KeypadEnter || evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.Space)
            {
                slideToggle?.ToggleValue();
                evt.StopPropagation();
            }
        }

        /// <summary>
        /// Toggles the current boolean value of the slide toggle control between true and false.
        /// This method is called by all interaction event handlers to change the toggle state.
        /// </summary>
        /// <remarks>
        /// This private method provides the core toggle functionality used by click, keyboard, and navigation
        /// event handlers. Setting the value property automatically triggers change notifications and visual updates
        /// through the BaseField change event system and SetValueWithoutNotify method.
        /// </remarks>
        private void ToggleValue()
        {
            value = !value;
        }

        /// <summary>
        /// Sets the boolean value of the slide toggle without triggering change event notifications.
        /// Updates the visual state of the toggle to reflect the new value through CSS class management.
        /// </summary>
        /// <param name="newValue">The new boolean value to assign to the toggle control.</param>
        /// <remarks>
        /// This override method extends the base functionality to include visual state updates for the slide toggle.
        /// It manages the checked CSS class to ensure the sliding animation and visual appearance correctly
        /// reflect the current boolean state. This method is called automatically when the value property changes
        /// through the BaseField change event system.
        /// </remarks>
        public override void SetValueWithoutNotify(bool newValue)
        {
            base.SetValueWithoutNotify(newValue);

            //This line of code styles the input element to look enabled or disabled.
            m_Input.EnableInClassList(InputCheckedUssClassName, newValue);
        }
    }
}