using GWG.USOUiElements.Utilities;
using UnityEngine.UIElements;
namespace GWG.USOUiElements
{
    [UxmlElement]
    public partial class UsoTwoPaneSplitView : TwoPaneSplitView, IUsoUiElement
    {

        private const string ElementStylesheet = "uso-object-field";
        private const string ElementValidationClass = "uso-field-validation";
        private const string DefaultBindProp = "value";
        private bool _fieldStatusEnabled;
        private FieldStatusTypes _fieldStatus;

        public VisualElement LeftPane { get; set; }
        public VisualElement RightPane { get; set; }

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
                if (_fieldStatusEnabled)
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
            name = fieldName;
            Add(LeftPane = new UsoVisualElement());
            Add(RightPane = new UsoVisualElement());

            AddToClassList(ElementStylesheet);
            FieldStatusEnabled = _fieldStatusEnabled;
        }

        public UsoTwoPaneSplitView()
        {
            InitElement();
        }

        public UsoTwoPaneSplitView(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoTwoPaneSplitView(string fieldName, out string newFieldName) : base()
        {
            InitElement(fieldName);
            newFieldName = name;
        }
    }
}