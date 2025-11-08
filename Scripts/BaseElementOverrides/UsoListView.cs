using System;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    [UxmlElement]
    public partial class UsoListView : ListView, IUsoUiElement
    {

#region UsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-list-view";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "itemsSource";
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

        public UsoListView() : base()
        {
            InitElement();
        }

        public UsoListView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoListView(string fieldName, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        public UsoListView(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode) : base()
        {
            InitElement(fieldName);
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoListView(string fieldName, string fieldBindingPath, BindingMode fieldBindingMode, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoListView(string fieldName, string headerText, string fieldBindingPath, BindingMode fieldBindingMode) : base()
        {
            InitElement(fieldName);
            headerTitle = headerText;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public UsoListView(string fieldName, string headerText, string fieldBindingPath, BindingMode fieldBindingMode, out UsoListView newField) : base()
        {
            InitElement(fieldName);
            headerTitle = headerText;
            newField = this;
            ApplyBinding(DefaultBindProp, fieldBindingPath, fieldBindingMode);
        }

        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            showBoundCollectionSize = true;
            allowAdd = true;
            allowRemove = true;
            showAddRemoveFooter = true;
            showFoldoutHeader = true;
            reorderable = true;
            reorderMode = ListViewReorderMode.Animated;
            showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            horizontalScrollingEnabled = true;
            selectionType = SelectionType.Single;
            style.flexGrow = 1;
            style.flexShrink = 0;
            makeNoneElement = MakeNoneElement;
            AddToClassList(ElementClass);
        }

        public virtual VisualElement MakeNoneElement()
        {
            var content = new UsoVisualElement();
            content.AddToClassList("uso-list-view__no-data");
            content.Add(new HelpBox("No Data Entered.\nUse the Add button at the bottom to create a new item", HelpBoxMessageType.Info));
            return content;
        }

        public virtual VisualElement MakeHeader(string labelText)
        {
            Label content = new Label();
            content.AddToClassList("uso-label--title");
            content.text = labelText;
            return content;
        }

        public virtual VisualElement MakeFooter(string labelText)
        {
            Label content = new Label();
            content.AddToClassList("uso-label--subtitle");
            content.text = labelText;
            return content;
        }
    }
}