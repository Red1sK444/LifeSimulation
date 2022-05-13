using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers
{
    public class LifecycleManager
    {
        public static readonly IList<Coords> Movement = new ReadOnlyCollection<Coords>(new[]
        {
            new Coords(-1, -1),
            new Coords(0, -1),
            new Coords(1, -1),
            new Coords(1, 0),
            new Coords(1, 1),
            new Coords(0, 1),
            new Coords(-1, 1),
            new Coords(-1, 0)
        });


        protected Entity Entity;
        protected Cell[,] _field;
        protected FinderWithCondition foodFinder;
        protected FinderWithCondition partnerFinder;
        protected Cell _foodCell;
        public Unit food;
        protected FreeMovement FreeMovement;
        protected TargetMovement PartnerMovement;
        protected TargetMovement FoodMovement;
        protected int _prevMove = 8;

        public LifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood, Func<Cell, bool> isPartner)
        {
            Entity = entity;
            _field = field;
            foodFinder = new FinderWithCondition(field, isFood);
            partnerFinder = new FinderWithCondition(field, isPartner);
            PartnerMovement = new StraightMovement(field);
            
            FreeMovement = new AntMovement(field, UpdatePrevMove);
            FoodMovement = new LadderMovement(field);
        }

        public void SetMovementMethods(TargetMovement foodMovement, FreeMovement freeMovement)
        {
            this.FoodMovement = foodMovement;
            this.FreeMovement = freeMovement;
        }

        protected virtual bool HaveToFindNewPartner()
        {
            return Entity.ReproductionCD <= 0 &&
                   (Entity.Partner == null || Entity.Partner.StateCheck() == EntityState.Dead);
        }

        protected virtual Cell IsNotStarvingActions(Cell current)
        {
            if (HaveToFindNewPartner())
            {
                var partnerCell = partnerFinder.Find(current.Position, _prevMove);
                if (partnerCell != null)
                {
                    Entity.Partner = partnerCell.GetEntityToReproduce(Entity);
                }

                if (Entity.Partner != null)
                {
                    Entity.Partner.Partner = Entity;
                }
            }

            if (Entity.Partner != null && Entity.Partner.ReproductionCD <= 0)
            {
                if (Math.Abs(current.Position.X - Entity.Partner.Cell.Position.X) <= 3
                    && Math.Abs(current.Position.Y - Entity.Partner.Cell.Position.Y) <= 3)
                {
                    return Entity.Partner.Cell;
                }

                return PartnerMovement.MoveByWay(current, Entity.Partner.Cell);
            }

            return DefaultMove(current);
        }

        protected virtual Cell DefaultMove(Cell current)
        {
            return FreeMovement.MoveTo(current.Position, _prevMove);
        }

        protected virtual bool HaveToFindFood()
        {
            return food == null || foodFinder.IsTargetCell(_foodCell) == false || !_foodCell.HasThisUnit(food);
        }

        protected virtual Unit GetFoodFromCell()
        {
            return _foodCell.GetPlant();
        }

        protected virtual Cell IsStarvingActions(Cell current)
        {
            // if (_foodCell == null || foodFinder.IsTargetCell(_foodCell) == false)
            // {
            //     _foodCell = foodFinder.Find(current.Position, _prevMove);
            // }
            //
            // Cell nextCell;
            // if (_foodCell != null && _foodCell.IsPlantHere())
            // {
            //     nextCell = foodMovement(_field, current, _foodCell);
            //     if (nextCell == current)
            //     {
            //         nextCell = freeMovement.MoveTo(current.Position, _prevMove);
            //     }
            // }
            // else
            // {
            //     nextCell = freeMovement.MoveTo(current.Position, _prevMove);
            // }
            //
            // return nextCell;

            if (HaveToFindFood())
            {
                _foodCell = foodFinder.Find(current.Position, _prevMove);
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
                nextCell = DefaultMove(current);
            }

            return nextCell;
        }


        public Cell MoveTo(bool isStarving, Cell current)
        {
            if (isStarving)
            {
                return IsStarvingActions(current);
            }

            return IsNotStarvingActions(current);
        }

        public void UpdatePrevMove(int prevMove)
        {
            _prevMove = prevMove;
        }
    }
}