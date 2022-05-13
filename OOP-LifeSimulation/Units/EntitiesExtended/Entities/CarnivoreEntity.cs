using System;
using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class CarnivoreEntity : Entity
    {
        public CarnivoreEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Crimson;
            Lifecycle = new CarnivoreLifecycleManager(this, Map.Field,
                current => current.IsEntityHere() && current.GetOtherEntityByType(this) != null,
                current => current.IsEntityHere() && current.GetEntityToReproduce(this) != null );
        }
    }
}