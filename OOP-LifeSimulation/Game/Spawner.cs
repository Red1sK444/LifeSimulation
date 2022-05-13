using System;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation
{
    public class Spawner
    {
        private const int InitEntitySpawnCount = 200; //200
        private const int InitPlantSpawnCount = 0; //150
        private readonly Map _map;

        public Spawner(Map map)
        {
            _map = map;
        }

        private void PlantSpawn(Cell cell)
        {
            var plant = SpreadingPlant.GetRandomSpreadingPlant(_map, cell);
            cell.UnitList.Add(plant);
            _map.PlantList.Add(plant);
            _map.ChangedCells.Add(cell);
        }

        private void EntitySpawn(Cell cell)
        {
            var entity = Entity.GetRandomEntity(_map, cell);
            cell.UnitList.Add(entity);
            _map.EntityList.Add(entity);
            _map.ChangedCells.Add(cell);
        }

        public Cell FindCellToSpawn()
        {
            var randomizer = new Random();
            while (true)
            {
                var rand1 = randomizer.Next(0, _map.MapSize - 1);
                var rand2 = randomizer.Next(0, _map.MapSize - 1);

                if (_map.Field[rand1, rand2].IsUnitHere() == false &&
                    _map.Field[rand1, rand2].Biome.Name != BiomesEnum.Lake)
                {
                    return _map.Field[rand1, rand2];
                }
            }
        }

        public void InitialUnitSpawn()
        {
            for (var i = 0; i < InitEntitySpawnCount; i++)
            {
                EntitySpawn(FindCellToSpawn());
            }

            for (var i = 0; i < InitPlantSpawnCount; i++)
            {
                PlantSpawn(FindCellToSpawn());
            }
        }
    }
}