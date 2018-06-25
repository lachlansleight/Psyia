using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PsyiaForce.asset", menuName = "ScriptableObjects/PsyiaForce", order = 1)]
public class PsyiaForce : ScriptableObject {
    public enum ForceAttenuationMode {
        Infinite,
        Linear,
        Hyperbolic,
        HyperbolicSquared,
        Sine
    }
    public enum ForceShape {
        Radial,
        Orbital,
        Linear,
        Dipole
    }

    public ForceShape Shape;
    public float Strength;
    public ForceAttenuationMode AttenuationMode;
    [Tooltip("The distance at which the force equals zero")] public float AttenuationDistance;
    
    public Vector3 Position;
    public Vector3 Rotation;

    public Vector2 Padding;

    private ForceData MyForceData;
    public ForceData GetForceData() {
        MyForceData.Strength = Strength;
        MyForceData.AttenuationMode = (int)AttenuationMode;
        MyForceData.AttenuationDistance = AttenuationDistance;
        MyForceData.Shape = (int)Shape;
        MyForceData.Position = Position;
        MyForceData.Rotation = Rotation;
        MyForceData.Padding = Padding;

        return MyForceData;
    }
    public ForceData GetForceData(Vector3 CustomPosition, Vector3 CustomRotation, float StrengthModifier) {
        MyForceData.Strength = Strength * StrengthModifier;
        MyForceData.AttenuationMode = (int)AttenuationMode;
        MyForceData.AttenuationDistance = AttenuationDistance;
        MyForceData.Shape = (int)Shape;
        MyForceData.Position = CustomPosition;
        MyForceData.Rotation = CustomRotation;
        MyForceData.Padding = Padding;

        return MyForceData;
    }

    public static ForceData EmptyForceData {
        get {
            ForceData EmptyData = new ForceData();
            EmptyData.Strength = 0;
            EmptyData.AttenuationMode = 0;
            EmptyData.AttenuationDistance = 1;
            EmptyData.Shape = 0;
            EmptyData.Position = Vector3.zero;
            EmptyData.Rotation = Vector3.zero;
            EmptyData.Padding = Vector2.zero;
            return EmptyData;
        }
    }
}
