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

public class UpdateTime : MonoBehaviour {

	public ComputeShader[] ComputeShaders;
	public Material[] Materials;

	public float TimeScale = 1f;
	private float _time;
	
	public void Update ()
	{
		_time += Time.deltaTime * TimeScale;
		
		foreach(var cs in ComputeShaders) {
			cs.SetFloat("Time", _time);
			cs.SetFloat("DeltaTime", Time.deltaTime * TimeScale);
		}
		foreach(var m in Materials) {
			m.SetFloat("Time", _time);
			m.SetFloat("DeltaTime", Time.deltaTime * TimeScale);
		}
	}
}
