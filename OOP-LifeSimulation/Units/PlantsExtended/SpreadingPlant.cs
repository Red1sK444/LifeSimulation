using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace OOP_LifeSimulation.PlantsExtended
{
    public abstract class SpreadingPlant : Plant
    {
        private static readonly Bitmap SeedSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\seed.png"));

        private static readonly Bitmap SproutSprite =
            new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\sprout.png"));

        private int _seedSpreadCount;
        private bool _spreadTime = false;

        public static SpreadingPlant GetRandomSpreadingPlant(Map map, Cell cell)
        {
            var randomInteger = Utils.GetRandomInt(6);
            return randomInteger switch
            {
                0 => new EatableFruitableNonToxicPlant(map, cell),
                1 => new EatableFruitableToxicPlant(map, cell),
                2 => new EatableNonFruitableNonToxicPlant(map, cell),
                3 => new EatableNonFruitableToxicPlant(map, cell),
                4 => new NonEatableFruitablePlant(map, cell),
                _ => new NonEatableNonFruitablePlant(map, cell)
            };
        }

        protected SpreadingPlant(Map map, Cell cell) : base(map, cell)
        {
            GrowthState = PlantGrowthState.Seed;
            GrowthStateDuration = 10 + Utils.GetRandomInt(10);
            _seedSpreadCount = Utils.GetRandomInt(4);
        }

        protected override void Grow()
        {
            switch (GrowthState)
            {
                case PlantGrowthState.Seed:
                    GrowthState = PlantGrowthState.Sprout;
                    break;
                case PlantGrowthState.Sprout:
                    GrowthState = PlantGrowthState.Grown;
                    _spreadTime = true;
                    break;
                case PlantGrowthState.Grown:
                    GrowthState = PlantGrowthState.Faded;
                    _spreadTime = false;
                    Die();
                    break;
                case PlantGrowthState.Faded:
                    break;
            }

            base.Grow();
        }

        public override void TimeTick(int? timeTickCount)
        {
            base.TimeTick(timeTickCount);
            if (Map.Season == SeasonEnum.Winter)
            {
                if (GrowthState.Equals(PlantGrowthState.Seed) == false && this is IReactingToSeasonChange == false)
                {
                    Die();
                }

                return;
            }

            if (GrowthState.Equals(PlantGrowthState.Faded) == false
                && IsGrowable(timeTickCount))
            {
                Grow();
            }

            if (_spreadTime && _seedSpreadCount > 0)
            {
                SpreadSeeds();
            }
        }

        public override void Draw(Drawer drawer)
        {
            Brush brush = new SolidBrush(new Forest().GetColor());
            drawer.Graphics.FillRectangle(brush,
                new Rectangle(Cell.Position.X * Drawer.CellSize,
                    Cell.Position.Y * Drawer.CellSize,
                    Drawer.CellSize, Drawer.CellSize));
            brush = new SolidBrush(Color);
            switch (GrowthState)
            {
                case PlantGrowthState.Seed:
                    drawer.Draw(Cell, SeedSprite);
                    // drawer.Graphics.FillRectangle(brush,
                    //     new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 3,
                    //         Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 3,
                    //         Drawer.CellSize / 3, Drawer.CellSize / 3));
                    break;
                case PlantGrowthState.Sprout:
                    drawer.Draw(Cell, SproutSprite);
                    // drawer.Graphics.FillRectangle(brush,
                    //     new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 4,
                    //         Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 4,
                    //         Drawer.CellSize / 2, Drawer.CellSize / 2));
                    break;
                case PlantGrowthState.Grown:
                    drawer.Draw(Cell, Sprite);
                    // drawer.Graphics.FillRectangle(brush,
                    //     new Rectangle(Cell.Position.X * Drawer.CellSize, Cell.Position.Y * Drawer.CellSize,
                    //         Drawer.CellSize, Drawer.CellSize));
                    break;
                case PlantGrowthState.Faded:
                    drawer.Draw(Cell, Sprite);
                    // drawer.Graphics.FillEllipse(brush,
                    //     new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 5,
                    //         Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 5,
                    //         Drawer.CellSize * 3 / 5, Drawer.CellSize * 3 / 5));
                    break;
            }
        }

        private void SpreadSeeds()
        {
            for (var x = -2; x <= 2; x++)
            {
                for (var y = -2; y <= 2; y++)
                {
                    if (0 <= Cell.Position.Y + y && Cell.Position.Y + y < Map.MapSize &&
                        0 <= Cell.Position.X + x && // Вынести условие в функцию
                        Cell.Position.X + x < Map.MapSize
                        && (x == 0 && y == 0) == false && (Math.Abs(x) == 2 || Math.Abs(y) == 2) == true &&
                        Map.Field[Cell.Position.Y + y, Cell.Position.X + x].IsUnitHere() == false &&
                        Map.Field[Cell.Position.Y + y, Cell.Position.X + x].Biome.Name != BiomesEnum.Lake)
                    {
                        var spreadingPlant = GetPlantToSpawn(Map.Field[Cell.Position.Y + y, Cell.Position.X + x]);
                        Map.PlantToAdd.Add(spreadingPlant);
                        Map.Field[Cell.Position.Y + y, Cell.Position.X + x].UnitList.Add(spreadingPlant);
                        Map.ChangedCells.Add(Map.Field[Cell.Position.Y + y, Cell.Position.X + x]);
                        _seedSpreadCount--;
                        if (_seedSpreadCount == 0)
                        {
                            return;
                        }
                    }
                }
            }
        }

        protected abstract SpreadingPlant GetPlantToSpawn(Cell cell);
    }
}