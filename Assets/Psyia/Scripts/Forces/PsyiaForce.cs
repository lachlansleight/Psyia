using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Psyia {
    public class PsyiaForce : MonoBehaviour {

        public enum ForceAttenuationMode {
            Infinite,
            Linear,
            Hyperbolic,
            HyperbolicSquared,
            Sine,
            HyperbolicSoftened,
            HyperbolicSquaredSoftened
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

        public Vector2 Padding;

        private ForceManager _Manager;
        private ForceManager Manager {
            get {
                if(_Manager == null) {
                    Transform MainParent = transform.parent;
                    while(MainParent.GetComponent<PsyiaSystem>() == null && MainParent != null) {
                        MainParent = MainParent.parent;
                    }
                    if(MainParent == null) {
                        Debug.LogError("Failed to find source psyia system!");
                        return null;
                    }
                    _Manager = MainParent.GetComponentInChildren<ForceManager>();
                }
                return _Manager;
            }
        }

        private ForceData MyForceData;
        public ForceData GetForceData() {
            MyForceData.Strength = Strength;
            MyForceData.AttenuationMode = (int)AttenuationMode;
            MyForceData.AttenuationDistance = AttenuationDistance;
            MyForceData.Shape = (int)Shape;
            MyForceData.Position = transform.position;
            MyForceData.Rotation = transform.eulerAngles;
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

        void Start() {
            Manager.AddSource(this);
        }

        void OnDestroy() {
            //we do not remove the basic internal gravity force because there must be always at least one force in the compute buffer
            if(GetComponent<PsyiaInternalGravityForce>() == null) Manager.RemoveSource(this);
        }
    }

    
}