using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    public class UsoCustomElementTemplate : UsoVisualElement
    {
        private const string UsoFieldStyleSheet = "uso-field-Label";

        public UsoCustomElementTemplate() : base()
        {
            InitElement();
        }

        public UsoCustomElementTemplate(string fieldName) : base()
        {
            InitElement(fieldName);
        }

        public UsoCustomElementTemplate(string fieldName, out string newFieldName) : base()
        {
            InitElement(fieldName);
            newFieldName = name;
        }

        public void InitElement(string fieldName = "")
        {
            name = fieldName;
            style.flexDirection = FlexDirection.Row;
            style.flexGrow = 1;
            AddToClassList(UsoFieldStyleSheet);
        }
    }
}