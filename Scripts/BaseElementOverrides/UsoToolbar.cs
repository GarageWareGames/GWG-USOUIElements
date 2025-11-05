using GWG.USOUiElements.Utilities;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoToolbar : Toolbar, IUsoUiElement
    {

        private const string ElementStylesheet = "uso-toolbar";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "value";
        private bool _fieldStatusEnabled;
        private FieldStatusTypes _fieldStatus;

        VisualElement _content;
        public override VisualElement contentContainer => _content;

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

        [UxmlAttribute]
        public ToolbarOrientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
                if (value == ToolbarOrientation.Horizontal)
                {
                    _content.style.flexDirection = FlexDirection.Row;
                }
                else
                {
                    _content.style.flexDirection = FlexDirection.Column;
                }
            }
        }
        private ToolbarOrientation _orientation = ToolbarOrientation.Horizontal;

        public void SetFieldStatus(FieldStatusTypes fieldStatus)
        {
            FieldStatus = fieldStatus;
        }
        public void ShowFieldStatus(bool status)
        {
            FieldStatusEnabled = status;
        }

        public void InitElement(string fieldName = null)
        {
            _content = new UsoVisualElement();
            _content.style.flexGrow = 1;
            hierarchy.Insert(0, _content);
            name = fieldName;
            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        public UsoLineItem GetParentLineItem()
        {
            return GetFirstAncestorOfType<UsoLineItem>();
        }

        public UsoToolbar()
        {
            InitElement();
        }

        public UsoToolbar(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoToolbar(string fieldName, out string newFieldName) : base()
        {
            InitElement(fieldName);
            newFieldName = name;
        }
    }

    public enum ToolbarOrientation { Horizontal, Vertical }

}