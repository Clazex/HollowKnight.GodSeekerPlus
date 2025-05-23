global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;

global using GlobalEnums;

global using GodSeekerPlus.Settings;
global using GodSeekerPlus.Utils;

global using HKReflect;
global using HKReflect.Static;

global using HutongGames.PlayMaker;
global using HutongGames.PlayMaker.Actions;

global using JetBrains.Annotations;

global using Modding;

global using Newtonsoft.Json;

global using Osmi;
global using Osmi.Game;
global using Osmi.Utils;

global using Satchel;
global using Satchel.Futils;

global using UnityEngine;
global using UnityEngine.EventSystems;
global using UnityEngine.SceneManagement;

global using static GodSeekerPlus.Utils.Logger;
global using static HKReflect.Singletons;

global using InvokeMethod = Osmi.FsmActions.InvokeMethod;
global using Lang = Language.Language;
global using Logger = GodSeekerPlus.Utils.Logger;
global using Module = GodSeekerPlus.Modules.Module;
global using ReflectionHelper = Modding.ReflectionHelper;
global using UObject = UnityEngine.Object;
global using USceneManager = UnityEngine.SceneManagement.SceneManager;
