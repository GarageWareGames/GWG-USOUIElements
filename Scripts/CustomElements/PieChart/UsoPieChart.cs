using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements
{
    /// <summary>
    /// Creates a pie chart based ont he supplied list of values and colors.
    /// Call the UpdateChartData method to update the chart data.
    /// using a list of PercentageColorData objects.
    /// Which are formatted as a percentage and a color.
    /// Example:
    /// PercentageColorData data = new PercentageColorData();
    /// data.Percentage = 40.0f;
    /// data.Color = new Color32(182, 235, 122, 255);
    /// ...
    /// UpdateChartData(new List<PercentageColorData> { data });
    ///
    /// </summary>
    public class UsoPieChart : VisualElement
    {
        float m_Radius = 100.0f;
        float m_Value = 40.0f;

        VisualElement m_Chart;

        public List<PercentageColorData> percentageColorData = new List<PercentageColorData>
        {
            new PercentageColorData { Percentage = 40.0f, Color = new Color32(182, 235, 122, 255) },
            new PercentageColorData { Percentage = 60.0f, Color = new Color32(251, 120, 19, 255) }
        };

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

        public float diameter => m_Radius * 2.0f;

        public float value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                MarkDirtyRepaint();
            }
        }

        public UsoPieChart()
        {
            generateVisualContent += DrawCanvas;
        }

        public void UpdateChartData(List<PercentageColorData> newData)
        {
            percentageColorData = newData;
            MarkDirtyRepaint();
        }

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

    [System.Serializable]
    public class PercentageColorData
    {
        public float Percentage;
        public Color32 Color;
    }
}