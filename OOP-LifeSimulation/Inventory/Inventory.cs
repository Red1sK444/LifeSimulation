using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation.Inventory
{
    public class Inventory : IShowingInfo
    {
        //private int _capacity = 10;
        public const int Capacity = 30;
        private Human owner;
        private List<Item> items = new List<Item>(Capacity);
        public Inventory(Human owner)
        {
            this.owner = owner;
        }
        
        public void DeleteItem(Item item)
        {
            items.Remove(item);
            item.SeparateFromInventory();
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }

        public Item FetchItem(Item item)
        {
            DeleteItem(item);
            return item;
        }

        public bool CanAddItem()
        {
            return items.Capacity > items.Count;
        }

        public Item FindItem(Func<Item, bool> compareFunc)
        {
            foreach (var item in items.ToList())
            {
                if (compareFunc(item))
                {
                    return item;
                }
            }

            return null;
        }
        
        public List<Item> FindItems(Func<Item, bool> compareFunc)
        {
            var itemList = new List<Item>();
            foreach (var item in items.ToList().Where(compareFunc))
            {
                itemList.Add(item);
            }
            return itemList;
        }
        
        public List<Resource> FindResourceMost()
        {
            var iron = items.OfType<Iron>().ToList();
            var gold = items.OfType<Gold>().ToList();
            
            var stone = items.OfType<Stone>().ToList();
            var wood = items.OfType<Wood>().ToList();
            
            var result = new List<Resource>();
            result.AddRange(gold);
            
            if (iron.Count > gold.Count)
            {
                result.Clear();
                result.AddRange(iron);
            }

            if (stone.Count > result.Count)
            {
                result.Clear();
                result.AddRange(stone);
            }

            if (wood.Count > result.Count)
            {
                result.Clear();
                result.AddRange(wood);
            }
            
            return result;
        }

        public string SendInfo()
        {
            var inventorySize =
                $"Inventory Size = {items.Count}/{Capacity}";
            
            var greeneryCounter = 0;
            var meatCounter = 0;
            
            foreach (var item in items)
            {
                if (item is IEatableForCarnivore) meatCounter++;
                else greeneryCounter++;
            }

            var ironCount = $"\tIronCount = {items.OfType<Iron>().Count()}";
            var woodCount = $"\tWoodCount = {items.OfType<Wood>().Count()}";
            var goldCount = $"\tGoldCount = {items.OfType<Gold>().Count()}";
            var stoneCount = $"\tStoneCount = {items.OfType<Stone>().Count()}";
            var meatCount = $"\tMeat Count = {meatCounter}";
            var plantCount = $"\tPlant Count = {greeneryCounter}";
            return $"{inventorySize}\n{meatCount}\n{plantCount}\n{ironCount}\n{woodCount}\n{goldCount}\n{stoneCount}";
        }
    }
}