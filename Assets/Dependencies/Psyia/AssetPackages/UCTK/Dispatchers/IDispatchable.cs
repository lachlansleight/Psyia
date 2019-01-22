using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCTK {
	public interface IDispatchable {

		void Dispatch();
		bool GetAutoDispatch();
		void SetAutoDispatch(bool NewValue);

	}
}