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
        
        private static SteamVR_Input_ActionSet_Psyia p_Psyia;
        
        public static SteamVR_Input_ActionSet_Psyia Psyia
        {
            get
            {
                return SteamVR_Actions.p_Psyia.GetCopy<SteamVR_Input_ActionSet_Psyia>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p_Psyia = ((SteamVR_Input_ActionSet_Psyia)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_Psyia>("/actions/Psyia")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions.Psyia};
        }
    }
}
