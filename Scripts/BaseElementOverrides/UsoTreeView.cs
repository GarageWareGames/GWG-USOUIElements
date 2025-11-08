using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoTreeView : TreeView, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-tree-view";
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
//            text = fieldLabelText;
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
        public UsoTreeView() : base()
        {
            InitElement();
        }

        public UsoTreeView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoTreeView(string fieldName, out UsoTreeView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoTreeView(Action btnAction) : base()
        {
            InitElement();
        }

        public UsoTreeView(string fieldName, Action btnAction) : base()
        {
            InitElement(fieldName);
        }

        public UsoTreeView(string fieldName, Action btnAction, out UsoTreeView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoTreeView(string fieldName, string fieldLabelText) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoTreeView(string fieldName, string fieldLabelText, out UsoTreeView newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        public UsoTreeView(string fieldName, string fieldLabelText, Action btnAction) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        public UsoTreeView(string fieldName, string fieldLabelText, Action btnAction, out UsoTreeView newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }
#endregion
    }
}