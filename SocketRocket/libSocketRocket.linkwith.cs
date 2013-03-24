using System;
using MonoTouch.ObjCRuntime;

[assembly: LinkWith ("libSocketRocket.a", LinkTarget.ArmV7 | LinkTarget.Simulator, ForceLoad = true)]
