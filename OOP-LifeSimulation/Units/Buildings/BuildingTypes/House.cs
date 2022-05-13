using System;
using System.Collections.Generic;
using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;
using OOP_LifeSimulation.Villages;

namespace OOP_LifeSimulation.Buildings
{
    public class House : Storage<Item>
    {
        public const int WoodCountToCreate = 4;
        public List<Human> Owners = new List<Human>();

        public House(Map map, Cell cell) : base(map, cell)
        {
            ResourceCountToBuild = 4;
            CalculateNeighbours();
        }

        public static House Create(List<Wood> wood, Map map, Cell cell)
        {
            if (wood.Find(item => item == null) != null) return null;

            var house = new House(map, cell)
            {
                _builtSprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\house.png"))
            };

            wood.ForEach(item => item.Use(house));
            return house;
        }

        public void AddOwner(Human owner)
        {
            Owners.Add(owner);
            Village?.AddVillager(owner);
        }

        private void CalculateNeighbours()
        {
            var neighbours = new List<House>();
            for (var i = -10; i <= 10; i++)
            {
                for (var j = -10; j <= 10; j++)
                {
                    var newCords = new Coords(Cell.Position.X + j,
                        Cell.Position.Y + i);
                    if (0 <= newCords.X && newCords.X < Map.MapSize && 0 <= newCords.Y && newCords.Y < Map.MapSize)
                    {
                        if (Map.Field[newCords.Y, newCords.X].IsHouseHere())
                        {
                            if (Map.Field[newCords.Y, newCords.X].GetHouse().Village != null)
                            {
                                var village = Map.Field[newCords.Y, newCords.X].GetHouse().Village;
                                neighbours.ForEach(house => house.SetVillage(village));
                                SetVillage(village);
                                return;
                            }
                            
                            neighbours.Add(Map.Field[newCords.Y, newCords.X].GetHouse());
                        }
                    }
                }
            }

            if (neighbours.Count >= Village.HouseCountToCreate)
            {
                var village = new Village();
                neighbours.ForEach(house => house.SetVillage(village));
                SetVillage(village);
            }
        }

        private void SetVillage(Village village)
        {
            Village = village;
            Owners.ForEach(human => Village.AddVillager(human));
        }

        public Item FindItem(Func<Item, bool> condition)
        {
            return Items.Find(item => condition(item));
        }

        public Item FindItemAndExtract(Func<Item, bool> condition)
        {
            var item = Items.Find(item => condition(item));
            Items.Remove(item);
            ResourceCount--;
            return item;
        }

        protected override bool PutItemTypeIsCorrect(Item item)
        {
            return Status == StorageState.InBuild && item is Wood || Status == StorageState.Built;
        }

        // public override string SendInfo()
        // {
        //     var villageId = $"VillageId = {Village?.id}";
        //     var type = "Type = House";
        //     var coords = $"Coordinates: X={Cell.Position.X};Y={Cell.Position.Y}";
        //     var count = $"Count: {ResourceCount}";
        //     return $"\n{villageId}\n{type}\n{coords}\n{count}";
        // }
    }
}