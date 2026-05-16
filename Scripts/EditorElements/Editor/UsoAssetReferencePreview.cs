using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

#if USO_ADDRESSABLES_PRESENT
using UnityEngine.AddressableAssets;

namespace GWG.UsoUIElements.CustomElements
{


    public class UsoAssetReferencePreview : VisualElement
    {
        private Image m_PreviewImage;
        private VisualElement m_Spinner;
        private SerializedProperty m_Property;
        private IVisualElementScheduledItem m_RotationJob;

        public UsoAssetReferencePreview()
        {
            style.width = 64;
            style.height = 64;
            style.backgroundColor = new Color(0.15f, 0.15f, 0.15f);
            style.justifyContent = Justify.Center;
            style.alignItems = Align.Center;

            m_PreviewImage = new Image();
            m_PreviewImage.style.position = Position.Absolute;
            m_PreviewImage.style.width = Length.Percent(100);
            m_PreviewImage.style.height = Length.Percent(100);

            // REPLACEMENT FOR unityBackgroundScaleMode: ScaleToFit
            m_PreviewImage.style.backgroundPositionX = new BackgroundPosition(BackgroundPositionKeyword.Center);
            m_PreviewImage.style.backgroundPositionY = new BackgroundPosition(BackgroundPositionKeyword.Center);
            m_PreviewImage.style.backgroundRepeat = new BackgroundRepeat(Repeat.NoRepeat, Repeat.NoRepeat);
            m_PreviewImage.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);

            Add(m_PreviewImage);

            m_Spinner = new VisualElement();
            m_Spinner.style.width = 40;
            m_Spinner.style.height = 40;
            m_Spinner.style.backgroundImage = (Texture2D)EditorGUIUtility.IconContent("Loading").image;
            m_Spinner.style.display = DisplayStyle.None;

            // REPLACEMENT FOR transform: Use style.rotate
            // Ensure rotation is centered
            m_Spinner.style.rotate = new Rotate(0);

            Add(m_Spinner);

            m_RotationJob = this.schedule.Execute(() =>
            {
                // Access current rotation through style and increment
                var currentAngle = m_Spinner.style.rotate.value.angle.value;
                m_Spinner.style.rotate = new Rotate(new Angle(currentAngle + 10));
            }).Every(20);

            m_RotationJob.Pause();
        }

        public void BindProperty(SerializedProperty property)
        {
            m_Property = property;
            this.TrackPropertyValue(m_Property, UpdatePreview);
            UpdatePreview(m_Property);
        }

        private void UpdatePreview(SerializedProperty property)
        {
            var assetRef = property.boxedValue as AssetReference;
            if (assetRef == null || !assetRef.RuntimeKeyIsValid())
            {
                SetLoadingState(false);
                m_PreviewImage.image = null;
                return;
            }

            SetLoadingState(true);
            EditorApplication.update -= PollForPreview;
            EditorApplication.update += PollForPreview;
        }

        private void SetLoadingState(bool isLoading)
        {
            if (isLoading)
            {
                m_Spinner.style.display = DisplayStyle.Flex;
                m_RotationJob.Resume();
                m_PreviewImage.style.opacity = 0.3f;
            }
            else
            {
                m_Spinner.style.display = DisplayStyle.None;
                m_RotationJob.Pause();
                m_PreviewImage.style.opacity = 1.0f;
            }
        }

        private void PollForPreview()
        {
            var assetRef = m_Property.boxedValue as AssetReference;
            if (assetRef?.editorAsset == null) return;

            Texture2D preview = AssetPreview.GetAssetPreview(assetRef.editorAsset);

            if (preview != null)
            {
                m_PreviewImage.image = preview;
                SetLoadingState(false);
                EditorApplication.update -= PollForPreview;
            }
        }
    }
}
#endif