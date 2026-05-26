using UnityEngine;

namespace GWG.UsoUIElements.CustomElements
{
    [CreateAssetMenu(menuName = "Garage-Ware Games/Extensions/Texture Asset")]
    public class TextureAsset : ScriptableObject
    {
        public Texture2D texture;

        public void Reset()
        {
            texture = null;
        }
    }
}