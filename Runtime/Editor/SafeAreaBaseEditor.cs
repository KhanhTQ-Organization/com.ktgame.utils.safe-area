#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace com.ktgame.utils.safe_area
{
	[CustomEditor(typeof(SafeAreaBase), true)]
	public class SafeAreaBaseEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var safeArea = target as SafeAreaBase;
			using (new EditorGUI.DisabledScope(Application.isPlaying == false))
			{
				if (GUILayout.Button("Update Rect"))
				{
					if (safeArea != null)
					{
						safeArea.UpdateRect();
					}

					SimulatorWindowProxy.Repaint();
				}
			}
		}
	}
}
#endif
