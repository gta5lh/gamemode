
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class TheFamilies : Npc
	{
		private enum TheFamiliesAction
		{
			Join,
			Leave,
			HowToProgress,
			WhereToGetCar,
			Exit,
			Back,
		}

		private Dictionary<TheFamiliesAction, Action> Actions = new Dictionary<TheFamiliesAction, Action>
		{
			{ TheFamiliesAction.Join, JoinAction },
			{ TheFamiliesAction.Leave, LeaveAction },
			{ TheFamiliesAction.HowToProgress, HowToProgressAction },
			{ TheFamiliesAction.WhereToGetCar, WhereToGetCarAction },
			{ TheFamiliesAction.Exit, ExitAction },
			{ TheFamiliesAction.Back, BackAction },
		};

		public async Task<Dialogue> OnInitDialogue()
		{
			bool isGangMember = Convert.ToBoolean(await Events.CallRemoteProc("IsGangMember"));

			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = "Фэмилис";

			if (isGangMember)
			{
				dialogue.Text = "По какому вопросу пришёл? Только не заставляй зарядить в тебя пулю. (Покидая банду ты потеряешь весь накопленный опыт).";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)TheFamiliesAction.Leave, "Покинуть банду"),
					new Answer((int)TheFamiliesAction.Exit, "Остаться"),
				};
			}
			else
			{
				dialogue.Text = "Приветствую тебя, мой п*дюк! Давно пулю в голову не получал? Вижу, что ты не трус, как эти балласы, нам как раз нужны такие как ты, чтобы отомстить этим неверным и отвоевать наши улицы. Я думаю, что тебе стоит запастись оружием и машиной. Если у тебя какие-либо вопросы, задавай.";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)TheFamiliesAction.Join, "Хорошо, как раз займусь этим"),
					new Answer((int)TheFamiliesAction.HowToProgress, "Как мне развиваться в банде?"),
					new Answer((int)TheFamiliesAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
					new Answer((int)TheFamiliesAction.Exit, "Закончить диалог"),
				};
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			TheFamiliesAction a = (TheFamiliesAction)action;
			this.Actions[a].Invoke();

			if (a == TheFamiliesAction.Join || a == TheFamiliesAction.Leave || a == TheFamiliesAction.Exit)
			{
				CloseNpcDialogue();
			}
		}

		private static void JoinAction()
		{
			Events.CallRemote("JoinGang", NpcNames.TheFamilies);
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
				new Answer((int)TheFamiliesAction.Back, "Понял"),
				new Answer((int)TheFamiliesAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void WhereToGetCarAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Возле меня ты найдёшь подходящие тебе маркеры, просто подойди к ним.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)TheFamiliesAction.Back, "Понял"),
				new Answer((int)TheFamiliesAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void BackAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Приветствую тебя, мой п*дюк! Давно пулю в голову не получал? Вижу, что ты не трус, как эти балласы, нам как раз нужны такие как ты, чтобы отомстить этим неверным и отвоевать наши улицы. Я думаю, что тебе стоит запастись оружием и машиной. Если у тебя какие-либо вопросы, задавай.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)TheFamiliesAction.Join, "Хорошо, как раз займусь этим"),
				new Answer((int)TheFamiliesAction.HowToProgress, "Как мне развиваться в банде?"),
				new Answer((int)TheFamiliesAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
				new Answer((int)TheFamiliesAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void ExitAction()
		{
		}
	}
}
