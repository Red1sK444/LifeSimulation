using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;

namespace OOP_LifeSimulation.EntityMovement.FreeMovement
{
    public abstract class FreeMovement
    {
        private readonly Cell[,] _field;
        private readonly Action<int> _updatePrevMoveCallback;
        private int _prevMove;
        protected readonly int FieldSize;

        public FreeMovement(Cell[,] field, Action<int> updatePrevMove)
        {
            _field = field;
            FieldSize = _field.GetLength(0);
            _updatePrevMoveCallback = updatePrevMove;
        }

        protected virtual bool IsDirectionWrong(int iMovement)
        {
            if (_prevMove != 8)
            {
                var wrongDirection = false;
                for (var j = 3; j <= 5; j++)
                {
                    if ((_prevMove + j) % 8 == iMovement)
                    {
                        wrongDirection = true;
                        break;
                    }
                }

                if (wrongDirection)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool IsCellSkipable(Coords newCoords)
        {
            return false;
        }

        private List<int> GetAvailableCellsToMove(Coords currentCoords)
        {
            var availableNow = new List<int>();
            for (var i = 0; i <= 7; i++)
            {
                if (IsDirectionWrong(i))
                {
                    continue;
                }

                var newCords = new Coords(currentCoords.X + LifecycleManager.Movement[i].X,
                    currentCoords.Y + LifecycleManager.Movement[i].Y);
                if (0 <= newCords.X && newCords.X < FieldSize && 0 <= newCords.Y && newCords.Y < FieldSize)
                {
                    if (_field[newCords.Y, newCords.X].Biome.Name != BiomesEnum.Lake
                        && IsCellSkipable(newCords) == false)
                    {
                        availableNow.Add(i);
                    }
                }
            }

            return availableNow;
        }

        private int GetDirection(Coords currentCoords)
        {
            var availableNow = GetAvailableCellsToMove(currentCoords);
            if (availableNow.Count == 0)
            {
                _prevMove = 8;
                return 8;
            }

            return availableNow[Utils.GetRandomInt(availableNow.Count)];
        }

        protected virtual void DoActionWhileStaying(Coords currentCoords)
        {
        }

        public Cell MoveTo(Coords currentCoords, int prevMove)
        {
            _prevMove = prevMove;
            var currentDirection = GetDirection(currentCoords);
            if (currentDirection < 8)
            {
                currentCoords = new Coords(currentCoords.X + LifecycleManager.Movement[currentDirection].X,
                    currentCoords.Y + LifecycleManager.Movement[currentDirection].Y);
                _prevMove = currentDirection;
            }
            else
            {
                DoActionWhileStaying(currentCoords);
            }

            _updatePrevMoveCallback(_prevMove);
            return _field[currentCoords.Y, currentCoords.X];
        }
    }
}