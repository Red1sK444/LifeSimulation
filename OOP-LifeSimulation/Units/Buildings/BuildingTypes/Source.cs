using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation.Buildings
{//Building where T: Resource, new()
    public class Source<T> : Building, ISource<T> where T: Resource, new()
    {
        private int _resourceQuantity = 20;

        public Source(Map map, Cell cell) : base(map, cell)
        {
            Cell.UnitList.Add(this);
            Map.BuildingList.Add(this);
        }

        public static void PlaceSource(Map map, Cell cell)
        {
            switch(Utils.GetRandomInt(4))
            {
                case 0:
                    var source1 = new Source<Gold>(map, cell)
                    {
                        Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\gold_source.png"))
                    };
                    break;
                case 1:
                    var source2 = new Source<Iron>(map, cell)
                    {
                        Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\iron_source.png"))
                    };
                    break;
                case 2:
                    var source3 = new Source<Wood>(map, cell)
                    {
                        Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\wood_source.png"))
                    };
                    break;
                case 3:
                    var source4 = new Source<Stone>(map, cell)
                    {
                        Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\stone_source.png"))
                    };
                    break;
            }
        }

        public T ExtractResource()
        {
            _resourceQuantity--;
            return new T();
        }
        
        public Type GetResourceType()
        {
            return typeof(T);
        }

        public override void TimeTick(int? timeTickCount = null)
        {
            if (_resourceQuantity <= 0) Die();
        }
    }
}