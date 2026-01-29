
using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.UsoUIElements.Editor
{
    /// <summary>
    /// A custom UI element that wraps Unity's ObjectField with enhanced functionality for the Uso UI system.
    /// This element provides field validation, data binding support, and standardized behavior consistent with other Uso UI elements.
    /// </summary>
    /// <remarks>
    /// The UsoObjectField extends Unity's standard ObjectField with additional features required by the Uso UI framework:
    /// - Integrated field validation with visual status indicators
    /// - Data binding capabilities for automatic synchronization with data sources
    /// - Consistent API with other Uso UI elements through the IUsoUiElement interface
    /// - Multiple constructor overloads for different initialization scenarios
    /// - Support for both scene and asset object selection
    ///
    /// This element is designed for use in Unity Editor contexts where object reference selection is required
    /// with enhanced validation and binding capabilities beyond the standard ObjectField.
    /// The element maintains full compatibility with Unity's serialization and Inspector systems.
    /// </remarks>
    [UxmlElement]
    public partial class UsoObjectField : BindableElement, INotifyValueChanged<Object>, IUsoUiElement
    {
        /// <summary>
        /// The internal ObjectField that handles the actual object selection functionality.
        /// </summary>
        /// <remarks>
        /// This field wraps Unity's standard ObjectField and provides the core object selection behavior.
        /// All ObjectField-specific functionality is delegated to this internal Instance.
        /// </remarks>
        private ObjectField _objectField;

        /// <summary>
        /// Gets or sets the type of objects that can be assigned to this ObjectField.
        /// This property determines what types of Unity objects are valid for selection.
        /// </summary>
        /// <value>
        /// A Type that represents the allowed object type. Must be derived from UnityEngine.Object.
        /// </value>
        /// <remarks>
        /// This property is exposed to UXML through the UxmlAttribute, allowing declarative specification
        /// of the allowed object type in UI layout files. The type restricts what objects can be assigned
        /// through drag-and-drop or the object picker dialog. Common values include typeof(GameObject),
        /// typeof(Texture2D), typeof(Material), etc.
        /// </remarks>
        [UxmlAttribute]
        public Type objectType
        {
            get
            {
                return _objectField.objectType;
            }
            set
            {
                _objectField.objectType = value;
            }
        }

        /// <summary>
        /// Gets or sets whether scene objects can be assigned to this ObjectField.
        /// When false, only asset objects from the project can be assigned.
        /// </summary>
        /// <value>
        /// True if scene objects are allowed; otherwise, false to restrict to project assets only.
        /// </value>
        /// <remarks>
        /// This property is exposed to UXML through the UxmlAttribute for declarative configuration.
        /// When set to false, the ObjectField will only accept objects that are stored as assets in the project,
        /// preventing assignment of objects that exist only in the current scene. This is useful for creating
        /// references that remain valid across different scenes and can be safely serialized in assets.
        /// </remarks>
        [UxmlAttribute]
        public bool allowSceneObjects
        {
            get
            {
                return _objectField.allowSceneObjects;
            }
            set
            {
                _objectField.allowSceneObjects = value;
            }
        }

#region UsoUiElement Implementation
        /// <summary>
        /// The CSS class name used for general Uso UI element styling.
        /// </summary>
        /// <remarks>
        /// This constant defines the base stylesheet class for Uso object field elements.
        /// </remarks>
        private const string ElementStylesheet = "uso-object-field";

        /// <summary>
        /// The CSS class name applied when field validation is enabled.
        /// </summary>
        /// <remarks>
        /// This class is conditionally applied based on the FieldStatusEnabled property to enable validation styling.
        /// </remarks>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// The default property name used for data binding operations.
        /// </summary>
        /// <remarks>
        /// This constant defines the default binding property name when no specific property is specified for binding.
        /// </remarks>
        private const string DefaultBindProp = "value";

        /// <summary>
        /// Gets or sets the current field validation status for visual feedback.
        /// This property controls the validation state styling of the element.
        /// </summary>
        /// <value>
        /// A FieldStatusTypes value indicating the current validation status of the field.
        /// </value>
        /// <remarks>
        /// Setting this property automatically updates the visual styling through the UsoUiHelper.SetFieldStatus method.
        /// The status affects how the element appears to indicate validation states like error, warning, or success.
        /// This property is exposed to UXML through the UxmlAttribute for declarative configuration.
        /// </remarks>
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
        /// The internal storage for the field status value.
        /// </summary>
        /// <remarks>
        /// This field stores the actual validation status and is used by the FieldStatus property.
        /// </remarks>
        private FieldStatusTypes _fieldStatus;

        /// <summary>
        /// Gets or sets whether field validation status display is enabled for this element.
        /// When enabled, the element can show validation states through CSS class changes.
        /// </summary>
        /// <value>
        /// True if field status validation is enabled; otherwise, false. Default is true.
        /// </value>
        /// <remarks>
        /// Setting this property to true adds the ElementValidationClass to enable validation styling.
        /// Setting it to false removes the validation class, disabling status-based visual feedback.
        /// This property is exposed to UXML through the UxmlAttribute for declarative configuration.
        /// </remarks>
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
        /// The internal storage for the field status enabled flag.
        /// </summary>
        /// <remarks>
        /// This field stores whether validation status display is enabled and defaults to true.
        /// </remarks>
        private bool _fieldStatusEnabled = true;

        /// <summary>
        /// The internal storage for the selected object value.
        /// </summary>
        /// <remarks>
        /// This field stores the currently selected Object and is used by the value property.
        /// </remarks>
        private Object _value;

        /// <summary>
        /// Initializes the internal ObjectField and configures the element with optional field name and styling.
        /// This method sets up the core functionality and event handling for the ObjectField wrapper.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to this element for identification purposes.</param>
        /// <remarks>
        /// This method creates the internal ObjectField, registers the value change callback, adds it to the element hierarchy,
        /// applies the base CSS class for styling, and ensures field status validation is properly configured.
        /// The method is called by all constructors to ensure consistent initialization regardless of construction parameters.
        /// </remarks>
        public void InitElement(string fieldName = null)
        {
            _objectField = new ObjectField();
            _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(_objectField);
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;

        }

        /// <summary>
        /// Applies data binding to the internal ObjectField using the specified property name, path, and mode.
        /// This method enables two-way data binding between the ObjectField and a data source.
        /// </summary>
        /// <param name="fieldBindingProp">The name of the property to bind to (typically "value").</param>
        /// <param name="fieldBindingPath">The path to the property in the data source.</param>
        /// <param name="fieldBindingMode">The binding mode (OneWay, TwoWay, etc.).</param>
        /// <exception cref="Exception">Thrown when binding setup fails, with the original exception preserved.</exception>
        /// <remarks>
        /// This method implements the IUsoUiElement interface requirement for binding support.
        /// It applies the binding to the internal ObjectField rather than the wrapper element to ensure
        /// the binding works with the actual input control. Any exceptions during binding setup are
        /// caught, logged to console, and re-thrown for proper error handling.
        /// </remarks>
        public void ApplyBinding(string fieldBindingProp, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            try
            {
                _objectField.SetBinding(fieldBindingProp, new DataBinding()
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
        /// Sets the field validation status to the specified value.
        /// This method provides external access to update the validation state of the element.
        /// </summary>
        /// <param name="fieldStatus">The new field status to apply.</param>
        /// <remarks>
        /// This method implements the IUsoUiElement interface requirement for status management.
        /// It updates the FieldStatus property, which automatically triggers visual styling changes.
        /// </remarks>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        /// <summary>
        /// Shows or hides the field validation status display based on the specified flag.
        /// This method controls whether validation visual feedback is active for the element.
        /// </summary>
        /// <param name="status">True to show field status validation; false to hide it.</param>
        /// <remarks>
        /// This method implements the IUsoUiElement interface requirement for status visibility control.
        /// It updates the FieldStatusEnabled property, which automatically manages the validation CSS class.
        /// </remarks>
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        /// <summary>
        /// Retrieves the first ancestor UsoLineItem element in the visual hierarchy.
        /// This method enables integration with parent line item containers for coordinated behavior.
        /// </summary>
        /// <returns>The first UsoLineItem ancestor found, or null if none exists.</returns>
        /// <remarks>
        /// This method implements the IUsoUiElement interface requirement for parent line item access.
        /// It searches up the visual tree to find the containing line item, enabling coordinated validation and behavior.
        /// </remarks>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
#endregion

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with default configuration.
        /// Creates an ObjectField with no initial label or binding configuration.
        /// </summary>
        /// <remarks>
        /// This parameterless constructor creates the most basic ObjectField configuration.
        /// Additional properties like objectType, label, and bindings must be set after construction.
        /// This constructor is suitable for programmatic creation where configuration will be applied separately.
        /// </remarks>
        public UsoObjectField() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with a specified label text.
        /// Creates an ObjectField with the provided label displayed to the user.
        /// </summary>
        /// <param name="labelText">The text to display as the field label.</param>
        /// <remarks>
        /// This constructor is convenient for creating labeled ObjectFields in programmatic UI construction.
        /// The label helps users understand what type of object should be assigned to the field.
        /// </remarks>
        public UsoObjectField(string labelText) : base()
        {
            InitElement();
            _objectField.label = labelText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with a field name and label text.
        /// Creates an ObjectField with both an element name for identification and a user-visible label.
        /// </summary>
        /// <param name="fieldName">The name to assign to this element for identification purposes.</param>
        /// <param name="labelText">The text to display as the field label.</param>
        /// <remarks>
        /// This constructor is useful when you need both programmatic identification (through the name)
        /// and user-friendly labeling. The field name can be used for element queries and identification
        /// while the label text provides clear user guidance.
        /// </remarks>
        public UsoObjectField(string fieldName, string labelText) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with a field name and label text, returning the assigned name.
        /// Creates an ObjectField and provides the actual assigned field name through an out parameter.
        /// </summary>
        /// <param name="fieldName">The desired name to assign to this element for identification purposes.</param>
        /// <param name="labelText">The text to display as the field label.</param>
        /// <param name="newFieldName">Returns the actual field name that was assigned to the element.</param>
        /// <remarks>
        /// This constructor variant allows the caller to verify the actual field name that was assigned,
        /// which is useful in scenarios where name conflicts might occur or where the assigned name needs
        /// to be stored for later reference. The out parameter provides the confirmed element name.
        /// </remarks>
        public UsoObjectField(string fieldName, string labelText, out UsoObjectField newFieldName) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
            newFieldName = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with field name, label, and data binding configuration.
        /// Creates an ObjectField with full configuration including automatic data synchronization.
        /// </summary>
        /// <param name="fieldName">The name to assign to this element for identification purposes.</param>
        /// <param name="labelText">The text to display as the field label.</param>
        /// <param name="bindingPath">The path to the property in the data source for binding.</param>
        /// <param name="bindingMode">The binding mode controlling data synchronization behavior. Default is ToTarget.</param>
        /// <remarks>
        /// This constructor provides complete ObjectField setup in a single call, including data binding.
        /// It's ideal for creating ObjectFields that need to automatically synchronize with data sources
        /// without requiring separate binding configuration. The default binding mode of ToTarget provides
        /// one-way data flow from the data source to the UI element.
        /// </remarks>
        public UsoObjectField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
            ApplyBinding("value", bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoObjectField with full configuration including name verification and data binding.
        /// Creates an ObjectField with complete setup and returns the assigned field name for confirmation.
        /// </summary>
        /// <param name="fieldName">The desired name to assign to this element for identification purposes.</param>
        /// <param name="labelText">The text to display as the field label.</param>
        /// <param name="bindingPath">The path to the property in the data source for binding.</param>
        /// <param name="bindingMode">The binding mode controlling data synchronization behavior.</param>
        /// <param name="newFieldName">Returns the actual field name that was assigned to the element.</param>
        /// <remarks>
        /// This constructor provides the most comprehensive ObjectField initialization, including full configuration
        /// with data binding and field name confirmation. It's suitable for scenarios where you need complete
        /// control over the ObjectField setup and want to verify the assigned element name for later reference.
        /// </remarks>
        public UsoObjectField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, out UsoObjectField newFieldName) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
            ApplyBinding("value", bindingPath, bindingMode);
            newFieldName = this;
        }

        /// <summary>
        /// Sets the value of the ObjectField without triggering change notifications or events.
        /// Updates the internal ObjectField value directly without propagating change events.
        /// </summary>
        /// <param name="newValue">The new Object value to set, or null to clear the selection.</param>
        /// <remarks>
        /// This method is used internally to update the ObjectField's state without triggering change events,
        /// which is important for avoiding infinite loops during value synchronization and data binding operations.
        /// It delegates directly to the internal ObjectField's SetValueWithoutNotify method.
        /// </remarks>
        public void SetValueWithoutNotify(Object newValue)
        {
            _objectField.SetValueWithoutNotify(newValue);
        }

        /// <summary>
        /// Gets or sets the current Object value of the ObjectField.
        /// Setting this property triggers change events and updates the field display.
        /// </summary>
        /// <value>The currently selected Object, or null if no object is selected.</value>
        /// <remarks>
        /// The setter implements the INotifyValueChanged pattern by creating and sending a ChangeEvent
        /// when the value actually changes. It uses SetValueWithoutNotify internally to update the display
        /// and then sends the appropriate change notification to any listeners.
        ///
        /// The getter returns the currently stored object value. This property provides the main interface
        /// for getting and setting the ObjectField's value programmatically and through data binding.
        /// </remarks>
        public Object value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == this.value)
                    return;
                var previous = this.value;
                SetValueWithoutNotify(value);

                using (var evt = ChangeEvent<Object>.GetPooled(previous, value))
                {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }

        /// <summary>
        /// Handles value changes from the internal ObjectField and updates the wrapper element's value accordingly.
        /// This method is called when the user selects a different object through the ObjectField interface.
        /// </summary>
        /// <param name="evt">The change event containing the old and new object values from the internal ObjectField.</param>
        /// <remarks>
        /// This callback ensures that changes in the internal ObjectField are properly propagated to the wrapper element's
        /// value property, which will trigger the INotifyValueChanged implementation and send appropriate change events.
        /// It acts as a bridge between the internal ObjectField's change events and the wrapper's change notification system.
        /// </remarks>
        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        /// <summary>
        /// Finalizer that ensures proper cleanup of event callbacks when the element is destroyed.
        /// Unregisters the value change callback from the internal ObjectField to prevent memory leaks.
        /// </summary>
        /// <remarks>
        /// This finalizer is important for preventing memory leaks by ensuring that event callbacks
        /// are properly unregistered when the element is garbage collected. It specifically removes
        /// the OnObjectFieldValueChanged callback that was registered during initialization.
        /// </remarks>
        ~UsoObjectField()
        {
            _objectField.UnregisterValueChangedCallback(OnObjectFieldValueChanged);
        }
    }
}