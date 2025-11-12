#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityScreen = UnityEngine.Screen;

namespace com.ktgame.utils.safe_area
{
	internal static class ShimManagerEvent
	{
		public static event Action OnActiveShimChanged;

		private static object _activeScreenShim;

		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			_activeScreenShim = ShimManagerProxy.GetActiveScreenShim();
			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate()
		{
			var currentActiveScreenShim = ShimManagerProxy.GetActiveScreenShim();
			if (_activeScreenShim != currentActiveScreenShim)
			{
				_activeScreenShim = currentActiveScreenShim;
				OnActiveShimChanged?.Invoke();
			}
		}
	}
}
#endif
