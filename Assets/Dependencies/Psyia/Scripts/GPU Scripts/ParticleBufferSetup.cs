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

public class ParticleBufferSetup : BufferSetup
{

	private bool _hasDefaultData;
	private ParticleData _defaultData;

	private ParticleData[] _pastData;

	public void Awake()
	{
		if (!_hasDefaultData) {
			_defaultData = new ParticleData();
			_defaultData.Position = Random.insideUnitSphere * 0.1f + new Vector3(0, 1, 0);
			_defaultData.Velocity = Vector3.zero;
			_defaultData.Color = Color.Lerp(Color.red, Color.blue, Random.Range(0f, 1f));
			_defaultData.Color.w = 1f;
			_defaultData.IsAlive = 0;
			_defaultData.Size = 1f;
			_defaultData.Random = Random.Range(0f, 1f);
			_defaultData.Padding = 0f;
		}

		_pastData = new ParticleData[1024 * 1024];
		for (var i = 0; i < _pastData.Length; i++) {
			_pastData[i] = _defaultData;
		}
	}
	
	protected override void CreateData() {
		var data = new ParticleData[Count];
		if (_pastData != null) {
			if (Count <= _pastData.Length) {
				System.Array.Copy(_pastData, 0, data, 0, data.Length);
			} else {
				System.Array.Copy(_pastData, 0, data, 0, _pastData.Length);
				for(var i = _pastData.Length; i < data.Length; i++) {
					data[i] = _defaultData;
				}
				_pastData = data;
			}
		} else {
			for (var i = 0; i < data.Length; i++) {
				data[i] = _defaultData;
			}
			_pastData = data;
		}

		MyGpuBuffer.SetData(data);

	}
}