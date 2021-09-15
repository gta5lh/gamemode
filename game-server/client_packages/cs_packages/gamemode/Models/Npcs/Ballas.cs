
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class Ballas : Npc
	{
		private enum BallasAction
		{
			Join,
			Leave,
			HowToProgress,
			WhereToGetCar,
			Exit,
			Back,
		}

		private Dictionary<BallasAction, Action> Actions = new Dictionary<BallasAction, Action>
		{
			{ BallasAction.Join, JoinAction },
			{ BallasAction.Leave, LeaveAction },
			{ BallasAction.HowToProgress, HowToProgressAction },
			{ BallasAction.WhereToGetCar, WhereToGetCarAction },
			{ BallasAction.Exit, ExitAction },
			{ BallasAction.Back, BackAction },
		};

		public async Task<Dialogue> OnInitDialogue()
		{
			bool isGangMember = Convert.ToBoolean(await Events.CallRemoteProc("IsGangMember"));

			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = "Баллас";

			if (isGangMember)
			{
				dialogue.Text = "Если ты хочешь сказать, что уходишь, я не обижусь! В тебе я не разглядела хорошего гангстера. Не передумал? (Покидая банду ты потеряешь весь накопленный опыт).";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)BallasAction.Leave, "Покинуть банду"),
					new Answer((int)BallasAction.Exit, "Остаться"),
				};
			}
			else
			{
				dialogue.Text = "Здравствуй, ниггер! Тебе повезло, что тебя ещё не пристрелили по пути к нам. Ты выбрал опасную банду Лос-Сантоса, трусов мы тут не приветствуем. В данный момент, я тебе советую сжать свои яйца в кулак, взять тачку, оружие и выдвигаться на перестрелку, неподалеку от сюда нужна твоя помощь, мы захватываем важную часть Лос-Сантоса. Если у тебя какие-либо вопросы, задавай.";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)BallasAction.Join, "Хорошо, как раз займусь этим"),
					new Answer((int)BallasAction.HowToProgress, "Как мне развиваться в банде?"),
					new Answer((int)BallasAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
					new Answer((int)BallasAction.Exit, "Закончить диалог"),
				};
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			BallasAction a = (BallasAction)action;
			this.Actions[a].Invoke();

			if (a == BallasAction.Join || a == BallasAction.Leave || a == BallasAction.Exit)
			{
				CloseNpcDialogue();
			}
		}

		private static void JoinAction()
		{
			Events.CallRemote("JoinGang", NpcNames.Ballas);
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
				new Answer((int)BallasAction.Back, "Понял"),
				new Answer((int)BallasAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void WhereToGetCarAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Возле меня ты найдёшь подходящие тебе маркеры, просто подойди к ним.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)BallasAction.Back, "Понял"),
				new Answer((int)BallasAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void BackAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Здравствуй, ниггер! Тебе повезло, что тебя ещё не пристрелили по пути к нам. Ты выбрал опасную банду Лос-Сантоса, трусов мы тут не приветствуем. В данный момент, я тебе советую сжать свои яйца в кулак, взять тачку, оружие и выдвигаться на перестрелку, неподалеку от сюда нужна твоя помощь, мы захватываем важную часть Лос-Сантоса. Если у тебя какие-либо вопросы, задавай.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)BallasAction.Join, "Хорошо, как раз займусь этим"),
				new Answer((int)BallasAction.HowToProgress, "Как мне развиваться в банде?"),
				new Answer((int)BallasAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
				new Answer((int)BallasAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void ExitAction()
		{
		}
	}
}
