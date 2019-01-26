using System.IO;
using UnityEngine;
using SimpleJSON;

public class SaveGameInterface : MonoBehaviour
{
	private int _playCount;
	public int PlayCount
	{
		get { return _playCount; }
		set
		{
			var oldValue = _playCount;
			_playCount = value;
			if(oldValue != value) Save();
		}
	}
	private int _infiniteModeWhite;
	public int InfiniteModeWhite
	{
		get { return _infiniteModeWhite; }
		set
		{
			var oldValue = _infiniteModeWhite;
			_infiniteModeWhite = value;
			if(oldValue != value) Save();
		}
	}
	private int _infiniteModeBlue;
	public int InfiniteModeBlue
	{
		get { return _infiniteModeBlue; }
		set
		{
			var oldValue = _infiniteModeBlue;
			_infiniteModeBlue = value;
			if(oldValue != value) Save();
		}
	}
	private int _infiniteModeRed;
	public int InfiniteModeRed
	{
		get { return _infiniteModeRed; }
		set
		{
			var oldValue = _infiniteModeRed;
			_infiniteModeRed = value;
			if(oldValue != value) Save();
		}
	}
	private int _infiniteModeGreen;
	public int InfiniteModeGreen
	{
		get { return _infiniteModeGreen; }
		set
		{
			var oldValue = _infiniteModeGreen;
			_infiniteModeGreen = value;
			if(oldValue != value) Save();
		}
	}
	private int _infiniteModePink;
	public int InfiniteModePink
	{
		get { return _infiniteModePink; }
		set
		{
			var oldValue = _infiniteModePink;
			_infiniteModePink = value;
			if(oldValue != value) Save();
		}
	}
	

	private static SaveGameInterface _main;
	public static SaveGameInterface Main
	{
		get
		{
			if (_main == null) _main = FindObjectOfType<SaveGameInterface>();
			return _main;
		}
	}

	public void Awake()
	{
		if (!File.Exists(GetSavePath()))
			Save();
		else
			Load();
	}

	public void Load()
	{
		if (!File.Exists(GetSavePath())) return;
		var reader = new StreamReader(GetSavePath());
		var jsonString = reader.ReadToEnd();
		var jsonObject = JSONNode.Parse(jsonString);
		LoadFromJson(jsonObject);
	}

	public void Save()
	{
		var sw = new System.IO.StreamWriter(GetSavePath());
		sw.Write(GetJsonString());
		sw.Close();
		sw.Dispose();
	}

	private void LoadFromJson(JSONNode json)
	{
		_playCount = json["PlayCount"];
		_infiniteModeWhite = json["InfiniteModeWhite"];
		_infiniteModeBlue = json["InfiniteModeBlue"];
		_infiniteModeRed = json["InfiniteModeRed"];
		_infiniteModeGreen = json["InfiniteModeGreen"];
		_infiniteModePink = json["InfiniteModePink"];
	}

	private string GetJsonString()
	{
		var json = new JSONObject();
		json.Add("PlayCount", PlayCount);
		json.Add("InfiniteModeWhite", InfiniteModeWhite);
		json.Add("InfiniteModeBlue", InfiniteModeBlue);
		json.Add("InfiniteModeRed", InfiniteModeRed);
		json.Add("InfiniteModeGreen", InfiniteModeGreen);
		json.Add("InfiniteModePink", InfiniteModePink);
		return json.ToString(4);
	}

	private string GetSavePath()
	{
		#if UNITY_EDITOR
		return Application.dataPath + "/_Data/save.json";
		#else
		return Application.dataPath + "/../save.json";
		#endif
	}
}