using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoVector3IntField : Vector3IntField, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-vector-3-int-field";
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

        public UsoVector3IntField() : base()
        {
            InitElement();
        }

        public UsoVector3IntField(string fieldLabel) : base(fieldLabel)
        {
            InitElement();
        }

        public UsoVector3IntField(string fieldName, string fieldBindPath, BindingMode fieldBindMode) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
        }

        public UsoVector3IntField(string fieldName, string fieldBindPath, BindingMode fieldBindMode, out UsoVector3IntField newField) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
            newField = this;
        }

        public UsoVector3IntField(string fieldName, string fieldLabel, string fieldBindPath, BindingMode fieldBindMode) : base(fieldLabel)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
        }

        public UsoVector3IntField(string fieldName, string fieldLabel, string fieldBindPath, BindingMode fieldBindMode, out UsoVector3IntField newField) : base(fieldLabel)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindPath, fieldBindMode);
            newField = this;
        }




    }
}