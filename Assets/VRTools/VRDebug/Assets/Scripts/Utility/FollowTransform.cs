using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public Vector3 rotationOffset;
	public bool dampen = false;
	[Range(0f, 1f)] public float dampening = 0.3f;

	// Use this for initialization
	void Start () {
		if(target == null) target = VRTools.VRInput.GetDevice("ViveLeft").transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, target.TransformPoint(offset), 1f - Mathf.Sqrt(dampening));
		transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation * Quaternion.Euler(rotationOffset), 1f - Mathf.Sqrt(dampening));
	}
}
