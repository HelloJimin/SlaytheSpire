﻿using UnityEngine;

namespace Febucci.UI.Core.Examples
{
    //Documentation link: https://www.febucci.com/textanimator-docs
    //Example class
    public class AppearanceTemplate : AppearanceBase
    {
        //float verticalOffset;

        Vector3 vert;
        float pct;

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            //verticalOffset = data.customs.templateOffset;
        }

        public override void ApplyEffect(ref CharactData data, int charIndex)
        {
            /*
            pct = charPCTs[charIndex] / showDuration;

            for (int i = 0; i < data.vertices.Length; i++)
            {
                //copies the current char position (e.g. if the character is waving etc)
                vert = data.vertices[i];

                //moves it vertically
                vert += Vector3.up * verticalOffset * effectIntensity;

                //Sets the vertex,
                //From "big" to "normal", with time
                data.vertices[i] = Vector3.Lerp(
                    vert,
                    data.vertices[i],
                    Tween.EaseInOut(pct));
            }
            */
        }
    }

}