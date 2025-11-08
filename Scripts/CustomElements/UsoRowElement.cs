using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoRowElement : VisualElement, IUsoUiElement
    {
        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-row-element";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "";
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
        private bool _fieldStatusEnabled = false;

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

        public UsoRowElement() : base()
        {
            InitElement();
        }

        public UsoRowElement(string fieldName) : base()
        {
            InitElement(fieldName);
            style.width = new StyleLength(Length.Percent(100));
        }

        public UsoRowElement(string fieldName, out UsoRowElement newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public new void InitElement(string fieldName = "")
        {
            name = fieldName;
            style.flexDirection = FlexDirection.Row;
            style.width = new StyleLength(Length.Percent(100));
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }
    }
}