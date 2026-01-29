using GWG.UsoUIElements.Utilities;
using UnityEngine.UIElements;
namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom two-pane split view control that extends Unity's TwoPaneSplitView with USO UI framework functionality.
    /// Provides enhanced styling, field validation, automatic pane creation, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, custom styling through CSS classes, and automatically creates left and right panes
    /// using UsoVisualElement instances. The control is designed for layouts that require resizable split-pane functionality
    /// with enhanced USO framework integration. The control automatically initializes with two UsoVisualElement panes
    /// that can be accessed through the LeftPane and RightPane properties for content management.
    /// </remarks>
    [UxmlElement]
    public partial class UsoTwoPaneSplitView : TwoPaneSplitView, IUsoUiElement
    {
        /// <summary>
        /// CSS class name applied to all UsoTwoPaneSplitView instances for styling purposes.
        /// Note: Currently uses "uso-object-field" class, which may be intended for a different control type.
        /// </summary>
        private const string ElementStylesheet = "uso-object-field";

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

        /// <summary>
        /// Gets or sets the left pane visual element of the split view.
        /// This pane is automatically created during initialization and can be used to add content to the left side.
        /// </summary>
        /// <value>The UsoVisualElement that serves as the left pane container.</value>
        public VisualElement LeftPane { get; set; }

        /// <summary>
        /// Gets or sets the right pane visual element of the split view.
        /// This pane is automatically created during initialization and can be used to add content to the right side.
        /// </summary>
        /// <value>The UsoVisualElement that serves as the right pane container.</value>
        public VisualElement RightPane { get; set; }

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
                if (_fieldStatusEnabled)
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
        /// Initializes the USO UI element with the specified field name and automatically creates the left and right panes.
        /// This method sets up the basic USO framework integration for the control and populates it with UsoVisualElement panes.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        /// <remarks>
        /// The method automatically creates two UsoVisualElement instances and assigns them as LeftPane and RightPane
        /// for convenient content management. The panes are added to the split view hierarchy during initialization.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            Add(LeftPane = new UsoVisualElement());
            Add(RightPane = new UsoVisualElement());

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
        /// Initializes a new Instance of the UsoTwoPaneSplitView class with default settings.
        /// Creates a split view with USO framework integration and automatically initialized left and right panes.
        /// </summary>
        public UsoTwoPaneSplitView()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTwoPaneSplitView class with the specified field name.
        /// Creates a split view with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this split view element.</param>
        public UsoTwoPaneSplitView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTwoPaneSplitView class with field name and returns a reference.
        /// Creates a split view with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this split view element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created split view.</param>
        public UsoTwoPaneSplitView(string fieldName, out UsoTwoPaneSplitView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }
    }
}