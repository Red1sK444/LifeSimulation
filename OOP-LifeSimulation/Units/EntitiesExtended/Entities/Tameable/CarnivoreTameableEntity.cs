using System;
using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class CarnivoreTameableEntity : TameableEntity
    {
        public CarnivoreTameableEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Crimson;

            bool IsFood(Cell current) => current.IsEntityHere() && current.GetOtherEntityByType(this) != null &&
                                         current.GetOtherEntityByType(this) != owner;

            bool IsPartner(Cell current) => current.IsEntityHere() && current.GetEntityToReproduce(this) != null;

            Lifecycle = new CarnivoreLifecycleManager(this, Map.Field, IsFood, IsPartner);
            TameableLifecycle = new CarnivoreTameableLifecycleManager(this, Map.Field, IsFood, IsPartner);
            DetermineFoodInInventory = item => item is IEatableForCarnivore;
        }
    }
}