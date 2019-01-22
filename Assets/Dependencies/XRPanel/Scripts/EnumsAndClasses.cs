using UnityEngine.Events;

namespace XRP
{
	[System.Serializable]
	public class UnityBoolEvent : UnityEvent<bool> { }
	
	[System.Serializable]
	public class UnityIntEvent : UnityEvent<int> { }
	
	[System.Serializable]
	public class UnityFloatEvent : UnityEvent<float> { }
	
	public enum State
	{
		Inactive,
		Hover,
		Touch,
		Press,
		Disabled
	}
}