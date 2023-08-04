using System.Linq;
using UnityEngine;

namespace COM3D2.CornerMessage
{
	public class CornerText
	{
		private readonly UILabel _screenText;

		internal string Text
		{
			get => _screenText.text;
			set => _screenText.text = value;
		}

		internal Color Color
		{
			get => _screenText.color;
			set => _screenText.color = value;
		}

		internal bool Visible => _screenText.isVisible;

		internal bool Active => _screenText.gameObject.activeInHierarchy;

		internal CornerText()
		{
			var messageWindow = GameObject.Find("SystemUI Root").GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("SystemDialog")).gameObject;

			var msgWindowFont = messageWindow.GetComponentInChildren<UILabel>().trueTypeFont;

			var uiRoot = GameObject.Find("SystemUI Root").GetComponent<UIRoot>();

			_screenText = NGUITools.AddChild<UILabel>(uiRoot.gameObject);

			var width = UIRoot.GetPixelSizeAdjustment(_screenText.gameObject) * Screen.width;
			var height = UIRoot.GetPixelSizeAdjustment(_screenText.gameObject) * Screen.height;

			_screenText.trueTypeFont = msgWindowFont;
			_screenText.transform.localPosition = new Vector3(20, height * 0.43f, 0);
			_screenText.width = (int)width;
			_screenText.fontSize = 19;
			_screenText.pivot = UIWidget.Pivot.TopLeft;
			_screenText.effectStyle = UILabel.Effect.Outline;
			_screenText.alignment = NGUIText.Alignment.Left;
			_screenText.overflowMethod = UILabel.Overflow.ResizeFreely;
			_screenText.supportEncoding = true;
			_screenText.keepCrispWhenShrunk = UILabel.Crispness.Always;
			//ScreenText.multiLine = true;

			_screenText.gameObject.SetActive(false);
		}

		internal void SetState(bool active)
		{
			_screenText.gameObject.SetActive(active);
		}
	}
}