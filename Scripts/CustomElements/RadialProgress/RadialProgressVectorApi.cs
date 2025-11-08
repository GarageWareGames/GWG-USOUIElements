using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    // An element that displays progress inside a partially filled circle
    [UxmlElement]
    public partial class RadialProgressVectorApi : VisualElement
    {
        // Expose the progress property to UXML
        [UxmlAttribute("progress")]
        public float progress
        {
            get => m_Progress;
            set
            {
                m_Progress = value;
                m_Label.text = Mathf.Clamp(Mathf.Round(value), 0, 100) + "%";
                MarkDirtyRepaint();
            }
        }

        public static readonly string ussClassName = "radial-progress";
        public static readonly string ussLabelClassName = "radial-progress__label";

        static CustomStyleProperty<Color> s_TrackColor = new CustomStyleProperty<Color>("--track-color");
        static CustomStyleProperty<Color> s_ProgressColor = new CustomStyleProperty<Color>("--progress-color");

        Color m_TrackColor = Color.gray;
        Color m_ProgressColor = Color.red;

        Label m_Label;
        float m_Progress;

        public RadialProgressVectorApi()
        {
            m_Label = new Label();
            m_Label.AddToClassList(ussLabelClassName);
            Add(m_Label);

            AddToClassList(ussClassName);

            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));
            generateVisualContent += GenerateVisualContent;

            progress = 0.0f;
        }

        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            RadialProgressVectorApi element = (RadialProgressVectorApi)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        void UpdateCustomStyles()
        {
            bool repaint = customStyle.TryGetValue(s_ProgressColor, out m_ProgressColor);

            if (customStyle.TryGetValue(s_TrackColor, out m_TrackColor))
                repaint = true;

            if (repaint)
                MarkDirtyRepaint();
        }

        void GenerateVisualContent(MeshGenerationContext context)
        {
            float width = contentRect.width;
            float height = contentRect.height;

            var painter = context.painter2D;
            painter.lineWidth = 10.0f;
            painter.lineCap = LineCap.Butt;

            // Draw the track
            painter.strokeColor = m_TrackColor;
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), width * 0.5f, 0.0f, 360.0f);
            painter.Stroke();

            // Draw the progress
            painter.strokeColor = m_ProgressColor;
            painter.BeginPath();
            painter.Arc(new Vector2(width * 0.5f, height * 0.5f), width * 0.5f, -90.0f, 360.0f * (progress / 100.0f) - 90.0f);
            painter.Stroke();
        }
    }
}