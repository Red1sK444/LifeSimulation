using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;
using OOP_LifeSimulation.Villages;

namespace OOP_LifeSimulation.Buildings
{
    public class Storage<T> : Building, IStorage<T> where T : Item
    {
        public Village Village;
        public StorageState Status = StorageState.InBuild;
        public int ResourceCountToBuild = 5;
        protected Bitmap _builtSprite;
        protected List<T> Items = new List<T>();
        public const int ResoursesToCreateCount = 5;
        public int ResourceCount { protected set; get; }

        public Storage(Map map, Cell cell) : base(map, cell)
        {
            Cell.UnitList.Add(this);
            Map.ChangedCells.Add(Cell);
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\building.png"));
        }

        public static IStorage<Resource> Create(Resource resourceType, Map map, Cell cell)
        {
            if (resourceType is Iron)
            {
                return new Storage<Iron>(map, cell)
                {
                    _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\iron_storage.png"))
                };
            }

            if (resourceType is Stone)
            {
                return new Storage<Stone>(map, cell)
                {
                    _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\stone_storage.png"))
                };
            }

            if (resourceType is Gold)
            {
                return new Storage<Gold>(map, cell)
                {
                    _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\gold_storage.png"))
                };
            }

            return new Storage<Wood>(map, cell)
            {
                _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\wood_storage.png"))
            };
        }
        
        

        public void PutResource(Item item)
        {
            if (PutItemTypeIsCorrect(item) == false)
            {
                return;
            }
            
            if (Status == StorageState.InBuild)
            {
                ResourceCountToBuild--;
                if (ResourceCountToBuild == 0)
                {
                    Status = StorageState.Built;
                    Sprite = _builtSprite;
                    ResourceCount = 0;
                }
            }
            else
            {
                if (item is T apprItem)
                {
                    Items.Add(apprItem);
                }
                ResourceCount++;
            }
        }

        protected virtual bool PutItemTypeIsCorrect(Item item)
        {
            return item is Resource;
        }

        public virtual T ExtractResource()
        {
            if (ResourceCount == 0) return null;
            ResourceCount--;
            var extracting = Items.Find(item=>true);
            Items.Remove(extracting);
            return extracting;
        }

        public Cell GetCell()
        {
            return Cell;
        }

        public void SetVillage(Village village)
        {
            Village = village;
        }

        public Type GetResourceType()
        {
            return typeof(T);
        }

        public override string SendInfo()
        {
            var villageId = $"VillageId = {Village?.id}";
            var type = $"Type = {typeof(T)}";
            var coords = $"Coordinates: X={Cell.Position.X};Y={Cell.Position.Y}";
            var count = $"Count: {ResourceCount}";
            return $"\n{villageId}\n{type}\n{coords}\n{count}";
        }
    }
}