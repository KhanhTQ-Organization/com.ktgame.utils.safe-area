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

        private void Update()
        {
            if (_safeArea == Screen.SafeArea)
            {
                return;
            }

            _safeArea = Screen.SafeArea;
            _target.UpdateRect();
        }
    }
}
