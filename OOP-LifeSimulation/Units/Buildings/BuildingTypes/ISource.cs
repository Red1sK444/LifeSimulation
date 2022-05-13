using System;
using OOP_LifeSimulation.Inventory.Resources;

namespace OOP_LifeSimulation.Buildings
{
    public interface ISource<out T>  where T: Resource
    {
        public Type GetResourceType();
        public T ExtractResource();
    }
}