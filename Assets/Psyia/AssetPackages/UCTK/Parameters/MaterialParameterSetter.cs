using System.Collections.Generic;
using UnityEngine;
using System;

namespace UCTK {

	public class MaterialParameterSetter : MonoBehaviour {
		public ShaderValues shaderValues;
		public Material[] TargetMaterials;
	
		void Update () {
			var pInfos = shaderValues.GetType().GetFields();
			foreach(var p in pInfos) {
				if (Attribute.IsDefined(p, typeof(ShaderValue))) {
					if(p.FieldType == typeof(Color)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetColor(p.Name, (Color)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(Color[])) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetColorArray(p.Name, (Color[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(List<Color>)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetColorArray(p.Name, (List<Color>)p.GetValue(shaderValues));
						}
					} else if (p.FieldType == typeof(float)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetFloat(p.Name, (float)p.GetValue(shaderValues));
						}
					} else if (p.FieldType == typeof(float[])) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetFloatArray(p.Name, (float[])p.GetValue(shaderValues));
						}
					} else if (p.FieldType == typeof(List<float>)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetFloatArray(p.Name, (List<float>)p.GetValue(shaderValues));
						}

					} else if(p.FieldType == typeof(int)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetInt(p.Name, (int)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(bool)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetInt(p.Name, (bool)p.GetValue(shaderValues) ? 1 : 0);
						}
					

					} else if(p.FieldType == typeof(Matrix4x4)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetMatrix(p.Name, (Matrix4x4)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(Matrix4x4[])) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetMatrixArray(p.Name, (Matrix4x4[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(List<Matrix4x4>)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetMatrixArray(p.Name, (List<Matrix4x4>)p.GetValue(shaderValues));
						}

					} else if(p.FieldType == typeof(string)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetOverrideTag(p.Name, (string)p.GetValue(shaderValues));
						}

					} else if(p.FieldType == typeof(Texture2D)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetTexture(p.Name, (Texture2D)p.GetValue(shaderValues));
						}
					
					//a vector2 can be a vector, or a material offset or scale
					//TODO: test this!
					} else if(p.FieldType == typeof(Vector2)) {
						if(Attribute.IsDefined(p, typeof(TextureOffset))) {
							foreach(Material TargetMaterial in TargetMaterials) {
								TargetMaterial.SetTextureOffset(((TextureOffset)Attribute.GetCustomAttribute(p, typeof(TextureOffset))).Name, (Vector2)p.GetValue(shaderValues));
							}
						} else if(Attribute.IsDefined(p, typeof(TextureScale))) {
							foreach(Material TargetMaterial in TargetMaterials) {
								TargetMaterial.SetTextureOffset(((TextureScale)Attribute.GetCustomAttribute(p, typeof(TextureScale))).Name, (Vector2)p.GetValue(shaderValues));
							}
						} else {
							foreach(Material TargetMaterial in TargetMaterials) {
								TargetMaterial.SetVector(p.Name, (Vector2)p.GetValue(shaderValues));
							}
						}
					} else if(p.FieldType == typeof(Vector3)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetVector(p.Name, (Vector3)p.GetValue(shaderValues));	
						}
				
					} else if(p.FieldType == typeof(Vector4)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetVector(p.Name, (Vector4)p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(Vector4[])) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetVectorArray(p.Name, (Vector4[])p.GetValue(shaderValues));
						}
					} else if(p.FieldType == typeof(List<Vector4>)) {
						foreach(Material TargetMaterial in TargetMaterials) {
							TargetMaterial.SetVectorArray(p.Name, (List<Vector4>)p.GetValue(shaderValues));
						}
					}
				}			
			}
		}
	}
}