using System;
using System.Drawing;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation
{
    public abstract class Plant : Unit
    {
        public Enum GrowthState;
        protected int GrowthStateDuration;
        private int _initialTimeTick;

        protected Plant(Map map, Cell cell) : base(map, cell) {}
        
        public override void Die()
        {
            Cell.RemovePlant(this);
            Map.PlantList.Remove(this);
            Map.ChangedCells.Add(Cell);
        }

        public override void Draw(Drawer drawer)
        {
            drawer.Draw(Cell, Sprite);
        }

        public override void TimeTick(int? timeTickCount)
        {
            if (_initialTimeTick == 0)
            {
                _initialTimeTick = (int) timeTickCount;
            }
        }

        protected bool IsGrowable(int? timeTickCount)
        {
            return timeTickCount != _initialTimeTick &&  (timeTickCount - _initialTimeTick) % GrowthStateDuration == 0;
        }

        protected virtual void Grow()
        {
            Map.ChangedCells.Add(Cell);
        }
        
        public override string SendInfo()
        {
            var type = $"Type = {GetType().Name}";
            var coords = $"Coordinates: X={Cell.Position.X};Y={Cell.Position.Y}";
            return $"\n{type}\n{coords}";
        }
    }
}

/*namespace OOP_LifeSimulation
{
    public class Plant : Unit
    {
        private readonly bool _toxic;
        public const int SatietyPoints = 5;
        public const int HpPoints = 10;

        public Plant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Gold;

            var randomizer = new Random();
            if (randomizer.Next(0, 10) > 9)
            {
                _toxic = true;
            }
            else
            {
                _toxic = false;    
            }
        }

        public bool IsToxic()
        {
            return _toxic;
        }

        public override void Die()
        {
            Cell.UnitList.Remove(this);
            Map.PlantList.Remove(this);
            Map.ChangedCells.Add(Cell);
        }

        public override void Draw(Drawer drawer)
        {
            drawer.Draw(Cell, Color);
        }
    }
}*/