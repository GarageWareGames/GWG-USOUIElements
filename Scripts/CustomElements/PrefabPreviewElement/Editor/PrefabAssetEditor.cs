using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;

namespace GWG.UsoUIElements
{
    [CustomEditor(typeof(PrefabAsset))]
    public class PrefabAssetEditor : UnityEditor.Editor
    {
        [SerializeField]
        VisualTreeAsset m_VisualTree;

        public override VisualElement CreateInspectorGUI()
        {
            return m_VisualTree.CloneTree();
        }
    }
}