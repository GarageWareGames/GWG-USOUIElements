using System;
using GWG.USOUiElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoToolbarButton : ToolbarButton, IUsoUiElement
    {

        private const string ElementStylesheet = "uso-toolbar-button";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "value";
        private bool _fieldStatusEnabled;
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

        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        public UsoToolbarButton()
        {
            InitElement();
        }

        public UsoToolbarButton(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoToolbarButton(string fieldName, out string newFieldName) : base()
        {
            InitElement(fieldName);
            newFieldName = name;
        }

        public UsoToolbarButton(string fieldName,string fieldText) : base()
        {
            InitElement(fieldName);
            base.text = fieldText;
        }

        public UsoToolbarButton(string fieldName,string fieldText, out string newFieldName) : base()
        {
            InitElement(fieldName);
            base.text = fieldText;
            newFieldName = name;
        }

        public UsoToolbarButton(Action btnAction) : base(btnAction)
        {
            InitElement();
        }

        public UsoToolbarButton(string fieldName, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName);
        }

        public UsoToolbarButton(string fieldName, Action btnAction, out string newFieldName) : base(btnAction)
        {
            InitElement(fieldName);
            newFieldName = name;
        }
    }
}