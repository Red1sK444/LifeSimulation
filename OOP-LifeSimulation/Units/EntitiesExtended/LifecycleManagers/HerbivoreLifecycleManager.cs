using System;
using OOP_LifeSimulation.EntityMovement;

namespace OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers
{
    public class HerbivoreLifecycleManager : LifecycleManager
    {
        public HerbivoreLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood, isPartner)
        {
            // freeMovement = new AreaAntMovement(field, UpdatePrevMove, entity.Cell.Position);
            // foodMovement = TargetMovement.LadderWay;
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