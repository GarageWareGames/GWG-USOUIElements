
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A custom pie chart visualization control that extends Unity's VisualElement to provide dynamic data-driven circular chart rendering.
    /// Creates interactive pie charts based on supplied percentage values and colors, with real-time update capabilities and customizable appearance.
    /// </summary>
    /// <remarks>
    /// This control provides comprehensive pie chart functionality within the USO UI framework's custom elements collection.
    /// It uses Unity's 2D painter system for high-performance rendering of circular chart segments with customizable colors and percentages.
    /// The chart automatically handles percentage-to-angle conversions, supports dynamic data updates, and provides smooth visual rendering
    /// through Unity's mesh generation context. The control is designed for displaying proportional data relationships in an intuitive
    /// circular format, commonly used for statistics, progress indicators, and data visualization scenarios.
    ///
    /// Usage Example:
    /// <code>
    /// PercentageColorData data = new PercentageColorData();
    /// data.Percentage = 40.0f;
    /// data.Color = new Color32(182, 235, 122, 255);
    /// chart.UpdateChartData(new List&lt;PercentageColorData&gt; { data });
    /// </code>
    ///
    /// The control supports real-time updates through the UpdateChartData method and automatically manages visual refresh
    /// through Unity's dirty repaint system for optimal performance.
    /// </remarks>
    public class UsoPieChart : VisualElement
    {
        /// <summary>
        /// Private backing field for the pie chart's radius in pixels.
        /// Controls the size of the circular chart area and affects the overall visual scale.
        /// </summary>
        float m_Radius = 100.0f;

        /// <summary>
        /// Private backing field for a legacy value property.
        /// Maintained for potential future functionality or backward compatibility.
        /// </summary>
        float m_Value = 40.0f;

        /// <summary>
        /// Visual element reference for the chart container.
        /// Used for managing chart-specific styling and layout properties.
        /// </summary>
        VisualElement m_Chart;

        /// <summary>
        /// Collection of percentage and color data that defines the pie chart segments.
        /// Each entry represents a slice of the pie with its proportional size and display color.
        /// </summary>
        /// <remarks>
        /// The list is initialized with default sample data showing a 40% green segment and 60% orange segment.
        /// This provides immediate visual feedback when the chart is first created and serves as an example
        /// of the expected data structure for chart customization.
        /// </remarks>
        public List<PercentageColorData> percentageColorData = new List<PercentageColorData>
        {
            new PercentageColorData { Percentage = 40.0f, Color = new Color32(182, 235, 122, 255) },
            new PercentageColorData { Percentage = 60.0f, Color = new Color32(251, 120, 19, 255) }
        };

        /// <summary>
        /// Gets or sets the radius of the pie chart in pixels.
        /// Automatically updates the chart's visual dimensions and triggers a repaint when changed.
        /// </summary>
        /// <value>The radius value in pixels. Default is 100.0f.</value>
        /// <remarks>
        /// Setting this property automatically updates both width and height of the chart to maintain
        /// a circular appearance. The chart dimensions are set to twice the radius (diameter) and a
        /// visual repaint is triggered to reflect the size change immediately.
        /// </remarks>
        public float radius
        {
            get => m_Radius;
            set
            {
                m_Radius = value;
                m_Chart.style.height = diameter;
                m_Chart.style.width = diameter;
                m_Chart.MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// Gets the diameter of the pie chart calculated as twice the radius.
        /// This read-only property provides the total width and height of the circular chart area.
        /// </summary>
        /// <value>The diameter in pixels, calculated as radius * 2.</value>
        /// <remarks>
        /// This property is used internally for setting chart dimensions and provides external access
        /// to the total chart size for layout calculations and positioning purposes.
        /// </remarks>
        public float diameter => m_Radius * 2.0f;

        /// <summary>
        /// Gets or sets a legacy value property maintained for potential future functionality.
        /// Triggers a visual repaint when changed to ensure chart consistency.
        /// </summary>
        /// <value>A floating-point value. Default is 40.0f.</value>
        /// <remarks>
        /// This property appears to be maintained for backward compatibility or future enhancement.
        /// Currently, it triggers a repaint when changed but doesn't directly affect chart rendering,
        /// which is controlled by the percentageColorData collection.
        /// </remarks>
        public float value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                MarkDirtyRepaint();
            }
        }

        /// <summary>
        /// Initializes a new Instance of the UsoPieChart class with default settings and visual content generation.
        /// Sets up the chart for rendering and establishes the drawing callback for dynamic visual updates.
        /// </summary>
        /// <remarks>
        /// The constructor registers the DrawCanvas method with Unity's generateVisualContent callback system,
        /// enabling the chart to render its visual content through Unity's 2D painter system. This approach
        /// provides high-performance rendering with automatic integration into Unity's UI rendering pipeline.
        /// </remarks>
        public UsoPieChart()
        {
            generateVisualContent += DrawCanvas;
        }

        /// <summary>
        /// Updates the pie chart's data with a new collection of percentage and color information.
        /// Replaces the current chart data and triggers an immediate visual refresh to display the changes.
        /// </summary>
        /// <param name="newData">A list of PercentageColorData objects defining the new chart segments.</param>
        /// <remarks>
        /// This method provides the primary interface for updating chart content dynamically. It completely
        /// replaces the existing data set and automatically triggers a repaint to ensure the visual representation
        /// matches the new data immediately. The method supports real-time data visualization scenarios where
        /// chart content needs to change based on user interaction or data updates.
        /// </remarks>
        public void UpdateChartData(List<PercentageColorData> newData)
        {
            percentageColorData = newData;
            MarkDirtyRepaint();
        }

        /// <summary>
        /// Renders the pie chart visual content using Unity's 2D painter system within the provided mesh generation context.
        /// Creates individual pie slices based on the percentageColorData collection with appropriate colors and proportions.
        /// </summary>
        /// <param name="ctx">The MeshGenerationContext providing access to Unity's 2D painter for rendering operations.</param>
        /// <remarks>
        /// This method implements the core rendering logic for the pie chart, converting percentage data into
        /// angular segments and drawing them as filled arc shapes. The rendering process:
        /// 1. Iterates through each data entry in percentageColorData
        /// 2. Converts percentage values to angular measurements (360° total)
        /// 3. Sets appropriate fill colors for each segment
        /// 4. Draws arc segments from the center point with calculated angles
        ///
        /// The method uses cumulative angle calculation to ensure segments are positioned correctly adjacent
        /// to each other, creating a complete circular representation. Each segment is drawn as a filled path
        /// starting from the chart center to create proper pie slice geometry.
        /// </remarks>
        void DrawCanvas(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;
            painter.strokeColor = Color.white;
            painter.fillColor = Color.white;

            float angle = 0.0f;
            float anglePct = 0.0f;

            foreach (var data in percentageColorData)
            {
                float pct = data.Percentage;
                Color32 color = data.Color;

                anglePct += 360.0f * (pct / 100);

                painter.fillColor = color;
                painter.BeginPath();
                painter.MoveTo(new Vector2(m_Radius, m_Radius));
                painter.Arc(new Vector2(m_Radius, m_Radius), m_Radius, angle, anglePct);
                painter.Fill();

                angle = anglePct;
            }
        }
    }

    /// <summary>
    /// A serializable data structure that represents a single segment of a pie chart with its percentage value and display color.
    /// Used by UsoPieChart to define individual chart slices with their proportional size and visual appearance.
    /// </summary>
    /// <remarks>
    /// This class serves as the fundamental data unit for pie chart visualization, combining the quantitative
    /// aspect (percentage) with the visual aspect (color) in a single, easily manageable structure.
    /// The Serializable attribute enables this class to be saved and loaded with Unity's serialization system,
    /// making it suitable for inspector configuration, data persistence, and runtime data management scenarios.
    ///
    /// Usage in pie chart context:
    /// - Percentage: Defines what portion of the total pie this segment represents (0-100)
    /// - Color: Determines the visual appearance of the segment in the rendered chart
    ///
    /// Multiple PercentageColorData instances are typically combined in a list to create complete pie charts
    /// with multiple segments representing different categories or values.
    /// </remarks>
    [System.Serializable]
    public class PercentageColorData
    {
        /// <summary>
        /// The percentage value representing the proportional size of this pie chart segment.
        /// Should be a value between 0 and 100, representing the portion of the total pie.
        /// </summary>
        /// <remarks>
        /// This value is converted to angular measurements during chart rendering, where the total
        /// of all percentage values in a chart typically equals 100 for a complete circle.
        /// Values outside the 0-100 range are supported but may produce unexpected visual results.
        /// </remarks>
        public float Percentage;

        /// <summary>
        /// The color used to render this pie chart segment.
        /// Uses Color32 for precise color specification with alpha channel support.
        /// </summary>
        /// <remarks>
        /// Color32 provides 8-bit precision for red, green, blue, and alpha channels,
        /// offering fine-grained control over segment appearance including transparency effects.
        /// This color is applied directly during the chart rendering process to fill the segment area.
        /// </remarks>
        public Color32 Color;
    }
}