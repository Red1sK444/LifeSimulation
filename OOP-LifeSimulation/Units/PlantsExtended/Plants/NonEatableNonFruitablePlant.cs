using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class NonEatableNonFruitablePlant : SpreadingPlant, IReactingToSeasonChange
    {
        public NonEatableNonFruitablePlant(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.DimGray;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\noneatablenonfruitable.png"));
        }

        protected override SpreadingPlant GetPlantToSpawn(Cell cell)
        {
            return new NonEatableNonFruitablePlant(Map, cell);
        }
    }
}