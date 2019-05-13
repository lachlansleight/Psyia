using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRP
{

	public class XrpForcedPointer : XrpPointer
	{
		public void OnEnable()
		{
			var panels = FindObjectsOfType<XrpPanel>();
		}
	}

}