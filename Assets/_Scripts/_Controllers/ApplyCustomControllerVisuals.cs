using UnityEngine;
using Valve.VR;

public class ApplyCustomControllerVisuals : MonoBehaviour
{

	public Material CustomMaterial;

	public string[] ModelNames = 
	{
		"vr_controller_vive_1_5",
		"oculus_cv1_controller_left",
		"oculus_cv1_controller_right"
	};

	public Mesh[] ModelMeshes;

	public void Awake()
	{
		SteamVR_Events.RenderModelLoaded.AddListener(ApplyCustomVisuals);
	}

	private void ApplyCustomVisuals(SteamVR_RenderModel model, bool action)
	{
		Debug.Log("Loaded model " + model.renderModelName);
		for (var i = 0; i < ModelNames.Length; i++) {
			if (model.renderModelName != ModelNames[i]) continue;

			var body = transform.Find("body");
			if (body == null) break;
			var meshFilter = body.GetComponent<MeshFilter>();
			if (meshFilter == null) break;
			
			meshFilter.mesh = ModelMeshes[i];
			break;
		}

		if (CustomMaterial == null) return;
		{
			var renderers = GetComponentsInChildren<Renderer>(true);
			foreach (var r in renderers) {
				r.sharedMaterial = CustomMaterial;
			}
		}
	}
}