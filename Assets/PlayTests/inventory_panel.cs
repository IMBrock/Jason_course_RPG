using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlayTests
{
    public class ui_inventory_slot
    {
        [Test]
        public void when_item_is_set_changes_icon_to_match()
        {
            var inventoryPanel = inventory_panel.GetInventoryPanel();
            var slot = inventoryPanel.Slots[0];
            var item = Substitute.For<IItem>();
            var sprite = Sprite.Create(Texture2D.redTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            item.Icon.Returns(sprite);
            
            slot.SetItem(item);
            
            Assert.AreSame(sprite, slot.Icon);
        }
        
        [Test]
        public void when_item_is_set_image_is_enabled()
        {
            var inventoryPanel = inventory_panel.GetInventoryPanel();
            var slot = inventoryPanel.Slots[0];
            var item = Substitute.For<IItem>();
            var sprite = Sprite.Create(Texture2D.redTexture, new Rect(0, 0, 4, 4), Vector2.zero);
            item.Icon.Returns(sprite);
            
            slot.SetItem(item);
            
            Assert.IsTrue(slot.IconImageEnabled);
        }
        
        [Test]
        public void when_item_is_not_set_image_is_disabled()
        {
            var inventoryPanel = inventory_panel.GetInventoryPanel();
            var slot = inventoryPanel.Slots[0];
            
            slot.SetItem(null);
            
            Assert.IsFalse(slot.IconImageEnabled);
        }
    }
    
    public class inventory_panel
    {
        [UnityTearDown]
        public IEnumerator teardown()
        {
            var inventory = Object.FindObjectOfType<Inventory>();
            var inventoryPanel = Object.FindObjectOfType<UIInventoryPanel>();

            if (inventory != null)
                Object.Destroy(inventory.gameObject);
            if (inventoryPanel != null)
                Object.Destroy(inventoryPanel.gameObject);

            yield return null;
        }
        
        
        [Test]
        public void has_25_slots()
        {
            var inventoryPanel = GetInventoryPanel();
            
            Assert.AreEqual(25, inventoryPanel.SlotCount);
        }

        [Test]
        public void bound_to_empty_inventory_has_all_slots_empty()
        {
            var inventoryPanel = GetInventoryPanel();
            var inventory = GetInventory();

            inventoryPanel.Bind(inventory);

            foreach (var slot in inventoryPanel.Slots)
            {
                Assert.IsTrue(slot.IsEmpty);
            }
        }

        [UnityTest]
        public IEnumerator bound_to_inventory_fills_slot_for_each_item([NUnit.Framework.Range(0, 25)] int numberOfItems)
        {
            var inventoryPanel = GetInventoryPanel();
            var inventory = GetInventory(numberOfItems);

            foreach (var slot in inventoryPanel.Slots)
            {
                Assert.IsTrue(slot.IsEmpty);
            }
            
            inventoryPanel.Bind(inventory);
            for (int i = 0; i < inventoryPanel.SlotCount; i++)
            {
                bool shouldBeEmpty = i >= numberOfItems;
                Assert.AreEqual(shouldBeEmpty, inventoryPanel.Slots[i].IsEmpty);
            }
            
            Object.Destroy(inventoryPanel.gameObject);
            Object.Destroy(inventory.gameObject);
            yield return null;
        }

        [Test]
        public void places_item_in_slot_when_item_is_added_to_inventory()
        {
            var inventoryPanel = GetInventoryPanel();
            var inventory = GetInventory();
            var item = GetItem();
            
            inventoryPanel.Bind(inventory);
            
            Assert.IsTrue(inventoryPanel.Slots[0].IsEmpty);
            
            inventory.Pickup(item);
            
            Assert.IsFalse(inventoryPanel.Slots[0].IsEmpty);
        }
        
        [Test]
        public void does_not_add_items_from_previously_bound_inventory_when_item_is_added()
        {
            var inventoryPanel = GetInventoryPanel();
            var inventoryOld = GetInventory();
            var inventoryCurrent = GetInventory();
            var item = GetItem();
            
            inventoryPanel.Bind(inventoryOld);
            inventoryPanel.Bind(inventoryCurrent);
            
            Assert.IsTrue(inventoryPanel.Slots[0].IsEmpty);
            
            inventoryOld.Pickup(item);
            
            Assert.IsTrue(inventoryPanel.Slots[0].IsEmpty);
        }

        [Test]
        public void bound_to_null_inventory_has_empty_slots()
        {
            var inventoryPanel = GetInventoryPanel();
            inventoryPanel.Bind(null);

            for (int i = 0; i < inventoryPanel.SlotCount; i++)
            {
                Assert.IsTrue(inventoryPanel.Slots[i].IsEmpty);
            }
        }
        
        [Test]
        public void bound_to_valid_inventory_then_bound_to_null_inventory_has_empty_slots()
        {
            var inventoryPanel = GetInventoryPanel();
            var inventory = GetInventory(1);
            
            inventoryPanel.Bind(inventory);
            inventoryPanel.Bind(null);

            for (int i = 0; i < inventoryPanel.SlotCount; i++)
            {
                Assert.IsTrue(inventoryPanel.Slots[i].IsEmpty);
            }
        }

        [Test]
        public void updates_slots_when_items_are_moved()
        {
            var inventoryPanel = GetInventoryPanel();
            var inventory = GetInventory(1);
            inventoryPanel.Bind(inventory);
            
            inventory.Move(0, 4);
            Assert.AreSame(inventory.GetItemInSlot(4), inventoryPanel.Slots[4].Item);
        }
        
        private Item GetItem()
        {
            return inventory_helpers.GetItem();
        }

        private Inventory GetInventory(int numberOfItems = 0)
        {
            return inventory_helpers.GetInventory(numberOfItems);
        }

        public static UIInventoryPanel GetInventoryPanel()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<UIInventoryPanel>("Assets/Prefabs/UI/Inventory Panels.prefab");
            return Object.Instantiate(prefab);
        }
    }
}