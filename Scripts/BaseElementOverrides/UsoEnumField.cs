using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoEnumField : EnumField, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-enum-field";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "itemsSource";

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

        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

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

        public UsoEnumField() : base()
        {
            InitElement(null);
        }

        public UsoEnumField(Enum fieldType) : base(fieldType)
        {
            InitElement();
        }

        public UsoEnumField(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoEnumField(string fieldName, out UsoEnumField newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoEnumField(string fieldName, Enum fieldType) : base(fieldType)
        {
            InitElement(fieldName);
        }

        public UsoEnumField(string fieldName, Enum fieldType, out UsoEnumField newField) : base(fieldType)
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoEnumField(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoEnumField(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode, out UsoEnumField newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoEnumField(string fieldName,string fieldLabel, Enum fieldType, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabel, fieldType)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoEnumField(string fieldName,string fieldLabel, Enum fieldType) : base(fieldLabel, fieldType)
        {
            InitElement(fieldName);
        }

        public UsoEnumField(string fieldName,string fieldLabel, Enum fieldType, string fieldBindingPath, BindingMode fieldBindingMode, out UsoEnumField newField) : base(fieldLabel, fieldType)
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }


    }
}