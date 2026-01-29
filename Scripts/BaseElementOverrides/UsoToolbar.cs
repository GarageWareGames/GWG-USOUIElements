using GWG.UsoUIElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom toolbar control that extends Unity's Toolbar with USO UI framework functionality.
    /// Provides enhanced styling, field validation, orientation control, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, custom styling through CSS classes, and flexible orientation control.
    /// The toolbar can be configured to display horizontally or vertically, with automatic flex direction adjustment.
    /// This control is typically used for grouping related UI controls and actions in a structured layout.
    /// The control includes commented code for potential future content container customization.
    /// </remarks>
    [UxmlElement]
    public partial class UsoToolbar : Toolbar, IUsoUiElement
    {
        /// <summary>
        /// CSS class name applied to all UsoToolbar instances for styling purposes.
        /// </summary>
        private const string ElementStylesheet = "uso-toolbar";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Currently set to "value" for potential future binding scenarios.
        /// </summary>
        private const string DefaultBindProp = "value";

        /// <summary>
        /// Private backing field for the FieldStatusEnabled property.
        /// </summary>
        private bool _fieldStatusEnabled;

        /// <summary>
        /// Private backing field for the FieldStatus property.
        /// </summary>
        private FieldStatusTypes _fieldStatus;

        //VisualElement _content;
        //public override VisualElement contentContainer => _content;

        /// <summary>
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is false.</value>
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

        /// <summary>
        /// Gets or sets the orientation of the toolbar, which controls the layout direction of child elements.
        /// Automatically adjusts the flex direction style property to match the specified orientation.
        /// </summary>
        /// <value>The ToolbarOrientation value indicating horizontal or vertical layout. Default is Horizontal.</value>
        /// <remarks>
        /// When set to Horizontal, the flex direction is set to Row.
        /// When set to Vertical, the flex direction is set to Column.
        /// This property provides a convenient way to control toolbar layout without manually setting CSS properties.
        /// </remarks>
        [UxmlAttribute]
        public ToolbarOrientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
                if (value == ToolbarOrientation.Horizontal)
                {
                    style.flexDirection = FlexDirection.Row;
                }
                else
                {
                    style.flexDirection = FlexDirection.Column;
                }
            }
        }
        private ToolbarOrientation _orientation = ToolbarOrientation.Horizontal;

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
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// The method includes commented code for potential future content container customization.
        /// Currently sets up basic styling and field status functionality.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            //_content = new UsoVisualElement();
            //_content.style.flexGrow = 1;
           // hierarchy.Insert(0, _content);
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
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

        /// <summary>
        /// Initializes a new Instance of the UsoToolbar class with default settings.
        /// Creates a toolbar with USO framework integration and horizontal orientation.
        /// </summary>
        public UsoToolbar()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbar class with the specified field name.
        /// Creates a toolbar with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar element.</param>
        public UsoToolbar(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoToolbar class with field name and returns a reference.
        /// Creates a toolbar with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this toolbar element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created toolbar.</param>
        public UsoToolbar(string fieldName, out UsoToolbar newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }
    }

    /// <summary>
    /// Enumeration that defines the available orientation options for UsoToolbar controls.
    /// Used to control the layout direction and arrangement of child elements within the toolbar.
    /// </summary>
    /// <remarks>
    /// This enumeration provides a simple way to specify toolbar layout without needing to work directly
    /// with CSS flex direction properties. The toolbar automatically applies appropriate styling based on the selected orientation.
    /// </remarks>
    public enum ToolbarOrientation
    {
        /// <summary>
        /// Arranges toolbar elements in a horizontal row layout (left to right).
        /// This is the default orientation and uses FlexDirection.Row for styling.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Arranges toolbar elements in a vertical column layout (top to bottom).
        /// Uses FlexDirection.Column for styling to stack elements vertically.
        /// </summary>
        Vertical
    }

}