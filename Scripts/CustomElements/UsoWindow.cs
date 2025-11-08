using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{

    public class UsoWindow : UsoVisualElement
    {
        public VisualElement Header;
        private VisualElement _content;
        public VisualElement Footer;
        public override VisualElement contentContainer => _content;
        public UsoWindow()
        {
            style.flexGrow = 1;
            hierarchy.Insert(0, Header = new VisualElement(){ style = { width = new StyleLength(Length.Percent(100)) } });
            hierarchy.Insert(1, _content = new VisualElement { style = { flexGrow = 1 } });
            hierarchy.Insert(2, Footer = new VisualElement(){ style = { width = new StyleLength(Length.Percent(100)) } });
        }
    }
}