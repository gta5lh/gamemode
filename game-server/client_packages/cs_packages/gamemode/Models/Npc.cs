
using System.Threading.Tasks;

namespace GamemodeClient.Models
{
	public interface Npc
	{
		Task<Dialogue> OnInitDialogue();
		void OnActionSelected(int action);
	}
}
