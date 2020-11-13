using FinalIKSanity;
using Harmony;
using MelonLoader;
using RootMotion.FinalIK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonInfo(typeof(FinalIKSanityMod), "Final IK Sanity", "1.0.5", "Ben")]

namespace FinalIKSanity
{
    public class FinalIKSanityMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            var harmonyInstance = HarmonyInstance.Create("FinalIKSanity");
            harmonyInstance.Patch(
                typeof(IKSolverHeuristic).GetMethods().Where(m => m.Name.Equals("IsValid") && m.GetParameters().Length == 1).First(),
                prefix: new HarmonyMethod(typeof(FinalIKSanityMod).GetMethod("IsValid", BindingFlags.NonPublic | BindingFlags.Static)));
        }

        [HarmonyPrefix]
        private static bool IsValid(ref IKSolverHeuristic __instance, ref bool __result, ref string message)
        {
            if (__instance.maxIterations > 64)
            {
                __result = false;
                message = "The solver requested too many iterations.";

                return false;
            }

            return true;
        }
    }
}
