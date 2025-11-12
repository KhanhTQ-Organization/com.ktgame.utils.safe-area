#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace com.ktgame.utils.safe_area
{
	public static class SimulatorWindowEvent
	{
		public static event Action OnOpen;
		public static event Action OnClose;
		public static event Action OnFocus;
		public static event Action OnLostFocus;
		public static event Action<ScreenOrientation> OnOrientationChanged;

		private static bool _isOpen;
		private static bool _hasFocus;
		private static ScreenOrientation _orientation;

		[InitializeOnLoadMethod]
		private static void Initialize()
		{
			_isOpen = SimulatorWindowProxy.IsOpen;
			_hasFocus = SimulatorWindowProxy.HasFocus;
			_orientation = Screen.Orientation;

			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate()
		{
			if (_isOpen == false && SimulatorWindowProxy.IsOpen)
			{
				OnOpen?.Invoke();
				_isOpen = true;
			}

			if (_isOpen && SimulatorWindowProxy.IsOpen == false)
			{
				OnClose?.Invoke();
				_isOpen = false;
			}

			if ((_hasFocus == false) && SimulatorWindowProxy.HasFocus)
			{
				OnFocus?.Invoke();
				_hasFocus = true;
			}

			if (_hasFocus && (SimulatorWindowProxy.HasFocus == false))
			{
				OnLostFocus?.Invoke();
				_hasFocus = false;
			}

			if (_orientation != Screen.Orientation)
			{
				OnOrientationChanged?.Invoke(Screen.Orientation);
				_orientation = Screen.Orientation;
			}
		}
	}
}
#endif