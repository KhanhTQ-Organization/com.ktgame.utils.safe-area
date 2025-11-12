#if UNITY_EDITOR
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace com.ktgame.utils.safe_area
{
	public static class SimulatorWindowProxy
	{
		private const string AssemblyName = "UnityEditor.DeviceSimulatorModule";
		private const string SimulatorWindowTypeName = "UnityEditor.DeviceSimulation.SimulatorWindow";

		private static readonly Type SimulatorWindow;
		private static readonly FieldInfo PlayModeViewsFieldInfo;
		private static readonly MethodInfo RepaintImmediatelyMethodInfo;
		private static bool _shouldBeRepaint;

		public static bool IsOpen { get; private set; }

		public static bool HasFocus => (IsOpen && EditorWindow.focusedWindow && (EditorWindow.focusedWindow.GetType() == SimulatorWindow));

		public static void RepaintWithDelay() => _shouldBeRepaint = true;

		static SimulatorWindowProxy()
		{
			SimulatorWindow = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => assembly.GetName().Name == AssemblyName)
				.Select(assembly => assembly.GetType(SimulatorWindowTypeName))
				.First();

			PlayModeViewsFieldInfo = Assembly.Load("UnityEditor.dll")
				.GetType("UnityEditor.PlayModeView")
				.GetField("s_PlayModeViews", BindingFlags.Static | BindingFlags.NonPublic);

			RepaintImmediatelyMethodInfo = typeof(EditorWindow).GetMethod("RepaintImmediately", BindingFlags.Instance | BindingFlags.NonPublic);

			EditorApplication.update -= OnUpdate;
			EditorApplication.update += OnUpdate;
		}

		private static void OnUpdate()
		{
			var playModeViews = (IEnumerable)PlayModeViewsFieldInfo.GetValue(null);

			IsOpen = false;

			foreach (var playModeView in playModeViews)
			{
				if ((UnityEngine.Object)playModeView == null)
				{
					continue;
				}

				if (playModeView.GetType() != SimulatorWindow)
				{
					continue;
				}

				IsOpen = true;
				break;
			}

			if (_shouldBeRepaint)
			{
				Repaint();
			}
		}

		public static void Repaint()
		{
			if (IsOpen == false)
			{
				return;
			}

			var playModeViews = (IEnumerable)PlayModeViewsFieldInfo.GetValue(null);

			foreach (EditorWindow playModeView in playModeViews)
			{
				if (playModeView == null)
				{
					continue;
				}

				if (playModeView.GetType() != SimulatorWindow)
				{
					continue;
				}

				RepaintImmediately(playModeView);
			}
		}

		private static void RepaintImmediately(EditorWindow window)
		{
			if (window == null)
			{
				return;
			}

			RepaintImmediatelyMethodInfo.Invoke(window, null);
		}
	}
}
#endif
