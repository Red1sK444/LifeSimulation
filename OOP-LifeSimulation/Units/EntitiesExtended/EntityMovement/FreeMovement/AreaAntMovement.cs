using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OOP_LifeSimulation.EntityMovement
{
    public class AreaAntMovement : FreeMovement.FreeMovement
    {
        private Coords _initialPosition;
        private readonly int _areaRadius;

        public AreaAntMovement(Cell[,] field, Action<int> updatePrevMove, Coords initialPosition) : base(field,
            updatePrevMove)
        {
            _areaRadius = FieldSize / 20;
            _initialPosition = initialPosition;
        }

        protected override bool IsCellSkipable(Coords newCords)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(newCords.X - _initialPosition.X), 2) +
                             Math.Pow(Math.Abs(newCords.Y - _initialPosition.Y), 2)) > _areaRadius;
        }

        protected override void DoActionWhileStaying(Coords currentCoords)
        {
            UpdateAreaCenter(currentCoords);
        }

        private void UpdateAreaCenter(Coords newCenter)
        {
            _initialPosition = newCenter;
        }

        // private int GetDirection(Coords currentCoords)
        // {
        //     var availableNow = new List<int>();
        //     for (var i = 0; i <= 7; i++)
        //     {
        //         if (_prevMove != 8)
        //         {
        //             var wrongDirection = false;
        //             for (var j = 3; j <= 5; j++)
        //             {
        //                 if ((_prevMove + j) % 8 == i)
        //                 {
        //                     wrongDirection = true;
        //                     break;
        //                 }
        //             }
        //
        //             if (wrongDirection == true)
        //             {
        //                 continue;
        //             }
        //         }
        //
        //         var newCords = new Coords(currentCoords.X + MovementManager.Movement[i].X,
        //             currentCoords.Y + MovementManager.Movement[i].Y);
        //         if (0 <= newCords.X && newCords.X < _fieldSize && 0 <= newCords.Y && newCords.Y < _fieldSize)
        //         {
        //             if (_field[newCords.Y, newCords.X].Biom.Name != BiomsEnum.Lake
        //                 && Math.Sqrt(Math.Pow(Math.Abs(newCords.X - _initialPosition.X), 2) +
        //                              Math.Pow(Math.Abs(newCords.Y - _initialPosition.Y), 2)) <= _areaRadius)
        //             {
        //                 availableNow.Add(i);
        //             }
        //         }
        //     }
        //
        //     if (availableNow.Count == 0)
        //     {
        //         _prevMove = 8;
        //         return 8;
        //     }
        //
        //     return availableNow[Utils.GetRandomInt(availableNow.Count)];
        // }
    }
}