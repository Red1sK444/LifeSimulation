using System;
using OOP_LifeSimulation.Actions;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers
{
    public class HumanLifecycleManager : OmnivorousLifecycleManager
    {
        protected Human Entity;
        private FinderWithCondition homeSourceFinder;
        private FinderWithCondition houseFinder;
        protected HumanActions HumanActions;
        

        public HumanLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood, Func<Cell, bool> isPartner)
            : base(entity, field, isFood, isPartner)
        {
            Entity = (Human) entity;
            homeSourceFinder = new FinderWithCondition(field,
                cell => cell.IsSourceHere() && cell.GetSource().GetResourceType() == typeof(Wood));
            houseFinder = new FinderWithCondition(field, cell => cell.IsHouseHere());
            HumanActions = new HumanActions(Entity);
        }

        private Cell ReproductionActions(Cell current)
        {
            if (HaveToFindNewPartner())
            {
                FindPartnerAction(current);
            }

            if (HaveToFindNewPartner() == false && Entity.ReproductionCD <= 0 && Entity.House != null &&
                Entity.Partner.StateCheck() == EntityState.Healthy)
            {
                return ReproductionAction(current);
            }

            if (Entity.Inventory.CanAddItem())
            {
                if (Entity.House == null && Entity.Partner != null && Entity.Sex == EntitySex.Male)
                {
                    return CreateHouse(current);
                }

                if (Entity.House is {Status: StorageState.InBuild})
                {
                    return BuildHouse(current);
                }
            }

            return null;
        }

        protected virtual Cell DefaultActions(Cell current)
        {
            if (Entity.Inventory.CanAddItem())
            {
                return base.IsStarvingActions(current);
            }

            if (Entity.House is {Status: StorageState.Built})
            {
                return PutIntoHouse(current);
            }

            return DefaultMove(current);
        }


        protected override Cell IsNotStarvingActions(Cell current)
        {
            var nextCell = ReproductionActions(current);
            return nextCell ?? DefaultActions(current);
        }

        protected Cell PutIntoHouse(Cell current)
        {
            var nextCell = GoToHouse(current);
            if (Entity.House.Cell == nextCell)
            {
                Entity.CurrentAction = HumanActions.PutIntoHouseAction;
            }

            return nextCell;
        }

        private void FindPartnerAction(Cell current)
        {
            var partnerCell = partnerFinder.Find(current.Position, _prevMove);
            if (partnerCell != null)
            {
                Entity.Partner = partnerCell.GetEntityToReproduce(Entity);
            }

            if (Entity.Partner != null)
            {
                Entity.Partner.Partner = Entity; 
                if (Entity?.House?.Village != null)
                {
                    (Entity.Partner as Human)?.SetHouse(Entity.House);
                    return;
                }

                if ((Entity.Partner as Human)?.House?.Village != null)
                {
                    Entity.SetHouse((Entity.Partner as Human)?.House);
                    return;
                }
                
                (Entity.Partner as Human)?.SetHouse(Entity.House);
                Entity.SetHouse((Entity.Partner as Human)?.House);
            }
        }

        private Cell ReproductionAction(Cell current)
        {
            var nextCell = PartnerMovement.MoveByWay(current, Entity.House.Cell);
            if (Entity.House.Cell == nextCell && Entity.Partner.Cell == nextCell)
            {
                Entity.CurrentAction = Entity.ReproduceAction;
            }

            return nextCell;
        }

        private Cell CreateHouse(Cell current)
        {
            Cell nextCell;
            if (Entity.Inventory.FindItems(item => item is Wood).Count >= House.WoodCountToCreate)
            {
                var nearestHouseCell = houseFinder.Find(current.Position, _prevMove);
                if (nearestHouseCell != null)
                {
                    nextCell = PartnerMovement.MoveByWay(current, nearestHouseCell);
                    
                    if (1 <= Math.Abs(nextCell.Position.X - nearestHouseCell.Position.X) &&
                        Math.Abs(nextCell.Position.X - nearestHouseCell.Position.X) <= 2
                        && 1 <= Math.Abs(nextCell.Position.Y - nearestHouseCell.Position.Y) &&
                        Math.Abs(nextCell.Position.Y - nearestHouseCell.Position.Y) <= 2)
                    {
                        Entity.CurrentAction = HumanActions.CreateHouseAction;
                    }
                    
                    return nextCell;
                }

                Entity.CurrentAction = HumanActions.CreateHouseAction;
                return DefaultMove(current);
            }

            var sourceCell = homeSourceFinder.Find(current.Position, _prevMove);
            if (sourceCell == null)
            {
                return DefaultMove(current);
            }

            nextCell = PartnerMovement.MoveByWay(current, sourceCell);
            if (sourceCell == nextCell)
            {
                Entity.CurrentAction = HumanActions.SourceAction;
            }

            return nextCell;
        }

        private Cell BuildHouse(Cell current)
        {
            Cell nextCell;
            if (Entity.Inventory.FindItems(item => item is Wood).Count == Entity.House.ResourceCountToBuild)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Cell);
                if (Entity.House.Cell == nextCell)
                {
                    Entity.CurrentAction = HumanActions.BuildHouseAction;
                }

                return nextCell;
            }

            var sourceCell = homeSourceFinder.Find(current.Position, _prevMove);
            if (sourceCell == null)
            {
                return DefaultMove(current);
            }

            nextCell = PartnerMovement.MoveByWay(current, sourceCell);
            if (sourceCell == nextCell)
            {
                Entity.CurrentAction = HumanActions.SourceAction;
            }

            return nextCell;
        }

        private Cell GoToHouse(Cell current)
        {
            return PartnerMovement.MoveByWay(current, Entity.House.Cell);
        }
        
        private Cell GoToBarn(Cell current)
        {
            return PartnerMovement.MoveByWay(current, Entity.House.Village.Barn.Cell);
        }

        protected override Cell IsStarvingActions(Cell current)
        {
            if (Entity.Inventory.FindItem(item => item is IEatable) != null)
            {
                Entity.CurrentAction = HumanActions.EatFromInventory;
                return IsNotStarvingActions(current);
            }

            if (Entity.House?.Village?.Barn?.ResourceCount > 0)
            {
                var nextCell = GoToBarn(current);
                if (nextCell == Entity.House.Village.Barn.Cell)
                {
                    Entity.CurrentAction = HumanActions.EatFromBarn;
                }

                return nextCell;
            }

            if (Entity.House?.FindItem(item => item is IEatable) != null)
            {
                var nextCell = GoToHouse(current);
                if (nextCell == Entity.House.Cell)
                {
                    Entity.CurrentAction = HumanActions.EatFromHouse;
                }

                return nextCell;
            }

            return base.IsStarvingActions(current);
        }

        protected override bool HaveToFindNewPartner()
        {
            return Entity.Partner == null || Entity.Partner.StateCheck().Equals(EntityState.Dead);
        }
    }
}