using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VRTools {

	/// <summary>
	/// Main debug class
	/// </summary>
	public class VRDebug : MonoBehaviour {

		static Mesh mesh;
		static Material lineMat;
		static Material lineDepthMat;
		static List<WorldDebugLine> lines;

		void OnEnable() {
			GameObject tempCylinder = GameObject.CreatePrimitive(PrimitiveType.Cube);
			mesh = new Mesh();
			Mesh copyMesh = tempCylinder.GetComponent<MeshFilter>().mesh;
			mesh.vertices = copyMesh.vertices;
			mesh.normals = copyMesh.normals;
			mesh.uv = copyMesh.uv;
			mesh.triangles = copyMesh.triangles;
			Destroy(tempCylinder);

			lines = new List<WorldDebugLine>();

			lineMat = new Material(Resources.Load("VRLineMat") as Material);
			lineDepthMat = new Material(Resources.Load("VRLineDepthMat") as Material);
		}

		
		public static void DrawRay(Vector3 start, Vector3 dir) {
			DrawLine(start, start + dir);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color) {
			DrawLine(start, start + dir, color);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) {
			DrawLine(start, start + dir, color, duration);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) {
			DrawLine(start, start + dir, color, duration, depthTest);
		}
		public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest, float radius) {
			DrawLine(start, start + dir, color, duration, depthTest, radius);
		}


		public static void DrawLine(Vector3 start, Vector3 end) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = new Material(lineMat);
			newLine.material.color = Color.white;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = new Material(lineMat);
			newLine.material.color = color;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, bool depthTest, float radius) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation;
			if((end - start).magnitude == 0) rotation = Quaternion.identity;
			else rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(radius, radius, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = 0;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;

			Graphics.DrawMesh(mesh, newLine.matrix, newLine.material, 0);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time, bool depthTest) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(0.005f, 0.005f, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}
		public static void DrawLine(Vector3 start, Vector3 end, Color color, float time, bool depthTest, float radius) {
			Vector3 position = start + (end - start) * 0.5f;
			Quaternion rotation = Quaternion.LookRotation((end - start).normalized);
			Vector3 scale = new Vector3(radius, radius, (end - start).magnitude);

			WorldDebugLine newLine = new WorldDebugLine();
			newLine.matrix = Matrix4x4.TRS(position, rotation, scale);
			newLine.stopTime = Time.time + time;
			newLine.material = depthTest ? new Material(lineDepthMat) : new Material(lineMat);
			newLine.material.color = color;
			lines.Add(newLine);
		}
	}

}
