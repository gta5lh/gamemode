
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class Marabunta : Npc
	{
		private enum MarabuntaAction
		{
			Join,
			Leave,
			HowToProgress,
			WhereToGetCar,
			Exit,
			Back,
		}

		private Dictionary<MarabuntaAction, Action> Actions = new Dictionary<MarabuntaAction, Action>
		{
			{ MarabuntaAction.Join, JoinAction },
			{ MarabuntaAction.Leave, LeaveAction },
			{ MarabuntaAction.HowToProgress, HowToProgressAction },
			{ MarabuntaAction.WhereToGetCar, WhereToGetCarAction },
			{ MarabuntaAction.Exit, ExitAction },
			{ MarabuntaAction.Back, BackAction },
		};

		public async Task<Dialogue> OnInitDialogue()
		{
			bool isGangMember = Convert.ToBoolean(await Events.CallRemoteProc("IsGangMember"));

			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = "Марабунта";

			if (isGangMember)
			{
				dialogue.Text = "Я догадываюсь, зачем ты у меня. Не забудь удалить тату, которые ты у нас уже успел сделать. Если хоть кто-то мне скажет, что ты проболтался о нас, я лично приеду и застрелю тебя. (Покидая банду ты потеряешь весь накопленный опыт).";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)MarabuntaAction.Leave, "Покинуть банду"),
					new Answer((int)MarabuntaAction.Exit, "Остаться"),
				};
			}
			else
			{
				dialogue.Text = "Салам, я надеюсь ты понимаешь куда ты приехал. Наши люди кровожадные, им нужна только кровь и деньги. Наши преимущество в том, что мы находимся подальше от эти группировок, но мы также скитаемся по всему Лос-Сантосу, не давая никому пощады. Возьми у нас машину и оружия, они тебе понадобятся. Если у тебя какие-либо вопросы, задавай.";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)MarabuntaAction.Join, "Хорошо, как раз займусь этим"),
					new Answer((int)MarabuntaAction.HowToProgress, "Как мне развиваться в банде?"),
					new Answer((int)MarabuntaAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
					new Answer((int)MarabuntaAction.Exit, "Закончить диалог"),
				};
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			MarabuntaAction a = (MarabuntaAction)action;
			this.Actions[a].Invoke();

			if (a == MarabuntaAction.Join || a == MarabuntaAction.Leave || a == MarabuntaAction.Exit)
			{
				CloseNpcDialogue();
			}
		}

		private static void JoinAction()
		{
			Events.CallRemote("JoinGang", NpcNames.Marabunta);
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
				new Answer((int)MarabuntaAction.Back, "Понял"),
				new Answer((int)MarabuntaAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void WhereToGetCarAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Возле меня ты найдёшь подходящие тебе маркеры, просто подойди к ним.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)MarabuntaAction.Back, "Понял"),
				new Answer((int)MarabuntaAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void BackAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Салам, я надеюсь ты понимаешь куда ты приехал. Наши люди кровожадные, им нужна только кровь и деньги. Наши преимущество в том, что мы находимся подальше от эти группировок, но мы также скитаемся по всему Лос-Сантосу, не давая никому пощады. Возьми у нас машину и оружия, они тебе понадобятся. Если у тебя какие-либо вопросы, задавай.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)MarabuntaAction.Join, "Хорошо, как раз займусь этим"),
				new Answer((int)MarabuntaAction.HowToProgress, "Как мне развиваться в банде?"),
				new Answer((int)MarabuntaAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
				new Answer((int)MarabuntaAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void ExitAction()
		{
		}
	}
}
