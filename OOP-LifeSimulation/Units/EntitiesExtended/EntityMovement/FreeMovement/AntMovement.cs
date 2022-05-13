using System;
using System.Collections.Generic;

namespace OOP_LifeSimulation.EntityMovement.FreeMovement
{
    public class AntMovement : FreeMovement
    {
        public AntMovement(Cell[,] field, Action<int> updatePrevMove) : base(field, updatePrevMove) {}

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
        //             if (_field[newCords.Y, newCords.X].Biom.Name != BiomsEnum.Lake)
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