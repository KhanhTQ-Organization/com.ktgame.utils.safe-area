#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
#endif

using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.ktgame.utils.safe_area
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public abstract class SafeAreaBase : MonoBehaviour, ISafeAreaUpdatable
	{
		private RectTransform _rectTransform;

		protected RectTransform RectTransform => (_rectTransform != null) ? _rectTransform : _rectTransform = GetComponent<RectTransform>();

		private void Reset()
		{
			ResetRect();
			UpdateRect();

			if (GetComponent<RuntimeSafeAreaUpdater>() == false)
			{
				gameObject.AddComponent<RuntimeSafeAreaUpdater>();
			}
		}

		private void Start()
		{
#if UNITY_EDITOR
			SetDirty();
#else
            UpdateRect();
#endif
		}

		public virtual void ResetRect()
		{
			RectTransform.sizeDelta = Vector3.zero;
			RectTransform.anchoredPosition = Vector3.zero;
			RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
			RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
			RectTransform.pivot = new Vector2(0.5f, 0.5f);
			RectTransform.localRotation = Quaternion.identity;
			RectTransform.localScale = Vector3.one;
		}

		public void UpdateRect()
		{
			UpdateRect(Screen.SafeArea, Screen.Width, Screen.Height);
		}

		public abstract void UpdateRect(Rect safeArea, int width, int height);

#if UNITY_EDITOR
		private bool _isDirty;

		private void LockRect() => RectTransform.hideFlags = HideFlags.NotEditable;

		private void UnlockRect() => RectTransform.hideFlags = HideFlags.None;

		private void OnEnable()
		{
			SimulatorWindowEvent.OnOpen += SetDirty;
			SimulatorWindowEvent.OnClose += SetDirty;
			SimulatorWindowEvent.OnOrientationChanged += OnOrientationChanged;
			ShimManagerEvent.OnActiveShimChanged += SetDirty;
			EditorSceneManager.sceneSaving += OnSceneSaving;
			EditorSceneManager.sceneSaved += OnSceneSaved;
			PrefabStage.prefabSaving += OnPrefabSaving;
			PrefabStage.prefabSaved += OnPrefabSaved;

			LockRect();
			TryUpdateRect();
		}

		private void OnDisable()
		{
			SimulatorWindowEvent.OnOpen -= SetDirty;
			SimulatorWindowEvent.OnClose -= SetDirty;
			SimulatorWindowEvent.OnOrientationChanged -= OnOrientationChanged;
			ShimManagerEvent.OnActiveShimChanged -= SetDirty;
			EditorSceneManager.sceneSaving -= OnSceneSaving;
			EditorSceneManager.sceneSaved -= OnSceneSaved;
			PrefabStage.prefabSaving -= OnPrefabSaving;
			PrefabStage.prefabSaved -= OnPrefabSaved;

			UnlockRect();
		}

		private void OnOrientationChanged(ScreenOrientation orientation)
		{
			if (EditorApplication.isPlaying == false)
			{
				SetDirty();
			}
		}

		private void OnSceneSaving(Scene scene, string path) => TryResetRect();
		private void OnSceneSaved(Scene scene) => TryUpdateRect();
		private void OnPrefabSaving(GameObject prefabContentsRoot) => TryResetRect();
		private void OnPrefabSaved(GameObject prefabContentsRoot) => TryUpdateRect();

		private void TryResetRect()
		{
			if (RectTransform)
			{
				ResetRect();
			}
		}

		private void TryUpdateRect()
		{
			if (RectTransform)
			{
				UpdateRect();
			}
		}

		private void SetDirty() => _isDirty = true;

		private void OnValidate()
		{
			if (EditorApplication.isPlaying == false)
			{
				SetDirty();
			}
		}

		private void OnGUI()
		{
			EditorApplication.delayCall += () =>
			{
				if (_isDirty == false)
				{
					return;
				}

				_isDirty = false;
				TryUpdateRect();

				if (EditorApplication.isPlaying == false)
				{
					SimulatorWindowProxy.RepaintWithDelay();
				}
			};
		}
#endif
	}
}
