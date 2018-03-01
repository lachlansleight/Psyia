using UnityEngine;
using System;

namespace Foliar.Compute {

	public class ComputeParameterSetter : MonoBehaviour {

		public ComputeShader TargetShader;
		public ShaderValues shaderValues;
	
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
					if(p.FieldType == typeof(bool))
						TargetShader.SetBool(p.Name, (bool)p.GetValue(shaderValues));

					else if(p.FieldType == typeof(float))
						TargetShader.SetFloat(p.Name, (float)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(float[]))
						TargetShader.SetFloats(p.Name, (float[])p.GetValue(shaderValues));

					else if(p.FieldType == typeof(int))
						TargetShader.SetInt(p.Name, (int)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(float[]))
						TargetShader.SetInts(p.Name, (int[])p.GetValue(shaderValues));

					else if(p.FieldType == typeof(Matrix4x4))
						TargetShader.SetMatrix(p.Name, (Matrix4x4)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(float[]))
						TargetShader.SetMatrixArray(p.Name, (Matrix4x4[])p.GetValue(shaderValues));

					else if(p.FieldType == typeof(Texture)) {
						if(Attribute.IsDefined(p, typeof(ComputeKernel))) {
							string KernelName = ((ComputeKernel)Attribute.GetCustomAttribute(p, typeof(ComputeKernel))).Name;
							TargetShader.SetTexture(TargetShader.FindKernel(KernelName), p.Name, (Texture)p.GetValue(shaderValues));
						}
					}
					if(p.FieldType == typeof(Vector4) || p.FieldType == typeof(Vector3) || p.FieldType == typeof(Vector2) || p.FieldType == typeof(Color))
						TargetShader.SetVector(p.Name, (Vector4)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(Vector4[]))
						TargetShader.SetVectorArray(p.Name, (Vector4[])p.GetValue(shaderValues));
				}
			}
		}
	}
}