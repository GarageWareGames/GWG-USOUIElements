using System;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.USOUiElements.Editor
{
    [UxmlElement]
    public partial class UsoObjectField : BindableElement, INotifyValueChanged<Object>, IUsoUiElement
    {
        private ObjectField _objectField;
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
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
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
        private Object _value;

        public void InitElement(string fieldName = null)
        {
            _objectField = new ObjectField();
            _objectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(_objectField);
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;

        }

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
        public UsoObjectField() : base()
        {
            InitElement();
        }

        public UsoObjectField(string labelText) : base()
        {
            InitElement();
            _objectField.label = labelText;
        }

        public UsoObjectField(string fieldName, string labelText) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
        }

        public UsoObjectField(string fieldName, string labelText, out string newFieldName) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
            newFieldName = name;
        }

        public UsoObjectField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            _objectField.label = labelText;
            ApplyBinding("value", bindingPath, bindingMode);
        }

        public UsoObjectField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, out string newFieldName) : base()
        {
            InitElement(fieldName);
            newFieldName = name;
            _objectField.label = labelText;
            ApplyBinding("value", bindingPath, bindingMode);
        }
        public void SetValueWithoutNotify(Object newValue)
        {
            _objectField.SetValueWithoutNotify(newValue);
        }
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
        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }
        ~UsoObjectField()
        {
            _objectField.UnregisterValueChangedCallback(OnObjectFieldValueChanged);
        }
    }
}
