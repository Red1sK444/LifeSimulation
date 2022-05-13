using System;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;

namespace OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers
{
    public class OmnivorousLifecycleManager : LifecycleManager
    {
        public OmnivorousLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood, isPartner)
        {
            // freeMovement = new AllRandomMovement(field, UpdatePrevMove);
            // foodMovement = TargetMovement.StraightWay;
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