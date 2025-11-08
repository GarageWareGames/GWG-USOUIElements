using System;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    public class UsoElementTemplate : Toggle // change to the type being extended
    {
        private string styleSheetName = "uso-element-template";
        private string defaultBindProp = "value";

        public UsoElementTemplate() : base()
        {
            InitElement();
        }

        public UsoElementTemplate(string fieldName) : base()
        {
            name = fieldName;
            InitElement();
        }

        public UsoElementTemplate(string fieldName, out string newFieldName) : base()
        {
            name = fieldName;
            newFieldName = name;
            InitElement();
        }

        public UsoElementTemplate(string fieldName, string fieldLabelText) : base(fieldLabelText)
        {
            name = fieldName;
            InitElement();
        }

        public UsoElementTemplate(string fieldName, string fieldLabelText, out string newFieldName) : base(fieldLabelText)
        {
            name = fieldName;
            newFieldName = name;
            InitElement();
        }

        public UsoElementTemplate(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode) : base(fieldLabelText)
        {
            name = fieldName;
            InitElement();
            ApplyBinding(defaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoElementTemplate(string fieldName, string fieldLabelText, string fieldBindingPath, BindingMode fieldBindingMode, out string newFieldName) : base(fieldLabelText)
        {
            name = fieldName;
            newFieldName = name;
            InitElement();
            ApplyBinding(defaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        private void InitElement()
        {
            AddToClassList(styleSheetName);
            labelElement?.AddToClassList("uso-field-label");
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
    }
}
