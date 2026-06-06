using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

public abstract class PuzzleLogic : MonoBehaviour
{
	protected Camera puzzleCamera = null;
	public abstract bool IsComplete();

	void Awake()
	{
		this.puzzleCamera = new GameObject("Puzzle Camera").AddComponent<Physics2DRaycaster>().AddComponent<Camera>();
		this.puzzleCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
		Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(this.puzzleCamera);
	}
}
