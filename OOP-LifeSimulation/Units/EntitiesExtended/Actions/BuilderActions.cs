using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;

namespace OOP_LifeSimulation.Actions
{
    public class BuilderActions
    {
        private Human _owner;

        public BuilderActions(Human owner)
        {
            _owner = owner;
        }
        
        public void PutIntoStorageAction()
        {
            var storage = _owner.Cell.GetStorage();
            if (storage == null)
            {
                return;
            }

            _owner.Inventory.FindItems(item => item.GetType() == storage.GetResourceType())
                .ForEach(item => item.Use(storage));
        }
        
        public void CreateBarnAction()
        {
            var toCreateBarn = _owner.Inventory.FindItems(item => item is Stone);
            if (_owner.Cell.IsBuildingHere()
                || toCreateBarn.Count < Barn.StoneCountToCreate)
            {
                return;
            }

            _owner.House.Village.Barn = Barn.Create(toCreateBarn.ConvertAll(item => item as Stone), _owner.Map, _owner.Cell);
            _owner.House.Village.Barn.Village = _owner.House.Village;
        }
        
        public void BuildBarnAction()
        {
            var toBuildBarn = _owner.Inventory.FindItems(item => item is Stone);
            if (_owner.Cell != _owner.House.Village.Barn.Cell
                || toBuildBarn.Count == 0)
            {
                return;
            }

            toBuildBarn.ForEach(item => item.Use(_owner.House.Village.Barn));
        }
        
        public void CreateStorageAction()
        {
            var toCreateStorage = _owner.Inventory.FindItems(item =>
                item.GetType() == (_owner.ProfessionLifecycle as BuilderLifecycleManager)?.StorageTypeToCreate);
            if (_owner.Cell.IsBuildingHere()
                || toCreateStorage.Count < Storage<Resource>.ResoursesToCreateCount)
            {
                return;
            }

            var newStorage = Storage<Resource>.Create(toCreateStorage[0] as Resource, _owner.Map, _owner.Cell);
            toCreateStorage.ForEach(resource => resource.Use(newStorage));
            _owner.House.Village.Storages.Add(newStorage);
            newStorage.SetVillage(_owner.House.Village);
        }
    }
}