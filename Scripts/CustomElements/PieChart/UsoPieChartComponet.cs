using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A Unity MonoBehaviour component that manages a custom pie chart UI element.
    /// This component automatically creates and adds a UsoPieChart to the UIDocument's root visual element.
    /// </summary>
    /// <remarks>
    /// This component requires a UIDocument component to be present on the same GameObject.
    /// The pie chart is created and added to the UI hierarchy during the Start phase.
    /// </remarks>
    [RequireComponent(typeof(UIDocument))]
    public class UsoPieChartComponent : MonoBehaviour
    {
        /// <summary>
        /// The pie chart visual element instance that is managed by this component.
        /// </summary>
        /// <remarks>
        /// This field is initialized in the Start method and represents the custom UI element
        /// that will be added to the UIDocument's root visual element.
        /// </remarks>
        UsoPieChart m_PieChart;

        /// <summary>
        /// Unity's Start method called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// Creates a new UsoPieChart instance and adds it to the UIDocument's root visual element.
        /// </summary>
        /// <remarks>
        /// This method assumes that a UIDocument component is present on the same GameObject (enforced by RequireComponent attribute).
        /// The pie chart is added as a child element to the root visual element of the UI document.
        /// </remarks>
        void Start()
        {
            m_PieChart = new UsoPieChart();
            GetComponent<UIDocument>().rootVisualElement.Add(m_PieChart);
        }
    }
}