using System;
using UnityEngine.UIElements;

namespace GWG.UsoUiElements
{
    public static class VisualElementExtensions {
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes) {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        // Usefull for creating a child of a specific type and getting a local var to manipulate in a single line.
        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new() {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement {
            parent.Add(child);
            return child;
        }

        public static T AddClass<T>(this T visualElement, params string[] classes) where T : VisualElement {
            foreach (string cls in classes) {
                if (!string.IsNullOrEmpty(cls)) {
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

        public static T WithManipulator<T>(this T visualElement, IManipulator manipulator) where T : VisualElement {
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
}