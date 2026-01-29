using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// A custom image control that extends Unity's Image with USO UI framework functionality.
    /// Provides enhanced styling, field validation, data binding capabilities, and integration with the USO UI system.
    /// </summary>
    /// <remarks>
    /// This control implements the IUsoUiElement interface to provide consistent behavior across the USO UI framework.
    /// It supports field status indicators, automatic data binding for texture content, and custom styling through CSS classes.
    /// The control includes a convenient Image property wrapper for UXML attribute binding and supports various constructor overloads
    /// for different initialization scenarios including texture assignment and data binding configuration.
    /// </remarks>
    [UxmlElement]
    public partial class UsoImage : Image, IUsoUiElement
    {
#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoImage instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-image";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'image' property which controls the displayed texture content.
        /// </summary>
        private const string DefaultBindProp = "image";

        /// <summary>
        /// Gets the current field status type, which determines the visual state and validation feedback.
        /// This property is automatically reflected in the UI through CSS class modifications.
        /// </summary>
        /// <value>The current FieldStatusTypes value indicating the field's validation state.</value>
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
        /// Gets or sets whether field status/validation functionality is enabled for this control.
        /// When enabled, adds validation CSS class for styling. When disabled, removes validation styling.
        /// </summary>
        /// <value>True if field status functionality is enabled; otherwise, false. Default is true.</value>
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

        /// <summary>
        /// Gets or sets the texture displayed in the image control.
        /// This property provides a convenient wrapper around the base image property for UXML attribute binding.
        /// </summary>
        /// <value>The Texture to display in the image control. Can be null to display no image.</value>
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

        /// <summary>
        /// Initializes the USO UI element with the specified field name and applies necessary styling classes.
        /// This method sets up the basic USO framework integration for the control.
        /// </summary>
        /// <param name="fieldName">Optional name to assign to the element. If null, no name is set.</param>
        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        /// <summary>
        /// Applies data binding to the specified property of this control using Unity's data binding system.
        /// Configures the binding with the provided path and mode for automatic data synchronization.
        /// </summary>
        /// <param name="fieldBindingProp">The property name on this control to bind to.</param>
        /// <param name="fieldBindingPath">The path to the data source property to bind from.</param>
        /// <param name="fieldBindingMode">The binding mode that determines how data flows between source and target.</param>
        /// <exception cref="Exception">Thrown when binding setup fails. Original exception is preserved and re-thrown.</exception>
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
        /// Updates the field's status type, which affects its visual appearance and validation state.
        /// The status change is automatically reflected in the UI through the FieldStatus property.
        /// </summary>
        /// <param name="fieldStatus">The new field status type to apply.</param>
        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }

        /// <summary>
        /// Controls the visibility and functionality of the field status/validation system.
        /// When disabled, removes validation-related styling from the control.
        /// </summary>
        /// <param name="status">True to enable field status functionality; false to disable it.</param>
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        /// <summary>
        /// Retrieves the first ancestor UsoLineItem control in the visual tree hierarchy.
        /// This is useful for accessing parent container functionality and maintaining proper UI structure.
        /// </summary>
        /// <returns>The parent UsoLineItem if found; otherwise, null.</returns>
        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
#endregion

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with default settings.
        /// Creates an empty image with USO framework integration enabled.
        /// </summary>
        public UsoImage() : base()
        {
            InitElement();
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with the specified texture.
        /// Creates an image pre-loaded with the provided texture content.
        /// </summary>
        /// <param name="viewImage">The Texture2D to display in the image control.</param>
        public UsoImage(Texture2D viewImage) : base()
        {
            InitElement();
            image = viewImage;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with field name and texture.
        /// Creates an image with custom identification and pre-loaded texture content.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="viewImage">The Texture2D to display in the image control.</param>
        public UsoImage(string fieldName, Texture2D viewImage) : base()
        {
            InitElement(fieldName);
            image = viewImage;
        }

        public UsoImage(string fieldName, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with field name, texture, and returns a reference.
        /// Creates an image with custom identification, texture content, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="viewImage">The Texture2D to display in the image control.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created image.</param>
        public UsoImage(string fieldName, Texture2D viewImage, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            image = viewImage;
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with field name, texture, and data binding configuration.
        /// Creates a fully configured image with custom identification, initial texture, and automatic data binding.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="viewImage">The Texture2D to display initially in the image control.</param>
        /// <param name="bindingPath">The path to the data source property for automatic binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target. Default is ToTarget.</param>
        public UsoImage(string fieldName, Texture2D viewImage, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            image = viewImage;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with field name and data binding configuration.
        /// Creates an image with custom identification and automatic data binding, without initial texture content.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="bindingPath">The path to the data source property for automatic binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target. Default is ToTarget.</param>
        public UsoImage(string fieldName, string bindingPath, BindingMode bindingMode = BindingMode.ToTarget) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with field name, data binding, and returns a reference.
        /// Creates an image with custom identification, automatic data binding, and provides an out parameter for immediate access.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="bindingPath">The path to the data source property for automatic binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created image.</param>
        public UsoImage(string fieldName, string bindingPath, BindingMode bindingMode, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }

        /// <summary>
        /// Initializes a new Instance of the UsoImage class with complete configuration including texture, binding, and reference output.
        /// Creates a fully configured image with custom identification, initial texture, automatic data binding, and immediate access to the created Instance.
        /// </summary>
        /// <param name="fieldName">The name to assign to this image element.</param>
        /// <param name="viewImage">The Texture2D to display initially in the image control.</param>
        /// <param name="bindingPath">The path to the data source property for automatic binding.</param>
        /// <param name="bindingMode">The binding mode that controls data flow between source and target.</param>
        /// <param name="newField">Output parameter that receives a reference to the newly created image.</param>
        public UsoImage(string fieldName, Texture2D viewImage, string bindingPath, BindingMode bindingMode, out UsoImage newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            image = viewImage;
            ApplyBinding(DefaultBindProp, bindingPath, bindingMode);
        }



    }
}