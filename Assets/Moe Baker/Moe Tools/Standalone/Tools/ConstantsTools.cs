﻿using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Moe.Tools
{
    public static partial class MoeTools
    {
        public static class Constants
        {
            public static class Paths
            {
                public const string Menu = "Moe/";

                public const string Tools = Menu + "Tools/";
            }
        }
    }
}