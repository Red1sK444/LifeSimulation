using System;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement.FreeMovement;

namespace OOP_LifeSimulation.EntityMovement
{
    public class CarnivoreLifecycleManager : LifecycleManager
    {
        public CarnivoreLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(
            entity, field, isFood, isPartner)
        {
            // freeMovement = new AntMovement(field, UpdatePrevMove);
            // foodMovement = TargetMovement.BurstWay;
        }

        protected override bool HaveToFindFood()
        {
            return food == null || ((Entity) food).StateCheck() == EntityState.Dead;
        }

        protected override Unit GetFoodFromCell()
        {
            return _foodCell.GetOtherEntityByType(Entity); 
        }

        // protected override Cell IsStarvingActions(Cell current)
        // {
        //     if (food == null || food.StateCheck() == EntityState.Dead)
        //     {
        //         _foodCell = foodFinder.Find(current.Position, _prevMove);
        //         if (_foodCell != null)
        //         {
        //             food = _foodCell.GetOtherEntityByType(Entity);    
        //         }
        //         else
        //         {
        //             food = null;
        //         }
        //     }
        //
        //     Cell nextCell;
        //     if (food != null)
        //     {
        //         nextCell = foodMovement(_field, current, food.Cell);
        //         if (nextCell == current)
        //         {
        //             nextCell = freeMovement.MoveTo(current.Position, _prevMove);
        //         }
        //     }
        //     else
        //     {
        //         nextCell = freeMovement.MoveTo(current.Position, _prevMove);
        //     }
        //
        //     return nextCell;
        // }
    }
}