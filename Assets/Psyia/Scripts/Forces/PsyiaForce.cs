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

namespace Psyia {
    public class PsyiaForce : MonoBehaviour {

        public enum ForceAttenuationMode {
            Infinite,
            Linear,
            Hyperbolic,
            HyperbolicSquared,
            HyperbolicSoftened,
            HyperbolicSquaredSoftened,
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
        [HideInInspector] public float StrengthMultiplier = 1f;
        public ForceAttenuationMode AttenuationMode;
        [Tooltip("The distance at which the force equals zero")] public float AttenuationDistance;

        public Vector2 Padding;

        private ForceManager _Manager;
        private ForceManager Manager {
            get {
                if(_Manager == null) {
                    /*
                    Transform MainParent = transform.parent;
                    while(MainParent.GetComponent<PsyiaSystem>() == null && MainParent != null) {
                        MainParent = MainParent.parent;
                    }
                    if(MainParent == null) {
                        Debug.LogError("Failed to find source psyia system!");
                        return null;
                    }
                    _Manager = MainParent.GetComponentInChildren<ForceManager>();
                    */
                    _Manager = GameObject.FindObjectOfType<ForceManager>();
                }
                return _Manager;
            }
        }

        private ForceData MyForceData;
        public ForceData GetForceData() {
            MyForceData.Strength = Strength * StrengthMultiplier;
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

        void Start() 
        {
            Manager.AddSource(this);
        }

        void OnDestroy() 
        {
            //we do not remove the basic internal gravity force because there must be always at least one force in the compute buffer
            var grav = GetComponent<PsyiaInternalGravityForce>();
            if(Manager == null) return;
            if(grav == null) Manager.RemoveSource(this);
        }
    }

    
}