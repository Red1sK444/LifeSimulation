using System.Drawing;

namespace OOP_LifeSimulation
{
    public abstract class Unit : IDrawable, ITickObject, IShowingInfo
    {
        public readonly Map Map;
        public string Name;
        public Color Color;
        public Cell Cell;
        protected Bitmap Sprite;

        protected Unit(Map map, Cell cell)
        {
            Map = map;
            Cell = cell;
        }

        public abstract void Die();
        public abstract void Draw(Drawer drawer);
        public abstract void TimeTick(int? timeTickCount);
        public abstract string SendInfo();
    }
}