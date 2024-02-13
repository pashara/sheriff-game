using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore;

namespace Fonts
{
    [CreateAssetMenu]
    public class FontConfigCopyHandler : ScriptableObject
    {
        [SerializeField] private TMP_FontAsset assetSource;
        [SerializeField] private TMP_FontAsset destination;

        [SerializeField] private List<Glyph> glyphs;

        [Button]
        private void Apply()
        {
            if (destination == null)
                return;

            var element = destination;
            foreach (var glyph in glyphs)
            {
                var index = element.glyphTable.FindIndex(x => x.index == glyph.index);
                if (index == -1)
                    continue;
                
                var g = new Glyph(element.glyphTable[index]);
                g.metrics = new GlyphMetrics(glyph.metrics.width, glyph.metrics.height,
                    glyph.metrics.horizontalBearingX, glyph.metrics.horizontalBearingY,
                    glyph.metrics.horizontalAdvance);


                element.glyphTable[index] = g;
            }
        }

        [Button]
        void ReadConfig()
        {
            if (assetSource != null) 
                glyphs = assetSource.glyphTable.Select(x => new Glyph(x)).ToList();
        }
    }
}
