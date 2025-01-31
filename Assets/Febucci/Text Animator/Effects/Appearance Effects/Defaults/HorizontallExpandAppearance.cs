﻿using UnityEngine;

namespace Febucci.UI.Core
{
    class HorizontalExpandAppearance : AppearanceBase
    {
        //expand type
        public enum ExpType
        {
            Left, //from left to right
            Middle, //expands from the middle to te extents
            Right //from right to left
        }

        ExpType type = ExpType.Left;

        public override void SetDefaultValues(AppearanceDefaultValues data)
        {
            showDuration = data.defaults.horizontalExpandDuration;
            type = data.defaults.horizontalExpandStart;
        }

        public override void ApplyEffect(ref CharactData data, int charIndex)
        {
            Vector2 startTop;
            Vector2 startBot;

            float pct = Tween.EaseInOut(data.passedTime / showDuration);

            switch (type)
            {
                case ExpType.Left:
                    //top left and bot left
                    startTop = data.vertices[1];
                    startBot = data.vertices[0];

                    data.vertices[2] = Vector3.LerpUnclamped(startTop, data.vertices[2], pct);
                    data.vertices[3] = Vector3.LerpUnclamped(startBot, data.vertices[3], pct);
                    break;

                case ExpType.Right:
                    //top right and bot right
                    startTop = data.vertices[2];
                    startBot = data.vertices[3];

                    data.vertices[1] = Vector3.LerpUnclamped(startTop, data.vertices[1], pct);
                    data.vertices[0] = Vector3.LerpUnclamped(startBot, data.vertices[0], pct);
                    break;

                case ExpType.Middle:
                    //Middle positions
                    startTop = (data.vertices[1] + data.vertices[2]) / 2;
                    startBot = (data.vertices[0] + data.vertices[3]) / 2;

                    //top vertices
                    data.vertices[1] = Vector3.LerpUnclamped(startTop, data.vertices[1], pct);
                    data.vertices[2] = Vector3.LerpUnclamped(startTop, data.vertices[2], pct);

                    //bottom vertices
                    data.vertices[0] = Vector3.LerpUnclamped(startBot, data.vertices[0], pct);
                    data.vertices[3] = Vector3.LerpUnclamped(startBot, data.vertices[3], pct);

                    break;
            }
            
        }

    }

}