using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {

	public class DispatchQueue : MonoBehaviour {

		public bool RunOnUpdate = true;
		public bool RunOnAwake = false;

		void Awake() {
			if(RunOnAwake) RunQueue();
		}

		void Start () {
			for(int i = 0; i < transform.childCount; i++) {
				EnforceNoAutoDispatch(i);
			}
		}
	
		void Update () {
			if(RunOnUpdate) RunQueue();
		}

		public void RunQueue() {
			for(int i = 0; i < transform.childCount; i++) {
				EnforceNoAutoDispatch(i);

				DispatchQueueItem CurrentItem = transform.GetChild(i).GetComponent<DispatchQueueItem>();
				if(transform.GetChild(i).gameObject.activeSelf && Time.frameCount % CurrentItem.DispatchInterval == 0) {
					CurrentItem.Dispatch();
				}
			}
		}

		void EnforceNoAutoDispatch(int index) {
			if(index < 0 || index >= transform.childCount) return;

			DispatchQueueItem CurrentItem = transform.GetChild(index).GetComponent<DispatchQueueItem>();
			if(CurrentItem.Dispatcher != null) {
				if(CurrentItem.Dispatcher.GetAutoDispatch()) {
					Debug.LogWarning("Dispatcher " + index + " has auto dispatch set to on. " + gameObject.name + "'s DispatchOrder component is switching it off.");
					CurrentItem.Dispatcher.SetAutoDispatch(false);
				}
			}
		}
	}

}