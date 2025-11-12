#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityScreen = UnityEngine.Screen;

namespace com.ktgame.utils.safe_area
{
	internal static class ShimManagerProxy
	{
		private const string AssemblyName = "UnityEditor.DeviceSimulatorModule";

		private static readonly Type ShimManagerType = Assembly.Load("UnityEngine.dll").GetType("UnityEngine.ShimManager");

		private static readonly FieldInfo ActiveScreenShimFieldInfo =
			ShimManagerType.GetField("s_ActiveScreenShim", BindingFlags.Static | BindingFlags.NonPublic);

		private static readonly PropertyInfo WidthPropertyInfo;
		private static readonly PropertyInfo HeightPropertyInfo;

		static ShimManagerProxy()
		{
			var screenSimulationType = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => assembly.GetName().Name == AssemblyName)
				.Select(assembly => assembly.GetType("UnityEditor.DeviceSimulation.ScreenSimulation"))
				.First();

			WidthPropertyInfo = screenSimulationType.GetProperty("width");
			HeightPropertyInfo = screenSimulationType.GetProperty("height");
		}

		public static object GetActiveScreenShim()
		{
			var activeScreenShim = ActiveScreenShimFieldInfo.GetValue(null);
			if (activeScreenShim is System.Collections.IEnumerable enumerable)
			{
				var enumerator = enumerable.GetEnumerator();
				while (enumerator.MoveNext())
				{
					activeScreenShim = enumerator.Current;
					if (activeScreenShim != null && activeScreenShim.GetType().Name == "ScreenSimulation")
					{
						break;
					}
				}
			}
			return activeScreenShim;
		}

		public static int Width
		{
			get
			{
				var activeScreenShim = GetActiveScreenShim();
				if (activeScreenShim == null)
				{
					return UnityScreen.width;
				}
				
				return activeScreenShim.GetType().GetProperty("width")?.GetValue(activeScreenShim) as int? ?? UnityScreen.width;
			}
		}

		public static int Height
		{
			get
			{
				var activeScreenShim = GetActiveScreenShim();
				if (activeScreenShim == null)
				{
					return UnityScreen.height;
				}

				return activeScreenShim.GetType().GetProperty("height")?.GetValue(activeScreenShim) as int? ?? UnityScreen.height;
			}
		}
	}
}
#endif
