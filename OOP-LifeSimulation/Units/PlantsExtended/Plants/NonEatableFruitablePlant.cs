using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class NonEatableFruitablePlant : SpreadingPlant
    {
        private Fruitable _fruitable;
        public NonEatableFruitablePlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Aqua;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\noneatablefruitable.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new NonEatableFruitablePlant(Map, cell);
        }
        
        protected override void Grow()
        {
            base.Grow();
            if (GrowthState.Equals(PlantGrowthState.Grown))
            {
                _fruitable = new Fruitable(Map, Cell);
            }
        }
        
        public override void Draw(Drawer drawer)
        {
            base.Draw(drawer);
            if (_fruitable != null && _fruitable.HasOneMoreFruit() &&
                GrowthState.Equals(PlantGrowthState.Faded) == false)
            {
                _fruitable.DrawFruit(drawer);
            }
        }
        
        public override void TimeTick(int? timeTickCount)
        {
            base.TimeTick(timeTickCount);
            if (_fruitable != null && _fruitable.HasOneMoreFruit() &&
                GrowthState.Equals(PlantGrowthState.Faded) == false)
            {
                _fruitable.TimeTick(timeTickCount);
            }
        }
    }
}