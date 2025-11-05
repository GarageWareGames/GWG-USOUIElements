
using GWG.USOUiElements.Utilities;
using UnityEngine.UIElements;

namespace GWG.USOUiElements
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
}