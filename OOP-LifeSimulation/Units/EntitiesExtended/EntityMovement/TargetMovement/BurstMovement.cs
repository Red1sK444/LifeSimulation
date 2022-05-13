using System;

namespace OOP_LifeSimulation.EntityMovement.TargetMovement
{
    public class BurstMovement : TargetMovement
    {
        private readonly TargetMovement _basicMovement;

        public BurstMovement(Cell[,] field) : base(field)
        {
            _basicMovement = new StraightMovement(_field);
        }
        
        public override Cell MoveByWay(Cell current, Cell target)
        {
            var xDistance = Math.Abs(current.Position.X - target.Position.X);
            var yDistance = Math.Abs(current.Position.Y - target.Position.Y);
            var travelDistance = DetermineTravelDistance(_field.GetLength(0), xDistance, yDistance);
            if (travelDistance > 2)
            {
                return _basicMovement.MoveByWay(current, target);
            }
            return target;
        }
    }
}