using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Actions
{
    public class CollectorHunterActions
    {
        private Human _owner;

        public CollectorHunterActions(Human owner)
        {
            _owner = owner;
        }

        public void PutIntoBarnAction()
        {
            if (_owner.House.Village.Barn.Cell != _owner.Cell)
            {
                return;
            }

            _owner.Inventory.FindItems(item => item is IEatable).ForEach(item => item.Use(_owner.House.Village.Barn));
        }
    }
}