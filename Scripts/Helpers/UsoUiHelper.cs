using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using System.Linq;

namespace GWG.UsoUIElements.Utilities
{
    public static class UsoUiHelper
    {
        public static List<string> FieldValidationClasses = new List<string>()
        {
            "uso-field-label--error",
            "uso-field-label--warning",
            "uso-field-label--success",
            "uso-field-label--info",
            "uso-field-label--disabled"
        };

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


        /// <summary>
        /// Sorts the elements of a sequence in ascending order according
        /// to a key and returns an ordered sequence.
        /// </summary>
        /// <param name="query">The elements to be sorted.</param>
        /// <param name="keySelector">A function to extract a sort key from an element.</param>
        /// <param name="default">The Comparer to compare keys.</param>
        public static IEnumerable<T> OrderBy<T, TKey>(this UQueryBuilder<T> query, Func<T, TKey> keySelector, Comparer<TKey> @default)
            where T : VisualElement
        {
            return query.ToList().OrderBy(keySelector, @default);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according
        /// to a numeric key and returns an ordered sequence.
        /// </summary>
        /// <param name="query">The elements to be sorted.</param>
        /// <param name="keySelector">A function to extract a numeric key from an element.</param>
        public static IEnumerable<T> SortByNumericValue<T>(this UQueryBuilder<T> query, Func<T, float> keySelector)
            where T : VisualElement
        {
            return query.OrderBy(keySelector, Comparer<float>.Default);
        }


        /// <summary>
        /// Returns the first element of a sequence, or a default value if no element is found.
        /// </summary>
        /// <param name="query">The elements to search in.</param>
        public static T FirstOrDefault<T>(this UQueryBuilder<T> query)
            where T : VisualElement
        {
            return query.ToList().FirstOrDefault();
        }

        /// <summary>
        /// Counts the number of elements in the sequence that satisfy the condition specified by the predicate function.
        /// </summary>
        /// <param name="query">The sequence of elements to be processed.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        public static int CountWhere<T>(this UQueryBuilder<T> query, Func<T, bool> predicate)
            where T : VisualElement
        {
            return query.ToList().Count(predicate);
        }


        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        // Usefull for creating a child of a specific type and getting a local var to manipulate in a single line.
        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement
        {
            foreach (string cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    visualElement.AddToClassList(cls);
                }
            }
            return visualElement;
        }

        public static void AddClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.AddToClassList(className);
        }

        public static void RemoveClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.RemoveFromClassList(className);
        }

        public static void ToggleClass(this VisualElement ele, string className)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.ToggleInClassList(className);
        }

        public static void AddClickListener(this VisualElement ele, EventCallback<ClickEvent> callback)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.RegisterCallback(callback);
        }

        public static bool IsVisible(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            return ele.resolvedStyle.display != DisplayStyle.None;
        }

        public static void ToggleVisibility(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            ele.style.display = ele.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
        }

        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement
        {
            visualElement.AddManipulator(manipulator);
            return visualElement;
        }

        /// <summary>
        /// Recursively finds the root element of the document.
        /// </summary>
        /// <param name="ele">The starting VisualElement.</param>
        /// <returns>The root VisualElement.</returns>
        public static VisualElement GetDocumentRoot(this VisualElement ele)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            return (ele.parent == null ? ele : ele.parent.GetDocumentRoot());
        }

        /// <summary>
        /// Finds a child element by its name.
        /// </summary>
        /// <param name="ele">The starting VisualElement.</param>
        /// <param name="name">The name of the child element to find.</param>
        /// <returns>The child VisualElement if found, otherwise null.</returns>
        public static VisualElement FindChildByName(this VisualElement ele, string name)
        {
            if (ele == null) throw new ArgumentNullException(nameof(ele));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name cannot be null or empty", nameof(name));

            foreach (var child in ele.Children())
            {
                if (child.name == name)
                    return child;

                var result = child.FindChildByName(name);
                if (result != null)
                    return result;
            }

            return null;
        }
    }

    namespace GWG.USOUiElements
    {
        public static class UsoStatic
        {

        }
    }
}