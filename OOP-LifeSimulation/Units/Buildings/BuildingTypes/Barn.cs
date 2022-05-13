using System.Collections.Generic;
using System.Drawing;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation.Buildings
{
    public class Barn : Storage<FoodItem>
    {
        public const int StoneCountToCreate = 5;
        
        public Barn(Map map, Cell cell) : base(map, cell)
        {
            ResourceCountToBuild = 7;
        }
        
        public static Barn Create(List<Stone> stones, Map map, Cell cell)
        {
            if (stones.Find(item => item == null) != null) return null;

            var barn = new Barn(map, cell)
            {
                _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\barn.png"))
            };

            stones.ForEach(item => item.Use(barn));
            return barn;
        }
        
        protected override bool PutItemTypeIsCorrect(Item item)
        {
            return Status == StorageState.InBuild && item is Stone || Status == StorageState.Built;
        }
    }
}