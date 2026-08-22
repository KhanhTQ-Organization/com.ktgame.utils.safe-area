using UnityEngine;

namespace com.ktgame.utils.safe_area
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ISafeAreaUpdatable))]
    public class RuntimeSafeAreaUpdater : MonoBehaviour
    {
        private ISafeAreaUpdatable _target;
        private Rect _safeArea;

        private void Start()
        {
            _target = GetComponent<ISafeAreaUpdatable>();
            _safeArea = Screen.SafeArea;
            _target.UpdateRect();
        }

        private bool _lastKeyboardVisible;
        private float _lastKeyboardHeight;

        private void Update()
        {
            bool needsUpdate = false;

            if (_safeArea != Screen.SafeArea)
            {
                _safeArea = Screen.SafeArea;
                needsUpdate = true;
            }

#if UNITY_ANDROID || UNITY_IOS
            bool isKbVisible = TouchScreenKeyboard.visible;
            float kbHeight = isKbVisible ? TouchScreenKeyboard.area.height : 0f;

            if (_lastKeyboardVisible != isKbVisible || Mathf.Abs(_lastKeyboardHeight - kbHeight) > 1f)
            {
                _lastKeyboardVisible = isKbVisible;
                _lastKeyboardHeight = kbHeight;
                needsUpdate = true;
            }
#endif

            if (needsUpdate)
            {
                _target.UpdateRect();
            }
        }
    }
}
