using System.Collections.Generic;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory.Resources;

namespace OOP_LifeSimulation.Villages
{
    public class Village
    {
        public int id = Utils.GetRandomInt(100);
        public const int HouseCountToCreate = 3;
        public Barn Barn;
        public List<IStorage<Resource>> Storages = new List<IStorage<Resource>> (4);
        private List<Human> _villagers = new List<Human>();

        public void AddVillager(Human human)
        {
            _villagers.Add(human);
            TriggerVillager(human);
        }

        private void TriggerVillager(Human human)
        {
            human.PerformVillageAppearsAction();
        }

        public bool RemoveVillager(Human human)
        {
            return _villagers.Remove(human);
        }
    }
}