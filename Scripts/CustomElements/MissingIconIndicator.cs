using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace GWG.UsoUIElements.CustomElements
{
    /// <summary>Draws a yellow missing-image warning without an artwork asset or asset acquisition.</summary>
    /// <remarks>
    /// This generic visual owns no item, configuration, Addressables handle or persistence. Consumers
    /// bind Icon independently of their real Image and set ContentPresent only for an occupied item.
    /// A loading consumer must keep ContentPresent false until absence is established. Null textures
    /// never mutate the source record. Detachment requires no release because this visual acquires nothing.
    /// </remarks>
    public class MissingIconIndicator : VisualElement
    {
        private Texture _icon;
        private bool _contentPresent;
        /// <summary>The resolved icon; bind ToTarget to the same source as the real image.</summary>
        [CreateProperty]
        public Texture Icon { get => _icon; set { _icon = value; MarkDirtyRepaint(); } }
        /// <summary>Whether actual content exists; empty slots and unresolved loads must use false.</summary>
        [CreateProperty]
        public bool ContentPresent { get => _contentPresent; set { _contentPresent = value; MarkDirtyRepaint(); } }
        /// <summary>True only for present content with a missing or destroyed resolved texture.</summary>
        public bool IsWarningVisible => _contentPresent && _icon == null;
        /// <summary>Creates a non-picking overlay with no assets or background work.</summary>
        public MissingIconIndicator()
        {
            name = "missingIconIndicator";
            pickingMode = PickingMode.Ignore;
            style.position = Position.Absolute;
            style.left = style.right = style.top = style.bottom = 0;
            generateVisualContent += DrawWarning;
        }
        private void DrawWarning(MeshGenerationContext context)
        {
            if (!IsWarningVisible) return;
            float size = Mathf.Min(contentRect.width, contentRect.height);
            if (size < 1f) return;
            float x = contentRect.center.x, y = contentRect.center.y;
            var painter = context.painter2D;
            // Resolve the host's existing component -> semantic -> palette token chain.
            // The required yellow baseline remains usable when no host stylesheet exists.
            Color warningColor = new Color(1f, 0.75f, 0.05f, 1f);
            var warningToken = new CustomStyleProperty<Color>("--uso-warning-color");
            for (VisualElement host = this; host != null; host = host.parent)
            {
                if (host.customStyle.TryGetValue(warningToken, out Color configured))
                {
                    warningColor = configured;
                    break;
                }
            }
            painter.fillColor = warningColor;
            painter.BeginPath();
            painter.MoveTo(new Vector2(x, y - size * 0.4f));
            painter.LineTo(new Vector2(x + size * 0.43f, y + size * 0.35f));
            painter.LineTo(new Vector2(x - size * 0.43f, y + size * 0.35f));
            painter.ClosePath(); painter.Fill();
            painter.strokeColor = Color.black;
            painter.lineWidth = Mathf.Max(1f, size * 0.075f);
            painter.BeginPath();
            painter.MoveTo(new Vector2(x, y - size * 0.13f));
            painter.LineTo(new Vector2(x, y + size * 0.09f)); painter.Stroke();
            painter.BeginPath();
            painter.MoveTo(new Vector2(x, y + size * 0.18f));
            painter.LineTo(new Vector2(x, y + size * 0.23f)); painter.Stroke();
        }
    }
}
