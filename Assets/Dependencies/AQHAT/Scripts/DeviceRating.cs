using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text.RegularExpressions;
using eageramoeba.DeviceRating;
using System.IO;

namespace eageramoeba.DeviceRating {

	public class DeviceRating : MonoBehaviour {
		protected DeviceRating() { }
		public bool runOnStart = true;

		[Header("Hardware Cap Settings")]
		[Tooltip("Enable hardware caps, to disable individual values set to -1 or lower. Defualt enabled")]
		public bool hardwareCaps = true;
		[Tooltip("Cap the amount of memory (RAM) the system takes into account. Default 16384")]
		public float memoryCap = 16384;
		[Tooltip("Cap the amount of graphics memory (GRAM) the system takes into account. Default 4096")]
		public float graphicsMemoryCap = 4096;
		[Tooltip("Cap the number of processor cores the system takes into account. Default 4")]
		public float processorCoreCap = 4;
		[Tooltip("Set the maximum processor frequency the system takes into account. Default -1 (disabled)")]
		public float processorFrequencyCap = -1;
		[Space(10)]

		[Header("Hardware Weight Settings")]
		[Tooltip("Graphics multithreading weight, if multithreading is available this modifies the graphics memory score. Multiplies by value set. Set to 1 to disable. Default 1")]
		public float multiThreadWeight = 1.25f;
		[Tooltip("Graphics memory weight, set to 0 to disable. Default 1")]
		public float graphicsMemoryWeight = 1f;
		[Tooltip("Graphics shader weight, set to 0 to disable. Default 1")]
		public float graphicsShaderWeight = 1f;
		[Tooltip("Memory (RAM) weight, set to 0 to disable. Default 1")]
		public float memoryWeight = 1f;
		[Tooltip("Processor core weight, set to 0 to disable. Default 1")]
		public float processorCoreWeight = 1f;
		[Tooltip("Processor frequency weight, set to 0 to disable. Default 1")]
		public float processorFrequencyWeight = 1f;
		[Space(10)]

		[Header("Generic Platform Weight Settings")]
		[Tooltip("Enable platform weights (done before OS weights), each value alters the score (multiply by value) depending on platform type. Default disabled")]
		public bool platformWeights = false;
		[Tooltip("Handheld device weight, phones etc. Default 1")]
		public float handheldWeight = 1;
		[Tooltip("Console weight, Playstation etc. Default 2")]
		public float consoleWeight = 2;
		[Tooltip("Desktop weight, Windows etc. Default 2")]
		public float desktopWeight = 2;
		[Tooltip("Unknown platform weight, default used. Default 2")]
		public float unknownWeight = 2;
		[Space(10)]

		[Header("OS Weight Settings")]
		[Tooltip("Enable OS weights (done after platform weights), each value alters the score (multiply by value) depending on platform type. Default enabled")]
		public bool osWeights = true;
		[Tooltip("Windows Phone 8 weight. Default 0.7")]
		public float windowsPhone8 = 0.7f;
		[Tooltip("UWP ARM (mobile) weight. Default 0.7")]
		public float windowsStoreARM = 0.7f;
		[Tooltip("UWP x86 weight. Default 1")]
		public float windowsStoreX86 = 1f;
		[Tooltip("UWP x64 weight. Default 1")]
		public float windowsStoreX64 = 1f;
		[Tooltip("Apple IOS weight. Default 1.1")]
		public float IOS = 1.1f;
		[Tooltip("Apple tvOS weight, Unity 5.3 or newer. Default 1.1")]
		public float tvOS = 1.1f;
		[Tooltip("Android weight. Default 1")]
		public float android = 1;
		[Space(5)]
		[Tooltip("XBOX 360 weight, Unity 4 only. Default 1")]
		public float xbox360 = 1;
		[Tooltip("XBOX ONE weight. Default 1")]
		public float xboxOne = 1;
		[Tooltip("PS3 weight, Unity 4 only. Default 1")]
		public float ps3 = 1;
		[Tooltip("PS4 weight. Default 1")]
		public float ps4 = 1;
		[Tooltip("PS Vita weight. Default 1")]
		public float psVita = 1;
		[Tooltip("Wii U weight, Unity 5.3 or newer. Default 1")]
		public float wiiU = 1;
		[Space(5)]
		[Tooltip("Tizen weight. Default 1")]
		public float tizen = 1;
		[Tooltip("Samsung TV weight. Default 1")]
		public float samsungTV = 1;
		[Tooltip("Web GL weight. Default 1")]
		public float webGL = 1;
		[Space(5)]
		[Tooltip("Windows weight. Default 1")]
		public float windows = 1;
		[Tooltip("OSX weight. Default 1")]
		public float osx = 1;
		[Tooltip("Linux weight. Default 1")]
		public float linux = 1;
		[Space(10)]

		[Header("IOS processor details file")]
		[Tooltip("A file containing all processor details for IOS/TVOS devices")]
		public TextAsset IosTvosProcessorFile;
		private string[] iosProcessorDetails;
		[Space(10)]

		[Header("Experimental hardware scan files")]
		[Tooltip("Use this method? (Overrides exact name). Default true")]
		public bool useExperimental = true;
		[Space(5)]
		[Tooltip("A file containing criteria for scanning and rating GPU names/criteria, min value of 0. Default set to orig file")]
		public TextAsset gfxScanFile;
		[Tooltip("Weight applied to GPU scan result, applied before minimum cap")]
		public float gfxScanWeight = 1;
		[Space(5)]
		[Tooltip("A file containing criteria for scanning and rating CPU names/criteria, min value of 0. Default set to orig file")]
		public TextAsset processorScanFile;
		[Tooltip("Weight applied to CPU scan result, applied before minimum cap")]
		public float processorScanWeight = 1;
		[Space(10)]

		[Header("Benchmark CSV hardware scan files")]
		[Tooltip("Use this method? (Overrided by experimental). Default false")]
		public bool useExact = false;
		[Tooltip("Field used to define GPU name in CSV file")]
		public string gpuNameFieldID = "Videocard";
		[Tooltip("Field used to define GPU rating in CSV file")]
		public string gpuRatingFieldID = "3D Rating";
		[Tooltip("A file containing criteria for scanning and rating GPU names/criteria, min value of 0. Default set to orig file")]
		public TextAsset gfxScanFileCsv;
		[Tooltip("Weight applied to GPU scan result, applied before minimum cap")]
		public float gfxScanWeightCsv = 0.001f;
		[Space(5)]
		[Tooltip("Field used to define CPU name in CSV file")]
		public string cpuNameFieldID = "CPU name";
		[Tooltip("Field used to define CPU rating in CSV file")]
		public string cpuRatingFieldID = "Rating";
		[Tooltip("A file containing criteria for scanning and rating CPU names/criteria, min value of 0. Default set to orig file")]
		public TextAsset processorScanFileCsv;
		[Tooltip("Weight applied to CPU scan result, applied before minimum cap")]
		public float processorScanWeightCsv = 0.001f;
		[Space(10)]

		[Header("Score Values")]
		[Tooltip("Total score")]
		public float totalScore;
		public static float TotalScore;
		[Tooltip("Total score divided by 6")]
		public float averageScore;
		public static float AverageScore;
		public static float GFXScore;
		public static float ProcessorScore;
		public static float GFXScoreNoScan;
		public static float ProcessorScoreNoScan;
		public static float MemoryScore;
		public static float ScanProcessorScore;
		public static float ScanGFXScore;
		public static float processorCores;
		public static string processorName;
		public static float processorFrequency;
		[Space(20)]

		[Header("Debug")]
		[Tooltip("Enabled ebug overlay")]
		public bool debugOverlay;
		[Space(20)]

		[Header("Cache file")]
		[Tooltip("Use the cache file to skip running the script if a match is detected.")]
		public bool useCache = true;
		[Tooltip("Cache file path to store data from first run, if subsequenct runs match hardware data cache is used.")]
		public string relativeFileCachePath = "/AQHAT/Data/cache/";
		[Tooltip("Cache file to store data from first run, if subsequenct runs match hardware data cache is used.")]
		public string relativeFileCache = "ratingcache.txt";
		private string fileCachePathSt = "";
		static string fileCachePathStStatic = "";
		private string cacheContents;

		private bool applyPCap;
		private bool applyPFCap;
		private bool applyMCap;
		private bool applyGMCap;

		private float mem;
		private float gMem;
		private string gfxN;
		private float procC;
		private string procN;
		private float procF;
		private float gShdr;
		private bool gMulti;
		private DeviceType devT;

		private float memScore;
		private float procFScore;
		private float procCScore;
		private float gMemScore;
		private float gShdrScore;
		private float devMult;
		private float osMult;
		private float gfx2Mult;
		private float proc2Mult;
		private string devName;
		private string devModel;
		private string osS;
		private string usingCache = "";

		static GameObject meOBJ;

		[Space(20)]

		[Header("Mock values")]
		[Tooltip("Used to test different GPUs")]
		public string mockGPU;
		[Tooltip("Used to test different Processors")]
		public string mockProcessor;
		[Tooltip("Used to test different IOS/TVOS devices (does not kick OS multiply in)")]
		public string mockIosTvos;
		[Tooltip("Used to test different RAM/memory compositions, set to -1 to disable")]
		public float mockMemory = -1;
		[Tooltip("Used to test different GPU RAM/memory compositions, set to -1 to disable")]
		public float mockGpuMemory = -1;
		[Tooltip("Used to test different processor core compositions, set to -1 to disable")]
		public float mockProcessorCores = -1;
		[Tooltip("Used to test different processor frequency compositions, set to -1 to disable")]
		public float mockProcessorFrequency = -1;
		[Tooltip("Used to test different GPU shader compositions, set to -1 to disable")]
		public float mockGpuShader = -1;


#if !UNITY_5_3_OR_NEWER && !UNITY_5_3
		[Space(20)]
		
		[Header("Unity 4 and 5 (under 5.3) processor estimates")]
		[Tooltip("Unity 4 and 5 (under 5.3) cannot read processor frequency, this is the value used on handheld devices")]
		public float handheldProcessorEstimate = 1200;
		[Tooltip("Unity 4 and 5 (under 5.3) cannot read processor frequency, this is the value used on console devices")]
		public float consoleProcessorEstimate = 1900;
		[Tooltip("Unity 4 and 5 (under 5.3) cannot read processor frequency, this is the value used on desktop devices")]
		public float desktopProcessorEstimate = 2600;
#endif

        private NumberFormatInfo numFmt = new NumberFormatInfo();

		// Use this for initialization
		void Start() {
            numFmt.NegativeSign = "-";
            meOBJ = gameObject;
			if (runOnStart) {
				RunMe();
			}
		}

		// Update is called once per frame
		void Update() {
			//DetectScore();
		}

		void OnGUI() {
			if (debugOverlay)  // or check the app debug flag
			{
				getValues();
				string devDetails = "Device name: " + devName + "\nModel: " + devModel + "\nOS: " + osS + "\nP Name:" + procN + "\nGFX:" + gfxN + ", "
#if UNITY_5_3_OR_NEWER || UNITY_5_1 || UNITY_5_2 || UNITY_5_3
				+ SystemInfo.graphicsDeviceType
#endif
					;
				string scoreT = "Current Quality: "+(QualitySettings.GetQualityLevel()+1)+"/"+QualitySettings.names.Length+ ", "+QualitySettings.names[QualitySettings.GetQualityLevel()]+"\n\nTotal Score: " + totalScore + "\nMean Score: " + averageScore;
				string systemT = "RAM: " + mem + "\nCores: " + procC + "\nP Freq: " + procF + "\nGFX RAM: " + gMem + "\nShader V: " + gShdr + "\nMulti-Thr: " + gMulti+"\n\nOS Multiply: "+osMult+"\nPlat Multiply: "+devMult+"\nGFX Dev Scr: "+gfx2Mult+"\nProc Dev Scr: "+proc2Mult+ usingCache;
				GUI.Label(new Rect(10, 10, 200, 600), scoreT + "\n\n" + devDetails + "\n\n" + systemT);
			}
		}

		int parseInt(string intVal){
			int tInt = 0;
			if(!int.TryParse(intVal, NumberStyles.Integer, numFmt, out tInt)){
				Debug.LogWarning("Parsing the following string to int failed '"+intVal+"', please contact Eager Amoeba with this message and your hardware configuration.");
			}
			return tInt;
		}

		float parseFloat(string fltVal){
			float tFloat = 0;
			if(!float.TryParse(fltVal, NumberStyles.Float, numFmt, out tFloat)){
				Debug.LogWarning("Parsing the following string to float failed '"+fltVal+"', please contact Eager Amoeba with this message and your hardware configuration.");
			}
			return tFloat;
		}

		void writeFile() {
			cacheContents = gfxN + procN + devName + devModel + osS + mem + procF + gMem + procC + gShdr;
			cacheContents = cacheContents.Replace (",", "");
			cacheContents += ",";
			cacheContents += GFXScoreNoScan+",";
			cacheContents += GFXScore + ",";
			cacheContents += MemoryScore + ",";
			cacheContents += ProcessorScoreNoScan + ",";
			cacheContents += ProcessorScore + ",";
			cacheContents += ScanGFXScore + ",";
			cacheContents += ScanProcessorScore + ",";
			cacheContents += TotalScore + ",";
			cacheContents += AverageScore + ",";
			cacheContents += proc2Mult + ",";
			cacheContents += gfx2Mult;
#if UNITY_5_3_OR_NEWER || UNITY_5_3
			if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
			if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
			if(Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
				if (relativeFileCache != "") {
#if !UNITY_WEBPLAYER && !UNITY_SAMSUNGTV
					File.WriteAllText(fileCachePathSt, cacheContents);
#endif
				}
			} else {
				PlayerPrefs.SetString("cacherating", cacheContents);
			}
		}

		public static void wipeCache() {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
			if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
			if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
			if(Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
				if (fileCachePathStStatic != "") {
#if !UNITY_WEBPLAYER && !UNITY_SAMSUNGTV
					File.WriteAllText(DeviceRating.fileCachePathStStatic, "");
#endif
				}
			} else {
				PlayerPrefs.SetString("cacherating", "");
			}
		}

		bool getFileDetails() {
			if (cacheContents != "" && useCache) {
				string[] splitCache = cacheContents.Split(',');
				string tVal = gfxN + procN + devName + devModel + osS + mem + procF + gMem + procC + gShdr;
				tVal = tVal.Replace (",", "");
				if (splitCache[0].ToString() == tVal) {
					GFXScoreNoScan = parseFloat(splitCache[1]);
					GFXScore = parseFloat(splitCache[2]);
					MemoryScore = parseFloat(splitCache[3]);
					ProcessorScoreNoScan = parseFloat(splitCache[4]);
					ProcessorScore = parseFloat(splitCache[5]);
					ScanGFXScore = parseFloat(splitCache[6]);
					ScanProcessorScore = parseFloat(splitCache[7]);
					TotalScore = parseFloat(splitCache[8]);
					AverageScore = parseFloat(splitCache[9]);
					proc2Mult = parseFloat(splitCache[10]);
					gfx2Mult = parseFloat(splitCache[11]);
					totalScore = TotalScore;
					averageScore = AverageScore;
					usingCache = "\n\nUSING CACHE";
					return true;
				}
			}
			return false;
		}

		static public void RunMe() {
			meOBJ.GetComponent<DeviceRating>().DetectScore();
		}

		public void DetectScore() {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
			if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
			if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
			if(Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
				cacheContents = "";
				DeviceRating.fileCachePathStStatic = "";
				if (relativeFileCache != "") {
					fileCachePathSt = Application.persistentDataPath + relativeFileCachePath + relativeFileCache;
					DeviceRating.fileCachePathStStatic = fileCachePathSt;
					if (!Directory.Exists(Application.persistentDataPath + relativeFileCachePath)) {
						Directory.CreateDirectory(Application.persistentDataPath + relativeFileCachePath);
					}
					if (!File.Exists(fileCachePathSt)) {
						//File.CreateText(fileCachePathSt);
						cacheContents = "";
					} else {
#if !UNITY_WEBPLAYER && !UNITY_SAMSUNGTV
						cacheContents = File.ReadAllText(fileCachePathSt);
#endif
					}
				} else {
					Debug.Log("Device Rating: Relative file cache file name is blank, cache will not be used.");
				}
			} else {
				cacheContents = PlayerPrefs.GetString("cacherating");
			}
			getDataFiles();
			getValues();
			applyCaps();
			getWeights();
			if (!getFileDetails()) {
				if (useExperimental) {
					hdwrDevScrAll();
				}
				if (useExact) {
					scanExactHardwareNames();
				}
				getWeights();
				getScores();
				calcScores();
				writeFile();
			}
		}

		void getDataFiles() {
			if (IosTvosProcessorFile) {
				string tS = IosTvosProcessorFile.text.Replace("\r", "").Replace("\n", "");
				iosProcessorDetails = tS.Split(';');
			} else {
				Debug.Log("Device Rating: IOS processor details file is unassigned, IOS processor details will not be read.");
			}
		}

		void getValues() {
			devT = SystemInfo.deviceType;
#if UNITY_5
			gMulti = SystemInfo.graphicsMultiThreaded;
#else
			gMulti = false;
#endif
			gMem = SystemInfo.graphicsMemorySize;
			gShdr = SystemInfo.graphicsShaderLevel;
			gfxN = SystemInfo.graphicsDeviceName;
			mem = SystemInfo.systemMemorySize;
			procC = SystemInfo.processorCount;
			devName = SystemInfo.deviceName;
			devModel = SystemInfo.deviceModel;
			osS = SystemInfo.operatingSystem;
			if (devModel.Contains("Watch") ||
#if UNITY_5_3_OR_NEWER || UNITY_5_3
					Application.platform == RuntimePlatform.tvOS ||
#endif
				Application.platform == RuntimePlatform.IPhonePlayer || mockIosTvos != "") {
				procF = getIOS();
			} else {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
				procF = SystemInfo.processorFrequency;
#else
				if (devT == DeviceType.Handheld) {
				procF = handheldProcessorEstimate;
			} else if (devT == DeviceType.Console) {
				procF = consoleProcessorEstimate;
			} else if (devT == DeviceType.Desktop) {
				procF = desktopProcessorEstimate;
			}
#endif
				procN = SystemInfo.processorType;
			}
			if (mockGPU != "") {
				gfxN = mockGPU;
			}
			if (mockProcessor != "") {
				procN = mockProcessor;
			}
			if (mockGpuMemory >= 0) {
				gMem = mockGpuMemory;
			}
			if (mockGpuShader >= 0) {
				gShdr = mockGpuShader;
			}
			if (mockMemory >= 0) {
				mem = mockMemory;
			}
			if (mockProcessorCores >= 0) {
				procC = mockProcessorCores;
			}
			if (mockProcessorFrequency >= 0) {
				procF = mockProcessorFrequency;
			}
			DeviceRating.processorName = procN;
			DeviceRating.processorFrequency = procF;
			DeviceRating.processorCores = procC;
		}

		float getIOS() {
			if (iosProcessorDetails.Length > 1) {
				int i = 0;
				string[] tempArr;
				string tDN = devModel;
				string tDN2;
				if (mockIosTvos != "") {
					tDN = mockIosTvos;
				}
				for (i = 0; i < iosProcessorDetails.Length; i++) {
					tempArr = iosProcessorDetails[i].Split('/');
					tDN2 = tempArr[0].Replace(" ", "");
					tDN = tDN.Replace(" ", "");
					tDN = tDN.Replace('.', ',');
					if (tDN.ToLower() == tDN2.ToLower()) {
						procC = parseInt(tempArr[1]);
						procN = tempArr[3];
						return parseInt(tempArr[2]);
					}
				}
				procC = 2;
				procN = "Apple device model guess, no match";
			}
			return 1500;
		}

		void applyCaps() {
			if (hardwareCaps) {
				if (memoryCap >= 0) {
					applyMCap = true;
				}
				if (graphicsMemoryCap >= 0) {
					applyGMCap = true;
				}
				if (processorCoreCap >= 0) {
					applyPCap = true;
				}
				if (processorFrequencyCap >= 0) {
					applyPFCap = true;
				}
				if (applyMCap) {
					if (mem > memoryCap) {
						mem = memoryCap;
					}
				}
				if (applyGMCap) {
					if (gMem > graphicsMemoryCap) {
						gMem = graphicsMemoryCap;
					}
				}
				if (applyPCap) {
					if (procC > processorCoreCap) {
						procC = processorCoreCap;
					}
				}
				if (applyPFCap) {
					if (procF > processorFrequencyCap) {
						procF = processorFrequencyCap;
					}
				}
			}
		}

		void getScores() {
			gMemScore = gMem / 1024 * graphicsMemoryWeight * multiThreadWeight;
			memScore = mem / 512 * memoryWeight;
			gShdrScore = gShdr / 25 * graphicsShaderWeight;
			procCScore = procC * 0.75f * processorCoreWeight;
			procFScore = procF / 600f * processorFrequencyWeight;
		}

		void getWeights() {
			if (platformWeights) {
				devMult = unknownWeight;
				if (devT == DeviceType.Handheld) {
					devMult = handheldWeight;
				} else if (devT == DeviceType.Console) {
					devMult = consoleWeight;
				} else if (devT == DeviceType.Desktop) {
					devMult = desktopWeight;
				}
			}
			if (osWeights) {
				osMult = 1;
				if (Application.platform == RuntimePlatform.Android) {
					osMult = android;
				}
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					osMult = IOS;
				}
				if (Application.platform == RuntimePlatform.LinuxPlayer
#if UNITY_5_5_OR_NEWER
				|| Application.platform == RuntimePlatform.LinuxEditor
#endif
				) {
					osMult = linux;
				}
				if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor
#if !UNITY_5_5_OR_NEWER
				|| Application.platform == RuntimePlatform.OSXDashboardPlayer
#endif
				) {
					osMult = osx;
				}
#if !UNITY_5_5_OR_NEWER
				if (Application.platform == RuntimePlatform.PS3) {
					osMult = ps3;
				}
#endif
				if (Application.platform == RuntimePlatform.PS4) {
					osMult = ps4;
				}
				if (Application.platform == RuntimePlatform.PSP2) {
					osMult = psVita;
				}
				if (Application.platform == RuntimePlatform.SamsungTVPlayer) {
					osMult = samsungTV;
				}
				if (Application.platform == RuntimePlatform.TizenPlayer) {
					osMult = tizen;
				}
#if UNITY_5
				if (Application.platform == RuntimePlatform.WebGLPlayer) {
					osMult = webGL;
				}
#endif

#if UNITY_5_3_OR_NEWER || UNITY_5_3
				if (Application.platform == RuntimePlatform.tvOS) {
					osMult = tvOS;
				}		
				if (Application.platform == RuntimePlatform.WiiU) {
					osMult = wiiU;
				}
#endif
				if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) {
					osMult = windows;
				}
#if !UNITY_5_3_OR_NEWER
				if (Application.platform == RuntimePlatform.WP8Player) {
					osMult = windowsPhone8;
				}
#endif
#if UNITY_5
				if (Application.platform == RuntimePlatform.WSAPlayerARM) {
					osMult = windowsStoreARM;
				}
				if (Application.platform == RuntimePlatform.WSAPlayerX64) {
					osMult = windowsStoreX64;
				}
				if (Application.platform == RuntimePlatform.WSAPlayerX86) {
					osMult = windowsStoreX86;
				}
#endif
#if !UNITY_5_5_OR_NEWER
				if (Application.platform == RuntimePlatform.XBOX360) {
					osMult = xbox360;
				}
#endif
				if (Application.platform == RuntimePlatform.XboxOne) {
					osMult = xboxOne;
				}
			}
		}

		void calcScores() {
			GFXScoreNoScan = gMemScore + gShdrScore;
			GFXScore = GFXScoreNoScan + gfx2Mult;
			MemoryScore = memScore;
			ProcessorScoreNoScan = procCScore + procFScore;
			ProcessorScore = ProcessorScoreNoScan + proc2Mult;
			ScanGFXScore = gfx2Mult;
			ScanProcessorScore = proc2Mult;
			totalScore = ProcessorScore + MemoryScore + GFXScore;
			if (platformWeights) {
				totalScore = devMult * totalScore;
			}
			if (osWeights) {
				totalScore = osMult * totalScore;
			}
			averageScore = totalScore / 6;
			TotalScore = totalScore;
			AverageScore = averageScore;
		}

		void hdwrDevScrAll() {
			string tS;
			string[] hdwrDArrO;
			if (gfxScanFile) {
				tS = gfxScanFile.text;
				tS = tS.Replace("\r", "").Replace("\n", "");
				hdwrDArrO = tS.Split(';');
				for (int i = 0; i < hdwrDArrO.Length; i++) {
					gfx2Mult += hdwrDevScr(hdwrDArrO[i], mockGPU, gfxN);
				}
				gfx2Mult *= gfxScanWeight;
				if (gfx2Mult < 0) {
					gfx2Mult = 0;
				}
			} else {
				Debug.Log("Device Rating: Experimental GFX scan file is unassigned, GPU will not be scanned experimentally.");
			}
			if (processorScanFile) {
				tS = processorScanFile.text;
				tS = tS.Replace("\r", "").Replace("\n", "");
				hdwrDArrO = tS.Split(';');
				for (int i = 0; i < hdwrDArrO.Length; i++) {
					proc2Mult += hdwrDevScr(hdwrDArrO[i], mockProcessor, procN);
				}
				proc2Mult *= processorScanWeight;
				if (proc2Mult < 0) {
					proc2Mult = 0;
				}
			} else {
				Debug.Log("Device Rating: Experimental processor scan file is unassigned, CPU will not be scanned experimentally.");
			}
		}

		float hdwrDevScr(string gfxD, string mockGPU, string gfxDevNT) {
			gfxD = gfxD.ToLower();
			string[] gfxDArrO = gfxD.Split(',');
			string[] gfxDArr;
			string[] numericA;
			string[] numericA2;
			float tempMultiply = 0;
			string tempg1 = "";
			if (mockGPU != "") {
				gfxDevNT = mockGPU;
			}
			int verNum2;
			gfxDevNT = gfxDevNT.ToLower();
			string verName = gfxDevNT;
			verName = verName.Replace("(tm)-", "");
			verName = verName.Replace("(tm)", "");
			verName = verName.Replace("tm-", "");
			verName = verName.Replace("tm", "");
			verName = verName.Replace("(r)", "");
			if (verName.Contains("@")) { 
				verName = verName.Split('@')[0];
			}
			int verNum = 0;
			if (Regex.Replace(verName, "[^0-9]", "") != "") {
				verNum = parseInt(Regex.Replace(verName, "[^0-9]", ""));
			}
			string vNT1 = "";
			int vNT2 = 0;
			int numZ = 0;
			bool foundDesc = false;
			bool pass1st = false;
			string[] gfxDarrCM;
			string[] gfxDarrNI;
			string niGfx;
			string tt1;
			int i2 = 0;
			if (gfxDevNT != null && gfxDevNT != "") {
				for (int i = 0; i < gfxDArrO.Length; i++) {
					pass1st = false;
					tempg1 = gfxDArrO[i];
					if (tempg1.Contains("/")) {
						gfxDArr = gfxDArrO[i].Split('/');
						niGfx = "";
						if (gfxDArr.Length > 5) {
							niGfx = gfxDArr[5];
						}
						if (gfxDArr[4] == "c") {
							if (verName.Contains(gfxDArr[0])) {
								pass1st = true;
							}
						} else if (gfxDArr[4] == "m") {
							if (gfxDArr[0] == verName.Substring(0, verName.Length - 1)) {
								pass1st = true;
							}
						} else if (gfxDArr[4] == "cm") {
							gfxDarrCM = gfxDArr[0].Split('.');
							pass1st = true;
							for (i2 = 0; i2 < gfxDarrCM.Length; i2++) {
								if (!verName.Contains(gfxDarrCM[i2])) {
									pass1st = false;
								}
							}
						}
						if (niGfx != "") {
							gfxDarrNI = gfxDArr[5].Split('.');
							for (i2 = 0; i2 < gfxDarrNI.Length; i2++) {
								if (verName.Contains(gfxDarrNI[i2])) {
									pass1st = false;
								}
							}
						}
						if (pass1st) {
							numericA = gfxDArr[2].Split('=');
							if (numericA[0] == "0") {
								tempMultiply += parseFloat(numericA[1]);
								foundDesc = true;
							} else {
								numZ = parseInt(gfxDArr[3]);
								verNum2 = 0;
								vNT1 = verNum + "";
								Debug.Log(vNT1);
								Debug.Log(vNT1+": "+numZ+" "+vNT1.Length);
								if (numZ == vNT1.Length) {
									vNT1 = vNT1.Substring(parseInt(gfxDArr[1]), parseInt(numericA[0]));
									if (vNT1.Length > 1) {
										verNum2 = parseInt(vNT1[1] + "");
										if(verNum2 == 0 && vNT1.Length >= 3){
											verNum2 = parseInt(vNT1[2] + "");
										}
									}
									vNT2 = parseInt(vNT1[0] + "");
									tempMultiply += parseFloat((vNT2 + "")[0] + "." + verNum2) * parseFloat(numericA[1]);
									foundDesc = true;
								}
							}
						}
					} else if(foundDesc) {
                        numericA = gfxDArrO[i].Split('=');
						if (numericA[0].Contains("*")) {
							numericA2 = numericA[0].Split('*');
							if (verName.Contains(numericA2[0] + "")) {
								tt1 = Regex.Replace(verName, "[^" + numericA2[0] + "]", "");
								if (tt1.Length == parseInt(numericA2[1])) {
									tempMultiply += parseFloat(numericA[1]);
								}
							}
						} else {
							if (verName.Contains(numericA[0] + "")) {
                                tempMultiply += parseFloat(numericA[1]);
							}
						}
					}
				}
				return tempMultiply;
			}
			return 1;
		}

		void scanExactHardwareNames() {
			string[,] csvArr;
			int i = 0;
			int nameF = 0;
			int ratingF = 0;
			string verName;

			if (gfxScanFileCsv) {
				verName = gfxN;
				csvArr = CSVReader.SplitCsvGrid(gfxScanFileCsv.text);
				if (mockGPU != "") {
					verName = mockGPU;
				}
				verName = verName.ToLower();
				verName = verName.Replace("(tm)-", "");
				verName = verName.Replace("(tm)", "");
				verName = verName.Replace("tm-", "");
				verName = verName.Replace("tm", "");
				for (i = 0; i < csvArr.GetLength(0); i++) {
					if (gpuNameFieldID == csvArr[i, 0]) {
						nameF = i;
					}
					if (gpuRatingFieldID == csvArr[i, 0]) {
						ratingF = i;
					}
				}
				for (i = 0; i < csvArr.GetLength(1); i++) {
					if (csvArr[nameF, i] != null) {
						if (verName.Contains(csvArr[nameF, i].ToLower())) {
							gfx2Mult = parseFloat(csvArr[ratingF, i]);
						}
					}
				}
				gfx2Mult = gfx2Mult * gfxScanWeightCsv;
				if (gfx2Mult < 0) {
					gfx2Mult = 0;
				}
			} else {
				Debug.Log("Device Rating: GPU benchmark data file is unassigned, GPU will not be scanned for benchmark data.");
			}

			if (gfxScanFileCsv) {
				csvArr = CSVReader.SplitCsvGrid(processorScanFileCsv.text);
				verName = procN;
				if (mockProcessor != "") {
					verName = mockProcessor;
				}
				verName = verName.ToLower();
				verName = verName.Replace("(tm)-", "");
				verName = verName.Replace("(tm)", "");
				verName = verName.Replace("tm-", "");
				verName = verName.Replace("tm", "");
				for (i = 0; i < csvArr.GetLength(0); i++) {
					if (cpuNameFieldID == csvArr[i, 0]) {
						nameF = i;
					}
					if (cpuRatingFieldID == csvArr[i, 0]) {
						ratingF = i;
					}
				}
				for (i = 0; i < csvArr.GetLength(1); i++) {
					if (csvArr[nameF, i] != null) {
						if (verName.Contains(csvArr[nameF, i].ToLower())) {
							proc2Mult = parseFloat(csvArr[ratingF, i]);
						}
					}
				}
				proc2Mult = proc2Mult * processorScanWeightCsv;
				if (proc2Mult < 0) {
					proc2Mult = 0;
				}
			} else {
				Debug.Log("Device Rating: CPU benchmark data file is unassigned, CPU will not be scanned for benchmark data.");
			}
		}
	}
}