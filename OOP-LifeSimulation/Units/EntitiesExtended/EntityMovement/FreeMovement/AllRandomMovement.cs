using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OOP_LifeSimulation.EntityMovement.FreeMovement
{
    public class AllRandomMovement : FreeMovement
    {
        public AllRandomMovement(Cell[,] field, Action<int> updatePrevMove) : base(field, updatePrevMove) {}

        protected override bool IsDirectionWrong(int iMovement)
        {
            return false;
        }
        
        // private int GetDirection(Coords currentCoords)
        // {
        //     var availableNow = new List<int>();
        //     for (var i = 0; i <= 7; i++)
        //     {
        //         var newCords = new Coords(currentCoords.X + MovementManager.Movement[i].X, currentCoords.Y + MovementManager.Movement[i].Y);
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