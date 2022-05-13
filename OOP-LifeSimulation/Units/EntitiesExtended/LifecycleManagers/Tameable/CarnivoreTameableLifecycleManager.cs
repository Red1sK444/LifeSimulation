using System;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntitiesExtended.LifecycleManagers;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.Inventory;

namespace OOP_LifeSimulation.EntityMovement
{
    public class CarnivoreTameableLifecycleManager : TameableLifecycleManager
    {
        public CarnivoreTameableLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood,
            isPartner)
        {
        }

        protected override bool HaveToFindFood()
        {
            return food == null || ((Entity) food).StateCheck() == EntityState.Dead;
        }

        protected override Unit GetFoodFromCell()
        {
            return _foodCell.GetOtherEntityByType(Entity);
        }
    }
}