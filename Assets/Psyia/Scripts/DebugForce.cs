using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTools;

[RequireComponent(typeof(ForceSource))]
public class DebugForce : MonoBehaviour {

	public float Range = 5f;
	public int MaxArrowCount = 500;
	public float ArrowLength = 0.05f;

	ForceSource MyForceSource;

	void Start() {
		MyForceSource = GetComponent<ForceSource>();
	}

	void DrawArrow(Vector3 Start, Vector3 End, Color Color, float ArrowHeadLength = 0.4f, float Radius = 0.001f)
	{
		Debug.DrawLine(Start, End, Color);

		Vector3 Offset = End - Start;
		Vector3 Direction = Vector3.Normalize(Offset);
 
		Vector3 Right = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(0,180+45,0) * new Vector3(0,0,1));
		Vector3 Left = Vector3.Normalize(Quaternion.LookRotation(Direction) * Quaternion.Euler(0,180-45,0) * new Vector3(0,0,1));
		Debug.DrawLine(End, End + Right * (ArrowHeadLength * Offset.magnitude), Color);
		Debug.DrawLine(End, End + Left * (ArrowHeadLength * Offset.magnitude), Color);
	}
	void Update () {
		if(MyForceSource.StrengthModifier == 0 || MyForceSource.Force.Strength == 0) return;

		int ArrowLengthCount = Mathf.RoundToInt(Mathf.Pow((float)MaxArrowCount, 1f/3f));
		for(int i = 0; i < ArrowLengthCount; i++) {
			float XPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, i));
			for(int j = 0; j < ArrowLengthCount; j++) {
				float YPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, j));
				for(int k = 0; k < ArrowLengthCount; k++) {
					float ZPos = Mathf.Lerp(-Range/2f, Range/2f, Mathf.InverseLerp(0, ArrowLengthCount - 1, k));
					Vector3 Pos = transform.position + new Vector3(XPos, YPos, ZPos);
					Vector3 Force = ForcesMirror.GetForceAtPoint(MyForceSource.Force.GetForceData(transform.position, transform.eulerAngles, MyForceSource.StrengthModifier), Pos);

					DrawArrow(Pos, Pos + Force * ArrowLength, Color.white);
				}
			}
		}
	}
}
