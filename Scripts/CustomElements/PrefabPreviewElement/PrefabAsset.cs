using UnityEditor;
using UnityEngine;

namespace GWG.UsoUIElements
{
    //[CreateAssetMenu(menuName = "Garage-Ware Games/Extensions/Prefab Preview Asset")]
    public class PrefabAsset : ScriptableObject
    {
        public GameObject prefab;
        public Texture texture;


        private void OnValidate()
        {
            texture = AssetPreview.GetAssetPreview(prefab);
        }

        public void Reset()
        {
            prefab = null;
            texture  = null;
        }
    }
}