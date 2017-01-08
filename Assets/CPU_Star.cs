using UnityEngine;
using System.Collections;

public class CPU_Star : MonoBehaviour {
	//input data:

	public bool invisible = false;

	//controller and head positions
	public Vector3 controllerPositionL;
	public Vector3 controllerForwardL;
	public Vector3 controllerVelocityL;
	public float controllerChargeL;

	public Vector3 controllerPositionR;
	public Vector3 controllerForwardR;
	public Vector3 controllerVelocityR;
	public float controllerChargeR;

	//time parameters
	public float timeScale;
	public float fps;
	public float time;

	//physics parameters
	public float velocityDampening;
	public float softeningFactor;
	public float particleMass;
	public float distanceExponent;
	public float vortexStrength;

	public Vector3 myVelocity = Vector3.zero;

	public Material myMat;
	public Renderer myRenderer;

	struct StarData {
		Vector3 pos;
		Vector3 velocity;
		Vector3 scale;
		Vector4 color;
		Vector4 randomSeed;
		Vector4 anchor;
		float age;
	};

	void Start() {
		
	}

	public void Initialise() {
		myRenderer = GetComponent<Renderer>();
		myMat = myRenderer.material;
		myRenderer.enabled = false;
	}

	public void PopIn() {
		myRenderer.enabled = true;
		StartCoroutine(PopInRoutine());
	}

	IEnumerator PopInRoutine() {
		for(float i = 0; i < 1f; i += Time.deltaTime / 0.5f) {
			transform.localScale = Vector3.one * Mathf.Lerp(0f, 0.01f, i);
			yield return null;
		}
	}

	public void Kill(float delay) {
		StartCoroutine(KillRoutine(delay));
	}

	IEnumerator KillRoutine(float delay) {
		yield return new WaitForSeconds(delay);
		float startScale = transform.localScale.x;
		for(float i = 0; i < 1f; i += Time.deltaTime / 0.5f) {
			transform.localScale = Vector3.one * Mathf.Lerp(startScale, 0f, i);
			yield return null;
		}

		transform.localScale = Vector3.zero;

		invisible = true;
	}

	void Update() {
		Physics();
		Graphics();
	}

	void Physics()
	{

		float finalTimeScale = timeScale * (90f / fps);

		float gravConst = 0.000006674f;
		//we use these a lot
		Vector3 distL = controllerPositionL - transform.position;
		Vector3 distR = controllerPositionR - transform.position;
		float lengthL = Vector3.Magnitude(distL);
		float lengthR = Vector3.Magnitude(distR);
		Vector3 normL = Vector3.Normalize(distL);
		Vector3 normR = Vector3.Normalize(distR);

		Vector3 leftForce;
		Vector3 rightForce;
		Vector3 dampingForce = -1.0f * myVelocity * velocityDampening;

		//NEW WAY - SOFTENING FACTOR
		distL = distL * (1.0f + softeningFactor);
		distR = distR * (1.0f + softeningFactor);
		lengthL = lengthL + softeningFactor;
		lengthR = lengthR + softeningFactor;
		leftForce = normL * ((controllerChargeL * gravConst) / Mathf.Pow(lengthL, distanceExponent));
		rightForce = normR * ((controllerChargeR * gravConst) / Mathf.Pow(lengthR, distanceExponent));

		float magL = controllerChargeL * gravConst / Mathf.Pow(lengthL, distanceExponent);
		Vector3 omegaL = magL * controllerForwardL;
		Vector3 vortexForceLeft = Vector3.Cross(omegaL, distL);

		float magR = controllerChargeR * gravConst / Mathf.Pow(lengthR, distanceExponent);
		Vector3 omegaR = magR * controllerForwardR;
		Vector3 vortexForceRight = Vector3.Cross(omegaR, distR);

		leftForce = leftForce + vortexForceLeft * vortexStrength;
		rightForce = rightForce + vortexForceRight * vortexStrength;

		Vector3 forceSum = (leftForce + rightForce) * 90f;

		myVelocity = myVelocity + (forceSum / particleMass) * finalTimeScale;
		
		myVelocity = myVelocity + (dampingForce / particleMass) * finalTimeScale;

		//Update position
		transform.position = transform.position + (myVelocity * finalTimeScale);

	}



	void Graphics()
	{
		//Get Color
		Vector3 col = new Vector3(1, 1, 1);

		if (Vector3.Magnitude(myVelocity) > 0) {
			//Turn color into vector
			col = Vector3.Normalize(myVelocity) + new Vector3(1f, 1f, 1f);
			col *= 0.5f;
			if (Vector3.Magnitude(col) > 0.8) { col.x = 1.0f - col.x; }
			myMat.color = Color.Lerp(myMat.color, new Color(col.x, col.y, col.z, 1f), 0.1f);
		}
	}
}
