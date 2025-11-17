
using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityEditor.UIElements;

namespace GWG.UsoUIElements.Editor
{
    /// <summary>
    /// A custom UI element that provides texture selection and preview functionality with detailed texture information display.
    /// This element combines an ObjectField for texture selection with an automatic preview image and comprehensive metadata display including texture properties and file information.
    /// </summary>
    /// <remarks>
    /// This element is specifically designed for editor use and integrates with Unity's asset system to display texture previews and information.
    /// It implements the IUsoUiElement interface for consistent field validation and binding support across the Uso UI system.
    /// The element displays texture name, dimensions, and calculated file size information alongside a 64x64 pixel preview image.
    /// Field validation status can be enabled/disabled and various status types are supported for form validation scenarios.
    /// The element automatically handles showing/hiding details based on whether a valid texture is selected.
    /// </remarks>
    [UxmlElement]
    public partial class UsoTexturePreviewElement : BindableElement, INotifyValueChanged<Object>, IUsoUiElement
    {
        /// <summary>
        /// The path to the USS stylesheet resource that styles this texture preview element.
        /// </summary>
        /// <remarks>
        /// This stylesheet should be placed in the TexturePreviewElement Resources folder and contains the visual styling for the texture preview element.
        /// </remarks>
        public static readonly string USSStylesheetPath = "texture_preview_element";

        /// <summary>
        /// The primary CSS class name applied to the texture preview element.
        /// </summary>
        /// <remarks>
        /// This class name is used for styling the main container of the texture preview element.
        /// </remarks>
        public static readonly string USSClassName = "texture-preview-element";

        /// <summary>
        /// The Image element that displays the texture preview.
        /// </summary>
        /// <remarks>
        /// This image shows a visual representation of the selected texture and is sized to 64x64 pixels with fixed dimensions.
        /// The image is automatically updated when a new texture is selected through the ObjectField.
        /// </remarks>
        private readonly Image _previewImage;

        /// <summary>
        /// The ObjectField that allows users to select Texture2D assets.
        /// </summary>
        /// <remarks>
        /// This field is configured to only accept Texture2D objects and triggers preview updates when its value changes.
        /// It provides the standard Unity asset selection interface including drag-and-drop support.
        /// </remarks>
        private ObjectField _objectField;

        /// <summary>
        /// The currently selected Texture2D asset.
        /// </summary>
        /// <remarks>
        /// This field stores the actual texture reference and is synchronized with the ObjectField selection.
        /// It is used to populate the preview image and metadata displays.
        /// </remarks>
        private Texture2D _texture;

        /// <summary>
        /// Label that displays the name of the selected texture.
        /// </summary>
        /// <remarks>
        /// Shows the texture asset's name with HTML bold formatting for emphasis in the details display.
        /// </remarks>
        private readonly Label _textureName;

        /// <summary>
        /// Label that displays the dimensions of the selected texture.
        /// </summary>
        /// <remarks>
        /// Shows the texture's width and height in pixels with HTML bold formatting for the label.
        /// Displays as "Viewable Size: WIDTHxHEIGHT" format.
        /// </remarks>
        private readonly Label _textureSize;

        /// <summary>
        /// Label that displays the calculated file size of the selected texture.
        /// </summary>
        /// <remarks>
        /// Shows an estimated file size calculation based on width × height × 4 bytes (assuming RGBA format).
        /// This provides an approximate memory footprint rather than actual disk file size.
        /// </remarks>
        private readonly Label _textureFileSize;

        /// <summary>
        /// Label that would display texture tags information.
        /// </summary>
        /// <remarks>
        /// This field is declared but not currently used in the implementation.
        /// Reserved for potential future functionality to display texture asset tags.
        /// </remarks>
        private Label _textureTags;

        /// <summary>
        /// Label displayed when no texture is selected, prompting the user to select one.
        /// </summary>
        /// <remarks>
        /// This label provides user guidance when the element is in an empty state.
        /// It is hidden when a valid texture is selected and shown when the selection is cleared.
        /// </remarks>
        private readonly Label _selectItemLabel;

        /// <summary>
        /// Container element that holds the preview image and texture details in a horizontal layout.
        /// </summary>
        /// <remarks>
        /// This element uses horizontal flex direction to arrange the preview image alongside the texture information.
        /// It is hidden when no texture is selected and becomes visible when a valid texture is chosen.
        /// </remarks>
        private readonly VisualElement _detailsRow;

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
        /// Gets or sets the current field status for validation display.
        /// This property controls the visual validation state of the element.
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
        /// Applies data binding to this element using the specified property name, path, and mode.
        /// This method enables two-way data binding between the element and a data source.
        /// </summary>
        /// <param name="fieldBindingProp">The name of the property to bind to (typically "value").</param>
        /// <param name="fieldBindingPath">The path to the property in the data source.</param>
        /// <param name="fieldBindingMode">The binding mode (OneWay, TwoWay, etc.).</param>
        /// <exception cref="Exception">Thrown when binding setup fails, with the original exception preserved.</exception>
        /// <remarks>
        /// This method implements the IUsoUiElement interface requirement for binding support.
        /// It creates a DataBinding with the specified parameters and applies it to the element.
        /// Any exceptions during binding setup are caught, logged to console, and re-thrown for proper error handling.
        /// </remarks>
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

        /// <summary>
        /// The internal storage for the selected texture object value.
        /// </summary>
        /// <remarks>
        /// This field stores the currently selected Object (specifically Texture2D) and is used by the value property.
        /// </remarks>
        private Object _value;

        /// <summary>
        /// Initializes a new instance of the UsoTexturePreviewElement, setting up the complete UI structure and event handlers.
        /// Creates all child elements including the ObjectField, preview image, detail labels, and configures the layout and styling.
        /// </summary>
        /// <remarks>
        /// The constructor loads the associated stylesheet and creates a complex UI hierarchy including:
        /// - A prompt label for when no texture is selected
        /// - An ObjectField configured for Texture2D selection
        /// - A details row containing a preview image and information labels
        /// - Multiple labels for displaying texture name, size, and file size information
        ///
        /// The ObjectField is configured with a value change callback, and the preview image is sized to 64x64 pixels.
        /// The details row uses horizontal layout and is initially hidden until a texture is selected.
        /// The element starts with field validation enabled and applies appropriate CSS classes.
        /// </remarks>
        public UsoTexturePreviewElement()
        {
            styleSheets.Add(Resources.Load<StyleSheet>($"TexturePreviewElement/{USSStylesheetPath}"));
            AddToClassList(USSClassName);
            style.flexShrink = 0;


            _selectItemLabel = new Label("Select a Texture");
            _selectItemLabel.style.display = DisplayStyle.None;
            Add(_selectItemLabel);

            // Create an ObjectField, set its object type, and register a callback when its value changes.
            _objectField = new ObjectField();
            _objectField.objectType = typeof(Texture2D);
            _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(_objectField);

            _detailsRow = new VisualElement();
            _detailsRow.style.flexDirection = FlexDirection.Row;
            _detailsRow.style.alignItems = Align.FlexStart;
            _detailsRow.style.display = DisplayStyle.None;
            Add(_detailsRow);

            // Create a preview image.
            _previewImage = new Image();
            _previewImage.AddToClassList("preview-image");
            _previewImage.style.flexShrink = 0;
            _previewImage.style.flexGrow = 0;
            _previewImage.style.width = 64;
            _previewImage.style.height = 64;
            _detailsRow.Add(_previewImage);

            VisualElement detailsColumn = new VisualElement();
            detailsColumn.style.flexShrink = 0;
            detailsColumn.AddToClassList("details-text-column");
            _detailsRow.Add(detailsColumn);

            _textureName = new Label();
            detailsColumn.Add(_textureName);
            Label textureType1 = new Label();
            detailsColumn.Add(textureType1);
            Label textureUniqueId1 = new Label();
            detailsColumn.Add(textureUniqueId1);
            _textureSize = new Label();
            detailsColumn.Add(_textureSize);
            _textureFileSize = new Label();
            detailsColumn.Add(_textureFileSize);

        }

        /// <summary>
        /// Handles value changes from the ObjectField and updates the element's value accordingly.
        /// This method is called when the user selects a different texture in the ObjectField.
        /// </summary>
        /// <param name="evt">The change event containing the old and new texture values from the ObjectField.</param>
        /// <remarks>
        /// This callback ensures that changes in the ObjectField are properly propagated to the element's value property,
        /// which will trigger the preview update and notification events. It acts as a bridge between the ObjectField's
        /// change events and the element's INotifyValueChanged implementation.
        /// </remarks>
        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        /// <summary>
        /// Sets the value of the element without triggering change notifications or events.
        /// Updates the internal texture reference, preview image, detail labels, and ObjectField display.
        /// </summary>
        /// <param name="newValue">The new Texture2D value to set, or null to clear the selection.</param>
        /// <exception cref="ArgumentException">Thrown when the newValue is not null and not a Texture2D.</exception>
        /// <remarks>
        /// This method is used internally to update the element's state without triggering change events,
        /// which is important for avoiding infinite loops during value synchronization.
        /// The method validates that the provided value is either null or a Texture2D before proceeding.
        ///
        /// When a valid texture is set, the method:
        /// - Updates the preview image with the texture
        /// - Populates the detail labels with texture information
        /// - Shows the details row and hides the selection prompt
        /// - Calculates an estimated file size based on dimensions and RGBA format
        ///
        /// When null is set, it hides the details and shows the selection prompt label.
        /// </remarks>
        public void SetValueWithoutNotify(Object newValue)
        {
            if (newValue == null || newValue is Texture2D)
            {
                // Update the preview Image and update the ObjectField.
                _texture = newValue as Texture2D;
                _previewImage.image = _texture;
                if(_previewImage.image != null)
                {
                    _selectItemLabel.style.display = DisplayStyle.None;
                    _detailsRow.style.display = DisplayStyle.Flex;
                    _textureName.text = "<b>Name:</b> " +_previewImage.image.name;
                    _textureSize.text = "<b>Viewable Size:</b> " + _previewImage.image.width + "x" + _previewImage.image.height;
                    _textureFileSize.text = "<b>File Size:</b> " + (_previewImage.image.width * _previewImage.image.height * 4) + " bytes";
                }
                else
                {
                    _detailsRow.style.display = DisplayStyle.None;
                    _selectItemLabel.style.display = DisplayStyle.Flex;
                }

                // Notice that this line calls the ObjectField's SetValueWithoutNotify() method instead of just setting
                // objectField.value. This is very important; you don't want objectField to send a ChangeEvent.
                _objectField.SetValueWithoutNotify(_texture);
            }
            else throw new ArgumentException($"Expected object of type {typeof(Texture2D)}");
        }

        /// <summary>
        /// Gets or sets the current Texture2D value of the element.
        /// Setting this property triggers change events and updates the preview display.
        /// </summary>
        /// <value>The currently selected Texture2D asset, or null if no texture is selected.</value>
        /// <remarks>
        /// The setter implements the INotifyValueChanged pattern by creating and sending a ChangeEvent
        /// when the value actually changes. It uses SetValueWithoutNotify internally to update the display
        /// and then sends the appropriate change notification to any listeners.
        ///
        /// The getter returns the currently stored texture reference. The property provides type-safe
        /// access to the texture while maintaining compatibility with the generic Object interface.
        /// </remarks>
        public Object value
        {
            get => _texture;
            // The setter is called when the user changes the value of the ObjectField, which calls
            // OnObjectFieldValueChanged(), which calls this.
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
        /// Finalizer that ensures proper cleanup of event callbacks when the element is destroyed.
        /// Unregisters the value change callback from the ObjectField to prevent memory leaks.
        /// </summary>
        /// <remarks>
        /// This finalizer is important for preventing memory leaks by ensuring that event callbacks
        /// are properly unregistered when the element is garbage collected. It specifically removes
        /// the OnObjectFieldValueChanged callback that was registered during construction.
        /// </remarks>
        ~UsoTexturePreviewElement()
        {
            _objectField.UnregisterValueChangedCallback(OnObjectFieldValueChanged);
        }

    }
}