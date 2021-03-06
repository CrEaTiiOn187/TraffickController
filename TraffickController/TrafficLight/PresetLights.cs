﻿using System;
using System.Collections.Generic;
using System.Timers;

using TraffickController.JsonStrings;

namespace TraffickController.TrafficLight
{
    public class PresetLights : IPresetLights
    {
        private readonly List<List<string>> _presets = new List<List<string>>() {
            new List<string>() { "A1", "A2", "A3", "A4", "D3" },
            new List<string>() { "B1", "B2", "B3", "B4", "C3" },
            new List<string>() { "FF2", "FF1", "FV1", "FV2", "FV3", "FV4", "B5", "E1", "EV1", "EV2", "EV3", "EV4", "GF1", "GF2", "GV1", "GV2", "GV3", "GV4" },
            new List<string>() { "D1", "D2", "D3", "B4" },
            new List<string>() { "C1", "C2", "C3", "A4" },
            new List<string>() { "D3", "A1", "AB1" },
            new List<string>() { "D1", "D3", "AB2", "B4" },
            new List<string>() { "FF2", "FF1", "FV1", "FV2", "FV3", "FV4", "B5", "BB1", "C1", "C3" }
        };
        private int _count = 0;
        private Dictionary<string, int> _lightsAtSameTime = new Dictionary<string, int>();
        private Timer _aTimer;
        private bool _elapsed = false;
        private readonly IJsonStringBuilder _jsonStringBuilder;

        public PresetLights(IJsonStringBuilder jsonStringBuilder)
        {
            _jsonStringBuilder = jsonStringBuilder;
            
            _aTimer = new Timer
            {
                Interval = 20000,
                AutoReset = true,
                Enabled = true
            };
        }

        public string ReturnPreset(Dictionary<string, int> lightsReceived, string state)
        {
            switch(state){
                case "Green":
                    if (lightsReceived == null)
                        return _jsonStringBuilder.BuildJsonString();

                    var result = FindPreset(lightsReceived); // Find preset when simulator sends amount of traffic at trafficlight

                    return result;
                case "Orange":
                    return SetLightColor(1); // Set light color of currently green trafficlights
                case "Red":
                    return _jsonStringBuilder.BuildJsonString(); // Set trafficlights to red
                default:
                    return _jsonStringBuilder.BuildJsonString();
            }
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            _elapsed = true;
        }

        // Loop through presets, give priority to presets with buses
        private string FindPreset(Dictionary<string, int> trafficAtLights)
        {
            int oldCount = -1;

            _aTimer.Start();

            _aTimer.Elapsed += OnTimedEvent;

            _lightsAtSameTime = new Dictionary<string, int>();

            if(trafficAtLights.GetValueOrDefault("AB1") >= 1 && _elapsed)
            {
                oldCount = _count;
                _count = 5;
                _elapsed = false;
            }
            else if (trafficAtLights.GetValueOrDefault("AB2") >= 1 && _elapsed)
            {
                oldCount = _count;
                _count = 6;
                _elapsed = false;
            }
            else if (trafficAtLights.GetValueOrDefault("BB1") >= 1 && _elapsed)
            {
                oldCount = _count;
                _count = 7;
                _elapsed = false;
            }

            foreach (var x in _presets[_count])
            {
                if (trafficAtLights.ContainsKey(x))
                {
                    foreach (var i in trafficAtLights)
                    {
                        if (x == i.Key)
                            _lightsAtSameTime.Add(x, i.Value);
                    }
                }
            }
            if (oldCount > -1) _count = oldCount; // If bus had priority set counter back to old count
            else _count++;

            if(_count > 4){
                _count = 0;
            }

            return SetLightColor(2); // Set lights from preset to green
        }

        private string SetLightColor(int lightColor)
        {
            #region Initialize Variables
            int A1 = 0;
            int A2 = 0;
            int A3 = 0;
            int A4 = 0;
            int AB1 = 0;
            int AB2 = 0;
            int B1 = 0;
            int B2 = 0;
            int B3 = 0;
            int B4 = 0;
            int B5 = 0;
            int BB1 = 0;
            int C1 = 0;
            int C2 = 0;
            int C3 = 0;
            int D1 = 0;
            int D2 = 0;
            int D3 = 0;
            int E1 = 0;
            int EV1 = 0;
            int EV2 = 0;
            int EV3 = 0;
            int EV4 = 0;
            int FF1 = 0;
            int FF2 = 0;
            int FV1 = 0;
            int FV2 = 0;
            int FV3 = 0;
            int FV4 = 0;
            int GF1 = 0;
            int GF2 = 0;
            int GV1 = 0;
            int GV2 = 0;
            int GV3 = 0;
            int GV4 = 0;
            #endregion

            foreach (var y in _lightsAtSameTime)
            {
                // Switch case to set light color based on lights in _lightsAtSameTime
                #region Set Lights Values
                switch (y.Key)
                {
                    case "A1":
                        if (y.Value != 0)
                            A1 = lightColor;
                        break;
                    case "A2":
                        if (y.Value != 0)
                            A2 = lightColor;
                            A3 = lightColor;
                        break;
                    case "A3":
                        if (y.Value != 0)
                            A3 = lightColor;
                            A2 = lightColor;
                        break;
                    case "A4":
                        if (y.Value != 0)
                            A4 = lightColor;
                        break;
                    case "AB1":
                        if (y.Value != 0)
                            AB1 = lightColor;
                        break;
                    case "AB2":
                        if (y.Value != 0)
                            AB2 = lightColor;
                        break;
                    case "B1":
                        if (y.Value != 0)
                            B1 = lightColor;
                        break;
                    case "B2":
                        if (y.Value != 0)
                            B2 = lightColor;
                            B3 = lightColor;
                        break;
                    case "B3":
                        if (y.Value != 0)
                            B3 = lightColor;
                            B2 = lightColor;
                        break;
                    case "B4":
                        if (y.Value != 0)
                            B4 = lightColor;
                        break;
                    case "B5":
                        if (y.Value != 0)
                            B5 = lightColor;
                        break;
                    case "BB1":
                        if (y.Value != 0)
                            BB1 = lightColor;
                        break;
                    case "C1":
                        if (y.Value != 0)
                            C1 = lightColor;
                        break;
                    case "C2":
                        if (y.Value != 0)
                            C2 = lightColor;
                        break;
                    case "C3":
                        if (y.Value != 0)
                            C3 = lightColor;
                        break;
                    case "D1":
                        if (y.Value != 0)
                            D1 = lightColor;
                        break;
                    case "D2":
                        if (y.Value != 0)
                            D2 = lightColor;
                        break;
                    case "D3":
                        if (y.Value != 0)
                            D3 = lightColor;
                        break;
                    case "E1":
                        if (y.Value != 0)
                            E1 = lightColor;
                        break;
                    case "EV1":
                        if (y.Value != 0)
                            EV1 = lightColor;
                        break;
                    case "EV2":
                        if (y.Value != 0)
                            EV2 = lightColor;
                        break;
                    case "EV3":
                        if (y.Value != 0)
                            EV3 = lightColor;
                        break;
                    case "EV4":
                        if (y.Value != 0)
                            EV4 = lightColor;
                        break;
                    case "FF1":
                        if (y.Value != 0)
                            FF1 = lightColor;
                        break;
                    case "FF2":
                        if (y.Value != 0)
                            FF2 = lightColor;
                        break;
                    case "FV1":
                        if (y.Value != 0)
                            FV1 = lightColor;
                        break;
                    case "FV2":
                        if (y.Value != 0)
                            FV2 = lightColor;
                        break;
                    case "FV3":
                        if (y.Value != 0)
                            FV3 = lightColor;
                        break;
                    case "FV4":
                        if (y.Value != 0)
                            FV4 = lightColor;
                        break;
                    case "GF1":
                        if (y.Value != 0)
                            GF1 = lightColor;
                        break;
                    case "GF2":
                        if (y.Value != 0)
                            GF2 = lightColor;
                        break;
                    case "GV1":
                        if (y.Value != 0)
                            GV1 = lightColor;
                        break;
                    case "GV2":
                        if (y.Value != 0)
                            GV2 = lightColor;
                        break;
                    case "GV3":
                        if (y.Value != 0)
                            GV3 = lightColor;
                        break;
                    case "GV4":
                        if (y.Value != 0)
                            GV4 = lightColor;
                        break;
                    default:
                        break;
                }
                #endregion
            }

            // Generate JSON string from trafficlights data
            return _jsonStringBuilder.BuildJsonString(
                A1: A1, A2: A2, A3: A3,
                A4: A4, AB1: AB1, AB2: AB2,
                B1: B1, B2: B2, B3: B3,
                B4: B4, B5: B5, BB1: BB1,
                C1: C1, C2: C2, C3: C3,
                D1: D1, D2: D2, D3: D3,
                E1: E1, EV1: EV1, EV2: EV2,
                EV3: EV3, EV4: EV4, FF1: FF1,
                FF2: FF2, FV1: FV1, FV2: FV2,
                FV3: FV3, FV4: FV4, GF1: GF1,
                GF2: GF2, GV1: GV1, GV2: GV2,
                GV3: GV3, GV4: GV4);
        }
    }
}