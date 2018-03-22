using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class DispatchQueue : MonoBehaviour {

		[System.Serializable]
		public class DispatchOrderItem {
			public ComputeDispatcher Dispatcher;
			public bool UseEvent;
			public UnityEngine.Events.UnityEvent DispatchEvent;
			public bool Enabled;
			public int DispatchInterval;
		}

		[HideInInspector] public List<DispatchOrderItem> _Dispatchers;
		public List<DispatchOrderItem> Dispatchers {
			get {
				if(_Dispatchers == null) _Dispatchers = new List<DispatchOrderItem>();
				return _Dispatchers;
			} set {
				_Dispatchers = value;
			}
		}
		
		void Start () {
			for(int i = 0; i < Dispatchers.Count; i++) {
				EnforceNoAutoDispatch(i);
			}
		}
	
		void Update () {
			for(int i = 0; i < Dispatchers.Count; i++) {
				EnforceNoAutoDispatch(i);

				if(Dispatchers[i].Enabled && Time.frameCount % Dispatchers[i].DispatchInterval == 0) {
					Dispatchers[i].DispatchEvent.Invoke();
					if(Dispatchers[i].Dispatcher != null) {
						Dispatchers[i].Dispatcher.Dispatch();
					}
				}
			}
		}

		void EnforceNoAutoDispatch(int index) {
			if(index < 0 || index >= Dispatchers.Count) return;

			if(Dispatchers[index].Dispatcher != null) {
				if(Dispatchers[index].Dispatcher.AutoDispatch) {
					Debug.LogWarning("Dispatcher " + index + " (" + Dispatchers[index].Dispatcher.gameObject.name + ") has auto dispatch set to on. " + gameObject.name + "'s DispatchOrder component is switching it off.");
					Dispatchers[index].Dispatcher.AutoDispatch = false;
				}
			}
		}

		//----------------------------------
		//----Custom inspector methods------
		//----------------------------------

		public void MoveItemUp(int index) {
			if(index < Dispatchers.Count - 1) {
				DispatchOrderItem a = Dispatchers[index];
				DispatchOrderItem b = Dispatchers[index + 1];
				Dispatchers[index] = b;
				Dispatchers[index + 1] = a;
			}
		}

		public void MoveItemDown(int index) {
			if(index > 0) {
				DispatchOrderItem a = Dispatchers[index];
				DispatchOrderItem b = Dispatchers[index - 1];
				Dispatchers[index] = b;
				Dispatchers[index + 1] = a;
			}
		}

		public void AddItem() {
			DispatchOrderItem NewItem = new DispatchOrderItem();
			Dispatchers.Add(NewItem);
		}

		public void RemoveItem(int index) {
			if(index >= 0 && index < Dispatchers.Count) {
				Dispatchers.RemoveAt(index);
			}
		}
	}

}