using System;
using System.Collections.Generic;
using GWG.USOUiElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoToolbarMenu : ToolbarMenu, IUsoUiElement
    {

        private const string ElementStylesheet = "uso-toolbar-menu";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "value";
        private bool _fieldStatusEnabled;
        private FieldStatusTypes _fieldStatus;

        /*[UxmlAttribute]
        public List<DropdownMenuItem> Actions
        {
            get
            {
                return menu.MenuItems();
            }
            set
            {
                menu.AppendAction(value);
            }
        }
        public List<DropdownMenuItem> _actions;*/

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
    }
}