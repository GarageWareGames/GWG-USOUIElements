using System.Collections.Generic;
using UnityEngine.UIElements;

namespace GWG.USOUiElements.Utilities
{
    public static class UsoUiHelper
    {
        public static List<string> FieldValidationClasses = new List<string>(){"uso-field-label--error","uso-field-label--warning",
            "uso-field-label--success","uso-field-label--info","uso-field-label--disabled"};



        public static void SetFieldStatus(VisualElement element, FieldStatusTypes fieldStatus)
        {
            foreach (string className in FieldValidationClasses)
            {
                element.RemoveFromClassList(className);
            }

            switch (fieldStatus)
            {
                case FieldStatusTypes.Error:
                    element.AddToClassList("uso-field-label--error");
                    break;
                case FieldStatusTypes.Warning:
                    element.AddToClassList("uso-field-label--warning");
                    break;
                case FieldStatusTypes.Success:
                    element.AddToClassList("uso-field-label--success");
                    break;
                case FieldStatusTypes.Info:
                    element.AddToClassList("uso-field-label--info");
                    break;
                case FieldStatusTypes.Disabled:
                    element.AddToClassList("uso-field-label--disabled");
                    break;
                case FieldStatusTypes.Default:
                default:
                    break;
            }
        }

    }
}