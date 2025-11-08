using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEditor;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    /// <summary>
    /// A custom text field component extending the functionality of UnityEngine.UIElements.TextField
    /// </summary>
    public partial class UsoTextField : TextField, IUsoUiElement
    {


#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-text-field";
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
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion


        /// <summary>
        /// A custom text field component extending functionality from UnityEngine.UIElements.TextField.
        /// Provides customized initialization with additional properties like decoration types and data binding.
        /// </summary>
        /// <param name="fieldName">Name for the new field</param>
        /// <param name="labelText">Label to display for the new field</param>
        /// <param name="newFieldName">(Optional) string returned with the new field name</param>
        public UsoTextField(string fieldName, string labelText, out UsoTextField newFieldName) : base(labelText)
        {
            InitElement(fieldName, out newFieldName);
        }

        /// <summary>
        /// A custom text field component inheriting and enhancing the functionality of UnityEngine.UIElements.TextField.
        /// Supports additional features such as decoration types and flexible initialization for various UI applications.
        /// </summary>
        /// <param name="fieldName">The name to assign to the text field, used for identification.</param>
        /// <param name="labelText">(Optional) label text to be displayed with the field.</param>
        public UsoTextField(string fieldName, string labelText = null) : base(labelText)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// A custom text field component that extends the functionality of UnityEngine.UIElements.TextField.
        /// Enables additional customization options such as decoration types, data binding, and dynamic initialization.
        /// </summary>
        /// <param name="fieldName">The unique identifier for the text field.</param>
        /// <param name="labelText">The display label associated with the text field.</param>
        /// <param name="bindingPath">The data source path for data binding.</param>
        /// <param name="bindingMode">The type of data binding mode to use (e.g., one-way or two-way).</param>
        /// <param name="newFieldName">(Optional) Outputs the generated name for the new field.</param>
        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, out UsoTextField newFieldName) : base(labelText)
        {
            InitElement(fieldName, bindingPath, bindingMode, out newFieldName);
        }

        /// <summary>
        /// A custom text field component that extends the functionality of UnityEngine.UIElements.TextField.
        /// Enables additional customization options such as decoration types, data binding, and dynamic initialization.
        /// </summary>
        /// <param name="fieldName">The unique identifier for the text field.</param>
        /// <param name="bindingPath">The data source path for data binding.</param>
        /// <param name="bindingMode">The type of data binding mode to use (e.g., one-way or two-way).</param>
        /// <param name="newFieldName">(Optional) Outputs the generated name for the new field.</param>
        public UsoTextField(string fieldName, string bindingPath, BindingMode bindingMode, out UsoTextField newFieldName) : base()
        {
            InitElement(fieldName, bindingPath, bindingMode, out newFieldName);
        }

        /// <summary>
        /// A custom text field component that extends the functionality of UnityEngine.UIElements.TextField.
        /// Enables additional customization options such as decoration types, data binding, and dynamic initialization.
        /// </summary>
        /// <param name="fieldName">The unique identifier for the text field.</param>
        /// <param name="bindingPath">The data source path for data binding.</param>
        /// <param name="bindingMode">The type of data binding mode to use (e.g., one-way or two-way).</param>
        public UsoTextField(string fieldName, string bindingPath, BindingMode bindingMode) : base()
        {
            InitElement(fieldName, bindingPath, bindingMode);
        }

        /// <summary>
        /// A custom text field component extending from UnityEngine.UIElements.TextField.
        /// Allows initialization with parameters for data binding and field-specific properties.
        /// </summary>
        /// <param name="fieldName">The unique name of the text field.</param>
        /// <param name="labelText">The label displayed alongside the text field.</param>
        /// <param name="bindingPath">The path used for data binding the text field's value.</param>
        /// <param name="bindingMode">Specifies the binding mode for data synchronization.</param>
        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode) : base(labelText)
        {
            InitElement(fieldName, bindingPath, bindingMode);
        }

        public UsoTextField(string fieldName, string labelText, string bindingPath, BindingMode bindingMode, Object fieldDatasource) : base(labelText)
        {
            InitElement(fieldName);
            dataSource = fieldDatasource;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        private void InitElement(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode, out UsoTextField newFieldName)
        {
            InitElement(fieldName, out newFieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        private void InitElement(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode)
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        private void InitElement(string fieldName, out UsoTextField newField)
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoTextField()
        {
            InitElement();
        }
    }
}