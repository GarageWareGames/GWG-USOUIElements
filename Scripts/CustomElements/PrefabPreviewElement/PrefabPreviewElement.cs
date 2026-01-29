
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using UnityEditor.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A custom UI element that provides a visual preview of a GameObject prefab along with its detailed information.
    /// This element combines an ObjectField for prefab selection with an automatically generated preview image and metadata display.
    /// </summary>
    /// <remarks>
    /// This element implements INotifyValueChanged to support Unity's data binding system and inherits from BindableElement.
    /// The preview image is generated asynchronously using Unity's AssetPreview system, with a delay to ensure proper loading.
    /// The element displays prefab name, unique Instance ID, and tags alongside the visual preview.
    /// Requires Unity Editor as it uses AssetPreview and ObjectField functionality.
    /// </remarks>
    [UxmlElement]
    public partial class PrefabPreviewElement : BindableElement, INotifyValueChanged<Object>
    {
        /// <summary>
        /// The path to the USS stylesheet resource that styles this element.
        /// </summary>
        /// <remarks>
        /// This stylesheet should be placed in a Resources folder and contains the visual styling for the prefab preview element.
        /// </remarks>
        public static readonly string ussStylesheetPath = "prefab_preview_element";

        /// <summary>
        /// The CSS class name applied to the main container of the prefab preview element.
        /// </summary>
        /// <remarks>
        /// Used for styling the overall container that holds all child elements of the prefab preview.
        /// </remarks>
        public static readonly string ussContainerClassName = "prefab-preview-container";

        /// <summary>
        /// The CSS class name applied to the preview image element.
        /// </summary>
        /// <remarks>
        /// Used for styling the image that displays the visual preview of the selected prefab.
        /// </remarks>
        public static readonly string ussElementClassName = "prefab-preview-element";

        /// <summary>
        /// The CSS class name applied to the details text column containing prefab metadata.
        /// </summary>
        /// <remarks>
        /// Used for styling the container that holds the prefab name, ID, type, and tags labels.
        /// </remarks>
        public static readonly string ussDetailsClassName = "prefab-preview-details";

        /// <summary>
        /// The Image element that displays the visual preview of the selected prefab.
        /// </summary>
        /// <remarks>
        /// This image is populated asynchronously using Unity's AssetPreview system and shows a rendered view of the prefab.
        /// </remarks>
        Image m_PreviewElement;

        /// <summary>
        /// The ObjectField that allows users to select and assign GameObject prefabs.
        /// </summary>
        /// <remarks>
        /// This field is configured to only accept GameObject types and triggers preview updates when its value changes.
        /// </remarks>
        ObjectField m_ObjectField;

        /// <summary>
        /// The currently selected GameObject prefab value.
        /// </summary>
        /// <remarks>
        /// This represents the main value of the element and is synchronized with the ObjectField selection.
        /// </remarks>
        GameObject m_Value;

        /// <summary>
        /// The cached preview texture generated for the current prefab.
        /// </summary>
        /// <remarks>
        /// This texture is generated asynchronously by Unity's AssetPreview system and cached to avoid repeated generation.
        /// </remarks>
        Texture2D m_Preview;

        /// <summary>
        /// Label that displays the name of the selected prefab.
        /// </summary>
        /// <remarks>
        /// Shows the prefab's name with HTML formatting for better visual presentation.
        /// </remarks>
        Label m_PrefabName;

        /// <summary>
        /// Label that displays the unique Instance ID of the selected prefab.
        /// </summary>
        /// <remarks>
        /// Shows the Unity-generated unique identifier for the prefab Instance with HTML formatting.
        /// </remarks>
        Label m_PrefabUniqueId;

        /// <summary>
        /// Label that displays the type information of the selected prefab.
        /// </summary>
        /// <remarks>
        /// Currently defined but not actively populated with type information in the current implementation.
        /// </remarks>
        Label m_PrefabType;

        /// <summary>
        /// Label that displays the tags associated with the selected prefab.
        /// </summary>
        /// <remarks>
        /// Shows the Unity tags assigned to the prefab with HTML formatting for better presentation.
        /// </remarks>
        Label m_PrefabTags;

        /// <summary>
        /// Container element that wraps the preview image and details text column.
        /// </summary>
        /// <remarks>
        /// This element uses horizontal flex direction to arrange the preview and details side by side.
        /// It's initially hidden and becomes visible when a valid prefab is selected and preview is generated.
        /// </remarks>
        VisualElement m_DetailsWrapper;

        /// <summary>
        /// Initializes a new Instance of the PrefabPreviewElement, setting up the UI structure and styling.
        /// Creates and configures all child elements including the ObjectField, preview image, and detail labels.
        /// </summary>
        /// <remarks>
        /// The constructor loads the associated stylesheet, configures the ObjectField for GameObject selection,
        /// creates the preview image and detail labels, and sets up the initial layout structure.
        /// The details wrapper is initially hidden and will be shown when a valid prefab preview is available.
        /// </remarks>
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

        /// <summary>
        /// Handles value changes from the ObjectField and updates the element's value accordingly.
        /// This method is called when the user selects a different GameObject in the ObjectField.
        /// </summary>
        /// <param name="evt">The change event containing the old and new values from the ObjectField.</param>
        /// <remarks>
        /// This callback ensures that changes in the ObjectField are properly propagated to the element's value property,
        /// which will trigger the preview update and notification events.
        /// </remarks>
        void OnObjectFieldValueChanged(ChangeEvent<Object> evt)
        {
            value = evt.newValue;
        }

        /// <summary>
        /// Sets the value of the element without triggering change notifications or events.
        /// Updates the internal GameObject reference, preview image, and ObjectField display.
        /// </summary>
        /// <param name="newValue">The new GameObject value to set, or null to clear the selection.</param>
        /// <exception cref="ArgumentException">Thrown when the newValue is not null and not a GameObject.</exception>
        /// <remarks>
        /// This method is used internally to update the element's state without triggering change events,
        /// which is important for avoiding infinite loops during value synchronization.
        /// The method validates that the provided value is either null or a GameObject before proceeding.
        /// </remarks>
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

        /// <summary>
        /// Gets or sets the current GameObject value of the element.
        /// Setting this property triggers change events and updates the preview display.
        /// </summary>
        /// <value>The currently selected GameObject prefab, or null if no prefab is selected.</value>
        /// <remarks>
        /// The setter implements the INotifyValueChanged pattern by creating and sending a ChangeEvent
        /// when the value actually changes. It uses SetValueWithoutNotify internally to update the display
        /// and then sends the appropriate change notification to any listeners.
        /// </remarks>
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

        /// <summary>
        /// Generates and sets the preview image and details for the specified GameObject.
        /// This method handles the asynchronous nature of Unity's AssetPreview system with a scheduled execution.
        /// </summary>
        /// <param name="newValue">The GameObject to generate a preview for, or null to clear the preview.</param>
        /// <remarks>
        /// The method uses Unity's AssetPreview.GetAssetPreview to generate a visual representation of the prefab.
        /// Due to the asynchronous nature of preview generation, the method schedules an execution with a 2-frame delay
        /// to ensure the preview texture is available. When successful, it populates the preview image and all detail labels,
        /// then makes the details wrapper visible. If the preview generation fails, the details remain hidden.
        /// </remarks>
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