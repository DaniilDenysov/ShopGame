#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace ShopGame.Utilities
{
    public static class DebugUtility
    {
#if UNITY_EDITOR
        [MenuItem("Tools/Enable Debugging")]
        public static void EnableDebug()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if (!symbols.Contains("ENABLE_DEBUG"))
            {
                symbols += ";ENABLE_DEBUG";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
            }
        }

        [MenuItem("Tools/Disable Debugging")]
        public static void DisableDebug()
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            symbols = symbols.Replace("ENABLE_DEBUG", "").Replace(";;", ";");
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        }
#endif
        public static void PrintLine(string message)
        {
#if ENABLE_DEBUG
            Debug.Log(message);
#endif
        }

        public static void PrintWarning(string message)
        {
#if ENABLE_DEBUG
            Debug.LogWarning(message);
#endif
        }

        public static void PrintError(string message)
        {
#if ENABLE_DEBUG
            Debug.LogError(message);
#endif
        }
    }
}
