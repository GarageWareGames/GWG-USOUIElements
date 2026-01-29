using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A custom UI element that displays progress as a partially filled circular arc with a percentage label using Unity's Vector API for rendering.
    /// This element provides an alternative implementation to RadialProgress that uses the painter2D API instead of custom mesh generation for simpler rendering.
    /// </summary>
    /// <remarks>
    /// This element renders progress as a stroked circular arc that fills clockwise from the top position based on the progress value.
    /// Unlike the mesh-based RadialProgress, this implementation uses Unity's Vector API (painter2D) for simpler drawing operations.
    /// The element consists of two rendered components: a background track circle and a progress arc that represents the completed portion.
    /// The progress is displayed both visually through the filled arc and textually via a centered percentage label.
    /// The element supports custom CSS properties for styling the track and progress colors with fallback defaults.
    /// </remarks>
    [UxmlElement]
    public partial class RadialProgressVectorApi : VisualElement
    {
        /// <summary>
        /// Gets or sets the progress value as a percentage between 0 and 100.
        /// Setting this value updates both the visual progress arc and the percentage label display.
        /// </summary>
        /// <value>
        /// A float value representing the progress percentage. Values outside the 0-100 range are clamped when displayed in the label.
        /// </value>
        /// <remarks>
        /// This property is exposed to UXML through the UxmlAttribute, allowing it to be set in UI layout files with the "progress" attribute.
        /// When set, the property updates the label text with the rounded, clamped percentage and triggers a visual repaint.
        /// The visual progress arc will extend clockwise from the top position based on this percentage value.
        /// The arc length is calculated as a fraction of the full 360-degree circle corresponding to the progress percentage.
        /// </remarks>
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

        /// <summary>
        /// The primary USS class name for the radial progress control.
        /// </summary>
        /// <remarks>
        /// This class name is applied to the root element and should be used in stylesheets to target the overall control styling.
        /// Shared with the RadialProgress element for consistent styling across different implementations.
        /// </remarks>
        public static readonly string ussClassName = "radial-progress";

        /// <summary>
        /// The USS class name specifically for the percentage label within the radial progress control.
        /// </summary>
        /// <remarks>
        /// This class name is applied to the internal label element and can be used to style the percentage text display.
        /// Shared with the RadialProgress element for consistent label styling across different implementations.
        /// </remarks>
        public static readonly string ussLabelClassName = "radial-progress__label";

        /// <summary>
        /// Custom CSS property definition for the background track color.
        /// </summary>
        /// <remarks>
        /// This property allows CSS stylesheets to specify the color of the background circular track using the "--track-color" property.
        /// The track represents the full circle background that shows the total progress range.
        /// If not specified in CSS, the element falls back to the default gray color.
        /// </remarks>
        static CustomStyleProperty<Color> s_TrackColor = new CustomStyleProperty<Color>("--track-color");

        /// <summary>
        /// Custom CSS property definition for the progress arc color.
        /// </summary>
        /// <remarks>
        /// This property allows CSS stylesheets to specify the color of the progress arc using the "--progress-color" property.
        /// The progress color is used for the arc portion that indicates completed progress.
        /// If not specified in CSS, the element falls back to the default red color.
        /// </remarks>
        static CustomStyleProperty<Color> s_ProgressColor = new CustomStyleProperty<Color>("--progress-color");

        /// <summary>
        /// The color used for rendering the background track circle.
        /// </summary>
        /// <remarks>
        /// This field stores the resolved track color, either from custom CSS properties or the default gray fallback.
        /// The color is applied to the full circular track that provides visual context for the progress indicator.
        /// </remarks>
        Color m_TrackColor = Color.gray;

        /// <summary>
        /// The color used for rendering the progress arc.
        /// </summary>
        /// <remarks>
        /// This field stores the resolved progress color, either from custom CSS properties or the default red fallback.
        /// The color is applied to the arc that represents the completed portion of the progress.
        /// </remarks>
        Color m_ProgressColor = Color.red;

        /// <summary>
        /// The label that displays the progress percentage as text in the center of the circle.
        /// </summary>
        /// <remarks>
        /// This label is automatically updated whenever the progress value changes and shows the rounded percentage value.
        /// The label uses the ussLabelClassName for styling purposes and is positioned at the center of the element.
        /// </remarks>
        Label m_Label;

        /// <summary>
        /// The internal progress value stored as a float.
        /// </summary>
        /// <remarks>
        /// This field stores the actual progress value and is used by the progress property getter and setter.
        /// The value represents the percentage of completion and affects both the visual arc length and label text.
        /// </remarks>
        float m_Progress;

        /// <summary>
        /// Initializes a new Instance of the RadialProgressVectorApi element, setting up the UI structure and event callbacks.
        /// Creates the internal percentage label, applies CSS classes, and registers callbacks for styling and rendering.
        /// </summary>
        /// <remarks>
        /// This constructor creates the percentage label and adds it to the element hierarchy with appropriate styling classes.
        /// It registers callbacks for custom style resolution and visual content generation using the Vector API.
        /// The initial progress is set to 0%, and the element is configured to use painter2D for rendering operations.
        /// The constructor sets up default colors that can be overridden by custom CSS properties.
        /// </remarks>
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

        /// <summary>
        /// Static callback method invoked when custom CSS properties are resolved for this element.
        /// Triggers the UpdateCustomStyles method on the appropriate RadialProgressVectorApi Instance.
        /// </summary>
        /// <param name="evt">The custom style resolved event containing information about the style changes.</param>
        /// <remarks>
        /// This method serves as a bridge between the static callback registration and the Instance method that handles
        /// the actual style updates. It extracts the RadialProgressVectorApi element from the event and calls UpdateCustomStyles.
        /// </remarks>
        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            RadialProgressVectorApi element = (RadialProgressVectorApi)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        /// <summary>
        /// Processes resolved custom CSS properties and applies them to the track and progress colors.
        /// Retrieves color values from custom CSS properties and triggers repainting if any colors have changed.
        /// </summary>
        /// <remarks>
        /// This method is called after custom CSS properties are resolved and applies the "--track-color" and "--progress-color"
        /// properties to their respective color fields. If either color property is successfully resolved from CSS,
        /// the element is marked for repainting to ensure the visual changes are applied immediately.
        /// The method safely handles cases where custom properties are not defined by using TryGetValue and maintaining fallback colors.
        /// </remarks>
        void UpdateCustomStyles()
        {
            bool repaint = customStyle.TryGetValue(s_ProgressColor, out m_ProgressColor);

            if (customStyle.TryGetValue(s_TrackColor, out m_TrackColor))
                repaint = true;

            if (repaint)
                MarkDirtyRepaint();
        }

        /// <summary>
        /// Generates the visual content for the radial progress element using Unity's Vector API (painter2D).
        /// Draws both the background track circle and the progress arc based on the current progress value.
        /// </summary>
        /// <param name="context">The mesh generation context that provides access to the painter2D API for vector-based drawing.</param>
        /// <remarks>
        /// This method uses Unity's painter2D API to draw two circular elements: a complete background track and a partial progress arc.
        /// The track is drawn as a full 360-degree circle using the track color to provide visual context.
        /// The progress arc starts from -90 degrees (top of circle) and extends clockwise based on the progress percentage.
        /// Both elements use a fixed line width of 10.0f and butt line caps for clean stroke appearance.
        /// The circles are centered within the element's content rectangle and sized to fit the available space.
        /// This approach is simpler than mesh generation but may be less performant for complex scenarios.
        /// </remarks>
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