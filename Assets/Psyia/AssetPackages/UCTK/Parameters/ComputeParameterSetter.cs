using UnityEngine;
using System;

namespace UCTK {

	public class ComputeParameterSetter : MonoBehaviour {
		public ShaderValues shaderValues;
		public ComputeShader[] TargetShaders;
	
		public void ApplyNow() {
			ApplyValues();
		}

		void Update () {
			ApplyValues();
		}

		void ApplyValues() {
			var pInfos = shaderValues.GetType().GetFields();
			foreach(var p in pInfos) {
				if (Attribute.IsDefined(p, typeof(ComputeValue))) {
					if(p.FieldType == typeof(bool)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetBool(p.Name, (bool)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(float)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetFloat(p.Name, (float)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(float[])) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetFloats(p.Name, (float[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(int)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetInt(p.Name, (int)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(float[])) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetInts(p.Name, (int[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(Matrix4x4)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetMatrix(p.Name, (Matrix4x4)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(float[])) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetMatrixArray(p.Name, (Matrix4x4[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(Texture) || p.FieldType == typeof(Texture2D) || p.FieldType == typeof(RenderTexture)) {
						if(Attribute.IsDefined(p, typeof(ComputeTexture))) {
							string ShaderName = ((ComputeTexture)Attribute.GetCustomAttribute(p, typeof(ComputeTexture))).ShaderName;
							string KernelName = ((ComputeTexture)Attribute.GetCustomAttribute(p, typeof(ComputeTexture))).KernelName;
							foreach(ComputeShader TargetShader in TargetShaders) {
								if(ShaderName == TargetShader.name) {
									TargetShader.SetTexture(TargetShader.FindKernel(KernelName), p.Name, (Texture)p.GetValue(shaderValues));
								}
							}
						}
					}
					else if(p.FieldType == typeof(Color)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetVector(p.Name, (Color)p.GetValue(shaderValues));
						}
					}
					else if(p.FieldType == typeof(Vector2)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetVector(p.Name, (Vector2)p.GetValue(shaderValues));
						}
					}
					else if(p.FieldType == typeof(Vector3)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetVector(p.Name, (Vector3)p.GetValue(shaderValues));
						}
					}
					else if(p.FieldType == typeof(Vector4)) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetVector(p.Name, (Vector4)p.GetValue(shaderValues));
						}
					}
					else if(p.FieldType == typeof(Vector4[])) {
						foreach(ComputeShader TargetShader in TargetShaders) {
							TargetShader.SetVectorArray(p.Name, (Vector4[])p.GetValue(shaderValues));
						}
					}
				}
			}
		}
	}
}