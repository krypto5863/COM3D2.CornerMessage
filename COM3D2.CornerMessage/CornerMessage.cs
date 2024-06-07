using BepInEx;
using BepInEx.Logging;
using System.Security;
using System.Security.Permissions;
using JetBrains.Annotations;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace COM3D2.CornerMessage
{
	[BepInPlugin("COM3D2.CornerMessage", "COM3D2.CornerMessage", "2.0")]
	public class CornerMessage : BaseUnityPlugin
	{
		internal static CornerMessage Instance;
		internal static ManualLogSource PluginLogger => Instance.Logger;
		private static CornerText _mainCornerText;

		[UsedImplicitly]
		private void Awake()
		{
			Instance = this;
			_mainCornerText = CornerText.GetOrCreateCornerText();
		}

		public static void DisplayMessage(string message, float messageDuration = 6f) =>
			_mainCornerText.QueueMessage(message, messageDuration);
	}
}