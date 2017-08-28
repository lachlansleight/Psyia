using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VRTools;


public class StarIntro : MonoBehaviour {

	[Header("External GameObjects")]
	public AudioData audioData;
	public Collider inactiveZone;
    public GameObject roomTemp;
	public Transform leftController;
	public Transform rightController;
	public MeshFilter text;

	[Header("Shaders")]
	public ComputeShader compute;
	public Material[] graphics;
    
	[Header("Non-user-exposed Parameters")]
	[Range(-5f, 5f)] public float distanceExponent = 1f;
	[Range(0f, 1f)] public float activeSpawnRate = 1f;
	[Range(0f, 1f)] public float passiveSpawnRate = 0f;
	[Range(0f, 0.005f)] public float cubeSpawnVelocity = 0.0002f;
	[Range(0f, 0.5f)] public float softeningFactor = 0.01f;
	public Vector3 roomSize = new Vector3(4f, 3f, 4f);

	//shader info
	ComputeBuffer buffer;
    ComputeBuffer spawnBuffer;
    int stride;
	KernelData[] kernels;

	//source arrays
	StarData[] data;
	List<StarData> dataList;
    StarData[] spawnData;
    List<StarData> spawnList;

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
	[HideInInspector] public bool introFinished = false;
	[HideInInspector] public bool leftInactive = false;
	[HideInInspector] public bool rightInactive = false;
	[HideInInspector] public float timeScale = 1f;
	float audioZeroTime = 0f;
	[HideInInspector] public bool noAudio = false;

	//Data set via UI
	int activeGraphic = 0;
	int colorMode;
	float lineLength;
	float shipSize = 0.005f;

	
	float vortexStrength = 0.9f;
	float velocityDampening = 0.01f;
	float particleMass = 1f;
	bool roomCollision = false;
	bool jellyMode = false;

	float charge = 1f;
	int leftControllerMode = 0;
	int rightControllerMode = 0;
	bool fancyControllers = true;
	float controllerLength = 0f;

	float audioStrengthGraphics = 0.5f;
	float audioStrengthPhysics = 0.5f;
	
	int count = 50000;

	public void Reset() {
		Deactivate();

		if(count != PsyiaSettings.ParticleCount) count = PsyiaSettings.ParticleCount;

		CreateDictionaries();
		InitialiseBuffers();
		FillBuffers();
		active = true;
		StartCoroutine(burstOut());
	}

	// Use this for initialization
	void Start () {
		viveLeft = VRInput.GetDevice("ViveLeft");
		viveRight = VRInput.GetDevice("ViveRight");
		viveHMD = VRInput.GetDevice("ViveHMD");

		//viveLeft.ListFunctionality();

		//UpdateFromSettings();
		//count = PsyiaSettings.ParticleCount;
		count = 100000;

		CreateDictionaries();
		InitialiseBuffers();
		FillBuffers();
		active = true;

		compute.SetInt("customMode", 2);
	}

	void OnDestroy() {
		buffer.Release();
		buffer.Dispose();
		spawnBuffer.Release();
		spawnBuffer.Dispose();
	}

	public void Deactivate() {
		active = false;
		buffer.Release();
		buffer.Dispose();
        spawnBuffer.Release();
        spawnBuffer.Dispose();
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

		vectors.Add("controllerPositionL", Vector4.zero);
		vectors.Add("controllerForwardL", Vector4.zero);
		vectors.Add("controllerVelocityL", Vector4.zero);
		floats.Add("controllerChargeL", 0f);

		vectors.Add("controllerPositionR", Vector4.zero);
		vectors.Add("controllerForwardR", Vector4.zero);
		vectors.Add("controllerVelocityR", Vector4.zero);
		floats.Add("controllerChargeR", 0f);

		vectors.Add("roomSize", roomSize);
		vectors.Add("shipSize", Vector3.one * shipSize);
		
		floats.Add("timeScale", timeScale);
		floats.Add("fps", 90f);
		floats.Add("time", Time.time);

		floats.Add("vortexStrength", vortexStrength);
		floats.Add("velocityDampening", 0.01f);
		floats.Add("softeningFactor", 0.01f);
		floats.Add("particleMass", 0.1f);
		floats.Add("distanceExponent", 1);
        floats.Add("spawnRangeMin", 0);
        floats.Add("spawnRangeMax", 0);
        floats.Add("LRSpawnBalance", 0.5f);
		
		ints.Add("colorMode", 0);

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
			buffer.Dispose();
			buffer.Release();
		}
		if(spawnBuffer != null) {
			spawnBuffer.Dispose();
			spawnBuffer.Release();
		}
		*/

		buffer = new ComputeBuffer(count, stride);
        spawnBuffer = new ComputeBuffer(count, stride);

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
        spawnData = new StarData[count];

		Mesh sourceMesh = text.mesh;
		Vector3[] sourceVerts = sourceMesh.vertices;
		int[] sourceTris = sourceMesh.triangles;
		int triangleCount = sourceTris.Length / 3;
		for(int i = 0; i < data.Length; i++) {
			data[i] = new StarData();
            spawnData[i] = new StarData();

			int triStart = (int)(((float)i / (float)data.Length) * (triangleCount - 1)) * 3;

			float posRot = Random.Range(0f, Mathf.PI * 2f);
			float posRad = Random.Range(0f, 5f);
			float posHeight = Random.Range(0f, 5f);
			Vector3 pos = new Vector3(posRad * Mathf.Cos(posRot), posHeight, posRad * Mathf.Sin(posRot));//new Vector3(0f, 1.2f, 0f) + Random.insideUnitSphere * 4.5f;
			data[i].position = new Vector4(pos.x, pos.y, pos.z, 0);
			data[i].scale = new Vector4(vectors["shipSize"].x, vectors["shipSize"].y, vectors["shipSize"].z, 0);
			data[i].color = Color.Lerp(Color.cyan, Color.magenta, Random.Range(0f, 1f));
			data[i].velocity = Vector4.zero;//-data[i].position * 0.0001f;
            Vector3 sphere = Random.insideUnitSphere;
            data[i].randomSeed = new Vector4(sphere.x, sphere.y, sphere.z, Random.Range(0f, 1f));

			Vector3 vertA = sourceVerts[sourceTris[triStart]];
			Vector3 vertB = sourceVerts[sourceTris[triStart+1]];
			Vector3 vertC = sourceVerts[sourceTris[triStart+2]];
			Vector3 anchorPos = text.transform.TransformPoint(vertA + Vector3.Lerp(vertA - vertB, vertA - vertC, Random.Range(0f, 1f)) * Random.Range(0f, 1f));

			data[i].anchor = new Vector4(anchorPos.x, anchorPos.y, anchorPos.z, Random.Range(0.2f, 1f));

            spawnData[i].position = Vector4.zero;
			spawnData[i].scale = new Vector4(vectors["shipSize"].x, vectors["shipSize"].y, vectors["shipSize"].z, 0);
            spawnData[i].color = Color.Lerp(Color.cyan, Color.magenta, Random.Range(0f, 1f));
			spawnData[i].velocity = Vector4.zero;
            sphere = Random.insideUnitSphere;
            spawnData[i].randomSeed = new Vector4(sphere.x, sphere.y, sphere.z, Random.Range(0f, 1f));
			spawnData[i].anchor = new Vector4(0,0,0, 0);
		}
		buffer.SetData(data);
        spawnBuffer.SetData(spawnData);

		for(int i = 0; i < kernels.Length; i++) {
            compute.SetBuffer(kernels[i].index, "outputBuffer", buffer);
            compute.SetBuffer(kernels[i].index, "spawnBuffer", spawnBuffer);
        }
		for(int i = 0; i < graphics.Length; i++) graphics[i].SetBuffer("inputBuffer", buffer);
	}

	void OnRenderObject() {
		if(!active) return;

		SetData();

		for(int i = 0; i < kernels.Length; i++) compute.Dispatch(kernels[i].index, (int)kernels[i].x, (int)kernels[i].y, (int)kernels[i].z);
		graphics[activeGraphic].SetPass(0);
		Graphics.DrawProcedural(MeshTopology.Points, buffer.count);
	}

	void UpdateFromSettings() {
		activeGraphic = PsyiaSettings.ParticleForm;
		colorMode = PsyiaSettings.ColorMode;
		lineLength = PsyiaSettings.LineLength;
		shipSize = PsyiaSettings.ShipSize;

		charge = PsyiaSettings.ControllerForce;
		leftControllerMode = PsyiaSettings.LeftTouchpadFunction;
		rightControllerMode = PsyiaSettings.RightTouchpadFunction;
		controllerLength = PsyiaSettings.ControllerDistance;
		fancyControllers = PsyiaSettings.ControllerModels;

		//keep audio at 'average' level (i.e. no change from default values) if we haven't heard any noise in a while (check Update() if confused)
		audioStrengthGraphics = noAudio ? 0.5f : PsyiaSettings.VisualizationStrengthGraphics;
		audioStrengthPhysics = noAudio ? 0.5f : PsyiaSettings.VisualizationStrengthPhysics;

		particleMass = PsyiaSettings.ParticleMass;
		velocityDampening = PsyiaSettings.VelocityDampening;
		vortexStrength = PsyiaSettings.VortexStrength;
		roomCollision = PsyiaSettings.RoomCollision;
		jellyMode = PsyiaSettings.JellyMode;
	}

	void SetData() {
		//UpdateFromSettings();

		//custom mode data
		compute.SetFloat("burstLaunchVelocity", 0.000002f);
		compute.SetFloat("returnPullForce", 0.000002f);

		/*
		vectors["controllerPositionL"] = leftController.position;
		vectors["controllerPositionR"] = rightController.position;
		vectors["controllerForwardL"] = leftController.forward;
		vectors["controllerForwardR"] = rightController.forward;
		vectors["controllerVelocityL"] = (leftController.position - lastLeftPos);
		vectors["controllerVelocityR"] = (rightController.position - lastRightPos);
		*/

		float audioChargeMin = 0.3f;
		float audioChargeMax = 1.7f;

		float finalCharge = Mathf.Lerp(charge * audioChargeMin, charge * audioChargeMax, Mathf.Lerp(0.5f - 0.5f * audioStrengthPhysics, 0.5f + 0.5f * audioStrengthPhysics, audioData.avgVol));

		floats["velocityDampening"] = velocityDampening;
		floats["particleMass"] = particleMass;
		floats["distanceExponent"] = distanceExponent;
		floats["vortexStrength"] = vortexStrength;
		floats["cubeSpawnVelocity"] = cubeSpawnVelocity;

        floats["spawnRangeMin"] = 0f;
        floats["spawnRangeMax"] = 0f;

        floats["LRSpawnBalance"] = 0.5f;

		floats["softeningFactor"] = softeningFactor;

		ints["colorMode"] = colorMode;


		floats["time"] = Time.time;
		floats["timeScale"] = timeScale;
		//note - wait two seconds just in case there are hiccups on load
		if(Time.time > 2f) floats["fps"] = StaticFPS.frameRate;
		
		vectors["roomSize"] = roomSize;
		vectors["shipSize"] = Vector3.one * shipSize;

		foreach(KeyValuePair<string, Vector4> pair in vectors) compute.SetVector(pair.Key, pair.Value);
		foreach(KeyValuePair<string, float> pair in floats) compute.SetFloat(pair.Key, pair.Value);
		foreach(KeyValuePair<string, int> pair in ints) compute.SetInt(pair.Key, pair.Value);

		graphics[2].SetFloat("_PointSize", Mathf.Lerp(1f - audioStrengthGraphics, 1f + audioStrengthGraphics, audioData.avgVol));
		graphics[1].SetFloat("_LineLength", lineLength);
		graphics[0].SetColor("_Color", Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, Mathf.Lerp(0.5f - (audioStrengthGraphics * 0.5f), 0.5f + (audioStrengthGraphics * 0.5f), audioData.avgVol)));
		graphics[1].SetColor("_Color", Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, Mathf.Lerp(0.5f - (audioStrengthGraphics * 0.5f), 0.5f + (audioStrengthGraphics * 0.5f), audioData.avgVol)));
		graphics[2].SetFloat("_LineLength", lineLength);
	}

	void Update() {

		if(viveLeft.GetButton("Grip") || viveRight.GetButton("Grip")) {
			if(introFinished) {
				bool bothHeld = viveLeft.GetButton("Grip") && viveRight.GetButton("Grip");
				timeScale = Mathf.Lerp(timeScale, bothHeld ? 0f : 0.1f, 0.05f);
			}
		} else {
			if(introFinished) {
				timeScale = Mathf.Lerp(timeScale, 1f, 0.05f);
			}
		}

		
		if(audioData.avgVol == 0) {
			audioZeroTime += Time.deltaTime;
		} else {
			audioZeroTime = 0f;
		}

		noAudio = audioZeroTime > 5f;
	}
	public IEnumerator burstOut() {
		compute.SetInt("customMode", 1);
		yield return new WaitForEndOfFrame();
		compute.SetInt("customMode", 0);
		introFinished = true;
	}

	public IEnumerator suckIn() {
		compute.SetInt("customMode", 2);
		yield return null;
	}
}