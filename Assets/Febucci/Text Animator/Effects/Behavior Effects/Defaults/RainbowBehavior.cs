﻿using UnityEngine;

namespace Febucci.UI.Core
{
    class RainbowBehavior : BehaviorBase
    {
        float hueShiftSpeed = 0.8f;
        float hueShiftWaveSize = 0.08f;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            hueShiftSpeed = data.defaults.hueShiftSpeed;
            hueShiftWaveSize = data.defaults.hueShiftWaveSize;
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //frequency
                case "f": ApplyModifierTo(ref hueShiftSpeed, modifierValue); break;
                //wave size
                case "s": ApplyModifierTo(ref hueShiftWaveSize, modifierValue); break;
            }
        }

        public override void ApplyEffect(ref CharactData data, int charIndex)
        {
            for (byte i = 0; i < data.colors.Length; i++)
            {
                data.colors[i] = Color.HSVToRGB(Mathf.PingPong(animatorTime * hueShiftSpeed + charIndex * hueShiftWaveSize, 1), 1, 1);
            }
        }


        public override string ToString()
        {
            return $"hueShiftSpeed: {hueShiftSpeed}\n" +
                $"hueShiftWaveSize: {hueShiftWaveSize}" +
                $"\n{base.ToString()}";
        }

    }
}