using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityEditor.UIElements;

namespace GWG.UsoUiElements
{
    [UxmlElement]
    public partial class PrefabPreviewElement : BindableElement, INotifyValueChanged<Object>
    {
        public static readonly string ussStylesheetPath = "prefab_preview_element";
        public static readonly string ussContainerClassName = "prefab-preview-container";
        public static readonly string ussElementClassName = "prefab-preview-element";
        public static readonly string ussDetailsClassName = "prefab-preview-details";

        Image m_PreviewElement;
        ObjectField m_ObjectField;
        GameObject m_Value;
        Texture2D m_Preview;
        Label m_PrefabName;
        Label m_PrefabUniqueId;
        Label m_PrefabType;
        Label m_PrefabTags;
        VisualElement m_DetailsWrapper;

        public PrefabPreviewElement()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(ussStylesheetPath));
            style.flexShrink = 0;
            AddToClassList(ussContainerClassName);

            m_ObjectField = new ObjectField();
            m_ObjectField.objectType = typeof(GameObject);
            m_ObjectField.RegisterValueChangedCallback(OnObjectFieldValueChanged);
            Add(m_ObjectField);

            m_DetailsWrapper = new VisualElement();
            m_DetailsWrapper.style.flexDirection = FlexDirection.Row;

            m_PreviewElement = new Image();
            m_PreviewElement.AddToClassList(ussElementClassName);
            m_PreviewElement.style.flexShrink = 0;
            m_DetailsWrapper.Add(m_PreviewElement);

            VisualElement detailsTextColumn = new VisualElement();
            detailsTextColumn.style.flexShrink = 0;
            detailsTextColumn.AddToClassList(ussDetailsClassName);
            m_DetailsWrapper.Add(detailsTextColumn);

            m_PrefabName = new Label();
            detailsTextColumn.Add(m_PrefabName);

            m_PrefabUniqueId = new Label();
            detailsTextColumn.Add(m_PrefabUniqueId);

            m_PrefabType = new Label();
            detailsTextColumn.Add(m_PrefabType);

            m_PrefabTags = new Label();
            detailsTextColumn.Add(m_PrefabTags);
            Add(m_DetailsWrapper);
            m_DetailsWrapper.style.display = DisplayStyle.None;

        }

        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        public void SetValueWithoutNotify(Object newValue)
        {
            if (newValue == null || newValue is GameObject)
            {
                // Update the preview Image and update the ObjectField.
                m_Value = newValue as GameObject;
                SetPreview(m_Value);
                // Notice that this line calls the ObjectField's SetValueWithoutNotify() method instead of just setting
                // m_ObjectField.value. This is very important; you don't want m_ObjectField to send a ChangeEvent.
                m_ObjectField.SetValueWithoutNotify(m_Value);
            }
            else throw new ArgumentException($"Expected object of type {typeof(GameObject)} but got {newValue.GetType()}");
        }

        public Object value
        {
            get => m_Value;
            // The setter is called when the user changes the value of the ObjectField, which calls
            // OnObjectFieldValueChanged(), which calls this.
            set
            {
                if (value == this.value)
                {
                    return;
                }

                var previous = this.value;
                SetValueWithoutNotify(value);

                using (var evt = ChangeEvent<Object>.GetPooled(previous, value))
                {
                    evt.target = this;
                    SendEvent(evt);
                }
            }
        }
        private void SetPreview(Object newValue)
        {
            m_Preview = AssetPreview.GetAssetPreview(newValue);
            schedule.Execute(() =>
            {
                //Debug.Log("Setting Image");
                if (m_Preview == null)
                {
                    if (m_ObjectField != null)
                    {
                        m_Preview = AssetPreview.GetAssetPreview(m_ObjectField.value as GameObject);
                    }
                    else
                    {
                        return;
                    }
                }
                if(m_Preview == null) return;
                m_PreviewElement.image = m_Preview;
                m_PrefabName.text = "<b>Name:</b> " + m_Value.name;
                m_PrefabUniqueId.text = "<b>Unique ID:</b> " + m_Value.GetInstanceID();
                m_PrefabTags.text = "<b>Tags:</b> " + m_Value.tag;
                m_DetailsWrapper.style.display = DisplayStyle.Flex;
            }).ExecuteLater(2);  //Until(() => m_PreviewElement.image != null);
        }
    }
}