using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation
{
    public class Map : ITickObject
    {
        public Spawner Spawner;
        public readonly Cell[,] Field;
        public readonly int MapSize;
        public readonly List<Cell> ChangedCells = new List<Cell>();
        public readonly List<Plant> PlantList = new List<Plant>();
        public readonly List<Plant> PlantToAdd = new List<Plant>();
        public readonly List<Entity> EntityList = new List<Entity>();
        public readonly List<Building> BuildingList = new List<Building>();
        public int SeasonDuration = 200;
        public static SeasonEnum Season = SeasonEnum.Summer;
        

        public Map(int mapSize)
        {
            MapSize = mapSize;
            Field = new Cell[mapSize, mapSize];
        }

        public void GenerateLandscape(Drawer drawer)
        {
            for (var i = 0; i < MapSize; i++)
            {
                for (var j = 0; j < MapSize; j++)
                {
                    Field[i,j] = new Cell(j, i);
                }
            }
            GenerateBioms();
            GenerateSources();
            drawer.DrawMap(Field, MapSize);
        }

        private void GenerateBioms()
        {
            var lakeCount = Utils.GetRandomInt(3);
            for (var i = 0; i < lakeCount; i++)
            {
                Lake.GenerateLake(Field, MapSize);
            }
        }
        
        private void GenerateSources()
        {
            var sourceCount = MapSize*MapSize/6;
            for (var i = 0; i < sourceCount; i++)
            {
                Source<Gold>.PlaceSource(this, Spawner.FindCellToSpawn());
            }
        }

        public void ChangeSeason(Drawer drawer)
        {
            Season = Season == SeasonEnum.Summer ? SeasonEnum.Winter : SeasonEnum.Summer; 
            Entity.StarvingPerTick = Season == SeasonEnum.Winter ? 3 : 1;
            drawer.DrawSeasonChanging(Field, MapSize);
        }
        
        public void TimeTick(int? timeTickCount)
        {
            foreach (var entity in EntityList.ToList())
            {
                entity.TimeTick();
            }

            foreach (var plant in PlantList.ToList())
            {
                plant.TimeTick(timeTickCount);
            }

            foreach (var plant in PlantToAdd)
            {
                PlantList.Add(plant);
            }

            foreach (var building in BuildingList.ToList())
            {
                building.TimeTick();
            }
            PlantToAdd.Clear();
        }
    }
}