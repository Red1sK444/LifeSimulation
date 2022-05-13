using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using OOP_LifeSimulation.Actions;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers
{
    public class BuilderLifecycleManager : HumanLifecycleManager
    {
        private FinderWithCondition _barnSourceFinder;
        private FinderWithCondition _sourceFinder;
        public Type StorageTypeToCreate;
        private List<int> _storageShuffleList;
        private Cell _targetStorageCell;
        private BuilderActions _builderActions;

        public BuilderLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood, isPartner)
        {
            _barnSourceFinder = new FinderWithCondition(field,
                cell => cell.IsSourceHere() && cell.GetSource().GetResourceType() == typeof(Stone));
            _sourceFinder = new FinderWithCondition(field, cell => cell.IsSourceHere());
            _builderActions = new BuilderActions(Entity);
        }

        protected override Cell DefaultActions(Cell current)
        {
            var nextCell = BarnActions(current);
            return nextCell ?? StorageActions(current);
        }

        private Cell BarnActions(Cell current)
        {
            if (Entity.Inventory.CanAddItem())
            {
                if (Entity.House.Village?.Barn == null)
                {
                    return CreateBarn(current);
                }

                if (Entity.House.Village.Barn is {Status: StorageState.InBuild})
                {
                    return BuildBarn(current);
                }
            }

            return null;
        }

        private Cell CreateBarn(Cell current)
        {
            Cell nextCell;
            if (Entity.Inventory.FindItems(item => item is Stone).Count >= Barn.StoneCountToCreate)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Cell);
                for (var i = -2; i <= 2; i++)
                {
                    for (var j = -2; j <= 2; j++)
                    {
                        var newCords = new Coords(nextCell.Position.X + j,
                            nextCell.Position.Y + i);
                        if (0 <= newCords.X && newCords.X < _field.GetLength(0) && 0 <= newCords.Y &&
                            newCords.Y < _field.GetLength(0))
                        {
                            if (_field[newCords.Y, newCords.X].IsHouseHere() &&
                                _field[newCords.Y, newCords.X].GetHouse().Village == Entity.House.Village)
                            {
                                Entity.CurrentAction = _builderActions.CreateBarnAction;
                                return nextCell;
                            }
                        }
                    }
                }

                return nextCell;
            }

            return FindBarnSource(current);
        }

        private Cell FindBarnSource(Cell current)
        {
            var sourceCell = _barnSourceFinder.Find(current.Position, _prevMove);
            if (sourceCell == null)
            {
                return DefaultMove(current);
            }

            var nextCell = PartnerMovement.MoveByWay(current, sourceCell);
            if (sourceCell == nextCell)
            {
                Entity.CurrentAction = HumanActions.SourceAction;
            }

            return nextCell;
        }

        private Cell BuildBarn(Cell current)
        {
            Cell nextCell;
            if (Entity.Inventory.FindItems(item => item is Stone).Count ==
                Entity.House.Village.Barn.ResourceCountToBuild)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Village.Barn.Cell);
                if (Entity.House.Village.Barn.Cell == nextCell)
                {
                    Entity.CurrentAction = _builderActions.BuildBarnAction;
                }

                return nextCell;
            }

            return FindBarnSource(current);
        }

        private Cell StorageActions(Cell current)
        {
            Cell nextCell;
            if (Entity.Inventory.CanAddItem())
            {
                nextCell = BuildStorages(current);
                if (nextCell != null)
                {
                    return nextCell;
                }

                return CollectSources(current);
            }

            return PutIntoStorages(current);
        }

        private Cell CollectSources(Cell current)
        {
            var sourceCell = _sourceFinder.Find(current.Position, _prevMove);
            if (sourceCell == null)
            {
                return DefaultMove(current);
            }

            var nextCell = PartnerMovement.MoveByWay(current, sourceCell);
            if (sourceCell == nextCell)
            {
                Entity.CurrentAction = HumanActions.SourceAction;
            }

            return nextCell;
        }

        private Cell PutIntoStorages(Cell current)
        {
            if (_targetStorageCell == null)
            {
                _targetStorageCell = Entity.House.Village.Storages.Find(storage =>
                    Entity.Inventory.FindItem(item => item.GetType() == storage.GetResourceType()) != null)?.GetCell();
            }

            if (_targetStorageCell == null)
            {
                return DefaultMove(current);
            }

            var nextCell = PartnerMovement.MoveByWay(current, _targetStorageCell);
            if (_targetStorageCell == nextCell)
            {
                Entity.CurrentAction = _builderActions.PutIntoStorageAction;
            }

            return nextCell;
        }

        private Cell CheckAndCreateStorage<T>(Cell current) where T : Resource
        {
            if (Entity.House.Village.Storages.Find(storage => storage.GetResourceType() == typeof(T)) == null)
            {
                return CreateStorage<T>(current);
            }

            return null;
        }

        private Cell BuildStorages(Cell current)
        {
            Cell nextCell;
            if (_storageShuffleList == null)
            {
                
                _storageShuffleList = new List<int>()
                {
                    1, 2, 3, 4
                };
                var random = new RNGCryptoServiceProvider();
                _storageShuffleList = _storageShuffleList.OrderBy(x => Utils.GetNextInt32(random)).ToList();
            }

            // for (var i = 0; i < _storageShuffleList.Count; i++)
            // {
                switch (_storageShuffleList[0])
                {
                    case 1:
                        nextCell = CheckAndCreateStorage<Wood>(current);
                        break;
                    case 2:
                        nextCell = CheckAndCreateStorage<Stone>(current);
                        break;
                    case 3:
                        nextCell = CheckAndCreateStorage<Gold>(current);
                        break;
                    default:
                        nextCell = CheckAndCreateStorage<Iron>(current);
                        break;
                }

                if (nextCell != null)
                {
                    return nextCell;
                }
           // }

            return null;
        }

        private Cell CreateStorage<T>(Cell current) where T : Resource
        {
            Cell nextCell;
            if (Entity.Inventory.FindItems(item => item is T).Count >= Storage<Resource>.ResoursesToCreateCount)
            {
                nextCell = PartnerMovement.MoveByWay(current, Entity.House.Cell);
                for (var i = -3; i <= 3; i++)
                {
                    for (var j = -3; j <= 3; j++)
                    {
                        var newCords = new Coords(nextCell.Position.X + j,
                            nextCell.Position.Y + i);
                        if (0 <= newCords.X && newCords.X < _field.GetLength(0) && 0 <= newCords.Y &&
                            newCords.Y < _field.GetLength(0))
                        {
                            if (_field[newCords.Y, newCords.X].IsHouseHere() &&
                                _field[newCords.Y, newCords.X].GetHouse().Village == Entity.House.Village)
                            {
                                StorageTypeToCreate = typeof(T);
                                Entity.CurrentAction = _builderActions.CreateStorageAction;
                                return nextCell;
                            }
                        }
                    }
                }

                return nextCell;
            }

            return FindStorageSource<T>(current);
        }

        private Cell FindStorageSource<T>(Cell current) where T : Resource
        {
            var sourceFinder = new FinderWithCondition(_field,
                cell => cell.IsSourceHere() && cell.GetSource().GetResourceType() == typeof(T));
            var sourceCell = sourceFinder.Find(current.Position, _prevMove);
            if (sourceCell == null)
            {
                return DefaultMove(current);
            }

            var nextCell = PartnerMovement.MoveByWay(current, sourceCell);
            if (sourceCell == nextCell)
            {
                Entity.CurrentAction = HumanActions.SourceAction;
            }

            return nextCell;
        }

        // private Cell BuildStorage<T>(Cell current) where T : Resource
        // {
        //     var storage =
        //         Entity.House.Village.Storages.Find(storage => storage.GetResourceType() == typeof(Wood)) as
        //             Storage<Resource>;
        //     Cell nextCell;
        //     if (Entity.Inventory.FindItems(item => item is T).Count >=
        //         storage?.ResourceCountToBuild)
        //     {
        //         nextCell = PartnerMovement.MoveByWay(current, storage.Cell);
        //         if (storage.Cell == nextCell)
        //         {
        //             Entity.CurrentAction = Entity.BuildStorageAction;
        //         }
        //
        //         return nextCell;
        //     }
        //
        //     return FindStorageSource<T>(current);
        // }
    }
}