using System;
using OOP_LifeSimulation.Buildings;

namespace OOP_LifeSimulation.Inventory.Resources
{
    public class Resource : Item
    {
        protected Resource(Inventory inventory) : base(inventory)
        {
        }
        
        public Resource(){}
        public override void Use(Object useOn)
        {
            Inventory?.DeleteItem(this);
            (useOn as IStorage<Item>)?.PutResource(this);
        }
    }
}