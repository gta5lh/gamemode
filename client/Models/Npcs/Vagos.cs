
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class Vagos : Npc
	{
		private enum VagosAction
		{
			Join,
			Leave,
			HowToProgress,
			WhereToGetCar,
			Exit,
			Back,
		}

		private Dictionary<VagosAction, Action> Actions = new Dictionary<VagosAction, Action>
		{
			{ VagosAction.Join, JoinAction },
			{ VagosAction.Leave, LeaveAction },
			{ VagosAction.HowToProgress, HowToProgressAction },
			{ VagosAction.WhereToGetCar, WhereToGetCarAction },
			{ VagosAction.Exit, ExitAction },
			{ VagosAction.Back, BackAction },
		};

		public async Task<Dialogue> OnInitDialogue()
		{
			bool isGangMember = Convert.ToBoolean(await Events.CallRemoteProc("IsGangMember"));

			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = "Вагос";

			if (isGangMember)
			{
				dialogue.Text = "Если ты хочешь сказать, что уходишь, я не обижусь! В тебе я не разглядела хорошего гангстера. Не передумал? (Покидая банду ты потеряешь весь накопленный опыт).";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)VagosAction.Leave, "Покинуть банду"),
					new Answer((int)VagosAction.Exit, "Остаться"),
				};
			}
			else
			{
				dialogue.Text = "Привет, красавчик! Только не говори, что ты сюда приехал полюбоваться мною, если ты конечно не девочка :) Без шуток, ты выбрал достаточно влиятельную банду в городе. Прямо сейчас, можешь взять машину, оружия и помогать нашим защищать территорию, на которую похищаются другие бандиты.";
				dialogue.Answers = new List<Answer>
				{
					new Answer((int)VagosAction.Join, "Хорошо, как раз займусь этим"),
					new Answer((int)VagosAction.HowToProgress, "Как мне развиваться в банде?"),
					new Answer((int)VagosAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
					new Answer((int)VagosAction.Exit, "Закончить диалог"),
				};
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			VagosAction a = (VagosAction)action;
			this.Actions[a].Invoke();

			if (a == VagosAction.Join || a == VagosAction.Leave || a == VagosAction.Exit)
			{
				CloseNpcDialogue();
			}
		}

		private static void JoinAction()
		{
			Events.CallRemote("JoinGang", NpcNames.Vagos);
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
				new Answer((int)VagosAction.Back, "Понял"),
				new Answer((int)VagosAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void WhereToGetCarAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Возле меня ты найдёшь подходящие тебе маркеры, просто подойди к ним.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)VagosAction.Back, "Понял"),
				new Answer((int)VagosAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void BackAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Привет, красавчик! Только не говори, что ты сюда приехал полюбоваться мною, если ты конечно не девочка :) Без шуток, ты выбрал достаточно влиятельную банду в городе. Прямо сейчас, можешь взять машину, оружия и помогать нашим защищать территорию, на которую похищаются другие бандиты.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)VagosAction.Join, "Хорошо, как раз займусь этим"),
				new Answer((int)VagosAction.HowToProgress, "Как мне развиваться в банде?"),
				new Answer((int)VagosAction.WhereToGetCar, "Где я могу взять машину и оружие?"),
				new Answer((int)VagosAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private static void ExitAction()
		{
		}
	}
}
