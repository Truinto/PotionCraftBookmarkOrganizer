﻿using HarmonyLib;
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
    public class StoreSubRailBookmarksAfterBookmarkMove
    { 
        [HarmonyPatch(typeof(Bookmark), nameof(Bookmark.UpdateMovingState))]
        public class Bookmark_UpdateMovingState
        {
            static void Postfix(BookmarkMovingState value)
            {
                Ex.RunSafe(() => UpdateLayerForActiveSubRailBookmark(value));
            }
        }

        private static void UpdateLayerForActiveSubRailBookmark(BookmarkMovingState value)
        {
            if (value != BookmarkMovingState.Idle) return;
            RecipeBookService.UpdateBookmarkGroupsForCurrentRecipe();
            SubRailService.UpdateSubRailForSelectedIndex(RecipeBook.Instance.currentPageIndex);
            return;
        }
    }
}
