using System;
using OOP_LifeSimulation.EntitiesExtended.LifecycleManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.Inventory;

namespace OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers
{
    public class HerbivoreTameableLifecycleManager : TameableLifecycleManager
    {
        public HerbivoreTameableLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood,
            isPartner)
        {
        }

        protected override bool HaveToFindFood()
        {
            return food == null || foodFinder.IsTargetCell(_foodCell) == false;
        }

        protected override Unit GetFoodFromCell()
        {
            return _foodCell.GetPlant();
        }
    }
}