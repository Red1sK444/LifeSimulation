using System.Drawing;

namespace OOP_LifeSimulation.Buildings
{
    public class Building : Unit
    {
        public Building(Map map, Cell cell) : base(map, cell) {}

        public override void Die()
        {
            Cell.UnitList.Remove(this);
            Map.ChangedCells.Add(Cell);
            Map.BuildingList.Remove(this);
        }

        public override void Draw(Drawer drawer)
        {
            drawer.Draw(Cell, Sprite);
        }

        public override void TimeTick(int? timeTickCount = null) {}
        public override string SendInfo()
        {
            return "";
        }
    }
}