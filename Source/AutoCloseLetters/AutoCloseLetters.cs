using System;
using System.Linq;
using System.Threading;
using HarmonyLib;
using HugsLib;
using RimWorld;
using Verse;

namespace AutoCloseLetters
{
    [StaticConstructorOnStartup]
    public static class AutoCloseLetters
    {
        static AutoCloseLetters()
        {
            var harmony = new Harmony("com.AutoCloseLetters.patch");
            harmony.PatchAll();
        }
    }

    public class AutoCloseLettersConfig : Mod
    {
        public AutoCloseLettersConfig(ModContentPack content) : base(content)
        {

        }
    }

    [HarmonyPatch]
    public static class AutoCloseLetters_Patch
    {
        [HarmonyPatch(typeof(GameDataSaveLoader), nameof(GameDataSaveLoader.SaveGame))]
        [HarmonyPrefix]
        public static void DeleteLettersInstantly()
        {
            Find.LetterStack.LettersListForReading.Where(x => x.arrivalTick < Find.TickManager.TicksGame - (60000 * Prefs.AutosaveIntervalDays) && !(x is LetterWithTimeout)).ToList().ForEach(Remove);
        }

        private static void Remove(Letter letter)
        {
            Find.LetterStack.RemoveLetter(letter);
        }
    }
}
