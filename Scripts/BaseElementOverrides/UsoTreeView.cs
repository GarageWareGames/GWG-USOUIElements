using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom tree view control that extends Unity's TreeView with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for hierarchical data structures, and custom styling through CSS classes.
    /// The control is designed for displaying hierarchical data in an expandable tree structure with enhanced USO framework integration.
    /// It supports various constructor overloads for different initialization scenarios including action callbacks and label text configuration.
    /// The control includes commented code indicating potential future label text functionality integration.
    /// </remarks>
    [UxmlElement]
    public partial class UsoTreeView : TreeView, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoTreeView instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-tree-view";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'value' property for potential future data binding scenarios.
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
        /// Private initialization method that combines element setup with label text configuration.
        /// This overload provides setup for constructors that need both element initialization and label text.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="fieldLabelText">The label text for potential future labeling functionality.</param>
        /// <remarks>
        /// The label text parameter is currently not applied due to commented code, but the method structure
        /// is maintained for future enhancement possibilities.
        /// </remarks>
        private void InitElement(string fieldName, string fieldLabelText)
        {
            InitElement(fieldName);
//            text = fieldLabelText;
        }

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

#region CONSTRUCTORS
        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with default settings.
        /// Creates a tree view with USO framework integration enabled.
        /// </summary>
        public UsoTreeView() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with the specified field name.
        /// Creates a tree view with custom identification for binding and reference purposes.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        public UsoTreeView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name and returns a reference.
        /// Creates a tree view with custom identification and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created tree view.</param>
        public UsoTreeView(string fieldName, out UsoTreeView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with an action callback.
        /// Creates a tree view with predefined action behavior and USO framework integration.
        /// </summary>
        /// <param name="btnAction">The action to associate with tree view interactions or events.</param>
        public UsoTreeView(Action btnAction) : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name and action callback.
        /// Creates a tree view with custom identification, predefined action behavior, and USO framework integration.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="btnAction">The action to associate with tree view interactions or events.</param>
        public UsoTreeView(string fieldName, Action btnAction) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name, action callback, and returns a reference.
        /// Creates a tree view with custom identification, predefined action behavior, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="btnAction">The action to associate with tree view interactions or events.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created tree view.</param>
        public UsoTreeView(string fieldName, Action btnAction, out UsoTreeView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name and label text.
        /// Creates a tree view with custom identification and label text for potential future labeling functionality.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="fieldLabelText">The label text for potential future labeling functionality.</param>
        /// <remarks>
        /// The label text is currently not applied to the tree view due to commented implementation,
        /// but the parameter structure is maintained for future enhancement possibilities.
        /// </remarks>
        public UsoTreeView(string fieldName, string fieldLabelText) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name, label text, and returns a reference.
        /// Creates a tree view with custom identification, label text, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="fieldLabelText">The label text for potential future labeling functionality.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created tree view.</param>
        /// <remarks>
        /// The label text is currently not applied to the tree view due to commented implementation,
        /// but the parameter structure is maintained for future enhancement possibilities.
        /// </remarks>
        public UsoTreeView(string fieldName, string fieldLabelText, out UsoTreeView newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with field name, label text, and action callback.
        /// Creates a tree view with custom identification, label text, and predefined action behavior.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="fieldLabelText">The label text for potential future labeling functionality.</param>
        /// <param name="btnAction">The action to associate with tree view interactions or events.</param>
        /// <remarks>
        /// The label text is currently not applied to the tree view due to commented implementation,
        /// but the parameter structure is maintained for future enhancement possibilities.
        /// </remarks>
        public UsoTreeView(string fieldName, string fieldLabelText, Action btnAction) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoTreeView class with complete configuration and returns a reference.
        /// Creates a tree view with custom identification, label text, predefined action behavior, and immediate access to the created Instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this tree view element.</param>
        /// <param name="fieldLabelText">The label text for potential future labeling functionality.</param>
        /// <param name="btnAction">The action to associate with tree view interactions or events.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created tree view.</param>
        /// <remarks>
        /// The label text is currently not applied to the tree view due to commented implementation,
        /// but the parameter structure is maintained for future enhancement possibilities.
        /// </remarks>
        public UsoTreeView(string fieldName, string fieldLabelText, Action btnAction, out UsoTreeView newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }
#endregion
    }
}