using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoToggle : Toggle, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-toggle";
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
        public UsoToggle() : base()
        {
            InitElement();
        }

        public UsoToggle(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoToggle(string fieldName, string fieldLabelText) : base(fieldLabelText)
        {
            InitElement(fieldName);
        }

        public UsoToggle(string fieldName, string fieldLabelText, out UsoToggle newField) : base(fieldLabelText)
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoToggle(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabelText)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoToggle(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode, out UsoToggle newField) : base(fieldLabelText)
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }


    }
}