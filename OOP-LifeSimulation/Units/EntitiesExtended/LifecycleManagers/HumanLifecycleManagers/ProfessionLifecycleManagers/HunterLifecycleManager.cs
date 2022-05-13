using System;
using OOP_LifeSimulation.Actions;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntityMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers
{
    public class HunterLifecycleManager : HumanLifecycleManager
    {
        private FinderWithCondition _entityToHuntFinder;
        private CollectorHunterActions _hunterActions;

        public HunterLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood, Func<Cell, bool> isPartner)
            : base(entity, field, isFood, isPartner)
        {
            bool EntityToHunt(Cell current) => current.IsEntityHere() && current.GetOtherEntityByType(Entity) != null;

            _entityToHuntFinder = new FinderWithCondition(field, EntityToHunt);
            _hunterActions = new CollectorHunterActions(Entity);
        }

        protected override Cell DefaultActions(Cell current)
        {
            if (Entity.Inventory.CanAddItem())
            {
                return HuntAction(current);
            }

            return PutHuntedFood(current);
        }

        private Cell PutHuntedFood(Cell current)
        {
            Cell nextCell;
            if (Entity.House.Village.Barn != null)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Village.Barn.Cell);
                if (Entity.House.Village.Barn.Cell == nextCell)
                {
                    Entity.CurrentAction = _hunterActions.PutIntoBarnAction;
                }

                return nextCell;
            }
            
            nextCell = PartnerMovement.MoveByWay(current, Entity.House.Cell);
            if (Entity.House.Cell == nextCell)
            {
                Entity.CurrentAction = HumanActions.PutIntoHouseAction;
            }

            return nextCell;
        }

        private Cell HuntAction(Cell current)
        {
            if (HaveToFindFood())
            {
                _foodCell = _entityToHuntFinder.Find(current.Position, _prevMove);
                if (_foodCell != null)
                {
                    food = GetFoodFromCell();
                }
                else
                {
                    food = null;
                }
            }

            Cell nextCell;
            if (food != null)
            {
                nextCell = FoodMovement.MoveByWay(current, food.Cell);
                if (nextCell == current)
                {
                    nextCell = DefaultMove(current);
                }
            }
            else
            {
                nextCell = PutHuntedFood(current);
            }

            return nextCell;
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