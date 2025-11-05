using System;
using GWG.USODataFramework.UI.Utilities;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    /// <summary>
    /// Represents a custom UI component that functions as a toggle control with sliding animation.<br/>
    /// Original file from the Unity documentation with only minor compatability modifications made
    /// </summary>
    /// <remarks>
    /// The SlideToggleControl is a specialized Boolean field component derived from the <see cref="bool"/> class.
    /// It provides a visually appealing <see cref="Toggle"/> functionality and is styled through associated USS classes.
    /// </remarks>
    // Derives from BaseField<bool> base class. Represents a container for its input part.
    [UxmlElement]
    public partial class UsoSlideToggle : BaseField<bool>, IUsoUiElement
    {

        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-slide-toggle";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "value";
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

        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion



        // In the spirit of the BEM standard, the SlideToggleControl has its own block class and two element classes. It also
        // has a class that represents the enabled state of the toggle.
        private const string USSClassName = "uso-slide-toggle";
        private const string LabelUssClassName = "uso-slide-toggle__label"; // Reserved lookup, there are many of these in the docs
        private const string InputUssClassName = "uso-slide-toggle__input";
        private const string InputKnobUssClassName = "uso-slide-toggle__input-knob";
        private const string InputCheckedUssClassName = "uso-slide-toggle__input--checked";

        VisualElement m_Input;
        VisualElement m_Knob;
        VisualElement m_Label;


        // Custom controls need a default constructor. This default constructor calls the other constructor in this
        // class.
        /// <summary>
        /// Creates A custom UI control representing a <see cref="Toggle"/> element with a sliding animation but no <see cref="Label"/>,
        /// designed to manage a boolean value in a visually interactive way.
        /// </summary>
        /// <remarks>
        /// SlideToggleControl is derived from <see cref="bool"/> and provides a stylized and interactive
        /// interface for enabling or disabling a boolean state. It includes a sliding knob and supports input
        /// events such as clicking, key presses, and navigation-based submissions, offering a user-friendly
        /// functionality. The control is styled through specific USS classes which can be customized further.
        /// </remarks>
        public UsoSlideToggle() : this(null) { }

        public UsoSlideToggle(string fieldName, string fieldLabelText) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
        }

        public UsoSlideToggle(string fieldName, string fieldLabelText, out UsoSlideToggle newField) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoSlideToggle(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabelText, null)
        {
            InitElement(fieldName);
            ApplyBinding("value", fieldBindingPath, fieldBindingMode);
        }

        // This constructor allows users to set the contents of the label.
        /// <summary>
        /// A custom UI control that visually represents a <see cref="Toggle"/> element with a sliding animation and <see cref="Label"/>,
        /// It serves as a container for its child control,
        /// providing an interactive element for enabling or disabling a boolean value.
        /// </summary>
        /// <remarks>
        /// This class derives from <see cref="bool"/> and is stylized to align
        /// content in a flexible and user-friendly manner.
        /// Users can activate or deactivate the toggle through multiple input methods:
        /// clicking, pressing a key, or submitting navigation input.
        /// The control includes custom styling and a knob element to enhance visualization.
        /// </remarks>
        public UsoSlideToggle(string label) : base(label, null)
        {
            InitElement();
        }
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
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

        // These three methods are static functions that are called by the event handlers.
        private static void OnClick(ClickEvent evt)
        {
            var slideToggle = evt.currentTarget as UsoSlideToggle;
            slideToggle?.ToggleValue();

            evt.StopPropagation();
        }

        // This method is static because it is called by the NavigationSubmitEvent.
        private static void OnSubmit(NavigationSubmitEvent evt)
        {
            var slideToggle = evt.currentTarget as UsoSlideToggle;
            slideToggle?.ToggleValue();

            evt.StopPropagation();
        }

        // This method is static because it is called by the KeyDownEvent.
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

        // All three callbacks call this method.
        /// <summary>
        /// Toggles the current value of the <see cref="SlideToggleControl"/> between true and false.
        /// Updates the visual representation of the toggle to reflect the changed state.
        /// </summary>
        /// <remarks>
        /// This method is used internally by event handlers such as <see cref="ClickEvent"/>, <see cref="KeyDownEvent"/>,
        /// and <see cref="NavigationSubmitEvent"/> to alter the toggle state in response to user interactions.
        /// It flips the boolean value and adjusts the toggle's appearance accordingly.
        /// </remarks>
        private void ToggleValue()
        {
            value = !value;
        }

        // Because ToggleValue() sets the value property, the BaseField class dispatches a ChangeEvent. This results in a
        // call to SetValueWithoutNotify(). This example uses it to style the toggle based on whether it's currently
        // enabled.
        /// <summary>
        /// Sets the value of the <see cref="Toggle"/> without triggering a change event notification.
        /// This method updates internal state and allows the control to reflect the new value
        /// without invoking any event handlers or property change listeners.
        /// </summary>
        /// <param name="newValue">The new value to be set for the toggle element.</param>
        public override void SetValueWithoutNotify(bool newValue)
        {
            base.SetValueWithoutNotify(newValue);

            //This line of code styles the input element to look enabled or disabled.
            m_Input.EnableInClassList(InputCheckedUssClassName, newValue);
        }
    }
}
