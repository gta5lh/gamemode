
using RAGE;

namespace GamemodeClient.Models
{
	public class GangCarSelectionData
	{
		public GangCarSelectionData(Vector3 cameraPosition, Vector3 cameraRotation)
		{
			this.CameraPosition = cameraPosition;
			this.CameraRotation = cameraRotation;
		}

		public Vector3 CameraPosition { get; set; }

		public Vector3 CameraRotation { get; set; }
	}
}
