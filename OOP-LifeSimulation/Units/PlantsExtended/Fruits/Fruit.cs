using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace OOP_LifeSimulation.PlantsExtended
{
    public abstract class Fruit : Plant, IEatableForHerbivore
    {
        private const int SatietyToRegen = 5;
        private const int HpToRegen = 10;
        private const int HpToApply = 3;
        private Color _plantColor;
        private bool _ripen;
        public bool Fallen { get; private set; }

        protected Fruit(Map map, Cell cell, Color plantColor) : base(map, cell)
        {
            GrowthState = FruitGrowthState.Seed;
            GrowthStateDuration = 1 + Utils.GetRandomInt(2);
            _plantColor = plantColor;
        }

        protected override void Grow()
        {
            switch (GrowthState)
            {
                case FruitGrowthState.Seed:
                    GrowthState = FruitGrowthState.HalfRipe;
                    break;
                case FruitGrowthState.HalfRipe:
                    GrowthState = FruitGrowthState.Ripe;
                    break;
                case FruitGrowthState.Ripe:
                    _ripen = true;
                    break;
            }
            base.Grow();
        }

        private bool FallNearPlant()
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    if (0 <= Cell.Position.Y + y && Cell.Position.Y + y < Map.MapSize && 0 <= Cell.Position.X + x &&
                        Cell.Position.X + x < Map.MapSize
                        && (x == 0 && y == 0) == false &&
                        Map.Field[Cell.Position.Y + y, Cell.Position.X + x].IsUnitHere() == false &&
                        Map.Field[Cell.Position.Y + y, Cell.Position.X + x].Biome.Name != BiomesEnum.Lake)
                    {
                        Map.ChangedCells.Add(Cell);
                        Cell = Map.Field[Cell.Position.Y + y, Cell.Position.X + x];
                        Cell.UnitList.Add(this);
                        Map.ChangedCells.Add(Cell);
                        return true;
                    }
                }
            }

            return false;
        }

        public override void TimeTick(int? timeTickCount)
        {
            base.TimeTick(timeTickCount);
            if (Map.Season == SeasonEnum.Winter && this is IReactingToSeasonChange) return;
            if (_ripen == false)
            {
                if (IsGrowable(timeTickCount))
                {
                    Grow();
                }
            }
            else if (Fallen == false)
            {
                Fallen = FallNearPlant();
            }
        }

        public override void Draw(Drawer drawer) //TODO иногда фрукты растут зимойц / i reacting on season changing интерфейс
        {
            // if (Fallen == true)
            // {
            //     Brush brush = new SolidBrush(new Forest().GetColor());
            //     drawer.Graphics.FillRectangle(brush,
            //         new Rectangle(Cell.Position.X * Drawer.CellSize,
            //             Cell.Position.Y * Drawer.CellSize,
            //             Drawer.CellSize, Drawer.CellSize));
            //     brush = new SolidBrush(Color);
            //     drawer.Graphics.FillEllipse(brush,
            //         new Rectangle(Cell.Position.X * Drawer.CellSize,
            //             Cell.Position.Y * Drawer.CellSize,
            //             Drawer.CellSize, Drawer.CellSize));
            //     brush = new SolidBrush(_plantColor);
            //     drawer.Graphics.FillEllipse(brush,
            //         new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 4,
            //             Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 4,
            //             Drawer.CellSize / 2, Drawer.CellSize / 2));
            // }
            // else
            // {
            //     Brush brush = new SolidBrush(Color);
            //     switch (GrowthState)
            //     {
            //         case FruitGrowthState.Seed:
            //             drawer.Graphics.FillEllipse(brush,
            //                 new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 3,
            //                     Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 3,
            //                     Drawer.CellSize / 3, Drawer.CellSize / 3));
            //             break;
            //         case FruitGrowthState.HalfRipe:
            //             drawer.Graphics.FillEllipse(brush,
            //                 new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 4,
            //                     Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 4,
            //                     Drawer.CellSize / 2, Drawer.CellSize / 2));
            //             break;
            //         case FruitGrowthState.Ripe:
            //             drawer.Graphics.FillEllipse(brush,
            //                 new Rectangle(Cell.Position.X * Drawer.CellSize + Drawer.CellSize / 7,
            //                     Cell.Position.Y * Drawer.CellSize + Drawer.CellSize / 7,
            //                     Drawer.CellSize * 5 / 7, Drawer.CellSize * 5 / 7));
            //             break;
            //     }
            // }
            if(Fallen || GrowthState.Equals(FruitGrowthState.Ripe)) drawer.Draw(Cell, Sprite);
        }

        public void BecomeEaten(Entity eater)
        {
            if (IsToxic() == false)
            {
                eater.HP.Increase(GetHpToRegen());
                eater.Satiety.Increase(GetSatietyToRegen());
                Die();
            }
            else
            {
                eater.HP.Decrease(GetHpToApply());
                Die();
            }
        }

        public int GetHpToRegen()
        {
            return HpToRegen;
        }

        public int GetSatietyToRegen()
        {
            return SatietyToRegen;
        }

        public abstract bool IsToxic();
        public int GetHpToApply()
        {
            return HpToApply;
        }
    }
}