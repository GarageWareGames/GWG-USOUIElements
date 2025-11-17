
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A utility class that generates mesh data for rendering elliptical shapes with customizable borders in Unity's UI Elements system.
    /// This class creates vertex and index arrays suitable for use with Unity's UI rendering pipeline to draw ellipses or circles.
    /// </summary>
    /// <remarks>
    /// The mesh is generated as a series of triangular segments forming a ring shape, where the outer edge defines the ellipse boundary
    /// and the inner edge creates the border effect. The mesh uses a dirty flag system to optimize regeneration only when properties change.
    /// The generated vertices include position data and tint colors compatible with Unity's UI Element rendering system.
    /// All angles are calculated in degrees and converted to radians for trigonometric calculations.
    /// </remarks>
    public class EllipseMesh
    {
        /// <summary>
        /// The number of steps (segments) used to approximate the ellipse curve.
        /// </summary>
        /// <remarks>
        /// Higher values create smoother ellipses but require more vertices and triangles.
        /// This value determines the angular resolution of the ellipse approximation.
        /// </remarks>
        int m_NumSteps;

        /// <summary>
        /// The width (horizontal radius) of the ellipse.
        /// </summary>
        /// <remarks>
        /// This represents half the total width of the ellipse and is used in conjunction with sine calculations
        /// to determine the horizontal positions of vertices around the ellipse perimeter.
        /// </remarks>
        float m_Width;

        /// <summary>
        /// The height (vertical radius) of the ellipse.
        /// </summary>
        /// <remarks>
        /// This represents half the total height of the ellipse and is used in conjunction with cosine calculations
        /// to determine the vertical positions of vertices around the ellipse perimeter.
        /// </remarks>
        float m_Height;

        /// <summary>
        /// The color applied to all vertices in the ellipse mesh.
        /// </summary>
        /// <remarks>
        /// This color is used as the tint color for all vertices, allowing the ellipse to be rendered
        /// in different colors without requiring separate materials or shaders.
        /// </remarks>
        Color m_Color;

        /// <summary>
        /// The thickness of the border, defining the difference between outer and inner radii.
        /// </summary>
        /// <remarks>
        /// This value is subtracted from both width and height to create the inner ellipse boundary.
        /// A larger border size creates a thicker ring, while a border size equal to the smallest radius creates a filled ellipse.
        /// </remarks>
        float m_BorderSize;

        /// <summary>
        /// Flag indicating whether the mesh data needs to be regenerated due to property changes.
        /// </summary>
        /// <remarks>
        /// This optimization flag prevents unnecessary mesh regeneration when properties haven't changed.
        /// It is set to true whenever any property affecting the mesh geometry or appearance is modified.
        /// </remarks>
        bool m_IsDirty;

        /// <summary>
        /// Array containing all vertex data for the ellipse mesh.
        /// Each vertex includes position and tint color information.
        /// </summary>
        /// <value>
        /// The array of vertices, or null if the mesh hasn't been generated yet.
        /// The array length is always numSteps * 2 (outer and inner vertex for each step).
        /// </value>
        /// <remarks>
        /// Vertices are arranged in pairs for each angular step: an outer vertex defining the ellipse boundary
        /// and an inner vertex defining the inner border. The vertices are compatible with Unity's UI Element rendering system.
        /// </remarks>
        public Vertex[] vertices { get; private set; }

        /// <summary>
        /// Array containing triangle indices that define how vertices are connected to form the ellipse mesh.
        /// </summary>
        /// <value>
        /// The array of indices, or null if the mesh hasn't been generated yet.
        /// The array length is always numSteps * 6 (two triangles per step, three indices per triangle).
        /// </value>
        /// <remarks>
        /// Indices are organized to create two triangles per angular step, forming a ring segment.
        /// The triangles connect the current and previous outer/inner vertex pairs to create a continuous ring shape.
        /// </remarks>
        public ushort[] indices { get; private set; }

        /// <summary>
        /// Initializes a new EllipseMesh with the specified number of steps for curve approximation.
        /// </summary>
        /// <param name="numSteps">The number of angular steps to use for approximating the ellipse curve.</param>
        /// <remarks>
        /// The constructor sets the mesh as dirty to ensure initial generation when UpdateMesh is called.
        /// Other properties (width, height, color, borderSize) must be set separately before calling UpdateMesh.
        /// A higher numSteps value will create a smoother ellipse at the cost of more vertices and triangles.
        /// </remarks>
        public EllipseMesh(int numSteps)
        {
            m_NumSteps = numSteps;
            m_IsDirty = true;
        }

        /// <summary>
        /// Generates or regenerates the mesh vertex and index data based on current properties.
        /// This method only performs work if the mesh is marked as dirty due to property changes.
        /// </summary>
        /// <remarks>
        /// The method creates a ring-shaped mesh by generating pairs of vertices (outer and inner) for each angular step.
        /// Each step creates two triangles that connect the current vertex pair with the previous pair, forming segments of the ring.
        /// The mesh generation uses trigonometric functions to calculate vertex positions around the ellipse perimeter.
        /// Vertex positions are offset by width and height to center the ellipse at the specified position.
        /// The method automatically handles array resizing if the number of required vertices or indices changes.
        /// </remarks>
        public void UpdateMesh()
        {
            if (!m_IsDirty)
                return;

            int numVertices = numSteps * 2;
            int numIndices = numVertices * 6;

            if (vertices == null || vertices.Length != numVertices)
                vertices = new Vertex[numVertices];

            if (indices == null || indices.Length != numIndices)
                indices = new ushort[numIndices];

            float stepSize = 360.0f / (float)numSteps;
            float angle = -180.0f;

            for (int i = 0; i < numSteps; ++i)
            {
                angle -= stepSize;
                float radians = Mathf.Deg2Rad * angle;

                float outerX = Mathf.Sin(radians) * width;
                float outerY = Mathf.Cos(radians) * height;
                Vertex outerVertex = new Vertex();
                outerVertex.position = new Vector3(width + outerX, height + outerY, Vertex.nearZ);
                outerVertex.tint = color;
                vertices[i * 2] = outerVertex;

                float innerX = Mathf.Sin(radians) * (width - borderSize);
                float innerY = Mathf.Cos(radians) * (height - borderSize);
                Vertex innerVertex = new Vertex();
                innerVertex.position = new Vector3(width + innerX, height + innerY, Vertex.nearZ);
                innerVertex.tint = color;
                vertices[i * 2 + 1] = innerVertex;

                indices[i * 6] = (ushort)((i == 0) ? vertices.Length - 2 : (i - 1) * 2); // previous outer vertex
                indices[i * 6 + 1] = (ushort)(i * 2); // current outer vertex
                indices[i * 6 + 2] = (ushort)(i * 2 + 1); // current inner vertex

                indices[i * 6 + 3] = (ushort)((i == 0) ? vertices.Length - 2 : (i - 1) * 2); // previous outer vertex
                indices[i * 6 + 4] = (ushort)(i * 2 + 1); // current inner vertex
                indices[i * 6 + 5] = (ushort)((i == 0) ? vertices.Length - 1 : (i - 1) * 2 + 1); // previous inner vertex
            }

            m_IsDirty = false;
        }

        /// <summary>
        /// Gets a value indicating whether the mesh data needs to be regenerated due to property changes.
        /// </summary>
        /// <value>
        /// True if any properties affecting the mesh have changed since the last UpdateMesh call; otherwise, false.
        /// </value>
        /// <remarks>
        /// This property can be used to determine if calling UpdateMesh will perform any work.
        /// The dirty flag is automatically managed by property setters and cleared by UpdateMesh.
        /// </remarks>
        public bool isDirty => m_IsDirty;

        /// <summary>
        /// Compares a field value with a new value and updates the field if they differ, marking the mesh as dirty.
        /// This utility method provides epsilon-based floating-point comparison to avoid precision issues.
        /// </summary>
        /// <param name="field">Reference to the field to potentially update.</param>
        /// <param name="newValue">The new value to compare against and potentially assign.</param>
        /// <remarks>
        /// This method is used internally by property setters to ensure the mesh is marked dirty only when
        /// actual changes occur. The epsilon comparison prevents unnecessary dirty marking due to floating-point precision.
        /// </remarks>
        void CompareAndWrite(ref float field, float newValue)
        {
            if (Mathf.Abs(field - newValue) > float.Epsilon)
            {
                m_IsDirty = true;
                field = newValue;
            }
        }

        /// <summary>
        /// Gets or sets the number of angular steps used to approximate the ellipse curve.
        /// </summary>
        /// <value>
        /// The number of steps, which determines the smoothness of the ellipse approximation.
        /// Higher values create smoother curves but require more vertices and triangles.
        /// </value>
        /// <remarks>
        /// Changing this value marks the mesh as dirty and will require regeneration.
        /// The total number of vertices will be numSteps * 2, and the total number of indices will be numSteps * 6.
        /// </remarks>
        public int numSteps
        {
            get => m_NumSteps;
            set
            {
                m_IsDirty = value != m_NumSteps;
                m_NumSteps = value;
            }
        }

        /// <summary>
        /// Gets or sets the width (horizontal radius) of the ellipse.
        /// </summary>
        /// <value>
        /// The width value representing half the total horizontal span of the ellipse.
        /// </value>
        /// <remarks>
        /// This value is used with sine calculations to determine horizontal vertex positions.
        /// Changing this value marks the mesh as dirty if the new value differs from the current value.
        /// The comparison uses epsilon-based floating-point comparison to avoid precision issues.
        /// </remarks>
        public float width
        {
            get => m_Width;
            set => CompareAndWrite(ref m_Width, value);
        }

        /// <summary>
        /// Gets or sets the height (vertical radius) of the ellipse.
        /// </summary>
        /// <value>
        /// The height value representing half the total vertical span of the ellipse.
        /// </value>
        /// <remarks>
        /// This value is used with cosine calculations to determine vertical vertex positions.
        /// Changing this value marks the mesh as dirty if the new value differs from the current value.
        /// The comparison uses epsilon-based floating-point comparison to avoid precision issues.
        /// </remarks>
        public float height
        {
            get => m_Height;
            set => CompareAndWrite(ref m_Height, value);
        }

        /// <summary>
        /// Gets or sets the color applied to all vertices in the ellipse mesh.
        /// </summary>
        /// <value>
        /// The Color value used as the tint for all vertices in the mesh.
        /// </value>
        /// <remarks>
        /// This color is applied to both outer and inner vertices, allowing the entire ellipse to be rendered
        /// in the specified color. Changing this value marks the mesh as dirty if the new color differs from the current color.
        /// </remarks>
        public Color color
        {
            get => m_Color;
            set
            {
                m_IsDirty = value != m_Color;
                m_Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the border thickness, defining the difference between outer and inner ellipse radii.
        /// </summary>
        /// <value>
        /// The border size value that is subtracted from both width and height to create the inner ellipse boundary.
        /// </value>
        /// <remarks>
        /// A larger border size creates a thicker ring effect, while a border size equal to or greater than
        /// the smallest radius will create a filled ellipse. Changing this value marks the mesh as dirty
        /// if the new value differs from the current value using epsilon-based comparison.
        /// </remarks>
        public float borderSize
        {
            get => m_BorderSize;
            set => CompareAndWrite(ref m_BorderSize, value);
        }

    }
}