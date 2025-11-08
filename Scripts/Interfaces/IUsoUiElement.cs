using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    public interface IUsoUiElement
    {

        bool FieldStatusEnabled { get; }

        FieldStatusTypes FieldStatus { get; }

        void ApplyBinding(string fieldBindingProp, string fieldBindingPath, BindingMode fieldBindingMode) { }

        void InitElement(string fieldName) { }

        void RemoveFromClassList(string className);

        void AddToClassList(string className);
        void SetFieldStatus(FieldStatusTypes fieldStatus);
        void ShowFieldStatus(bool status);
    }

    public enum ElementType
    {
        Button,
        Toggle,
        Slider,
        SliderInt,
        Label,
        HelpBox,
        Foldout,
        Window,
    }

    public enum LabelType
    {
        Default,
        Header,
        SubHeader,
        Title,
        Subtitle,
        Description,
    }

    public enum FieldStatusTypes
    {
        Default,
        Error,
        Warning,
        Success,
        Info,
        Disabled
    }
}