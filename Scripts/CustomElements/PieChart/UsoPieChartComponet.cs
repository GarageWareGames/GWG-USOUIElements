using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUiElements
{
    [RequireComponent(typeof(UIDocument))]
    public class UsoPieChartComponent : MonoBehaviour
    {
        UsoPieChart m_PieChart;

        void Start()
        {
            m_PieChart = new UsoPieChart();
            GetComponent<UIDocument>().rootVisualElement.Add(m_PieChart);
        }
    }
}