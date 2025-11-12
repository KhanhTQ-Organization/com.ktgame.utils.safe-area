using UnityEngine;
using UnityScreen = UnityEngine.Screen;

namespace com.ktgame.utils.safe_area
{
	internal static class Screen
	{
		public static int Width
		{
			get
			{
#if UNITY_EDITOR
				return ShimManagerProxy.Width;
#else
				return UnityScreen.width;
#endif
			}
		}

		public static int Height
		{
			get
			{
#if UNITY_EDITOR
				return ShimManagerProxy.Height;
#else
				return UnityScreen.height;
#endif
			}
		}

		public static float DPI => UnityScreen.dpi;

		public static Resolution CurrentResolution => UnityScreen.currentResolution;

		public static Resolution[] Resolutions => UnityScreen.resolutions;

		public static Rect SafeArea => UnityScreen.safeArea;

		public static Rect[] Cutouts => UnityScreen.cutouts;

		public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, int preferredRefreshRate = 0)
		{
			UnityScreen.SetResolution(width, height, fullscreenMode, preferredRefreshRate);
		}

		public static void SetResolution(int width, int height, bool fullscreen, int preferredRefreshRate = 0)
		{
			UnityScreen.SetResolution(width, height, fullscreen, preferredRefreshRate);
		}

		public static bool FullScreen
		{
			get => UnityScreen.fullScreen;
			set => UnityScreen.fullScreen = value;
		}

		public static FullScreenMode FullScreenMode
		{
			get => UnityScreen.fullScreenMode;
			set => UnityScreen.fullScreenMode = value;
		}

		public static bool AutorotateToPortrait
		{
			get => UnityScreen.autorotateToPortrait;
			set => UnityScreen.autorotateToPortrait = value;
		}

		public static bool AutorotateToPortraitUpsideDown
		{
			get => UnityScreen.autorotateToPortraitUpsideDown;
			set => UnityScreen.autorotateToPortraitUpsideDown = value;
		}

		public static bool AutorotateToLandscapeLeft
		{
			get => UnityScreen.autorotateToLandscapeLeft;
			set => UnityScreen.autorotateToLandscapeLeft = value;
		}

		public static bool AutorotateToLandscapeRight
		{
			get => UnityScreen.autorotateToLandscapeRight;
			set => UnityScreen.autorotateToLandscapeRight = value;
		}

		public static ScreenOrientation Orientation
		{
			get => UnityScreen.orientation;
			set => UnityScreen.orientation = value;
		}

		public static int SleepTimeout
		{
			get => UnityScreen.sleepTimeout;
			set => UnityScreen.sleepTimeout = value;
		}

		public static float Brightness
		{
			get => UnityScreen.brightness;
			set => UnityScreen.brightness = value;
		}
	}
}
