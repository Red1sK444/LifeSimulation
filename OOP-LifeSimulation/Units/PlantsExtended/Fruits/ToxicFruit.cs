using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class ToxicFruit : Fruit, IReactingToSeasonChange
    {
        public ToxicFruit(Map map, Cell cell, Color plantColor) : base(map, cell, plantColor)
        {
            Color = Color.Azure;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\toxicfruit.png"));
        }

        public override bool IsToxic()
        {
            return true;
        }
    }
}