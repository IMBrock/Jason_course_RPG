﻿using UnityEditor;
using UnityEngine;

namespace PlayTests
{
    public static class inventory_helpers
    {
        public static UIInventoryPanel GetInventoryPanelWithItems(int numberOfItems = 0)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<UIInventoryPanel>("Assets/Prefab/UI/InventoryPanel.prefab");
            var panel = Object.Instantiate(prefab);
            var inventory = GetInventory(numberOfItems);
            panel.Bind(inventory);
            return panel;
        }
        
        public static Inventory GetInventory(int numberOfItems = 0)
        {
            var inventory = new GameObject("Inventory").AddComponent<Inventory>();
            for (int i = 0; i < numberOfItems; i++)
            {
                var item = GetItem();
                inventory.Pickup(item);
            }

            return inventory;
        }
        public static Item GetItem()
        {
            var itemGameObject = new GameObject("Item", typeof(SphereCollider));
            return itemGameObject.AddComponent<Item>();
        }
    }
}