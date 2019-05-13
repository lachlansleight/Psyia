//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Single p_psyia_ApplyForce;
        
        private static SteamVR_Action_Pose p_psyia_Pose;
        
        private static SteamVR_Action_Skeleton p_psyia_SkeletonRightHand;
        
        private static SteamVR_Action_Skeleton p_psyia_SkeletonLeftHand;
        
        private static SteamVR_Action_Boolean p_psyia_SpawnParticles;
        
        private static SteamVR_Action_Boolean p_psyia_ToggleMenu;
        
        private static SteamVR_Action_Boolean p_psyia_SlowTime;
        
        private static SteamVR_Action_Vibration p_psyia_Haptic;
        
        public static SteamVR_Action_Single psyia_ApplyForce
        {
            get
            {
                return SteamVR_Actions.p_psyia_ApplyForce.GetCopy<SteamVR_Action_Single>();
            }
        }
        
        public static SteamVR_Action_Pose psyia_Pose
        {
            get
            {
                return SteamVR_Actions.p_psyia_Pose.GetCopy<SteamVR_Action_Pose>();
            }
        }
        
        public static SteamVR_Action_Skeleton psyia_SkeletonRightHand
        {
            get
            {
                return SteamVR_Actions.p_psyia_SkeletonRightHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Skeleton psyia_SkeletonLeftHand
        {
            get
            {
                return SteamVR_Actions.p_psyia_SkeletonLeftHand.GetCopy<SteamVR_Action_Skeleton>();
            }
        }
        
        public static SteamVR_Action_Boolean psyia_SpawnParticles
        {
            get
            {
                return SteamVR_Actions.p_psyia_SpawnParticles.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean psyia_ToggleMenu
        {
            get
            {
                return SteamVR_Actions.p_psyia_ToggleMenu.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean psyia_SlowTime
        {
            get
            {
                return SteamVR_Actions.p_psyia_SlowTime.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Vibration psyia_Haptic
        {
            get
            {
                return SteamVR_Actions.p_psyia_Haptic.GetCopy<SteamVR_Action_Vibration>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.psyia_ApplyForce,
                    SteamVR_Actions.psyia_Pose,
                    SteamVR_Actions.psyia_SkeletonRightHand,
                    SteamVR_Actions.psyia_SkeletonLeftHand,
                    SteamVR_Actions.psyia_SpawnParticles,
                    SteamVR_Actions.psyia_ToggleMenu,
                    SteamVR_Actions.psyia_SlowTime,
                    SteamVR_Actions.psyia_Haptic};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.psyia_ApplyForce,
                    SteamVR_Actions.psyia_Pose,
                    SteamVR_Actions.psyia_SkeletonRightHand,
                    SteamVR_Actions.psyia_SkeletonLeftHand,
                    SteamVR_Actions.psyia_SpawnParticles,
                    SteamVR_Actions.psyia_ToggleMenu,
                    SteamVR_Actions.psyia_SlowTime};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[] {
                    SteamVR_Actions.psyia_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    SteamVR_Actions.psyia_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[] {
                    SteamVR_Actions.psyia_Pose};
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.psyia_SpawnParticles,
                    SteamVR_Actions.psyia_ToggleMenu,
                    SteamVR_Actions.psyia_SlowTime};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[] {
                    SteamVR_Actions.psyia_ApplyForce};
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[0];
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    SteamVR_Actions.psyia_SkeletonRightHand,
                    SteamVR_Actions.psyia_SkeletonLeftHand};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.psyia_ApplyForce,
                    SteamVR_Actions.psyia_SpawnParticles,
                    SteamVR_Actions.psyia_ToggleMenu,
                    SteamVR_Actions.psyia_SlowTime};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_psyia_ApplyForce = ((SteamVR_Action_Single)(SteamVR_Action.Create<SteamVR_Action_Single>("/actions/Psyia/in/ApplyForce")));
            SteamVR_Actions.p_psyia_Pose = ((SteamVR_Action_Pose)(SteamVR_Action.Create<SteamVR_Action_Pose>("/actions/Psyia/in/Pose")));
            SteamVR_Actions.p_psyia_SkeletonRightHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/Psyia/in/SkeletonRightHand")));
            SteamVR_Actions.p_psyia_SkeletonLeftHand = ((SteamVR_Action_Skeleton)(SteamVR_Action.Create<SteamVR_Action_Skeleton>("/actions/Psyia/in/SkeletonLeftHand")));
            SteamVR_Actions.p_psyia_SpawnParticles = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Psyia/in/SpawnParticles")));
            SteamVR_Actions.p_psyia_ToggleMenu = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Psyia/in/ToggleMenu")));
            SteamVR_Actions.p_psyia_SlowTime = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/Psyia/in/SlowTime")));
            SteamVR_Actions.p_psyia_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Action.Create<SteamVR_Action_Vibration>("/actions/Psyia/out/Haptic")));
        }
    }
}