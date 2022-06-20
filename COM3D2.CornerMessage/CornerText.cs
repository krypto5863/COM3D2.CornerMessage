using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.CornerMessage
{
	public class CornerText
	{
		private UILabel ScreenText;
		internal string Text 
		{
			get => ScreenText.text;
			set 
			{
				ScreenText.text = value;
			}
		}
		internal Color Color
		{
			get => ScreenText.color;
			set
			{
				ScreenText.color = value;
			}
		}

		internal bool visible
		{
			get => ScreenText.isVisible;
		}

		internal bool active
		{
			get => ScreenText.gameObject.activeInHierarchy;
		}
		internal CornerText() 
		{
			var MessageWindow = GameObject.Find("SystemUI Root").GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("SystemDialog")).gameObject;

			var MsgWindowFont = MessageWindow.GetComponentInChildren<UILabel>().trueTypeFont;

			var UIRoot = GameObject.Find("SystemUI Root").GetComponent<UIRoot>();

			ScreenText = NGUITools.AddChild<UILabel>(UIRoot.gameObject);

			var width = UIRoot.GetPixelSizeAdjustment(ScreenText.gameObject) * Screen.width;
			var height = UIRoot.GetPixelSizeAdjustment(ScreenText.gameObject) * Screen.height;

			ScreenText.trueTypeFont = MsgWindowFont;
			ScreenText.transform.localPosition = new Vector3(20, height * 0.43f, 0);
			ScreenText.width = (int)width;
			ScreenText.fontSize = 19;
			ScreenText.pivot = UIWidget.Pivot.TopLeft;
			ScreenText.effectStyle = UILabel.Effect.Outline;
			ScreenText.alignment = NGUIText.Alignment.Left;
			ScreenText.overflowMethod = UILabel.Overflow.ResizeFreely;
			ScreenText.supportEncoding = true;
			ScreenText.keepCrispWhenShrunk = UILabel.Crispness.Always;
			//ScreenText.multiLine = true;

			ScreenText.gameObject.SetActive(false);
		}
		internal void SetState(bool active) 
		{
			ScreenText.gameObject.SetActive(active);
		}
	}
}
