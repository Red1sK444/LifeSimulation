using System.Drawing;

namespace OOP_LifeSimulation
{
    public class Forest : Biome
    {
        public Forest()
        {
            Color = Color.Green;
            Name = BiomesEnum.Forest;
        }

        public override Color GetColor()
        {
            return Map.Season == SeasonEnum.Summer ? Color.Green : Color.Azure;
        }
    }
}