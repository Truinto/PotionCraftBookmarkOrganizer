﻿using HarmonyLib;
using PotionCraft.ManagersSystem;
using PotionCraft.ObjectBased.UIElements.Bookmarks;
using PotionCraft.ObjectBased.UIElements.Books.RecipeBook;
using PotionCraft.ObjectBased.UIElements.PotionCraftPanel;
using PotionCraft.ScriptableObjects;
using PotionCraftBookmarkOrganizer.Scripts.ClassOverrides;
using PotionCraftBookmarkOrganizer.Scripts.Services;
using PotionCraftBookmarkOrganizer.Scripts.Storage;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine.Rendering;

namespace PotionCraftBookmarkOrganizer.Scripts.Patches
{
    /// <summary>
    /// Setup indexes for subbookmark and adjust all other indexes as needed
    /// If the first bookmark has changed update main bookmark with that icon
    /// </summary>
    public class DisableGrabbingForInactiveStaticBookmarkPatch
    { 
        [HarmonyPatch(typeof(BookmarkButtonInactive), nameof(BookmarkButtonInactive.OnGrabPrimary))]
        public class InactiveBookmarkButton_OnGrabPrimary
        {
            static bool Prefix(BookmarkButtonInactive __instance)
            {
                return Ex.RunSafe(() => DisableGrabbingForInactiveStaticBookmark(__instance));
            }
        }

        private static bool DisableGrabbingForInactiveStaticBookmark(BookmarkButtonInactive instance)
        {
            var bookmark = instance.bookmark;
            return bookmark != StaticStorage.StaticBookmark;
        }
    }
}
