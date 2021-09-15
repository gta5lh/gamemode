using System;

using System.Collections.Generic;
using System.Threading.Tasks;
using static GamemodeClient.Controllers.Cef.Cef;
using RAGE;
using GamemodeCommon.Models;

namespace GamemodeClient.Models
{
	public class SpawnNpc : Npc
	{
		public const string NameFranklin = "Франклин";
		public const string NameTrevor = "Тревор";
		public const string NameMichael = "Майкл";

		private string name;
		private string joinGangName;

		public SpawnNpc(string name)
		{
			this.name = name;
			this.Actions = new Dictionary<SpawnNpcAction, Action>
			{
				{ SpawnNpcAction.HowToGetThere, HowToGetThereAction },
				{ SpawnNpcAction.TellMeMoreAboutGangs, TellMeMoreAboutGangsAction },
				{ SpawnNpcAction.Ballas, BallasAction },
				{ SpawnNpcAction.Bloods, BloodsAction },
				{ SpawnNpcAction.Marabunta, MarabuntaAction },
				{ SpawnNpcAction.TheFamilies, TheFamiliesAction },
				{ SpawnNpcAction.Vagos, VagosAction },
				{ SpawnNpcAction.Join, JoinAction },
				{ SpawnNpcAction.Exit, ExitAction },
				{ SpawnNpcAction.Back, BackAction },
			};
		}

		private enum SpawnNpcAction
		{
			HowToGetThere,
			TellMeMoreAboutGangs,
			Ballas,
			Bloods,
			Marabunta,
			TheFamilies,
			Vagos,
			Join,
			Exit,
			Back,
		}

		private Dictionary<SpawnNpcAction, Action> Actions;

		public async Task<Dialogue> OnInitDialogue()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.NpcName = this.name;

			switch (this.name)
			{
				case NameFranklin:
					dialogue.Text = "Ну привет, дружище! Меня зовут Франклин, cмотрю ты у нас первый раз, думаю стоит тебя предупредить, что в нашем штате довольно опасно! Войны за территорию, никаких полицейских, только стволы и наркотики. Ты попал в настоящее гетто, ниггер!";
					dialogue.Answers = new List<Answer>
					{
						new Answer((int)SpawnNpcAction.HowToGetThere, "Что мне теперь делать?"),
						new Answer((int)SpawnNpcAction.Exit, "Я хочу быть послушным гражданином"),
					};

					break;

				case NameMichael:
					dialogue.Text = "Привет, друг! Как тебя сюда занесло? Вижу ты у нас впервые, стоит познакомиться, меня зовут Майкл. Неподалёку в штате идут перестрелки, банды дерутся за свою территорию, даже полицейские боятся туда суваться.";
					dialogue.Answers = new List<Answer>
					{
						new Answer((int)SpawnNpcAction.HowToGetThere, "Меня возьмут в какую-нибудь из банд?"),
						new Answer((int)SpawnNpcAction.Exit, "Я не хочу туда ехать, Майкл"),
					};

					break;

				case NameTrevor:
					dialogue.Text = "Приветствую, новичок! Вижу ты к нам первый раз прилетел. В штате сейчас полный переполох, банды отвоевывают территории. Некоторым нехватает тебя, тебе следует направиться к ним и показать на что ты способен.";
					dialogue.Answers = new List<Answer>
					{
						new Answer((int)SpawnNpcAction.HowToGetThere, "Как мне лучше доехать до туда?"),
						new Answer((int)SpawnNpcAction.Exit, "Я сюда летел не убивать"),
					};

					break;
			}

			return dialogue;
		}

		public void OnActionSelected(int action)
		{
			SpawnNpcAction a = (SpawnNpcAction)action;
			this.Actions[a].Invoke();
		}

		private void HowToGetThereAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Я тебе дам машину в аренду и ты должен выбрать одну из пяти банд и ехать туда.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.TellMeMoreAboutGangs, "Расскажи о бандах в штате"),
				new Answer((int)SpawnNpcAction.Exit, "Я хочу быть послушным гражданином"),
			};

			SetNpcDialogue(dialogue);
		}

		private void TellMeMoreAboutGangsAction()
		{
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "В Лос-Сантосе орудуют 5 группировок, это Ballas, Vagos, The Famillies, Bloods и на окраине Marabunta. Я тебе сейчас немного расскажу о каждой и ты уже решишь куда тебе ехать.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Ballas, "Ballas"),
				new Answer((int)SpawnNpcAction.Vagos, "Vagos"),
				new Answer((int)SpawnNpcAction.TheFamilies, "The Families"),
				new Answer((int)SpawnNpcAction.Bloods, "Bloods"),
				new Answer((int)SpawnNpcAction.Marabunta, "Marabunta"),
				new Answer((int)SpawnNpcAction.Exit, "Я пожалуй откажусь"),
			};

			SetNpcDialogue(dialogue);
		}

		private void BallasAction()
		{
			this.joinGangName = NpcNames.Ballas;
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Это афроамериканская банда в штате враждующая с The Families, они до сих пор носят одежду фиолетового (или пурпурного) цвета. По-прежнему занимаются торговлей наркотиками, оружием, крышуют предприятия, занимаются сутенерством, воюют за территории. Они достаточно агрессивная группировка.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Join, "Я хочу вступить в Ballas"),
				new Answer((int)SpawnNpcAction.Back, "Расскажи про другие группировки"),
				new Answer((int)SpawnNpcAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private void BloodsAction()
		{
			this.joinGangName = NpcNames.Bloods;
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "История стрелкового клуба Bloods Gang берет свое начало в 2010 году, в городе Лос Сантос, в районе Чумаши. Название Bloods Gang стало в честь труда и пролитой крови на войне. Bloods - (Кровь) Gang - (Родина, семья, отчизна, банда). Сейчас они занимают северную часть лос-сантоса. Носят красную одежду, считаются самой кровожадной группировкой в штате..";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Join, "Я хочу вступить в Bloods"),
				new Answer((int)SpawnNpcAction.Back, "Расскажи про другие группировки"),
				new Answer((int)SpawnNpcAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private void MarabuntaAction()
		{
			this.joinGangName = NpcNames.Marabunta;
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Сальвадорская уличная банда, принимающая активное участие в торговле наркотиками и оружием. Членов этой банды можно найти на территории Восточного Лос-Сантоса. В основном, они активны по ночам, в виде небольших групп, стоящих на тротуарах или сидящих на порогах домов. Если их спровоцировать, то сразу же начинают атаковать.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Join, "Я хочу вступить в Marabunta"),
				new Answer((int)SpawnNpcAction.Back, "Расскажи про другие группировки"),
				new Answer((int)SpawnNpcAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private void TheFamiliesAction()
		{
			this.joinGangName = NpcNames.TheFamilies;
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "Банда по-прежнему враждует с Баллас и Вагос, носят одежду зелёного (или салатового) цвета. Утратили значительное количество влияния, и даже потеряли Гроув-стрит, где теперь расположились Баллас и наркоторговцы. Занимает весь северо-запад Лос-Сантоса.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Join, "Я хочу вступить в The Families"),
				new Answer((int)SpawnNpcAction.Back, "Расскажи про другие группировки"),
				new Answer((int)SpawnNpcAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private void VagosAction()
		{
			this.joinGangName = NpcNames.Vagos;
			Dialogue dialogue = new Dialogue();
			dialogue.Text = "По слухам — самая многочисленная мексиканская уличная банда в Лос-Сантосе. Согласно телевизионной программе The Underbelly Of Paradise, вполне возможно, что они имеют связи с мафиозными семьями. С головой вовлечены в торговлю наркотиками. Несмотря на свою обычную внешность уличных головорезов, Vagos - чрезвычайно мощная децентрализованная преступная сеть. Их основной интерес - незаконный оборот наркотиков, но они диверсифицировались на дистрибуцию среднего уровня, продажи конечных потребителей и другую контрабанду.";
			dialogue.Answers = new List<Answer>
			{
				new Answer((int)SpawnNpcAction.Join, "Я хочу вступить в Bloods"),
				new Answer((int)SpawnNpcAction.Back, "Расскажи про другие группировки"),
				new Answer((int)SpawnNpcAction.Exit, "Закончить диалог"),
			};

			SetNpcDialogue(dialogue);
		}

		private void JoinAction()
		{
			Events.CallRemote("отчи", this.name, this.joinGangName);
			CloseNpcDialogue();
		}

		private void BackAction()
		{
			TellMeMoreAboutGangsAction();
		}

		private void ExitAction()
		{
			CloseNpcDialogue();
		}
	}
}
