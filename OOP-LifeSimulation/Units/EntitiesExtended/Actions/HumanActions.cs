using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Actions
{
    public class HumanActions
    {
        private Human _owner;

        public HumanActions(Human owner)
        {
            _owner = owner;
        }

        public void SourceAction()
        {
            if (!_owner.Cell.IsSourceHere())
            {
                return;
            }

            var source = _owner.Cell.GetSource();
            var resource = source?.ExtractResource();
            resource?.SetInventory(_owner.Inventory);
            _owner.Inventory.AddItem(resource);
        }

        public void CreateHouseAction()
        {
            if (!(_owner.House == null && _owner.Partner != null && _owner.Sex == EntitySex.Male))
            {
                return;
            }

            var toCreateHome = _owner.Inventory.FindItems(item => item is Wood);
            if (_owner.Cell.IsBuildingHere()
                || toCreateHome.Count < House.WoodCountToCreate)
            {
                return;
            }

            _owner.SetHouse(House.Create(toCreateHome.ConvertAll(item => item as Wood), _owner.Map, _owner.Cell));
            ((Human) _owner.Partner).SetHouse(_owner.House);
        }

        public void BuildHouseAction()
        {
            var toBuildHome = _owner.Inventory.FindItems(item => item is Wood);
            if (_owner.Cell != _owner.House.Cell
                || toBuildHome.Count == 0)
            {
                return;
            }

            toBuildHome.ForEach(item => item.Use(_owner.House));
        }

        public void EatFromInventory()
        {
            var food = _owner.Inventory.FindItem(item => item is IEatable);
            food?.Use(this);
        }

        public void EatFromHouse()
        {
            var food = _owner.House.FindItemAndExtract(item => item is IEatable);
            (food as FoodItem)?.Use(this);
        }

        public void EatFromBarn()
        {
            var food = _owner.House.Village.Barn.ExtractResource();
            food?.Use(this);
        }

        public void PutIntoHouseAction()
        {
            if (_owner.House == null
                || !(_owner.House.Cell.Position.X == _owner.Cell.Position.X &&
                     _owner.House.Cell.Position.Y == _owner.Cell.Position.Y))
            {
                return;
            }

            _owner.Inventory.FindItems(item => true).ForEach(item => item.Use(_owner.House));
        }
    }
}