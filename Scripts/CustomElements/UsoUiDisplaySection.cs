using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoUiDisplaySection : VisualElement, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementStylesheet = "uso-display-section";
        private const string ElementClass = "uso-display-section";
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

        public void InitElement(string fieldName = null)
        {
            style.flexGrow = 1;
            AddToClassList(ElementStylesheet);
            name = fieldName;

            ThemeStyleSheet usoTheme = Resources.Load<ThemeStyleSheet>("UsoUiElements/UsoUiElementsTheme");
            if (usoTheme != null)
            {
                styleSheets.Add(usoTheme);
            }
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

#endregion

        public UsoUiDisplaySection() : base()
        {
            InitElement();
        }

        public UsoUiDisplaySection(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoUiDisplaySection(string fieldName, out UsoUiDisplaySection newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoUiDisplaySection(string fieldName, Object fieldDatasource) : base()
        {
            InitElement(fieldName);
            dataSource = fieldDatasource;
        }

        public UsoUiDisplaySection(string fieldName, Object fieldDatasource, out UsoUiDisplaySection newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            dataSource = fieldDatasource;
        }
    }
}