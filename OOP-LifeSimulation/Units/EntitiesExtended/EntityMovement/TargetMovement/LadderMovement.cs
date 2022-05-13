using System;

namespace OOP_LifeSimulation.EntityMovement.TargetMovement
{
    public class LadderMovement : TargetMovement
    {
        public LadderMovement(Cell[,] field) : base(field){}
        public override Cell MoveByWay(Cell current, Cell target)
        {
            var fieldSize = _field.GetLength(0);
            var xDistance = Math.Abs(current.Position.X - target.Position.X);
            var yDistance = Math.Abs(current.Position.Y - target.Position.Y);
            var travelDistance = DetermineTravelDistance(fieldSize, xDistance, yDistance);

            if (xDistance > yDistance) // проверка на 0 < значение < fieldSize
            {
                for (var i = travelDistance; i > 0; i--)
                {
                    if (0 <= current.Position.X + i && current.Position.X + i < fieldSize
                        && Math.Abs(target.Position.X - (current.Position.X + i)) <
                            Math.Abs(target.Position.X - (current.Position.X - i))
                        && _field[current.Position.Y, current.Position.X + i].Biome.Name != BiomesEnum.Lake)
                    {
                        return _field[current.Position.Y, current.Position.X + i];
                    }

                    if (0 <= current.Position.X - i && current.Position.X - i < fieldSize
                        && _field[current.Position.Y, current.Position.X - i].Biome.Name != BiomesEnum.Lake)
                    {
                        return _field[current.Position.Y, current.Position.X - i];
                    }
                }
            }
            else
            {
                for (var i = travelDistance; i > 0; i--)
                {
                    if (0 <= current.Position.Y + i && current.Position.Y + i < fieldSize
                        && Math.Abs(target.Position.Y - (current.Position.Y + i)) <
                            Math.Abs(target.Position.Y - (current.Position.Y - i))
                        && _field[current.Position.Y + i, current.Position.X].Biome.Name != BiomesEnum.Lake)
                    {
                        return _field[current.Position.Y + i, current.Position.X];
                    }

                    if (0 < current.Position.Y - i && current.Position.Y - i < fieldSize 
                        && _field[current.Position.Y - i, current.Position.X].Biome.Name != BiomesEnum.Lake)
                    {
                        return _field[current.Position.Y - i, current.Position.X];
                    }
                }
            }

            return current;
        }
    }
}