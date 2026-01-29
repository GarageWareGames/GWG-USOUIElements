using GWG.UsoUIElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom toolbar menu control that extends Unity's ToolbarMenu with USO UI framework functionality.
    /// Provides enhanced styling, field validation, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, custom styling through CSS classes, and dropdown menu functionality within toolbar contexts.
    /// The control is specifically designed for use within toolbars to provide dropdown menu capabilities with enhanced USO framework integration.
    /// This control includes commented code for potential future menu item management functionality.
    /// The control maintains the standard Unity ToolbarMenu behavior while adding USO-specific enhancements for consistent styling and validation.
    /// </remarks>
    [UxmlElement]
    public partial class UsoToolbarMenu : ToolbarMenu, IUsoUiElement
    {
        /// <summary>
        /// CSS class name applied to all UsoToolbarMenu instances for styling purposes.
        /// </summary>
        private const string ElementStylesheet = "uso-toolbar-menu";

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

        /*[UxmlAttribute]
        public List<DropdownMenuItem> Actions
        {
            get
            {
                return menu.MenuItems();
            }
            set
            {
                menu.AppendAction(value);
            }
        }
        public List<DropdownMenuItem> _actions;*/

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

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
        }
    }
}