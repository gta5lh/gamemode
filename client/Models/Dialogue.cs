
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamemodeClient.Models
{
	public class Dialogue
	{
		[JsonProperty("name")]
		public string NpcName;

		[JsonProperty("text")]
		public string Text;

		[JsonProperty("answers")]
		public List<Answer> Answers;
	}

	public class Answer
	{
		[JsonProperty("id")]
		public int Id;

		[JsonProperty("text")]
		public string Text;

		public Answer(int id, string text)
		{
			this.Id = id;
			this.Text = text;
		}
	}
}
