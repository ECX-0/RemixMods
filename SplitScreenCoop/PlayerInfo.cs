using System;
using System.Linq;
using UnityEngine;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using HUD;
using System.Collections.Generic;
using static SplitScreenCoop.SplitScreenCoop;

namespace SplitScreenCoop
{
    internal class Players
    {
        public Player player;
        public AbstractCreature character;
        public CameraListener cameraListener;

    }
}
