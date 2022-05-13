using System;
using OOP_LifeSimulation.Actions;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers
{
    public class CollectorLifecycleManager : HumanLifecycleManager
    {
        private FinderWithCondition _plantToHuntFinder;
        private CollectorHunterActions _collectorActions;

        public CollectorLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood, isPartner)
        {
            bool PlantToHunt(Cell current) => current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                                                    && current.GetPlant().GrowthState
                                                                        .Equals(PlantGrowthState.Seed) ==
                                                                    false;
            
            _plantToHuntFinder = new FinderWithCondition(field, PlantToHunt);
            _collectorActions = new CollectorHunterActions(Entity);
        }

        protected override Cell DefaultActions(Cell current)
        {
            if (Entity.Inventory.CanAddItem())
            {
                return CollectAction(current);
            }

            return PutCollectedFood(current);
        }

        private Cell PutCollectedFood(Cell current)
        {
            Cell nextCell;
            if (Entity.House.Village.Barn != null)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Village.Barn.Cell);
                if (Entity.House.Village.Barn.Cell == nextCell)
                {
                    Entity.CurrentAction = _collectorActions.PutIntoBarnAction;
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

        private Cell CollectAction(Cell current)
        {
            if (HaveToFindFood())
            {
                _foodCell = _plantToHuntFinder.Find(current.Position, _prevMove);
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
                nextCell = PutCollectedFood(current);
            }

            return nextCell;
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