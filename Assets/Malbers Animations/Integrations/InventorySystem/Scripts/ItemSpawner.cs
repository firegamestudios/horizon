using MalbersAnimations.Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.InventorySystem
{
    [AddComponentMenu("Malbers/Inventory/Item Spanwer")]
    public class ItemSpawner : MonoBehaviour
    {
        [Tooltip("Items that can be spawned")]
        public List<ItemSpawnerData> items = new();

        public Transform spawnPoint;
        public float radius = 0.3f;

        public void SpawnItems()
        {
            float ChanceValue = Random.value; //Get a random value from 0 to 1

            foreach (var item in items)
            {
                if (item.CanSpawn(ChanceValue))
                {
                    Spawn(item.Item,
                        Random.Range(item.Min.Value,item.Max.Value), 
                        Random.Range(item.QuantityMin.Value, item.QuantityMax.Value));
                }
            }
        }

        private void Spawn(Item item, int ammount, int quantity)
        {
            for (int i = 0; i < ammount; i++)
            {
                var newItem = Instantiate(item.inWorldPrefab, spawnPoint.transform.TransformPoint(RandomPointOnCircleEdge(radius)), Quaternion.identity);

                if (newItem.TryGetComponent<InventoryItem>(out var inventoryItem))
                {
                    inventoryItem.quantity = quantity;
                }
            }

        }

        private Vector3 RandomPointOnCircleEdge(float radius)
        {
            var vector2 = Random.insideUnitCircle.normalized * radius;
            return new Vector3(vector2.x, 0, vector2.y);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            spawnPoint = transform;

            items = new() { new ItemSpawnerData() {  Chance = new(0,1f), Min = new(1), Max = new(1)  } };
        }

        void OnDrawGizmos()
        {
            if (spawnPoint != null)
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(spawnPoint.position, transform.up, radius);
            }
        }


        private void OnValidate()
        {
            foreach (var item in items)
            {
                if (item.Item != null)
                {
                    item.description = $"[{item.Item.name}] Amount[{item.Min.Value} ~ {item.Max.Value}]" +
                        $" Chance[{(item.Chance.maxValue - item.Chance.minValue):F2}]";

                    item.Min.Value = Mathf.Min(item.Min.Value, item.Max.Value);
                    item.Max.Value = Mathf.Max(item.Min.Value, item.Max.Value);
                    //item.Chance.Value = Mathf.Clamp01((float)item.Chance.Value);
                }
            }
        }
#endif

    }

    [System.Serializable]
    public class ItemSpawnerData
    {
        [HideInInspector] public string description;

        [Tooltip ("Item that can spawn")]
        public Item Item;

        [Tooltip("Minimum Amount of items to spawn")]
        public IntReference Min = new (1);
        [Tooltip("Maximum amount of items to spawn")]
        public IntReference Max = new(1);

        [Tooltip("Minimum Quantity to set on the Item quantity value")]
        public IntReference QuantityMin = new (1);
        [Tooltip("Maximum Quantity to set on the Item quantity value")]
        public IntReference QuantityMax = new (1);

        [Tooltip ("Probability for the Item to be spawned")]
        [MinMaxRange(0, 1)] 
        public RangedFloat Chance  = new(0,1);


        /// <summary>  Using the chance value, determines if the item can be spawned </summary>
        public bool CanSpawn(float value)  => Chance.IsInRange(value);  
    }                                 
}
