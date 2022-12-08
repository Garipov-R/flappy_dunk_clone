﻿using UnityEngine;

namespace Client.Utilities
{
	public class TimeUtility
	{
		private const int c_TargetFramerate = 60;

		public static float FramerateDeltaTime => Time.deltaTime * 60f;

		public static float DeltaTimeScaled => Time.deltaTime * Time.timeScale;
	}
}
