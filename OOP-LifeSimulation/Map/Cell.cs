using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.Inventory.Resources;

namespace OOP_LifeSimulation
{
    public class Cell : IDrawable
    {
        public readonly Coords Position;
        public Biome Biome = new Forest();
        public List<Unit> UnitList = new List<Unit>(2);

        public Cell(int positionX, int positionY)
        {
            Position = new Coords(positionX, positionY);
        }

        public bool IsUnitHere()
        {
            return UnitList.Count > 0;
        }

        public bool IsPlantHere()
        {
            return UnitList.OfType<Plant>().ToList().Count > 0;
        }

        public bool IsEntityHere()
        {
            return UnitList.OfType<Entity>().ToList().Count > 0;
        }

        public bool IsBuildingHere()
        {
            return UnitList.OfType<Building>().Any();
        }
        
        public bool IsHouseHere()
        {
            return UnitList.OfType<House>().Any();
        }

        public House GetHouse()
        {
            return UnitList.OfType<House>().ToList()[0];
        }
        
        public bool IsSourceHere()
        {
            return UnitList.OfType<ISource<Resource>>().Any();
        }
        
        public bool IsStorageHere()
        {
            return UnitList.OfType<IStorage<Resource>>().Any();
        }
        
        public ISource<Resource> GetSource()
        {
            return UnitList.OfType<ISource<Resource>>().ToList()[0];
        }
        
        public IStorage<Resource> GetStorage()
        {
            return UnitList.OfType<IStorage<Resource>>().ToList()[0];
        }

        public Building GetBuilding()
        {
            return UnitList.OfType<Building>().ToList()[0];
        }

        public Plant GetPlant()
        {
            return UnitList.OfType<Plant>().ToList()[0];
        }

        private Entity GetEntity()
        {
            return UnitList.OfType<Entity>().ToList()[0];
        }

        public bool HasThisUnit(Unit unit)
        {
            return UnitList.Contains(unit);
        }

        public int GetEntityCount()
        {
            return UnitList.OfType<Entity>().ToList().Count;
        }

        public Entity GetOtherEntityByType(Entity entity) //rename
        {
            return UnitList.OfType<Entity>().ToList().Find(e =>
                e.GetType() != entity.GetType() && e != entity
            );
        }

        public Entity GetEntityToReproduce(Entity entity) //rename
        {
            return UnitList.OfType<Entity>().ToList().Find(e =>
                e.GetType() == entity.GetType() && e != entity && e.Sex != entity.Sex
                && e.Partner == null && e.ReproductionCD <= 0 && e.StateCheck() == EntityState.Healthy
            );
        }

        public TameableEntity GetEntityToTame(Human human)
        {
            return UnitList.OfType<TameableEntity>().ToList().Find(te =>
                te.owner == null && human.Inventory.FindItem(te.DetermineFoodInInventory) != null &&
                te.StateCheck() != EntityState.Dead);
        }

        public void RemovePlant(Plant plant)
        {
            //UnitList.RemoveAll(unit => unit.GetType() == typeof(Entity));
            UnitList.Remove(plant);
        }

        public void RemoveEntity(Entity entity)
        {
            //UnitList.RemoveAll(unit => unit.GetType() == typeof(Entity));
            UnitList.Remove(entity);
        }

        public void Draw(Drawer drawer)
        {
            drawer.Draw(this, Biome.GetColor());
            if (IsBuildingHere())
            {
                GetBuilding().Draw(drawer);
            }

            if (IsEntityHere())
            {
                GetEntity().Draw(drawer);
                return;
            }
            
            if (IsPlantHere())
            {
                GetPlant().Draw(drawer);
            }
        }

        public IShowingInfo GetIShowingInfo()
        {
            if (IsUnitHere())
            {
                if (IsBuildingHere())
                {
                    return GetBuilding();
                }
                if (IsEntityHere())
                {
                    return GetEntity();
                }
                return GetPlant();
            }

            return null;
        }
    }
}