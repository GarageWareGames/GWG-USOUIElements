using UnityEditor;
using UnityEngine;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>
    /// A ScriptableObject that stores a prefab reference along with its automatically generated preview texture.
    /// This asset provides a convenient way to bundle a GameObject prefab with its visual representation for use in custom UI elements or editor tools.
    /// </summary>
    /// <remarks>
    /// The texture is automatically updated whenever the prefab field changes through the OnValidate method.
    /// The CreateAssetMenu attribute is commented out but can be enabled to allow creation through the Unity context menu.
    /// This class is particularly useful for custom UI elements that need to display prefab previews.
    /// </remarks>
    //[CreateAssetMenu(menuName = "Garage-Ware Games/Extensions/Prefab Preview Asset")]
    public class PrefabAsset : ScriptableObject
    {
        /// <summary>
        /// The GameObject prefab that this asset represents.
        /// </summary>
        /// <remarks>
        /// When this field is assigned or changed, the OnValidate method will automatically generate
        /// a preview texture for the prefab and assign it to the texture field.
        /// </remarks>
        public GameObject prefab;

        /// <summary>
        /// The automatically generated preview texture for the associated prefab.
        /// </summary>
        /// <remarks>
        /// This texture is populated automatically by the OnValidate method using Unity's AssetPreview.GetAssetPreview.
        /// The texture represents a visual preview of how the prefab appears and is updated whenever the prefab field changes.
        /// </remarks>
        public Texture texture;

        /// <summary>
        /// Unity callback method invoked when the ScriptableObject is validated, typically when values change in the inspector.
        /// Automatically generates and assigns a preview texture for the current prefab.
        /// </summary>
        /// <remarks>
        /// This method uses Unity's AssetPreview.GetAssetPreview to generate a visual representation of the prefab.
        /// It is called automatically by Unity when the object is modified, ensuring the texture stays synchronized with the prefab.
        /// The method will generate a texture even if the prefab is null, in which case the texture will also be null.
        /// </remarks>
        private void OnValidate()
        {
            texture = AssetPreview.GetAssetPreview(prefab);
        }

        /// <summary>
        /// Resets both the prefab and texture fields to null, clearing all data from this asset.
        /// </summary>
        /// <remarks>
        /// This method provides a clean way to clear the asset's contents, setting both the prefab reference
        /// and its associated preview texture to null. This can be useful for reusing the asset or clearing invalid data.
        /// </remarks>
        public void Reset()
        {
            prefab = null;
            texture  = null;
        }
    }
}