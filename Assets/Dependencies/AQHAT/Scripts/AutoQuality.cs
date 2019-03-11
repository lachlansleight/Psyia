using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using eageramoeba.DeviceRating;
using System.IO;

public class AutoQuality : MonoBehaviour {

	[Tooltip("")]
	public bool runOnStart = true;
	[Space(10)]
	//[Tooltip("Use this to scan through all quality settings, set the length to the highest quality setting possible (in int format). Each value is the amount required to exceed that quality setting. Works by checking if the score is lower than each value until a match is found. Reverts to highest quality setting if none are found")]
    [HideInInspector]
	public float[] qualityBands = {2, 4, 6, 8, 10, 12 };
	[Tooltip("Use mean/average or for measurement, if disabled will revet to total")]
	public bool useAverage = true;
	static public bool scriptEn = true;
	private float getQual;
	private int setQual;
	private float getScore;
	[Space(10)]

	[Tooltip("Delays script start to allow the score to be calculated. Default is 0.1, measured in seconds.")]
	public float startDelay = 0.1f;

	[Header("Cache file")]
	[Tooltip("Use the cache file to skip running the script if a match is detected.")]
	public bool useCache = true;
	[Tooltip("Cache file to store data from first run, if subsequenct runs match hardware data cache is used.")]
	public string relativeFileCachePath = "/AQHAT/Data/cache/";
	[Tooltip("Cache file to store data from first run, if subsequenct runs match hardware data cache is used.")]
	public string relativeFileCache = "qualitycache.txt";
	private string fileCachePathSt = "";
	static string fileCachePathStStatic = "";
	private string cacheContents;

    static GameObject meOBJ;

    private NumberFormatInfo numFmt = new NumberFormatInfo();

    // Use this for initialization
    void Start () {
        numFmt.NegativeSign = "-";
        meOBJ = gameObject;
		if (runOnStart) {
			RunMe();
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
	}

	static public void RunMe() {
		meOBJ.GetComponent<AutoQuality>().runAll();
	}

	int parseInt(string intVal){
		int tInt = 0;
		if(!int.TryParse(intVal, NumberStyles.Integer, numFmt, out tInt)){
			Debug.LogWarning("Parsing the following string to int failed '"+intVal+"', please contact Eager Amoeba with this message and your hardware configuration.");
		}
		return tInt;
	}

	public void runAll() {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
		if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
		if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
		if (Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
			fileCachePathStStatic = "";
			cacheContents = "";
			if (relativeFileCache != "") {
				fileCachePathSt = Application.persistentDataPath + relativeFileCachePath + relativeFileCache;
				AutoQuality.fileCachePathStStatic = fileCachePathSt;
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
				Debug.Log("Auto Quality: Relative file cache file name is blank, cache will not be used.");
			}
		} else {
			cacheContents = PlayerPrefs.GetString("cachequality");
		}
		Invoke("detectQuality", startDelay+0.05f);
	}

	public static void wipeCache() {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
		if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
		if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
		if(Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
			if (AutoQuality.fileCachePathStStatic != "") {
#if !UNITY_WEBPLAYER && !UNITY_SAMSUNGTV
				File.WriteAllText(AutoQuality.fileCachePathStStatic, "");
#endif
			}
		} else {
			PlayerPrefs.SetString("cacherating", "");
		}
	}

	public static void writeQuality(int qualSet) {
#if UNITY_5_3_OR_NEWER || UNITY_5_3
		if (Application.platform != RuntimePlatform.tvOS && Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#elif UNITY_5
		if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.SamsungTVPlayer) {
#else
		if(Application.platform != RuntimePlatform.SamsungTVPlayer){
#endif
			if (AutoQuality.fileCachePathStStatic != "") {
#if !UNITY_WEBPLAYER && !UNITY_SAMSUNGTV
				File.WriteAllText(fileCachePathStStatic, qualSet + "");
#endif
			}
		} else {
			PlayerPrefs.SetString("cachequality", qualSet + "");
		}
	}

	void detectQuality() {
		if (cacheContents != "" && useCache) {
			QualitySettings.SetQualityLevel(parseInt(cacheContents));
		} else {
			if (gameObject.activeSelf && scriptEn) {
				if (useAverage) {
					getScore = DeviceRating.AverageScore;
				} else {
					getScore = DeviceRating.TotalScore;
				}
				getQual = QualitySettings.GetQualityLevel();
				setQual = getSetting();
				QualitySettings.SetQualityLevel(setQual);
				writeQuality(setQual);
			}
		}
	}

	int getSetting() {
		if (qualityBands.Length > 1) {
			for (int i = 0; i < qualityBands.Length; i++) {
				if (getScore < qualityBands[i]) {
					return i;
				}
			}
			return qualityBands.Length;
		} else {
			Debug.Log("Auto Quality: Quality band array set to 0, auto quality will not run. Setting quality to 0.");
			return 0;
		}
	}
}
