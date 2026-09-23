// ---------------------------------------------------------------
//  Blickrichtung
//
//  Mit gehaltener Umsehen-Taste (ab Werk mittlere Maustaste) dreht
//  sich nur der Kopf. Laesst man los, holt das Spiel den Kopf
//  zurueck nach vorne. Dieses Plugin dreht es um: beim Loslassen
//  dreht sich der KOERPER in die Richtung, in die man gerade
//  schaut - die Kamera bleibt stehen.
//
//  WO DER HEBEL SITZT: Die Kopfdrehung liegt in Player._horizontal
//  (links/rechts) und Player._vertical (hoch/runter). Loslassen der
//  Taste loest ECommand.ResetLookDirection aus, das setzt nur
//  _resetLook = true. Im naechsten Player.Look() wird das
//  ausgewertet und der Kopf faedelt per Lerp zurueck auf 0.
//
//  Wir haengen uns VOR Player.Look(): steht _resetLook an, geben
//  wir den Kopfwinkel per Player.Rotate() an den Koerper weiter und
//  setzen ihn auf 0. Dann gibt es nichts mehr zurueckzuholen.
//
//  VORZEICHEN: Look() rechnet _horizontal -= MausX, Rotate() dagegen
//  Gierwinkel += MausX. Deshalb wandert -_horizontal in den Koerper.
//  Hoch/runter laufen gleichsinnig, dort wird der tatsaechlich
//  angezeigte Kopfwinkel (mit der Abschwaechung aus Look()) genommen.
//
//  Einstellbar unter F12 -> Blickrichtung.
// ---------------------------------------------------------------
using BepInEx;
using BepInEx.Configuration;
using EFT;
using HarmonyLib;
using UnityEngine;

namespace Blickrichtung
{
    [BepInPlugin("kaan.blickrichtung", "Blickrichtung", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ConfigEntry<bool> Aktiv;
        internal static ConfigEntry<bool> HoeheUebernehmen;

        private void Awake()
        {
            Aktiv = Config.Bind("Allgemein", "Aktiv", true,
                "Beim Loslassen der Umsehen-Taste dreht sich der Koerper in die Blickrichtung, statt dass der Kopf zurueckschnappt.");
            HoeheUebernehmen = Config.Bind("Allgemein", "Hoehe uebernehmen", true,
                "Auch den Blick nach oben/unten uebernehmen. Aus = nur links/rechts, die Hoehe springt zurueck.");
            new Harmony("kaan.blickrichtung").PatchAll(typeof(LookPatch));
            Logger.LogInfo("Blickrichtung geladen.");
        }
    }

    [HarmonyPatch(typeof(Player), nameof(Player.Look))]
    internal static class LookPatch
    {
        [HarmonyPrefix]
        private static void Prefix(Player __instance)
        {
            if (!Plugin.Aktiv.Value || !__instance._resetLook || !__instance.IsYourPlayer)
                return;

            float gier = -__instance._horizontal;
            float nick = Plugin.HoeheUebernehmen.Value ? __instance.HeadRotation.x : 0f;
            if (Mathf.Abs(gier) < 0.01f && Mathf.Abs(nick) < 0.01f)
                return;

            // ignoreClamp: sonst begrenzt die Drehgeschwindigkeit pro Bild
            // (und die Mausempfindlichkeit) den Winkel und der Koerper
            // dreht nur ein Stueck weit mit.
            __instance.Rotate(new Vector2(gier, nick), ignoreClamp: true);

            __instance._horizontal = 0f;
            if (Plugin.HoeheUebernehmen.Value)
                __instance._vertical = 0f;
        }
    }
}
