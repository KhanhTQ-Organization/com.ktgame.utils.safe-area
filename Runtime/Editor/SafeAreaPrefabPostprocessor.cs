#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace com.ktgame.utils.safe_area
{
	public class SafeAreaPrefabPostprocessor : AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			foreach (var path in importedAssets)
			{
				if (Path.GetExtension(path) != ".prefab")
				{
					continue;
				}

				var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				var isDirty = false;

				foreach (var safeArea in prefab.GetComponentsInChildren<ISafeAreaUpdatable>(true))
				{
					safeArea.ResetRect();
					isDirty = true;
				}

				if (isDirty)
				{
					EditorUtility.SetDirty(prefab);
					AssetDatabase.SaveAssetIfDirty(prefab);
				}
			}
		}
	}
}
#endif
