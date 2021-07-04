using RAGE;

namespace GamemodeClient.Controllers
{
	public class CameraPositionController : Events.Script
	{
		public CameraPositionController()
		{
			Events.Add("ShowCameraPosition", this.OnShowCameraPosition);
		}

		private void OnShowCameraPosition(object[] request)
		{
			Vector3 cords = RAGE.Game.Cam.GetGameplayCamCoord();
			Vector3 rot = RAGE.Game.Cam.GetGameplayCamRot(0);
			Chat.Output($"[Pos] X: {cords.X}, Y: {cords.Y}, Z: {cords.Z}, Fov: {RAGE.Game.Cam.GetGameplayCamFov()}");
			Chat.Output($"[Rot] X: {rot.X}, Y: {rot.Y}, Z: {rot.Z}");
		}
	}
}
