using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// UsoButton is a custom button implementation, extending the Unity Button class,
    /// and is part of the GWG.UsoUIElements namespace. It provides additional functionality
    /// to the standard Button with the core Unity UI Elements framework
    /// </summary>
    /// <remarks>
    /// Additional functionality includes a standardized signature, support for field status management / validation response,
    /// and simplified data binding capabilities.
    /// </remarks>
    [UxmlElement]
    public partial class UsoButton : Button, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        /// <summary>
        /// Default USS Style Class name for the element
        /// </summary>
        private const string ElementClass = "uso-button";
        /// <summary>
        /// Default USS Field Validation Style Class name for the UsoUIElements Framework
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";
        /// <summary>
        /// Default Binding Property for this element, used when setting the data binding in the constructor. Can be overriden by using the ApplyBinding method which allows for more flexibility.
        /// </summary>
        private const string DefaultBindProp = "value";
        /// <summary>
        /// FieldStatus is the validation feedback for the field and muct be one of the values in the FieldStatusTypes enum.
        /// </summary>
        /// <remarks>
        /// This property is used to display the validation feedback for the field.
        /// The default value is FieldStatusTypes.None.
        /// The FieldStatusTypes enum provides the following values:
        /// <list type="bullet">
        /// <item>None</item>
        /// <item>Valid</item>
        /// <item>Invalid</item>
        /// </list>
        /// The <seealso cref="FieldStatusEnabled"/> must be true for the validation to be displayed.
        /// </remarks>
        /// <seealso cref="FieldStatusTypes"/>
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

        /// <summary>
        /// FieldStatusEnabled is a boolean value that determines whether the validation feedback is displayed.
        /// The default value is true.
        /// </summary>
        /// <remarks>
        /// The FieldStatusEnabled property is used to determine whether the validation feedback is displayed.
        /// The default value is true.
        /// </remarks>
        /// <seealso cref="FieldStatus"/>
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

        /// <summary>
        /// InitElement is a helper method that initializes the element in a standard way for all constructors using an optional field name and label text.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldLabelText"></param>
        private void InitElement(string fieldName, string fieldLabelText)
        {
            InitElement(fieldName);
            text = fieldLabelText;
        }

        /// <summary>
        /// InitElement is a helper method that initializes the element in a standard way for all constructors using an optional field name.
        /// </summary>
        /// <param name="fieldName"></param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        /// <summary>
        /// ApplyBinding is a helper method that allows for more flexibility in applying the data binding.
        /// </summary>
        /// <remarks>
        /// This method is used to apply a data binding to the element.<br/>
        /// Any of the element properties can be bound to a data source using this method.
        /// </remarks>
        /// <example>
        /// example uses a button but can apply to all Uso UI Elements
        /// <code>
        /// UsoButton btn = new UsoButton("btn");
        /// btn.ApplyBinding("text", "myData.myField", BindingMode.OneWay);
        /// </code>
        /// </example>
        /// The Example above will bind the 'text' property of the UsoButton to the 'myField' property path within the 'myData' object in readonly mode causing the value stores in the data source to be displayed in the button.
        /// <param name="fieldBindingProp">Property of the button you want to bind to as a string value</param>
        /// <param name="fieldBindingPath">Data path within the assigned datasource that you wish to bind the property to.</param>
        /// <param name="fieldBindingMode">Binding mode to use, can be read-only, write-only, or read/write</param>
        /// <seealso cref="BindingMode"/>
        /// <seealso cref="PropertyPath"/>
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

        /// <summary>
        /// SetFieldStatus is a helper method that sets the FieldStatus property.
        /// </summary>
        /// <param name="fieldStatus"></param>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        /// <summary>
        /// ShowFieldStatus is a helper method that sets the FieldStatusEnabled property.
        /// </summary>
        /// <param name="status"></param>
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }


        /// <summary>
        /// GetParentLineItem is a helper method that returns the parent UsoLineItem.
        /// </summary>
        /// <remarks>
        /// This method is used to get the parent UsoLineItem of the UsoButton.<br/>
        /// When fully implementing this framework, the hierarchy would be similar to:
        /// <code>
        /// UsoUiDisplaySection / UsoForm
        /// - UsoLineItem
        /// - - UsoRowElement / UsoColumnElement
        /// - - - UsoButton / UsoTextField / UsoDropDown / etc
        /// </code>
        /// </remarks>
        /// <returns></returns>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

        public UsoForm GetParentForm()
        {
            return GetFirstAncestorOfType<UsoForm>();
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

#region CONSTRUCTORS

        /// <summary>
        /// Default constructor.
        /// </summary>
        public UsoButton() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Creates a new element with the specified icon image and click event handler.
        /// </summary>
        /// <param name="iconImage">Image to apply to the button</param>
        /// <param name="clickEvent">Event handler when a click event happens</param>
        public UsoButton(Background iconImage, Action clickEvent = null) : base(iconImage, clickEvent)
        {
            InitElement();
        }

        /// <summary>
        /// Default UI Toolkit Constructor that only needs an event handler
        /// </summary>
        public UsoButton(Action btnAction) : base(btnAction)
        {
            InitElement();
        }

        /// <summary>
        /// Creates an element with an assigned name
        /// </summary>
        /// <param name="fieldName">name of the new button</param>
        public UsoButton(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Creates an element with an assigned name and an output field containing the new button
        /// </summary>
        /// <param name="fieldName">Name of the new button</param>
        /// <param name="newField">Out field containing the button</param>
        public UsoButton(string fieldName, out UsoButton newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Creates a element with an assigned name and event handler
        /// </summary>
        /// <param name="fieldName">Name for the new button</param>
        /// <param name="btnAction">Click event handler</param>
        public UsoButton(string fieldName, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName);
        }

        /// <summary>
        /// Creates a element with an assigned name and event handler and an output field containing the new element
        /// </summary>
        /// <param name="fieldName">Name for the new button</param>
        /// <param name="btnAction">Click event handler</param>
        /// <param name="newField">Out field containing the new button</param>
        public UsoButton(string fieldName, Action btnAction, out UsoButton newField) : base(btnAction)
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Creates a element with an assigned name and label text
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the element</param>
        public UsoButton(string fieldName, string fieldLabelText) : base()
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Creates a element with an assigned name and label text and an output field containing the new element
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the element</param>
        /// <param name="newField">Out field containing the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, out UsoButton newField) : base()
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, and event handler
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="btnAction">Click event handler for the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Action btnAction) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, and event handler and an output field containing the new element
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="btnAction">Click event handler for the new element</param>
        /// <param name="newField">Out field containing the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Action btnAction, out UsoButton newField) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, and icon image
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="iconImage">Image to display in the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Background iconImage) : base(iconImage)
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, and icon image and an output field containing the new element
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="iconImage">Image to display in the new element</param>
        /// <param name="newField">Out field containing the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Background iconImage, out UsoButton newField) : base(iconImage)
        {
            InitElement(fieldName,fieldLabelText);
            newField = this;
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, icon image, and event handler
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="iconImage">Image to display in the new element</param>
        /// <param name="btnAction">Click event handler for the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Background iconImage, Action btnAction) : base(iconImage,btnAction)
        {
            InitElement(fieldName,fieldLabelText);
        }

        /// <summary>
        /// Creates a element with an assigned name, label text, icon image, and event handler and an output field containing the new element
        /// </summary>
        /// <param name="fieldName">Name of the new element</param>
        /// <param name="fieldLabelText">Text to display in the new element</param>
        /// <param name="iconImage">Image to display in the new element</param>
        /// <param name="btnAction">Click event handler for the new element</param>
        /// <param name="newFieldName">Out field containing the new element</param>
        public UsoButton(string fieldName, string fieldLabelText, Background iconImage, Action btnAction, out UsoButton newFieldName) : base(btnAction)
        {
            InitElement(fieldName,fieldLabelText);
            newFieldName = this;
        }
#endregion
    }
}