using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoImage : Image, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-image";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "image";
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

        [UxmlAttribute]
        public Texture Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
            }
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

        public UsoImage() : base()
        {
            InitElement();
        }

        public UsoImage(Texture2D viewImage) : base()
        {
            InitElement();
            image = viewImage;
        }

        public UsoImage(string fieldName, Texture2D viewImage) : base()
        {
            InitElement(fieldName);
            image = viewImage;
        }

        public UsoImage(string fieldName, Texture2D viewImage, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            image = viewImage;
        }

        public UsoImage(string fieldName, Texture2D viewImage, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            image = viewImage;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        public UsoImage(string fieldName, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        public UsoImage(string fieldName, string bindingPath, BindingMode bindingMode, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        public UsoImage(string fieldName, Texture2D viewImage, string bindingPath, BindingMode bindingMode, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            image = viewImage;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }



    }
}