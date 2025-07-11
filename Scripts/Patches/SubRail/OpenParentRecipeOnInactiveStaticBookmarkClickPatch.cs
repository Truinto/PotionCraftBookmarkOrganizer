﻿
using HarmonyLib;
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
using System.Numerics;
using System.Reflection;
using UnityEngine.Rendering;

namespace PotionCraftBookmarkOrganizer.Scripts.Patches
{
    /// <summary>
    /// Setup indexes for subbookmark and adjust all other indexes as needed
    /// If the first bookmark has changed update main bookmark with that icon
    /// </summary>
    public class OpenParentRecipeOnInactiveStaticBookmarkClickPatch
    {
        [HarmonyPatch(typeof(BookmarkButtonInactive), nameof(BookmarkButtonInactive.OnReleasePrimary))]
        public class InactiveBookmarkButton_OnReleasePrimary
        {
            static bool Prefix(BookmarkButtonInactive __instance)
            {
                return Ex.RunSafe(() => DisableGrabbingForInactiveStaticBookmark(__instance));
            }
        }

        private static bool DisableGrabbingForInactiveStaticBookmark(BookmarkButtonInactive instance)
        {
            var bookmark = instance.bookmark;
            if (bookmark != StaticStorage.StaticBookmark) return true;
            //Set the parent recipe of this group to be active
            var recipeIndex = RecipeBookService.GetBookmarkStorageRecipeIndexForSelectedRecipe();
            var parentBookmark = bookmark.rail.bookmarkController.GetBookmarkByIndex(recipeIndex);
            bookmark.rail.bookmarkController.bookmarkControllersGroupController.SetActiveBookmark(parentBookmark, isBookmarkClicked: true);

            //update the visual state of the static bookmark
            bookmark.CurrentVisualState = BookmarkVisualState.Active;
            bookmark.activeBookmarkButton.slot.Selected = true;
            if (bookmark.activeBookmarkButton.thisCollider.bounds.Contains(Managers.Input.controlsProvider.CurrentMouseWorldPosition))
                bookmark.activeBookmarkButton.SetHovered(true);

            instance.grabConditionChecked = true;

            return false;
        }
    }
}
