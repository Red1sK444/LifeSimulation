using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.EntityMovement
{
    public class FinderWithCondition
    {
        private readonly Cell[,] _field;
        private readonly int _fieldSize;
        public readonly Func<Cell, bool> IsTargetCell;
        private bool[,] _usedCells;
        private Cell[,] _prevCell;
        private Cell _foodCell;

        public FinderWithCondition(Cell[,] field, Func<Cell, bool> isTargetCell)
        {
            _field = field;
            _fieldSize = _field.GetLength(0);
            IsTargetCell = isTargetCell;
        }
        private void BreadthFirstSearch(Cell cell, int prevMove)
        {
            var depthCell = new int[_fieldSize, _fieldSize];
            var queue = new Queue<Cell>();

            _usedCells[cell.Position.Y, cell.Position.X] = true;
            queue.Enqueue(cell);

            while (queue.Count != 0)
            {
                var currentCell = queue.Dequeue();
                
                if (IsTargetCell(currentCell))
                {
                    _foodCell = currentCell;
                    return;
                }
                // if (currentCell.IsPlantHere() && currentCell.GetPlant() is IEatable
                //                               && currentCell.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                //                               false)
                // {
                //     _foodCell = currentCell;
                //     return;
                // }

                if (depthCell[currentCell.Position.Y, currentCell.Position.X] > _fieldSize / 3)
                {
                   // break;
                }

                foreach (var i in new List<int> {0, 1, 7})
                {
                    var newCords = new Coords(currentCell.Position.X + LifecycleManager.Movement[(prevMove + i) % 8].X,
                        currentCell.Position.Y + LifecycleManager.Movement[(prevMove + i) % 8].Y);
                    if (0 <= newCords.X && newCords.X < _fieldSize && 0 <= newCords.Y && newCords.Y < _fieldSize &&
                        _usedCells[newCords.Y, newCords.X] == false &&
                        _field[newCords.Y, newCords.X].Biome.Name != BiomesEnum.Lake)
                    {
                        depthCell[newCords.Y, newCords.X] =
                            depthCell[currentCell.Position.Y, currentCell.Position.X] + 1;
                        _usedCells[newCords.Y, newCords.X] = true;
                        _prevCell[newCords.Y, newCords.X] = currentCell;
                        queue.Enqueue(_field[newCords.Y, newCords.X]);
                    }
                }
            }
        }

        public Cell Find(Coords currentCoords, int prevMove)
        {
            _usedCells = new bool[_fieldSize, _fieldSize];
            _prevCell = new Cell[_fieldSize, _fieldSize];
            _foodCell = null;

            BreadthFirstSearch(_field[currentCoords.Y, currentCoords.X], prevMove);
            return _foodCell;
        }
    }
}