
using Unity.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A custom UI element that displays progress as a partially filled circular ring with a percentage label.
    /// The element renders two concentric circles: a background track and a progress indicator that fills clockwise based on the progress value.
    /// </summary>
    /// <remarks>
    /// This element uses custom mesh generation to create smooth circular progress visualization.
    /// The progress is displayed both visually through the filled portion of the circle and textually via a centered label.
    /// The element supports custom CSS properties for styling the track and progress colors.
    /// The circular approximation uses 200 steps for smooth rendering, and the border thickness is fixed at 10 units.
    /// </remarks>
    [UxmlElement]
    public partial class RadialProgress : VisualElement
    {
        /// <summary>
        /// The primary USS class name for the radial progress control.
        /// </summary>
        /// <remarks>
        /// This class name is applied to the root element and should be used in stylesheets to target the overall control styling.
        /// </remarks>
        public static readonly string ussClassName = "radial-progress";

        /// <summary>
        /// The USS class name specifically for the percentage label within the radial progress control.
        /// </summary>
        /// <remarks>
        /// This class name is applied to the internal label element and can be used to style the percentage text display.
        /// </remarks>
        public static readonly string ussLabelClassName = "radial-progress__label";

        /// <summary>
        /// Custom CSS property definition for the background track color.
        /// </summary>
        /// <remarks>
        /// This property allows CSS stylesheets to specify the color of the background circular track using the "--track-color" property.
        /// The track represents the full circle background that shows the total progress range.
        /// </remarks>
        static CustomStyleProperty<Color> s_TrackColor = new CustomStyleProperty<Color>("--track-color");

        /// <summary>
        /// Custom CSS property definition for the progress indicator color.
        /// </summary>
        /// <remarks>
        /// This property allows CSS stylesheets to specify the color of the progress fill using the "--progress-color" property.
        /// The progress color is used for the portion of the circle that indicates completed progress.
        /// </remarks>
        static CustomStyleProperty<Color> s_ProgressColor = new CustomStyleProperty<Color>("--progress-color");

        /// <summary>
        /// The mesh used to render the background track circle.
        /// </summary>
        /// <remarks>
        /// This mesh represents the full circular background that shows the complete progress range.
        /// It is rendered behind the progress mesh to provide visual context for the progress indicator.
        /// </remarks>
        EllipseMesh m_TrackMesh;

        /// <summary>
        /// The mesh used to render the progress indicator portion of the circle.
        /// </summary>
        /// <remarks>
        /// This mesh uses a subset of indices to render only the filled portion of the circle based on the current progress value.
        /// The number of rendered triangles corresponds directly to the progress percentage.
        /// </remarks>
        EllipseMesh m_ProgressMesh;

        /// <summary>
        /// The label that displays the progress percentage as text in the center of the circle.
        /// </summary>
        /// <remarks>
        /// This label is automatically updated whenever the progress value changes and shows the rounded percentage value.
        /// The label uses the ussLabelClassName for styling purposes.
        /// </remarks>
        Label m_Label;

        /// <summary>
        /// The number of angular steps used to approximate the circular shape.
        /// </summary>
        /// <remarks>
        /// This constant determines the smoothness of the circular approximation. Higher values create smoother circles
        /// but require more vertices and triangles. 200 steps provides a good balance between visual quality and performance.
        /// </remarks>
        const int k_NumSteps = 200;

        /// <summary>
        /// The internal progress value stored as a float.
        /// </summary>
        /// <remarks>
        /// This field stores the actual progress value and is used by the progress property getter and setter.
        /// The value should be between 0 and 100 representing the percentage of completion.
        /// </remarks>
        float m_Progress;

        /// <summary>
        /// Gets or sets the progress value as a percentage between 0 and 100.
        /// Setting this value updates both the visual progress indicator and the percentage label.
        /// </summary>
        /// <value>
        /// A float value representing the progress percentage. Values outside the 0-100 range are clamped when displayed.
        /// </value>
        /// <remarks>
        /// This property is exposed to UXML through the UxmlAttribute, allowing it to be set in UI layout files.
        /// When set, the property updates the label text with the rounded, clamped percentage and triggers a visual repaint.
        /// The visual progress indicator will fill clockwise from the top position based on this percentage.
        /// </remarks>
        [UxmlAttribute]
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
        /// Initializes a new instance of the RadialProgress element, setting up the UI structure and callbacks.
        /// Creates the internal label, initializes the mesh objects, and registers necessary event callbacks for styling and rendering.
        /// </summary>
        /// <remarks>
        /// This constructor creates all internal components including the percentage label and both track and progress meshes.
        /// It registers callbacks for custom style resolution and visual content generation, ensuring the element responds
        /// to CSS changes and renders correctly. The initial progress is set to 0%, and all necessary USS class names are applied.
        /// The meshes are configured to use k_NumSteps for smooth circular approximation.
        /// </remarks>
        public RadialProgress()
        {
            // Create a Label, add a USS class name, and add it to this visual tree.
            m_Label = new Label();
            m_Label.AddToClassList(ussLabelClassName);
            Add(m_Label);

            // Create meshes for the track and the progress.
            m_ProgressMesh = new EllipseMesh(k_NumSteps);
            m_TrackMesh = new EllipseMesh(k_NumSteps);

            // Add the USS class name for the overall control.
            AddToClassList(ussClassName);

            // Register a callback after custom style resolution.
            RegisterCallback<CustomStyleResolvedEvent>(evt => CustomStylesResolved(evt));

            // Register a callback to generate the visual content of the control.
            generateVisualContent += context => GenerateVisualContent(context);

            progress = 0.0f;
        }

        /// <summary>
        /// Static callback method invoked when custom CSS properties are resolved for this element.
        /// Triggers the UpdateCustomStyles method on the appropriate RadialProgress instance.
        /// </summary>
        /// <param name="evt">The custom style resolved event containing information about the style changes.</param>
        /// <remarks>
        /// This method serves as a bridge between the static callback registration and the instance method that handles
        /// the actual style updates. It extracts the RadialProgress element from the event and calls UpdateCustomStyles.
        /// </remarks>
        static void CustomStylesResolved(CustomStyleResolvedEvent evt)
        {
            RadialProgress element = (RadialProgress)evt.currentTarget;
            element.UpdateCustomStyles();
        }

        /// <summary>
        /// Processes resolved custom CSS properties and applies them to the mesh colors.
        /// Retrieves the track and progress colors from custom CSS properties and updates the corresponding meshes.
        /// </summary>
        /// <remarks>
        /// This method is called after custom CSS properties are resolved and applies the "--track-color" and "--progress-color"
        /// properties to their respective meshes. If either mesh becomes dirty due to color changes, the element is marked
        /// for repainting to ensure the visual changes are applied. The method safely handles cases where custom properties
        /// are not defined by using TryGetValue.
        /// </remarks>
        void UpdateCustomStyles()
        {
            if (customStyle.TryGetValue(s_ProgressColor, out var progressColor))
            {
                m_ProgressMesh.color = progressColor;
            }

            if (customStyle.TryGetValue(s_TrackColor, out var trackColor))
            {
                m_TrackMesh.color = trackColor;
            }

            if (m_ProgressMesh.isDirty || m_TrackMesh.isDirty)
                MarkDirtyRepaint();
        }

        /// <summary>
        /// Static callback method for the generateVisualContent event that delegates to the DrawMeshes method.
        /// Extracts the RadialProgress element from the mesh generation context and calls its DrawMeshes method.
        /// </summary>
        /// <param name="context">The mesh generation context provided by Unity's UI system for rendering custom visual content.</param>
        /// <remarks>
        /// This method serves as the entry point for custom mesh generation during the UI rendering pipeline.
        /// It acts as a bridge between Unity's static callback system and the instance-based DrawMeshes implementation.
        /// </remarks>
        static void GenerateVisualContent(MeshGenerationContext context)
        {
            RadialProgress element = (RadialProgress)context.visualElement;
            element.DrawMeshes(context);
        }

        /// <summary>
        /// Generates and renders the track and progress meshes based on the current element size and progress value.
        /// Creates the circular geometry for both the background track and the progress indicator, with the progress mesh showing only a partial circle based on the progress percentage.
        /// </summary>
        /// <param name="context">The mesh generation context that provides methods for allocating and setting mesh data.</param>
        /// <remarks>
        /// This method calculates the element's dimensions and configures both meshes with appropriate sizes and border thickness (10 units).
        /// The track mesh is rendered as a complete circle, while the progress mesh uses only a subset of indices to create a partial fill effect.
        /// The progress visualization starts from the top and fills clockwise. The method handles cases where the element is too small to render
        /// and ensures that progress values are properly clamped between 0 and 100. Index slicing is used to achieve the partial fill effect
        /// by calculating how many triangular segments to render based on the progress percentage.
        /// </remarks>
        void DrawMeshes(MeshGenerationContext context)
        {
            float halfWidth = contentRect.width * 0.5f;
            float halfHeight = contentRect.height * 0.5f;

            if (halfWidth < 2.0f || halfHeight < 2.0f)
                return;

            m_ProgressMesh.width = halfWidth;
            m_ProgressMesh.height = halfHeight;
            m_ProgressMesh.borderSize = 10;
            m_ProgressMesh.UpdateMesh();

            m_TrackMesh.width = halfWidth;
            m_TrackMesh.height = halfHeight;
            m_TrackMesh.borderSize = 10;
            m_TrackMesh.UpdateMesh();

            // Draw track mesh first
            var trackMeshWriteData = context.Allocate(m_TrackMesh.vertices.Length, m_TrackMesh.indices.Length);
            trackMeshWriteData.SetAllVertices(m_TrackMesh.vertices);
            trackMeshWriteData.SetAllIndices(m_TrackMesh.indices);

            // Keep progress between 0 and 100
            float clampedProgress = Mathf.Clamp(m_Progress, 0.0f, 100.0f);

            // Determine how many triangle are used to depending on progress, to achieve a partially filled circle
            int sliceSize = Mathf.FloorToInt((k_NumSteps * clampedProgress) / 100.0f);

            if (sliceSize == 0)
                return;

            // Every step is 6 indices in the corresponding array
            sliceSize *= 6;

            var progressMeshWriteData = context.Allocate(m_ProgressMesh.vertices.Length, sliceSize);
            progressMeshWriteData.SetAllVertices(m_ProgressMesh.vertices);

            var tempIndicesArray = new NativeArray<ushort>(m_ProgressMesh.indices, Allocator.Temp);
            progressMeshWriteData.SetAllIndices(tempIndicesArray.Slice(0, sliceSize));
            tempIndicesArray.Dispose();
        }

    }
}