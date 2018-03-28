using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foliar.Compute {
	public interface IDispatchable {

		void Dispatch();
		bool GetAutoDispatch();
		void SetAutoDispatch(bool NewValue);

	}
}