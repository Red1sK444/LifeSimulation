using System;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;

namespace OOP_LifeSimulation.EntityMovement.TargetMovement
{
    public class StraightMovement : TargetMovement
    {
        public StraightMovement(Cell[,] field) : base(field){}
        public override Cell MoveByWay(Cell current, Cell target)
        {
            var fieldSize = _field.GetLength(0);
            var xDistance = Math.Abs(current.Position.X - target.Position.X);
            var yDistance = Math.Abs(current.Position.Y - target.Position.Y);
            var travelDistance = DetermineTravelDistance(fieldSize, xDistance, yDistance);

            var minDistance = fieldSize * fieldSize;
            var nextCell = current;

            for (var tD = travelDistance; tD > 0; tD--)
            {
                for (var i = 0; i <= 7; i++)
                {
                    var newCords = new Coords(current.Position.X + LifecycleManager.Movement[i].X * tD,
                        current.Position.Y + LifecycleManager.Movement[i].Y * tD);
                    if (0 <= newCords.X && newCords.X < fieldSize && 0 <= newCords.Y && newCords.Y < fieldSize)
                    {
                        if (_field[newCords.Y, newCords.X].Biome.Name != BiomesEnum.Lake
                            && Math.Abs(newCords.X - target.Position.X) + Math.Abs(newCords.Y - target.Position.Y) <=
                            minDistance)
                        {
                            nextCell = _field[newCords.Y, newCords.X];
                            minDistance = Math.Abs(newCords.X - target.Position.X) +
                                          Math.Abs(newCords.Y - target.Position.Y);
                        }
                    }
                }
            }

            return nextCell;
        }
    }
}