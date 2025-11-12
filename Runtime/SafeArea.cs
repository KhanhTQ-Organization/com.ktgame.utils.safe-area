using System;
using UnityEngine;

namespace com.ktgame.utils.safe_area
{
    public sealed class SafeArea : SafeAreaBase
    {
        [Flags]
        public enum PaddingType
        {
            Top = 1 << 0,
            Bottom = 1 << 1,
            Left = 1 << 2,
            Right = 1 << 3,
        }

        [SerializeField, EnumFlags] private PaddingType _padding = (PaddingType)Enum.Parse(typeof(PaddingType), (-1).ToString());

        public PaddingType Padding
        {
            get => _padding;
            set => _padding = value;
        }

        public override void ResetRect()
        {
            base.ResetRect();
            RectTransform.anchorMin = Vector2.zero;
            RectTransform.anchorMax = Vector2.one;
        }

        public override void UpdateRect(Rect safeArea, int width, int height)
        {
            if ((safeArea.width == width) && (safeArea.height == height))
            {
                ResetRect();
                return;
            }

            var paddingTop = 0f;
            var paddingRight = 0f;
            var paddingLeft = 0f;
            var paddingBottom = 0f;

            if (Padding.HasFlag(PaddingType.Top))
            {
                paddingTop = height - (safeArea.height + safeArea.y);
            }

            if (Padding.HasFlag(PaddingType.Right))
            {
                paddingRight = width - (safeArea.width + safeArea.x);
            }

            if (Padding.HasFlag(PaddingType.Bottom))
            {
                paddingBottom = safeArea.y;
            }

            if (Padding.HasFlag(PaddingType.Left))
            {
                paddingLeft = safeArea.x;
            }

            RectTransform.sizeDelta = RectTransform.anchoredPosition = Vector3.zero;
            RectTransform.anchorMin = new Vector2(paddingLeft / width, paddingBottom / height);
            RectTransform.anchorMax = new Vector2((width - paddingRight) / width, (height - paddingTop) / height);
        }
    }
}
