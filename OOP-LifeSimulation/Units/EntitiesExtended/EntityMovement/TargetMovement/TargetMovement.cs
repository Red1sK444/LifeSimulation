using System;

namespace OOP_LifeSimulation.EntityMovement.TargetMovement
{
    public abstract class TargetMovement
    {
        protected Cell[,] _field;

        public TargetMovement(Cell[,] field)
        {
            _field = field;
        }
        protected static int DetermineTravelDistance(int fieldSize, int xDistance, int yDistance)
        {
            var travelDistance = 3;
            if (xDistance <= fieldSize / 10 && yDistance <= fieldSize / 10)
            {
                travelDistance = 2;
            }

            if (xDistance <= fieldSize / 20 && yDistance <= fieldSize / 20)
            {
                travelDistance = 1;
            }

            return travelDistance;
        }

        public abstract Cell MoveByWay(Cell current, Cell target);
    }
}