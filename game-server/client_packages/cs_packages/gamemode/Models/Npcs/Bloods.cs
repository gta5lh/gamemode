
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class Bloods : Npc
	{
		private enum BloodsAction
		{
			Join,
			Leave,
			HowToProgress,
			WhereToGetCar,
			Exit,
			Back,
		}

		private Dictionary<BloodsAction, Action> Actions = new Dictionary<BloodsAction, Action>
		{
			{ BloodsAction.Join, JoinAction },
			{ BloodsAction.Leave, LeaveAction },
			{ BloodsAction.HowToProgress, HowToProgressAction },
			{ BloodsAction.WhereToGetCar, WhereToGetCarAction },
			{ BloodsAction.Exit, ExitAction },
			{ BloodsAction.Back, BackAction },
		};

		public async Task<Dialogue> OnInitDialogue()
		{
			bool isGangMember = Convert.ToBoolean(await Events.CallRemoteProc("IsGangMember"));

			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = "Блудс";

			if (isGangMember)
			{
				dialogue.Text = "Уходишь? Ты думаешь, что можешь взять и уйти? Направьте на него пушки! Ладно, я шучу, своего мы не убьём, если конечно ты передумал. (Покидая банду ты потеряешь весь накопленный опыт).";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)BloodsAction.Leave, "Покинуть банду"),
					new Answer((int)BloodsAction.Exit, "Остаться"),
				};
			}
			else
			{
				dialogue.Text = "Привет, моё тебе уважение, что приехал именно к нам. Как раз нуждаемся в пополнение. Только знай, что наша группировка - это серьёзные ребята. Недавно ездили на окраину Лос-Сантоса проведать марабунту, эти черти в татуировках оставили мне шрам на лице. Им пощады не будет! Возьми у нас машину и оружия, они тебе понадобятся. Если у тебя какие-либо вопросы, задавай.";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)BloodsAction.Join, "Хорошо, как раз займусь этим"),
					new Answer((int)BloodsAction.HowToProgress, "Как мне развиваться в банде?"),
					new Answer((int)BloodsAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
					new Answer((int)BloodsAction.Exit, "Закончить диалог"),
				};
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			BloodsAction a = (BloodsAction)action;
			this.Actions[a].Invoke();

			if (a == BloodsAction.Join || a == BloodsAction.Leave || a == BloodsAction.Exit)
			{
				CloseNpcDialogue();
			}
		}

		private static void JoinAction()
		{
			Events.CallRemote("JoinGang", NpcNames.Bloods);
		}

		private static void LeaveAction()
		{
			Events.CallRemote("LeaveGang", "");
		}

		private static void HowToProgressAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Убивая ниггеров, ты будешь прокачивать свой прогресс и зарабатывать деньги. Каждые 30 минут, ты будешь получать опыт и деньги. За убийство на захватах территории, получается больше всего опыта. За захват территории, все участвующие в атаке бандиты, получают хороший размер опыта. Бандит, который достиг высокого звание, сможет устраивать нападение на территорию.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)BloodsAction.Back, "Понял"),
				new Answer((int)BloodsAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void WhereToGetCarAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Возле меня ты найдёшь подходящие тебе маркеры, просто подойди к ним.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)BloodsAction.Back, "Понял"),
				new Answer((int)BloodsAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void BackAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Привет, моё тебе уважение, что приехал именно к нам. Как раз нуждаемся в пополнение. Только знай, что наша группировка - это серьёзные ребята. Недавно ездили на окраину Лос-Сантоса проведать марабунту, эти черти в татуировках оставили мне шрам на лице. Им пощады не будет! Возьми у нас машину и оружия, они тебе понадобятся. Если у тебя какие-либо вопросы, задавай.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)BloodsAction.Join, "Хорошо, как раз займусь этим"),
				new Answer((int)BloodsAction.HowToProgress, "Как мне развиваться в банде?"),
				new Answer((int)BloodsAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
				new Answer((int)BloodsAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void ExitAction()
		{
		}
	}
}
