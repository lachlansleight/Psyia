using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Psyia;
using SimpleJSON;
using UnityEngine;
using Valve.VR;

public class Dance : MonoBehaviour
{
	[System.Serializable]
	public class TransformState
	{
		public Vector3 Position;
		public Quaternion Rotation;
	}
	
	[System.Serializable]
	public class ControllerInputState
	{
		public float Trigger;
		//public float Grip;
		//public Vector2 TouchpadPosition;
		//public float Touchpad;
		//public float Menu;
	}

	[System.Serializable]
	public class DanceMoment
	{
		public float Time;
		public TransformState HmdTransform;
		public TransformState LeftTransform;
		public TransformState RightTransform;
		public ControllerInputState LeftInput;
		public ControllerInputState RightInput;
	}

	public List<DanceMoment> Moments;

	public bool Record;
	public bool Playback;
	
	//TODO: Build Recorder (will require abstracting controller input) and Player
	public Transform RecordHmd;
	public Transform RecordLeft;
	public Transform RecordRight;
	public ControllerForce RecordForceLeft;
	public ControllerForce RecordForceRight;
	public float RecordInterval = 0.1f;

	public TextAsset DanceJson;
	public Transform PlaybackOffset;
	public Transform PlaybackHmd;
	public Transform PlaybackLeft;
	public Transform PlaybackRight;
	public ControllerForce PlaybackForceLeft;
	public ControllerForce PlaybackForceRight;

	private float _playbackTime;
	private int _playbackIndex;
	private int _lastPlaybackIndex;

	private float _recordTime;
	private float _lastRecordTime;

	public void Start()
	{
		ParseDanceJson(DanceJson);
	}

	public string GetDanceJson()
	{
		var outputJson = new JSONObject();

		outputJson.Add("DateTime", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		outputJson.Add("Duration", _recordTime);
		var momentArray = new JSONArray();
		foreach (var m in Moments) {
			var momentObject = new JSONObject();
				momentObject.Add("t", m.Time.ToString("0.000"));
			
				var hmdTransform = new JSONObject();
					hmdTransform.Add("posX", m.HmdTransform.Position.x.ToString("0.000"));
					hmdTransform.Add("posY", m.HmdTransform.Position.y.ToString("0.000"));
					hmdTransform.Add("posZ", m.HmdTransform.Position.z.ToString("0.000"));
					hmdTransform.Add("rotX", m.HmdTransform.Rotation.x.ToString("0.000"));
					hmdTransform.Add("rotZ", m.HmdTransform.Rotation.z.ToString("0.000"));
					hmdTransform.Add("rotW", m.HmdTransform.Rotation.w.ToString("0.000"));
					hmdTransform.Add("rotY", m.HmdTransform.Rotation.y.ToString("0.000"));
				momentObject.Add("hmdT", hmdTransform);
			
				var leftTransform = new JSONObject();
					leftTransform.Add("posX", m.LeftTransform.Position.x.ToString("0.000"));
					leftTransform.Add("posY", m.LeftTransform.Position.y.ToString("0.000"));
					leftTransform.Add("posZ", m.LeftTransform.Position.z.ToString("0.000"));
					leftTransform.Add("rotX", m.LeftTransform.Rotation.x.ToString("0.000"));
					leftTransform.Add("rotY", m.LeftTransform.Rotation.y.ToString("0.000"));
					leftTransform.Add("rotZ", m.LeftTransform.Rotation.z.ToString("0.000"));
					leftTransform.Add("rotW", m.LeftTransform.Rotation.w.ToString("0.000"));
				momentObject.Add("leftT", leftTransform);
			
				var rightTransform = new JSONObject();
					rightTransform.Add("posX", m.RightTransform.Position.x.ToString("0.000"));
					rightTransform.Add("posY", m.RightTransform.Position.y.ToString("0.000"));
					rightTransform.Add("posZ", m.RightTransform.Position.z.ToString("0.000"));
					rightTransform.Add("rotX", m.RightTransform.Rotation.x.ToString("0.000"));
					rightTransform.Add("rotY", m.RightTransform.Rotation.y.ToString("0.000"));
					rightTransform.Add("rotZ", m.RightTransform.Rotation.z.ToString("0.000"));
					rightTransform.Add("rotW", m.RightTransform.Rotation.w.ToString("0.000"));
				momentObject.Add("rightT", rightTransform);
			
				var leftInput = new JSONObject();
					leftInput.Add("tr", m.LeftInput.Trigger.ToString("0.000"));
				momentObject.Add("leftI", leftInput);
			
				var rightInput = new JSONObject();
					rightInput.Add("tr", m.RightInput.Trigger.ToString("0.000"));
				momentObject.Add("rightI", rightInput);
			
			momentArray.Add(momentObject);
		}

		outputJson.Add("Moments", momentArray);
		
		return outputJson.ToString(2);
	}

	public void ParseDanceJson(TextAsset text)
	{
		var json = (JSONObject) JSONNode.Parse(text.text);
		Moments = new List<DanceMoment>();

		var count = json["Moments"].AsArray.Count;
		for (var i = 0; i < count; i++) {
			Moments.Add(ParseFromNode(json["Moments"][i]));
		}
	}

	private DanceMoment ParseFromNode(JSONNode source)
	{
		return new DanceMoment()
		{
			Time = source["t"].AsFloat,
			HmdTransform = new TransformState()
			{
				Position = new Vector3(source["hmdT"]["posX"].AsFloat, source["hmdT"]["posY"].AsFloat, source["hmdT"]["posZ"].AsFloat),
				Rotation = new Quaternion(source["hmdT"]["rotX"].AsFloat, source["hmdT"]["rotY"].AsFloat, source["hmdT"]["rotZ"].AsFloat, source["hmdT"]["rotW"].AsFloat)
			},
			LeftTransform = new TransformState()
			{
				Position = new Vector3(source["leftT"]["posX"].AsFloat, source["leftT"]["posY"].AsFloat, source["leftT"]["posZ"].AsFloat),
				Rotation = new Quaternion(source["leftT"]["rotX"].AsFloat, source["leftT"]["rotY"].AsFloat, source["leftT"]["rotZ"].AsFloat, source["hmdT"]["rotW"].AsFloat)
			},
			RightTransform = new TransformState()
			{
				Position = new Vector3(source["rightT"]["posX"].AsFloat, source["rightT"]["posY"].AsFloat, source["rightT"]["posZ"].AsFloat),
				Rotation = new Quaternion(source["rightT"]["rotX"].AsFloat, source["rightT"]["rotY"].AsFloat, source["rightT"]["rotZ"].AsFloat, source["hmdT"]["rotW"].AsFloat)
			},
			LeftInput = new ControllerInputState()
			{
				Trigger = source["leftI"]["tr"].AsFloat
			},
			RightInput = new ControllerInputState()
			{
				Trigger = source["rightI"]["tr"].AsFloat
			}
		};
	}

	[ContextMenu("StartPlayback")]
	public void StartPlayback()
	{
		Playback = true;
		Record = false;
		_playbackTime = 0f;
	}

	[ContextMenu("StartRecording")]
	public void StartRecording()
	{
		Playback = false;
		Record = true;
		_recordTime = 0f;
		Moments = new List<DanceMoment>();
	}
	
#if UNITY_EDITOR
	[ContextMenu("Output to Project Directory")]
	public void DebugOutputJson()
	{
		var savePath = "C:\\Users\\Lachlan\\Development\\GitHub\\Psyia\\Assets\\_Data\\Dances\\JSON\\";
			
		var sw = new System.IO.StreamWriter(savePath + "dance_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".json");
		sw.Write(GetDanceJson());
		sw.Close();
		sw.Dispose();
	}
#endif

	public void FixedUpdate()
	{
		if (Playback) {
			PlaybackOffset.gameObject.SetActive(true);

			if (_playbackIndex < Moments.Count) {
				for (var i = _playbackIndex; i < Moments.Count; i++) {
					if (Moments[i].Time < _playbackTime) continue;

					_playbackIndex = i;
					break;
				}

				if(_playbackIndex != _lastPlaybackIndex)
					SetValues(_playbackIndex, _lastPlaybackIndex, _playbackTime);
			} else {
				Playback = false;
			}
			
			_lastPlaybackIndex = _playbackIndex;
			_playbackTime += Time.fixedDeltaTime;
		} else {
			PlaybackOffset.gameObject.SetActive(false);
			
			if (Record) {
				if (Time.fixedTime - _lastRecordTime > RecordInterval) {
					Moments.Add(RecordValues(_recordTime));
					_lastRecordTime = _recordTime;
				}
				
				_recordTime += Time.fixedDeltaTime;
			}
		}
	}

	private void SetValues(int index, int prevIndex, float time)
	{
		PlaybackHmd.localPosition = Vector3.Lerp(
			Moments[prevIndex].HmdTransform.Position,
			Moments[index].HmdTransform.Position,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		PlaybackHmd.localRotation = Quaternion.Lerp(
			Moments[prevIndex].HmdTransform.Rotation,
			Moments[index].HmdTransform.Rotation,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		
		PlaybackLeft.localPosition = Vector3.Lerp(
			Moments[prevIndex].LeftTransform.Position,
			Moments[index].LeftTransform.Position,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		PlaybackLeft.localRotation = Quaternion.Lerp(
			Moments[prevIndex].LeftTransform.Rotation,
			Moments[index].LeftTransform.Rotation,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		
		PlaybackRight.localPosition = Vector3.Lerp(
			Moments[prevIndex].RightTransform.Position,
			Moments[index].RightTransform.Position,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		PlaybackRight.localRotation = Quaternion.Lerp(
			Moments[prevIndex].RightTransform.Rotation,
			Moments[index].RightTransform.Rotation,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		
		PlaybackForceLeft.Value = Mathf.Lerp(
			Moments[prevIndex].LeftInput.Trigger,
			Moments[index].LeftInput.Trigger,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
		PlaybackForceRight.Value = Mathf.Lerp(
			Moments[prevIndex].RightInput.Trigger,
			Moments[index].RightInput.Trigger,
			Mathf.InverseLerp(
				Moments[prevIndex].Time,
				Moments[index].Time,
				time)
		);
	}

	private DanceMoment RecordValues(float time)
	{
		var moment = new DanceMoment()
		{
			Time = time,
			HmdTransform = new TransformState()
			{
				Position = RecordHmd.position,
				Rotation = RecordHmd.rotation
			},
			LeftTransform = new TransformState()
			{
				Position = RecordLeft.position,
				Rotation = RecordLeft.rotation
			},
			RightTransform = new TransformState()
			{
				Position = RecordRight.position,
				Rotation = RecordRight.rotation
			},
			LeftInput = new ControllerInputState()
			{
				Trigger = RecordForceLeft.Value
			},
			RightInput = new ControllerInputState()
			{
				Trigger = RecordForceRight.Value
			}
		};

		return moment;
	}
}