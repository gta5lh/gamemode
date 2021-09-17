using GamemodeClient.Models;
using Newtonsoft.Json;
using RAGE;

namespace GamemodeClient.Controllers.Cef
{
	public static partial class Cef
	{
		public static void SetNpcDialogue(Dialogue dialogue)
		{
			string dialogueJson = JsonConvert.SerializeObject(dialogue);
			IndexCef.ExecuteJs($"SetNpcDialogue('{dialogueJson}')");
		}

		public static void InitNpcDialogue(Dialogue dialogue)
		{
			string dialogueJson = JsonConvert.SerializeObject(dialogue);
			IndexCef.ExecuteJs($"InitNpcDialogue('{dialogueJson}')");
			Controllers.Menu.Open(true);
			disableUIStateChangedEvent(false);
		}

		public static void CloseNpcDialogue()
		{
			IndexCef.ExecuteJs($"CloseNpcDialogue()");
			Controllers.Menu.Close();
			disableUIStateChangedEvent(true);
		}
	}
}
