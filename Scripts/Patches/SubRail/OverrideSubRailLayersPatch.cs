using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.UIElements.Bookmarks;
using PotionCraft.ObjectBased.UIElements.Books;
using PotionCraft.ObjectBased.UIElements.Books.RecipeBook;
using PotionCraft.ObjectBased.UIElements.PotionCraftPanel;
using PotionCraft.ScriptableObjects;
using PotionCraftBookmarkOrganizer.Scripts.Services;
using PotionCraftBookmarkOrganizer.Scripts.Storage;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace PotionCraftBookmarkOrganizer.Scripts.Patches
{
    public class OverrideSubRailLayersPatch
    { 
        [HarmonyPatch(typeof(BookmarkRail), nameof(BookmarkRail.SpawnLayers))]
        public class BookmarkRail_SpawnLayers
        {
            static bool Prefix(BookmarkRail __instance)
            {
                return Ex.RunSafe(() => OverrideSubRailLayers(__instance));
            }
        }

        private static bool OverrideSubRailLayers(BookmarkRail instance)
        {
            if (SubRailService.IsInvisiRail(instance))
            {
                instance.layers = new[] { StaticStorage.InvisiRailLayer };
                return false;
            }
            if (!SubRailService.IsSubRail(instance)) return true;

            instance.layers = StaticStorage.SubRailLayers.ToArray();

            return false;
        }
    }
}
