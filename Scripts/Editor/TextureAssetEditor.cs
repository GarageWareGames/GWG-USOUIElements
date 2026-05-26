
using GWG.UsoUIElements.CustomElements;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace GWG.UsoUIElements.Editor
{
    [CustomEditor(typeof(TextureAsset))]
    public class TextureAssetEditor : UnityEditor.Editor
    {
        [SerializeField]
        VisualTreeAsset m_VisualTree;

        public override VisualElement CreateInspectorGUI()
        {
            return m_VisualTree.CloneTree();
        }
    }
}