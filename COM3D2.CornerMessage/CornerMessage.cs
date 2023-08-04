using BepInEx;
using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using JetBrains.Annotations;
using UnityEngine;

//These two lines tell your plugin to not give a flying fuck about accessing private variables/classes whatever. It requires a publicized stub of the library with those private objects though.
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace COM3D2.CornerMessage
{
	//This is the metadata set for your plugin.
	[BepInPlugin("COM3D2.CornerMessage", "COM3D2.CornerMessage", "1.0")]
	public class CornerMessage : BaseUnityPlugin
	{
		//static saving of the main Instance. This makes it easier to run stuff like co-routines from static methods or accessing non-static vars.
		public static CornerMessage Instance;

		//Static var for the PluginLogger so you can log from other classes.
		public static ManualLogSource PluginLogger => Instance.Logger;

		//Config entry variable. You set your configs to Instance.
		//internal static ConfigEntry<bool> ExampleConfig;

		public static CornerText MainCornerText;
		public static IEnumerator MessageManager;

		private static readonly Dictionary<string, float> MessagesDuration = new Dictionary<string, float>();

		[UsedImplicitly]
		private void Awake()
		{
			//Useful for engaging co-routines or accessing variables non-static variables. Completely optional though.
			Instance = this;

			//Installs the patches in the CornerMessage class.
			//Harmony.CreateAndPatchAll(typeof(CornerMessage));

			MainCornerText = new CornerText();
			MainCornerText.SetState(false);
			MainCornerText.Color = Color.white;
		}

		public static void DisplayMessage(string message, float messageDuration = 6f)
		{
			message = message.Trim();

			if (string.IsNullOrEmpty(message))
			{
				return;
			}

			MessagesDuration[message] = messageDuration;

			UpdateText();

			MainCornerText.SetState(true);

			if (MessageManager == null)
			{
				MessageManager = ManageMessage();
				Instance.StartCoroutine(MessageManager);
			}
		}

		public static IEnumerator ManageMessage()
		{
			while (true)
			{
				yield return new WaitForSecondsRealtime(1f);

				if (MainCornerText.Visible && MainCornerText.Active && GameMain.Instance.MainCamera.IsFadeStateNon())
				{
					var updateText = false;

					foreach (var text in MessagesDuration.Keys.ToArray())
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
					MainCornerText.SetState(false);
					MessageManager = null;
					yield break;
				}
			}
		}

		public static void UpdateText()
		{
			MainCornerText.Text = "";

			foreach (var keyVal in MessagesDuration)
			{
				MainCornerText.Text = MainCornerText.Text + keyVal.Key + "\n";
			}
		}
	}
}