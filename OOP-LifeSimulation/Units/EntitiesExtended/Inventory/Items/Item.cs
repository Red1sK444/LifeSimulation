using System;

namespace OOP_LifeSimulation.Inventory
{
    public abstract class Item
    {
        protected Inventory Inventory;

        public Item(Inventory inventory)
        {
            Inventory = inventory;
        }

        public Item(){}

        public void SetInventory(Inventory inventory)
        {
            Inventory = inventory;
        }

        public void SeparateFromInventory()
        {
            Inventory = null;
        }
        public abstract void Use(Object useOn);
    }
}