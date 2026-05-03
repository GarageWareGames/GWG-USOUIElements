using System;
using GWG.UsoUIElements.CustomElements;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;
namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoMultiColumnListView : MultiColumnListView, IUsoUiElement
    {
        #region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation

        /// <summary>
        /// CSS class name applied to all UsoListView instances for styling purposes.
        /// </summary>
        private const string ElementClass = "uso-multi-column-list-view";

        /// <summary>
        /// CSS class name applied when field validation/status functionality is enabled.
        /// </summary>
        private const string ElementValidationClass = "uso-field-validation";

        /// <summary>
        /// Default binding property used when applying data bindings to this field.
        /// Binds to the 'itemsSource' property which controls the collection data displayed in the list.
        /// </summary>
        private const string DefaultBindProp = "itemsSource";

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
                    RemoveFromClassList("uso-field");
                }
                else
                {
                    RemoveFromClassList(ElementValidationClass);
                    AddToClassList("uso-field");
                }
            }
        }
        private bool _fieldStatusEnabled = true;

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

        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            style.flexGrow = 0;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            reorderMode = ListViewReorderMode.Animated;
            showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            horizontalScrollingEnabled = true;
            selectionType = SelectionType.Single;
            //makeNoneElement = MakeNoneElement;
            //AddToClassList(ElementClass);
        }

        public VisualElement MakeNoneElement()
        {

            var content = new UsoVisualElement();
            content.AddToClassList("uso-list-view__no-data");
            content.Add(new HelpBox("No records found.\nUse the Add button at the bottom to create a new item", HelpBoxMessageType.Info));
            return content;
        }

        public UsoMultiColumnListView() : base()
        {
            InitElement();
        }

        public UsoMultiColumnListView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoMultiColumnListView(string fieldName, out UsoMultiColumnListView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }
    }
}