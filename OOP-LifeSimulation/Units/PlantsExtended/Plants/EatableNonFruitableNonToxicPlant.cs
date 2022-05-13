using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class EatableNonFruitableNonToxicPlant : EatablePlant, IReactingToSeasonChange
    {
        public EatableNonFruitableNonToxicPlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.LightBlue;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\eatablenonfruitablenontoxic.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new EatableNonFruitableNonToxicPlant(Map, cell);
        }

        public override bool IsToxic()
        {
            return false;
        }
    }
}