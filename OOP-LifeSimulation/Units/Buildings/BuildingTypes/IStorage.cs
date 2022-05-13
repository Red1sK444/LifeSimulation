using System;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Villages;

namespace OOP_LifeSimulation.Buildings
{
    public interface IStorage<out T> where T: Item
    {
        public Type GetResourceType();
        public void PutResource(Item item);
        public T ExtractResource();
        
        public Cell GetCell();
        public void SetVillage(Village village);
    }
}