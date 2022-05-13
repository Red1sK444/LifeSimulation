using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class EatableFruitableNonToxicPlant : EatablePlant, IReactingToSeasonChange
    {
        private Fruitable _fruitable;

        public EatableFruitableNonToxicPlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Bisque;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\eatablefruitablenontoxic.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new EatableFruitableNonToxicPlant(Map, cell);
        }

        public override bool IsToxic()
        {
            return false;
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