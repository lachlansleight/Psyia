using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {

	public class DispatchQueueItem : MonoBehaviour {
		public ComputeDispatcher Dispatcher;
		public UnityEngine.Events.UnityEvent DispatchEvent;
		public bool Enabled;
		public int DispatchInterval;

		public void Dispatch() {
			if(Dispatcher != null) Dispatcher.Dispatch();
			DispatchEvent.Invoke();
		}
	}

}