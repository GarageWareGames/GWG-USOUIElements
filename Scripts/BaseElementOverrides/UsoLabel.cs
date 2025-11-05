using System;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoLabel : Label, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-label";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "text";

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

        public UsoLabel() : base()
        {
            InitElement(null, LabelType.Default);
        }

        public UsoLabel(string fieldLabelText) : base(fieldLabelText)
        {
            InitElement(null, LabelType.Default);
        }


        public UsoLabel(string fieldLabelText, LabelType fieldLabelType) : base(fieldLabelText)
        {
            InitElement(null, fieldLabelType);
        }

        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
        }

        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, out string newFieldName) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
            newFieldName = name;
        }

        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, string bindingPath, BindingMode fieldBindingMode = BindingMode.ToTarget) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType, bindingPath, fieldBindingMode);
        }

        public UsoLabel(string fieldName, string fieldLabelText, LabelType fieldLabelType, string fieldBindingPath, BindingMode fieldBindingMode, out string newFieldName) : base(fieldLabelText)
        {
            InitElement(fieldName, fieldLabelType);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
            newFieldName = name;
        }

        private void InitElement(string fieldName, LabelType fieldLabelType, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            InitElement(fieldName, fieldLabelType);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        private void InitElement(string fieldName, LabelType fieldLabelType)
        {
            InitElement(fieldName);
            switch (fieldLabelType)
            {
                case LabelType.Header:
                    AddToClassList("uso-label--header");
                    break;
                case LabelType.SubHeader:
                    AddToClassList("uso-label--subheader");
                    break;
                case LabelType.Title:
                    AddToClassList("uso-label--title");
                    break;
                case LabelType.Subtitle:
                    AddToClassList("uso-label--subtitle");
                    break;
                case LabelType.Description:
                    AddToClassList("uso-label--description");
                    break;
                case LabelType.Default:
                default:
                    break;
            }
        }

    }
}