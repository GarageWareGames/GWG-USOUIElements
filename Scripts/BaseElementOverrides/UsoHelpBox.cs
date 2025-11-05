using System;
using GWG.USOUiElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoHelpBox : HelpBox, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-help-box";
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
        public UsoHelpBox(string message, HelpBoxMessageType helpType = HelpBoxMessageType.None) : base(message, helpType)
        {
            InitElement();
        }

        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            style.flexShrink = 0;
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
            if (fieldStatus == FieldStatusTypes.Error)
            {
                messageType = HelpBoxMessageType.Error;
                return;
            }

            if (fieldStatus == FieldStatusTypes.Warning)
            {
                messageType = HelpBoxMessageType.Warning;
                return;
            }

            if (fieldStatus == FieldStatusTypes.Info)
            {
                messageType = HelpBoxMessageType.Info;
                return;
            }
            messageType = HelpBoxMessageType.None;
        }

        public UsoHelpBox()
        {
            InitElement();
        }
    }
}