using System.Collections.Generic;
using UnityEngine;
using System;

namespace Foliar.Compute {

	public class MaterialParameterSetter : MonoBehaviour {

		public Material TargetMaterial;
		public ShaderValues shaderValues;
	
		void Update () {
			var pInfos = shaderValues.GetType().GetFields();
			foreach(var p in pInfos) {
				if (Attribute.IsDefined(p, typeof(ShaderValue))) {
					if(p.FieldType == typeof(Color))
						TargetMaterial.SetColor(p.Name, (Color)p.GetValue(shaderValues));

					else if(p.FieldType == typeof(Color[]))
						TargetMaterial.SetColorArray(p.Name, (Color[])p.GetValue(shaderValues));
					
					else if(p.FieldType == typeof(List<Color>))
						TargetMaterial.SetColorArray(p.Name, (List<Color>)p.GetValue(shaderValues));
					

					else if (p.FieldType == typeof(float))
						TargetMaterial.SetFloat(p.Name, (float)p.GetValue(shaderValues));
					else if (p.FieldType == typeof(float[]))
						TargetMaterial.SetFloatArray(p.Name, (float[])p.GetValue(shaderValues));
					else if (p.FieldType == typeof(List<float>))
						TargetMaterial.SetFloatArray(p.Name, (List<float>)p.GetValue(shaderValues));

					else if(p.FieldType == typeof(int))
						TargetMaterial.SetInt(p.Name, (int)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(bool))
						TargetMaterial.SetInt(p.Name, (bool)p.GetValue(shaderValues) ? 1 : 0);
					

					else if(p.FieldType == typeof(Matrix4x4))
						TargetMaterial.SetMatrix(p.Name, (Matrix4x4)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(Matrix4x4[]))
						TargetMaterial.SetMatrixArray(p.Name, (Matrix4x4[])p.GetValue(shaderValues));
					else if(p.FieldType == typeof(List<Matrix4x4>))
						TargetMaterial.SetMatrixArray(p.Name, (List<Matrix4x4>)p.GetValue(shaderValues));

					else if(p.FieldType == typeof(string))
						TargetMaterial.SetOverrideTag(p.Name, (string)p.GetValue(shaderValues));

					else if(p.FieldType == typeof(Texture))
						TargetMaterial.SetTexture(p.Name, (Texture)p.GetValue(shaderValues));
					
					//a vector2 can be a vector, or a material offset or scale
					//TODO: test this!
					else if(p.FieldType == typeof(Vector2)) {
						if(Attribute.IsDefined(p, typeof(TextureOffset)))
							TargetMaterial.SetTextureOffset(((TextureOffset)Attribute.GetCustomAttribute(p, typeof(TextureOffset))).Name, (Vector2)p.GetValue(shaderValues));
						else if(Attribute.IsDefined(p, typeof(TextureScale)))
							TargetMaterial.SetTextureOffset(((TextureScale)Attribute.GetCustomAttribute(p, typeof(TextureScale))).Name, (Vector2)p.GetValue(shaderValues));
						else
							TargetMaterial.SetVector(p.Name, (Vector2)p.GetValue(shaderValues));
					}
					
					else if(p.FieldType == typeof(Vector3))
						TargetMaterial.SetVector(p.Name, (Vector3)p.GetValue(shaderValues));	
				
					else if(p.FieldType == typeof(Vector4))
						TargetMaterial.SetVector(p.Name, (Vector4)p.GetValue(shaderValues));
					else if(p.FieldType == typeof(Vector4[]))
						TargetMaterial.SetVectorArray(p.Name, (Vector4[])p.GetValue(shaderValues));
					else if(p.FieldType == typeof(List<Vector4>))
						TargetMaterial.SetVectorArray(p.Name, (List<Vector4>)p.GetValue(shaderValues));
				}			
			}
		}
	}
}