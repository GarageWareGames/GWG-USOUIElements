using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoButton : Button, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-button";
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

        private void InitElement(string fieldName, string fieldLabelText)
        {
            InitElement(fieldName);
            text = fieldLabelText;
        }

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

#region CONSTRUCTORS
        // Standard Constructors
        public UsoButton() : base()
        {
            InitElement();
        }

        public UsoButton(Background iconImage, Action clickEvent = null) : base(iconImage, clickEvent)
        {
            InitElement();
        }

        public UsoButton(Action btnAction) : base(btnAction)
        {
            InitElement();
        }

        // Custom Constructors
        public UsoButton(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoButton(string fieldName, out UsoButton newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoButton(string fieldName, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName);
        }

        public UsoButton(string fieldName, Action btnAction, out UsoButton newField) : base(btnAction)
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoButton(string fieldName, string fieldLabelText) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoButton(string fieldName, string fieldLabelText, Background iconImage) : base(iconImage)
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoButton(string fieldName, string fieldLabelText, out UsoButton newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        public UsoButton(string fieldName, string fieldLabelText, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoButton(string fieldName, string fieldLabelText, Action btnAction, out UsoButton newField) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        public UsoButton(string fieldName, string fieldLabelText, Background iconImage, Action btnAction) : base(iconImage,btnAction)
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoButton(string fieldName, string fieldLabelText, Background iconImage, Action btnAction, out UsoButton newFieldName) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
            newFieldName = this;
        }
#endregion


    }
}