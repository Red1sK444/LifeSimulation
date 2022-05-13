using System;
using OOP_LifeSimulation.EntitiesExtended.LifecycleManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.Inventory;

namespace OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers
{
    public class OmnivorousTameableLifecycleManager : TameableLifecycleManager
    {
        public OmnivorousTameableLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood,
            isPartner)
        {
        }

        protected override bool HaveToFindFood()
        {
            return food == null || foodFinder.IsTargetCell(_foodCell) == false ||
                   food.GetType() == typeof(Entity) && ((Entity) food).StateCheck() == EntityState.Dead;
        }

        protected override Unit GetFoodFromCell()
        {
            if (_foodCell.IsPlantHere())
            {
                return _foodCell.GetPlant();
            }

            return _foodCell.GetOtherEntityByType(Entity);
        }
    }
}