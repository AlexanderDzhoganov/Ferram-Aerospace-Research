﻿/*
Ferram Aerospace Research v0.14.2
Copyright 2014, Michael Ferrara, aka Ferram4

    This file is part of Ferram Aerospace Research.

    Ferram Aerospace Research is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Ferram Aerospace Research is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Ferram Aerospace Research.  If not, see <http://www.gnu.org/licenses/>.

    Serious thanks:		a.g., for tons of bugfixes and code-refactorings
            			Taverius, for correcting a ton of incorrect values
            			sarbian, for refactoring code for working with MechJeb, and the Module Manager 1.5 updates
            			ialdabaoth (who is awesome), who originally created Module Manager
                        Regex, for adding RPM support
            			Duxwing, for copy editing the readme
 * 
 * Kerbal Engineer Redux created by Cybutek, Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License
 *      Referenced for starting point for fixing the "editor click-through-GUI" bug
 *
 * Part.cfg changes powered by sarbian & ialdabaoth's ModuleManager plugin; used with permission
 *	http://forum.kerbalspaceprogram.com/threads/55219
 *
 * Toolbar integration powered by blizzy78's Toolbar plugin; used with permission
 *	http://forum.kerbalspaceprogram.com/threads/60863
 */


using System;
using System.Collections.Generic;
using UnityEngine;

namespace ferram4
{
    public class FARPartModule : PartModule
    {
        protected Callback OnVesselPartsChange;
        public List<Part> VesselPartList = null;
        int VesselPartListCount = 0;

        public void ForceOnVesselPartsChange()
        {
            if(OnVesselPartsChange != null)
                OnVesselPartsChange();
        }

        public virtual void Start()
        {
            if (!CompatibilityChecker.IsAllCompatible())
            {
                this.enabled = false;
                return;
            }

            OnVesselPartsChange = UpdateShipPartsList;
            UpdateShipPartsList();

            if (HighLogic.LoadedSceneIsEditor)
            {
                part.OnEditorAttach += OnEditorAttach;
                part.OnEditorDetach += OnEditorAttach;
                part.OnEditorDestroy += OnEditorAttach;
            }
        }        

        public virtual void OnEditorAttach()
        {
            //print(part + " OnEditorAttach");

            FARGlobalControlEditorObject.EditorPartsChanged = true;
        }

        protected void UpdateShipPartsList()
        {
            VesselPartList = GetShipPartList();
        }

        public List<Part> GetShipPartList()
        {
            List<Part> list = null;
            if (HighLogic.LoadedSceneIsEditor)
                list = FARAeroUtil.AllEditorParts;
            else if (vessel)
                list = vessel.Parts;
            else
            {
                list = new List<Part>();
                if (part)
                    list.Add(part);
            }
            VesselPartListCount = list.Count;

//            print("Updated Vessel Part List...");

            return list;
        }

        //public override void OnSave(ConfigNode node)
        //{
        //    //By blanking this nothing should be saved to the craft file or the persistance file
        //}

    }
}
