using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class EatableFruitableToxicPlant : EatablePlant
    {
        private Fruitable _fruitable;
        public EatableFruitableToxicPlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Indigo;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\eatablefruitabletoxic.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new EatableFruitableToxicPlant(Map, cell);
        }

        public override bool IsToxic()
        {
            return true;
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