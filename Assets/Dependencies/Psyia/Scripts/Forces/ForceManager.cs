/*

Copyright (c) 2018 Lachlan Sleight

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UCTK;

namespace Psyia {
	public class ForceManager : MonoBehaviour {

		public GpuBuffer ForceBuffer;

		private List<PsyiaForce> _Sources;
		private List<PsyiaForce> Sources {
			get {
				if(_Sources == null) _Sources = new List<PsyiaForce>();
				return _Sources;
			} set {
				_Sources = value;
			}
		}

		public string[] CurrentForces;
		
		void Update () {
			//debug
			CurrentForces = new string[Sources.Count];
			for(int i = 0; i < CurrentForces.Length; i++) {
				CurrentForces[i] = Sources[i].gameObject.name;
			}

			SetData();
		}

		public void AddSource(PsyiaForce NewSource) {
			if(Sources.Contains(NewSource)) return;

			Sources.Add(NewSource);

			UpdateCount();
		}

		public void RemoveSource(int i) {
			if(i < 0 || i >= Sources.Count) return;

			Sources.RemoveAt(i);

			UpdateCount();
		}

		public void RemoveSource(PsyiaForce NewSource) {
			if(!Sources.Contains(NewSource)) return;

			Sources.Remove(NewSource);

			UpdateCount();
		}

		void UpdateCount() {
			ForceBuffer.SetCount(Sources.Count);
		}

		void SetData() {
			ForceData[] OutputData = new ForceData[Sources.Count];

			if(ForceBuffer.Count != OutputData.Length) UpdateCount();

			for(int i = OutputData.Length - 1; i >= 0; i--) {
				if(Sources[i] == null) RemoveSource(i);

				OutputData[i] = Sources[i].gameObject.activeSelf ? Sources[i].GetForceData() : PsyiaForce.EmptyForceData;
			}
			ForceBuffer.SetData(OutputData);
		}
	}
}