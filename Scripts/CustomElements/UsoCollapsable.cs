using System;
using System.Collections.Generic;
using GWG.UsoUIElements.Utilities;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    [UxmlElement]
    public partial class UsoCollapsable : VisualElement, IUsoUiElement
    {
        private UsoVisualElement _controlBar;
        private UsoVisualElement _content;
        private UsoVisualElement _iconContainer;
        private UsoImage _collapseIcon;
        public override VisualElement contentContainer => _content;

        public enum DisplayModes
        {
            Vertical,
            VerticalReversed,
            Horizontal,
            HorizontalReversed,
        }

        [UxmlAttribute]
        public DisplayModes DisplayMode
        {
            get
            {
                return _displayMode;
            }
            set
            {
                _displayMode = value;

            }
        }
        private DisplayModes _displayMode;

        [UxmlAttribute]
        public Texture CollapseIcon
        {
            get
            {
                return _collapseIcon.image;
            }
            set
            {
                _collapseIcon.image = value;
            }
        }

        //[UxmlAttribute]
        public List<CollapsableControlBarIconData> Icons
        {
            get
            {
                return _icons;
            }
            set
            {
                _icons = value;
                RebuildIcons();
            }
        }
        private List<CollapsableControlBarIconData> _icons;

        [Serializable]
        public class CollapsableControlBarIconData
        {
            [SerializeField]
            public string iconName;
            [SerializeField]
            public string tooltip;
            [SerializeField]
            public Texture icon;
        }

#region UsoUiElement Implementation

        // //////////////////////////////////////////////////////////////////
        // Start IUsoUiElement Implementation
        private const string ElementClass = "uso-collapsable";
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
                if (_controlBar == null)
                {
                    return false;
                }
                return _controlBar.FieldStatusEnabled;
            }

            private set
            {
                if (_controlBar == null)
                {
                    _controlBar = CreateControlBar();
                }
                _controlBar.ShowFieldStatus(value);
                if (value)
                {
                    _controlBar.AddToClassList(ElementValidationClass);
                }
                else
                {
                    _controlBar.RemoveFromClassList(ElementValidationClass);
                }
            }
        }
        //private bool _fieldStatusEnabled = false;


        public void InitElement(string fieldName = null)
        {
            name = fieldName;
            AddToClassList(ElementClass);
            CreateElement();
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

        public void ClearField()
        {
            SetFieldStatus(FieldStatusTypes.Default);
        }
        // End IUsoUiElement Implementation
        // //////////////////////////////////////////////////////////////////

#endregion

        public UsoCollapsable()
        {
            InitElement();
        }

        public UsoCollapsable(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoCollapsable(string fieldName, out UsoCollapsable newField) : base()
        {
            InitElement(fieldName);
            newField = this;
        }

        private void CreateElement()
        {
            this.FieldStatusEnabled = false;
            hierarchy.Insert(0, CreateControlBar() );
            hierarchy.Insert(1, CreateContent());
        }

        private UsoVisualElement CreateControlBar()
        {
            _controlBar = new UsoVisualElement("uso-collapsable-control-bar");
            _controlBar.AddToClassList("uso-collapsable-control-bar");
            _controlBar.AddToClassList("uso-collapsable-control-bar-" + DisplayMode.ToString().ToLower());
            _controlBar.Add(new UsoImage("collapse-icon", out _collapseIcon));
            _collapseIcon.AddToClassList("uso-collapsable-collapse-icon");

            _controlBar.Add(new UsoVisualElement("icons-container", out _iconContainer));
            _iconContainer.style.flexGrow = 1;
            _iconContainer.style.flexShrink = 0;
            _iconContainer.ShowFieldStatus(false);
            _iconContainer.AddToClassList("uso-collapsable-icons-container");
            return _controlBar;
        }

        private UsoVisualElement CreateContent()
        {
            _content = new UsoVisualElement();
            _content.AddToClassList("uso-collapsable-content");
            return _content;
        }

        private void RebuildIcons()
        {
            _iconContainer.Clear();
            foreach (var icon in _icons)
            {
                _iconContainer.Add(new UsoImage("icon.iconName", out UsoImage newIcon)
                {
                    image = icon.icon,
                    tooltip = icon.tooltip
                });
                newIcon.AddToClassList("uso-collapsable-control-bar-icon");
            }
        }
    }
}