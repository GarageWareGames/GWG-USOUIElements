using System;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoFoldout : Foldout, IUsoUiElement
    {
        private const string ElementHeaderStylesheet = "uso-foldout-header";

        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-foldout";
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
            Header = this.Q<Toggle>();
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

        public Toggle Header;
        private string _elementHeaderStylesheet;

        public UsoFoldout() : base()
        {
            InitElement();
        }

        public UsoFoldout(string headerText) : base()
        {
            InitElement();
            text = headerText;
        }

        public UsoFoldout(string fieldName, bool state) : base()
        {
            InitElement(fieldName);
            value = state;
        }

        public UsoFoldout(string fieldName, bool state, out string newFieldName) : base()
        {
            InitElement(fieldName);
            value = state;
            newFieldName = name;
        }

        public UsoFoldout(string fieldName, string headerText) : base()
        {
            InitElement(fieldName);
            text = headerText;
        }

        public UsoFoldout(string fieldName, string headerText, out string newFieldName) : base()
        {
            InitElement(fieldName);
            text = headerText;
            newFieldName = name;
        }

        public UsoFoldout(string fieldName, string headerText, bool state) : base()
        {
            InitElement(fieldName);
            value = state;
            text = headerText;
        }

        public UsoFoldout(string fieldName, string headerText, bool state, out string newFieldName) : base()
        {
            InitElement(fieldName);
            value = state;
            text = headerText;
            newFieldName = name;
        }

        ~UsoFoldout()
        {

        }


    }
}