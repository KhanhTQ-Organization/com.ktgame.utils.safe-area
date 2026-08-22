using UnityEngine;

namespace com.ktgame.utils.safe_area
{
    /// <summary>
    /// Gắn Script này vào Canvas. Nó sẽ tự động tạo một lớp Bảo vệ (Safe Area Container)
    /// và gom toàn bộ UI của bạn vào trong đó. Bạn không cần phải gắn SafeArea cho từng Panel nữa.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class SafeAreaCanvas : MonoBehaviour
    {
        [Tooltip("Tự động áp dụng Safe Area cho toàn bộ UI con")]
        public bool ApplyGlobally = true;

        [Tooltip("Bật tính năng né bàn phím ảo cho toàn bộ Canvas")]
        public bool AvoidKeyboard = true;

        private void Awake()
        {
            if (!ApplyGlobally) return;

            // Tìm xem đã có container chưa để tránh tạo trùng lặp
            Transform existingContainer = transform.Find("GlobalSafeAreaContainer");
            if (existingContainer != null) return;

            // 1. Tạo một GameObject mới làm Container
            GameObject containerGo = new GameObject("GlobalSafeAreaContainer");
            RectTransform containerRect = containerGo.AddComponent<RectTransform>();
            containerGo.transform.SetParent(transform, false);

            // Căng full màn hình lúc ban đầu
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchoredPosition = Vector2.zero;

            // 2. Gom toàn bộ UI hiện tại (trừ chính cái container) chui vào trong container này
            int childCount = transform.childCount;
            // Lặp ngược để không bị lỗi khi đổi Parent
            for (int i = childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                if (child != containerGo.transform)
                {
                    child.SetParent(containerGo.transform, false);
                    child.SetAsFirstSibling(); // Giữ nguyên thứ tự UI (Render Order)
                }
            }

            // 3. Gắn vũ khí SafeArea vào Container
            SafeArea safeArea = containerGo.AddComponent<SafeArea>();
            safeArea.Padding = SafeArea.PaddingType.Top | SafeArea.PaddingType.Bottom | SafeArea.PaddingType.Left | SafeArea.PaddingType.Right;
            
            // Dùng Reflection để set trường _avoidKeyboard (Vì nó là private field mình vừa tạo ở bước trước)
            var avoidField = typeof(SafeArea).GetField("_avoidKeyboard", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (avoidField != null)
            {
                avoidField.SetValue(safeArea, AvoidKeyboard);
            }

            // 4. Gắn cục Updater để nó tự động lắng nghe xoay màn hình & bàn phím
            containerGo.AddComponent<RuntimeSafeAreaUpdater>();
        }
    }
}
