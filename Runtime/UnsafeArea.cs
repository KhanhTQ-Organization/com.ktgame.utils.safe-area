using UnityEngine;

namespace com.ktgame.utils.safe_area
{
	public sealed class UnsafeArea : SafeAreaBase
	{
		public enum PositionType
		{
			Top,
			Bottom,
			Left,
			Right,
		}

		[SerializeField] private PositionType _position;

		public PositionType Position
		{
			get => _position;
			set => _position = value;
		}

		public override void UpdateRect(Rect safeArea, int width, int height)
		{
			if ((safeArea.width == width) && (safeArea.height == height))
			{
				ResetRect();
				return;
			}

			var anchorMin = Vector2.zero;
			var anchorMax = Vector2.one;

			switch (Position)
			{
				case PositionType.Top:
					anchorMin = new Vector2(0, safeArea.height + safeArea.y) / height;
					break;
				case PositionType.Bottom:
					anchorMax = new Vector2(1, safeArea.y / height);
					break;
				case PositionType.Left:
					anchorMax = new Vector2(safeArea.x / width, 1);
					break;
				case PositionType.Right:
					anchorMin = new Vector2(safeArea.width + safeArea.x, 0) / width;
					break;
			}

			RectTransform.anchorMin = anchorMin;
			RectTransform.anchorMax = anchorMax;
			RectTransform.anchoredPosition = Vector3.zero;
			RectTransform.sizeDelta = Vector2.zero;
		}
	}
}
