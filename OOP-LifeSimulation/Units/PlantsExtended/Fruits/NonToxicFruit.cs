using System.Drawing;

namespace OOP_LifeSimulation.PlantsExtended
{
    public class NonToxicFruit : Fruit
    {
        public NonToxicFruit(Map map, Cell cell, Color plantColor) : base(map, cell, plantColor)
        {
            Color = Color.Fuchsia;
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\fruit.png"));
        }

        public override bool IsToxic()
        {
            return false;
        }

        /*public bool BecomeEaten(Entity entity)
        {
            entity.Satiety.Increase(SatietyPoints);
            entity.HP.Increase(HpPoints);
            Die();
            return true;
        }*/
    }
}