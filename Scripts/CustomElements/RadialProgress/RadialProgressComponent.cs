using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A Unity MonoBehaviour component that creates and manages a RadialProgress UI element with animated progress values.
    /// This component demonstrates the usage of the RadialProgress custom element by creating it programmatically and animating its progress using a sine wave function.
    /// </summary>
    /// <remarks>
    /// This component requires a UIDocument component to be present on the same GameObject to provide the UI root element.
    /// The RadialProgress element is positioned absolutely at coordinates (20, 20) with a fixed size of 200x200 pixels.
    /// The progress animation creates a smooth oscillating effect that varies between 10% and 70% progress based on the current time.
    /// This serves as both a functional component and a demonstration of how to integrate custom UI elements into Unity scenes.
    /// </remarks>
    [RequireComponent(typeof(UIDocument))]
    public class RadialProgressComponent : MonoBehaviour
    {
        /// <summary>
        /// The RadialProgress UI element instance managed by this component.
        /// </summary>
        /// <remarks>
        /// This field stores the reference to the custom RadialProgress element that is created and added to the UI hierarchy.
        /// The progress value of this element is continuously updated in the Update method to create the animation effect.
        /// </remarks>
        RadialProgress m_RadialProgress;

        /// <summary>
        /// Unity's Start method called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// Creates a new RadialProgress element, configures its styling and position, and adds it to the UIDocument's root visual element.
        /// </summary>
        /// <remarks>
        /// This method assumes that a UIDocument component is present on the same GameObject (enforced by RequireComponent attribute).
        /// The RadialProgress element is created with absolute positioning at coordinates (20, 20) and sized to 200x200 pixels.
        /// The styling is applied inline using the style property for immediate configuration without requiring external stylesheets.
        /// </remarks>
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            m_RadialProgress = new RadialProgress()
            {
                style =
                {
                    position = Position.Absolute,
                    left = 20,
                    top = 20,
                    width = 200,
                    height = 200
                }
            };

            root.Add(m_RadialProgress);
        }

        /// <summary>
        /// Unity's Update method called once per frame to continuously animate the progress value of the RadialProgress element.
        /// Updates the progress using a sine wave function to create a smooth oscillating animation between 10% and 70% progress.
        /// </summary>
        /// <remarks>
        /// The animation uses Time.time to create a time-based sine wave that oscillates between -1 and 1.
        /// This value is normalized to a 0-1 range, scaled to a 60% range (0-60), and offset by 10% to create the final 10-70% range.
        /// The resulting animation provides a visually appealing demonstration of the RadialProgress element's capabilities.
        /// The update frequency matches Unity's frame rate, typically 60 FPS, providing smooth animation.
        /// </remarks>
        void Update()
        {
            m_RadialProgress.progress = ((Mathf.Sin(Time.time) + 1.0f) / 2.0f) * 60.0f + 10.0f;
        }
    }
}