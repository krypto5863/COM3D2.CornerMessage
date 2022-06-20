using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using UnityEngine;

//These two lines tell your plugin to not give a flying fuck about accessing private variables/classes whatever. It requires a publicized stubb of the library with those private objects though.
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace COM3D2.CornerMessage
{
	//This is the metadata set for your plugin.
	[BepInPlugin("COM3D2.CornerMessage", "COM3D2.CornerMessage", "1.0")]
	public class Main : BaseUnityPlugin
	{
		//static saving of the main instance. This makes it easier to run stuff like coroutines from static methods or accessing non-static vars.
		public static Main @this;

		//Static var for the logger so you can log from other classes.
		public static ManualLogSource logger;

		//Config entry variable. You set your configs to this.
		//internal static ConfigEntry<bool> ExampleConfig;

		public static CornerText mainCornerText;
		public static IEnumerator MessageManager;

		private static Dictionary<string, float> MessagesDuration = new Dictionary<string, float>();

		private void Awake()
		{
			//Useful for engaging coroutines or accessing variables non-static variables. Completely optional though.
			@this = this;

			//pushes the logger to a public static var so you can use the bepinex logger from other classes.
			logger = Logger;

			//Installs the patches in the Main class.
			//Harmony.CreateAndPatchAll(typeof(Main));

			mainCornerText = new CornerText();
			mainCornerText.SetState(false);
			mainCornerText.Color = Color.white;
		}

		public static void DisplayMessage(string message, float mesgDur = 6f) 
		{
			message = message.Trim();

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			MessagesDuration[message] = mesgDur;

			UpdateText();

			mainCornerText.SetState(true);

			if (MessageManager == null) 
			{
				MessageManager = ManageMessage();
				Main.@this.StartCoroutine(MessageManager);
			}
		}

		public static IEnumerator ManageMessage() 
		{
			while (true)
			{
				yield return new WaitForSecondsRealtime(1f);

				if (mainCornerText.visible == true && mainCornerText.active && GameMain.Instance.MainCamera.IsFadeStateNon())
				{

					bool updateText = false;

					foreach (string text in MessagesDuration.Keys.ToArray())
					{
						if (MessagesDuration[text] > 1)
						{
							MessagesDuration[text]--;
						}
						else
						{
							MessagesDuration.Remove(text);
							updateText = true;
						}
					}

					if (updateText)
					{
						UpdateText();
					}
				}

				if (MessagesDuration.Count <= 0)
				{
					mainCornerText.SetState(false);
					MessageManager = null;
					yield break;
				}
			}
		}

		public static void UpdateText() 
		{
			mainCornerText.Text = "";

			foreach (var keyVal in MessagesDuration) 
			{
				mainCornerText.Text = mainCornerText.Text + keyVal.Key + "\n";
			}
		}
	}
}
