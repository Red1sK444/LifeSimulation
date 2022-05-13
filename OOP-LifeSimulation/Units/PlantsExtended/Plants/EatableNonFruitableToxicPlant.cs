using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class EatableNonFruitableToxicPlant : EatablePlant
    {
        public EatableNonFruitableToxicPlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Chartreuse;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\eatablenonfruitabletoxic.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new EatableNonFruitableToxicPlant(Map, cell);
        }

        public override bool IsToxic()
        {
            return true;
        }
    }
}