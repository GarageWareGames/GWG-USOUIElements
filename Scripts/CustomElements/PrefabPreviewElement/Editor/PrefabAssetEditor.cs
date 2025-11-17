using GWG.EditorExtensions;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace GWG.UsoUIElements.Editor
{
    /// <summary>
    /// Custom editor for the PrefabAsset ScriptableObject type.1
    /// </summary>
    [CustomEditor(typeof(PrefabAsset))]
    public class PrefabAssetEditor : UnityEditor.Editor
    {
        [SerializeField]
        VisualTreeAsset m_VisualTree;
        /// <summary>
        /// Creates the inspector GUI for the PrefabAsset custom editor.
        /// </summary>
        /// <returns>
        /// The VisualElement tree representing the PrefabAsset inspector UI.
        /// </returns>
        public override VisualElement CreateInspectorGUI()
        {
            return m_VisualTree.CloneTree();
        }
    }
}