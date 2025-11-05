using System;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityEditor.UIElements;

namespace GWG.USOUiElements.Editor
{
    [UxmlElement]
    public partial class UsoTexturePreviewElement : BindableElement, INotifyValueChanged<Object>, IUsoUiElement
    {
        public static readonly string USSStylesheetPath = "texture_preview_element";
        public static readonly string USSClassName = "texture-preview-element";

        private readonly Image _previewImage;
        private ObjectField _objectField;
        private Texture2D _texture;
        private readonly Label _textureName;
        private readonly Label _textureSize;
        private readonly Label _textureFileSize;
        private Label _textureTags;
        private readonly Label _selectItemLabel;
        private readonly VisualElement _detailsRow;


        private const string ElementStylesheet = "uso-object-field";
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
        private bool _fieldStatusEnabled = true;

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

        private Object _value;


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

        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

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

        ~UsoTexturePreviewElement()
        {
            _objectField.UnregisterValueChangedCallback(OnObjectFieldValueChanged);
        }

    }
}