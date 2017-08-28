using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTools;

public class Meditation : MonoBehaviour {

	public int crystalCount = 20;
	public MeditationCrystal[] crystals;
	public Object crystalPrefab;
	public AudioClip[] tones;
	public float lowestInterval = 10f;
	public float highestInterval = 15f;
	int[] toneCounts;
	public float spawnInterval = 30f;
	public float deleteInterval = 300f;
	public float minHeight = 1.5f;
	public float maxHeight = 2.5f;
	public float chargeVariability = 0.1f;

	[Header("External GameObjects")]
	public AudioData audioData;

	[Header("Shaders")]
	public ComputeShader compute;
	public Material[] graphics;
    
	[Header("Non-user-exposed Parameters")]
	[Range(0f, 0.5f)] public float softeningFactor = 0.01f;

	//shader info
	ComputeBuffer meditationBuffer;
    int stride;
	KernelData[] kernels;

	//source arrays
	StarData[] data;
	List<StarData> dataList;

	//data to pass to shaders
	Dictionary<string, Vector4> vectors;
	Dictionary<string, float> floats;
	Dictionary<string, int> ints;
	
	//input devices
	VRInputDevice viveLeft;
	VRInputDevice viveRight;
	VRInputDevice viveHMD;
	Vector3 lastLeftPos;
	Vector3 lastRightPos;

	//utility variables
	[HideInInspector] public bool active;
	[HideInInspector] public float timeScale = 1f;
	float audioZeroTime = 0f;
	[HideInInspector] public bool noAudio = false;

	//Data set via UI
	public int activeGraphic = 0;
	public int colorMode;
	public float lineLength;
	public float shipSize = 0.005f;

	
	public float vortexStrength = 0.9f;
	public float velocityDampening = 0.01f;
	public float particleMass = 1f;

	public float charge = 1f;
	
	public int count = 50000;

	int totalCrystalCount = 0;

	public bool debug = false;
	public int debugMode = 1;

	public float menuTimer = 0;
	public float maxMenuTimer = 2f;

	public void Reset() {
		Deactivate();

		CreateDictionaries();
		InitialiseBuffers();
		FillBuffers();
		active = true;
	}

	// Use this for initialization
	void Start () {
		
		if(debug) PsyiaSettings.MeditationPosture = debugMode;

		QualitySettings.antiAliasing = 4;

		viveLeft = VRInput.GetDevice("ViveLeft");
		viveRight = VRInput.GetDevice("ViveRight");
		viveHMD = VRInput.GetDevice("ViveHMD");

		//viveLeft.ListFunctionality();

		crystals = new MeditationCrystal[crystalCount];
		toneCounts = new int[6];
		CreateCrystal();

		CreateDictionaries();
		InitialiseBuffers();
		FillBuffers();
		active = true;

		if(debug) {
			for(int i = 0; i < 19; i++) {
				CreateCrystal();
			}
		}
		StartCoroutine(CrystalSpawnRoutine());
	}

	void OnDestroy() {
		meditationBuffer.Release();
		meditationBuffer.Dispose();
	}

	public void Deactivate() {
		active = false;
		meditationBuffer.Release();
		meditationBuffer.Dispose();
        ClearDictionaries();
	}

	public void Activate() {
        //active = true;
        CreateDictionaries();
		InitialiseBuffers();
		FillBuffers();
	}

	void CreateDictionaries() {
		//for stars:
		vectors = new Dictionary<string, Vector4>();
		floats = new Dictionary<string, float>();
		ints = new Dictionary<string, int>();

		vectors.Add("shipSize", Vector3.one * shipSize);
		
		floats.Add("timeScale", timeScale);
		floats.Add("fps", 90f);
		floats.Add("time", Time.time);

		floats.Add("vortexStrength", vortexStrength);
		floats.Add("velocityDampening", 0.01f);
		floats.Add("softeningFactor", 0.01f);
		floats.Add("particleMass", 0.1f);
		floats.Add("distanceExponent", 1);
		
		ints.Add("colorMode", 2);

		SetData();
	}

    void ClearDictionaries() {
        vectors.Clear();
        floats.Clear();
        ints.Clear();
    }

	void InitialiseBuffers() {
		int vector3Stride = sizeof(float) * 3;
		int colorStride = sizeof(int) * 4;
        int vector2Stride = sizeof(float) * 2;
        int vector4Stride = sizeof(float) * 4;
		int floatStride = sizeof(float);
		stride = sizeof(float) * 4 * 6;

		/* 
		
		//don't think I need this
		if(buffer != null) {
			meditationBuffer.Dispose();
			meditationBuffer.Release();
		}
		if(spawnBuffer != null) {
			spawnmeditationBuffer.Dispose();
			spawnmeditationBuffer.Release();
		}
		*/
		meditationBuffer = new ComputeBuffer(count, stride);
		Debug.Log("Created buffer with " + meditationBuffer.count + ", " + meditationBuffer.stride);

		kernels = new KernelData[2];
		kernels[0] = new KernelData();
			kernels[0].index = compute.FindKernel("Physics");
			compute.GetKernelThreadGroupSizes(kernels[0].index, out kernels[0].x, out kernels[0].y, out kernels[0].z);


		kernels[1] = new KernelData();
			kernels[1].index = compute.FindKernel("Graphics");
			compute.GetKernelThreadGroupSizes(kernels[1].index, out kernels[1].x, out kernels[1].y, out kernels[1].z);

	}

	void FillBuffers() {
		data = new StarData[count];
		for(int i = 0; i < data.Length; i++) {
			data[i] = new StarData();

			float posRot = Random.Range(0f, Mathf.PI * 2f);
			float posRad = Random.Range(0f, 3f);
			float posHeight = Random.Range(0.01f, 0.1f);
			Vector3 pos = ChoosePosition();//new Vector3(posRad * Mathf.Cos(posRot), posHeight, posRad * Mathf.Sin(posRot));//new Vector3(0f, 1.2f, 0f) + Random.insideUnitSphere * 4.5f;
			
			data[i].position = new Vector4(pos.x, pos.y, pos.z, 0);
			data[i].scale = new Vector4(1f, 1f, 1f, 1f);
			data[i].color = Color.Lerp(Color.cyan, Color.magenta, Random.Range(0f, 1f));
			data[i].velocity = new Vector4(-data[i].position.x, -data[i].position.y, -data[i].position.z, 0) * 0.0001f;
            Vector3 sphere = Random.insideUnitSphere;
            data[i].randomSeed = new Vector4(sphere.x, sphere.y, sphere.z, Random.Range(0f, 1f));
			data[i].anchor = new Vector4(Random.Range(1f - chargeVariability, 1f + chargeVariability), pos.y, pos.z, 0);
		}

		meditationBuffer.SetData(data);

		for(int i = 0; i < kernels.Length; i++) {
            compute.SetBuffer(kernels[i].index, "outputBuffer", meditationBuffer);
        }
		for(int i = 0; i < graphics.Length; i++) graphics[i].SetBuffer("inputBuffer", meditationBuffer);
	}

	void Update() {
		if(VRInput.GetDevice("ViveLeft").GetButtonDown("Trigger") || VRInput.GetDevice("ViveRight").GetButtonDown("Trigger")) {
			colorMode = (colorMode + 1) % 5;
		}
		if(VRInput.GetDevice("ViveLeft").GetButton("Touchpad") && VRInput.GetDevice("ViveRight").GetButton("Touchpad")) {
			menuTimer += Time.deltaTime;
		} else {
			menuTimer -= Time.deltaTime * 5f;
			if(menuTimer < 0) menuTimer = 0;
		}

		if(menuTimer > maxMenuTimer) {
			UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
		}
	}

	void OnRenderObject() {
		if(!active) return;

		SetData();

		for(int i = 0; i < kernels.Length; i++) compute.Dispatch(kernels[i].index, (int)kernels[i].x, (int)kernels[i].y, (int)kernels[i].z);
		graphics[activeGraphic].SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Points, meditationBuffer.count);
	}

	void SetData() {
		floats["velocityDampening"] = velocityDampening;
		floats["particleMass"] = particleMass;
		floats["vortexStrength"] = vortexStrength;

		floats["softeningFactor"] = softeningFactor;

		ints["colorMode"] = colorMode;

		floats["time"] = Time.time;
		floats["timeScale"] = timeScale;
		//note - wait two seconds just in case there are hiccups on load
		if(Time.time > 2f) floats["fps"] = StaticFPS.frameRate;

		vectors["shipSize"] = Vector3.one * shipSize;

		foreach(KeyValuePair<string, Vector4> pair in vectors) compute.SetVector(pair.Key, pair.Value);
		foreach(KeyValuePair<string, float> pair in floats) compute.SetFloat(pair.Key, pair.Value);
		foreach(KeyValuePair<string, int> pair in ints) compute.SetInt(pair.Key, pair.Value);

		for(int i = 0; i < crystals.Length; i++) {
			if(crystals[i] != null) compute.SetVector("attractor" + i, new Vector4(crystals[i].transform.position.x, crystals[i].height + 0.095f, crystals[i].transform.position.z, crystals[i].outputCharge));
			else compute.SetVector("attractor" + i, Vector4.zero);
		}

		compute.SetFloat("heightCenter", maxHeight - (maxHeight - minHeight) / 2f);
		
		graphics[1].SetFloat("_LineLength", lineLength);
		graphics[2].SetFloat("_LineLength", lineLength);
	}

	void CreateCrystal() {
		int emptyIndex = -1;
		for(int i = 0; i < crystals.Length; i++) {
			if(crystals[i] == null) {
				emptyIndex = i;
				break;
			}
		}

		if(emptyIndex == -1) return;

		GameObject newObject = Instantiate(crystalPrefab) as GameObject;
		newObject.name = "Crystal";
		newObject.transform.parent = transform;
		crystals[emptyIndex] = newObject.GetComponent<MeditationCrystal>();

		//chose a tone that is either the rarest, or equal rarest
		int minToneCount = int.MaxValue;
		int chosenTone = 0;
		int startIndex = Random.Range(0, tones.Length);
		for(int i = startIndex; i < toneCounts.Length + startIndex; i++) {
			int j = i % toneCounts.Length;
			if(toneCounts[j] < minToneCount) {
				minToneCount = toneCounts[j];
				chosenTone = j;
			} else if(toneCounts[j] == minToneCount) {
				if(Random.Range(0f, 100f) < 50f) {
					minToneCount = toneCounts[j];
					chosenTone = j;
				}
			}
		}

		

		if(totalCrystalCount == 0) {
			crystals[emptyIndex].growTime = 5f;
			crystals[emptyIndex].Initialise(tones[0], new Vector3(0f, Random.Range(minHeight, maxHeight), 0.5f));

			toneCounts[0]++;
		}
		else {

			toneCounts[chosenTone]++;

			//make sure we don't spawn too close
			Vector3 chosenPosition = ChoosePosition();
			bool allClear = false;
			int attempts = 0;
			while(!allClear) {
				chosenPosition = ChoosePosition();
				float minDistance = float.MaxValue;
				for(int i = 0; i < crystals.Length; i++) {
					if(crystals[i] == null) continue;
					if(new Vector3(crystals[i].transform.position.x - chosenPosition.x, 0f, crystals[i].transform.position.z - chosenPosition.z).magnitude < minDistance) {
						minDistance = new Vector3(crystals[i].transform.position.x - chosenPosition.x, 0f, crystals[i].transform.position.z - chosenPosition.z).magnitude;
					}
				}

				if(minDistance > 0.4f) allClear = true;
				if(attempts > 1000) {
					Debug.LogWarning("Warning - couldn't find far enough distance to spawn after 1000 tries (minDistance was " + minDistance);
					allClear = true;
				}

				attempts++;
			}

			crystals[emptyIndex].Initialise(tones[chosenTone], chosenPosition);
			crystals[emptyIndex].intervalTime = Mathf.Lerp(lowestInterval, highestInterval, Mathf.InverseLerp(0, 5, chosenTone));
		}

		totalCrystalCount++;
	}

	Vector3 ChoosePosition() {
		float chosenRadius = 0;
		float chosenAngle = 0;
		float chosenHeight = 0;
		switch(PsyiaSettings.MeditationPosture) {
		case 0: //stand
			chosenRadius = Random.Range(0.4f, 2f);
			chosenAngle = Random.Range(-1f, 1f) * Mathf.PI;
			chosenHeight = Random.Range(0.3f, 2.5f);
			break;
		case 1: //chair sit
			chosenRadius = Random.Range(0.4f, 4.756f);
			chosenAngle = Random.Range(-1f, 1f) * Mathf.PI * (200f / 360f);
			chosenHeight = Random.Range(0.3f, 2.5f);
			break;
		case 2: //floor sit
			chosenRadius = Random.Range(0.4f, 4.756f);
			chosenAngle = Random.Range(-1f, 1f) * Mathf.PI * (200f / 360f);
			chosenHeight = Random.Range(0.3f, 1.75f);
			break;
		case 3: //lay
			chosenRadius = Random.Range(0.4f, 4.756f);
			chosenAngle = Random.Range(-1f, 1f) * Mathf.PI * (200f / 360f);
			chosenHeight = Random.Range(0.75f, 2.5f);
			break;
		}

		return new Vector3(chosenRadius * Mathf.Sin(chosenAngle), chosenHeight, chosenRadius * Mathf.Cos(chosenAngle));
	}

	void DeleteCrystal() {
		StartCoroutine(DeleteCrystalRoutine());
	}

	IEnumerator DeleteCrystalRoutine() {
		float maxAge = 0f;
		int chosenIndex = -1;
		for(int i = 0; i < crystals.Length; i++) {
			if(crystals[i] == null) continue;
			if(crystals[i].age > maxAge) {
				maxAge = crystals[i].age;
				chosenIndex = i;
			}
		}

		crystals[chosenIndex].Stop();

		while(!crystals[chosenIndex].deleteMe) {
			yield return null;
		}

		Destroy(crystals[chosenIndex].gameObject);
		crystals[chosenIndex] = null;
	}

	IEnumerator CrystalSpawnRoutine() {
		yield return new WaitForSeconds(spawnInterval);
		CreateCrystal();
		
		StartCoroutine(CrystalSpawnRoutine());

		if(totalCrystalCount == 1) {
			yield return new WaitForSeconds(10f);
			for(int i = 0; i < crystals.Length; i++) {
				if(crystals[i] == null) continue;
				crystals[i].growTime = 30f;
			}
		}
	}

	IEnumerator CrystalDeleteRoutine() {
		yield return new WaitForSeconds(deleteInterval);
		DeleteCrystal();
		StartCoroutine(CrystalDeleteRoutine());
	}
}