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

public class ParticleBufferSetup : BufferSetup {

	protected override void CreateData() {
		ParticleData[] Data = new ParticleData[Count];
		for(int i = 0; i < Data.Length; i++) {
			Data[i].Position = Random.insideUnitSphere * 0.1f + new Vector3(0, 1, 0);
			Data[i].Velocity = Vector3.zero;
			Data[i].Color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
			Data[i].Color.w = 1f;
			Data[i].IsAlive = 0;
			Data[i].Size = 1f;
			Data[i].Random = Random.Range(0f, 1f);
			Data[i].Padding = 0f;
		}

		MyGpuBuffer.SetData(Data);
	}
}