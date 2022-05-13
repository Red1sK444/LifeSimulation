using System.Drawing;

namespace OOP_LifeSimulation
{
    public abstract class Biome
    {
        public Color Color;
        public BiomesEnum Name;

        public abstract Color GetColor();
    }
}