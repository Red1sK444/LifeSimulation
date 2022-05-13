using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;

namespace OOP_LifeSimulation
{
    public class Lake : Biome
    {
        public Lake()
        {
            Color = Color.Blue;
            Name = BiomesEnum.Lake;
        }
        
        public override Color GetColor()
        {
            return Map.Season == SeasonEnum.Summer ? Color.Blue : Color.CadetBlue;
        }

        public static void GenerateLake(Cell[,] field, int fieldSize)
        {
            var provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[4];
            
            provider.GetBytes(byteArray);
            var randomX = BitConverter.ToUInt16(byteArray, 0) % fieldSize;
                
            provider.GetBytes(byteArray);
            var randomY = BitConverter.ToUInt16(byteArray, 0) % fieldSize;
            
            var center = new Coords(randomX, randomY);
            
            provider.GetBytes(byteArray);
            int randomInteger = BitConverter.ToUInt16(byteArray, 0);

            var lakeCells = new List<Coords>();
            var lakeRadius = (int)(0.05 * fieldSize + randomInteger % (0.15 * fieldSize));
            for (var y = center.Y - lakeRadius; y <= center.Y + lakeRadius; y++)
            {
                for (var x = center.X - lakeRadius; x <= center.X + lakeRadius; x++)
                {
                    if (0 <= x && x < fieldSize && 0 <= y && y < fieldSize &&
                        Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2) < Math.Pow(lakeRadius, 2))
                    {
                        // lakeCells.Add(new Coords(x, y));
                        field[y, x].Biome = new Lake();
                    }
                }
            }

            //return lakeCells;
        }
    }
}