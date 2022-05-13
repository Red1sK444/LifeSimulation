using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class HerbivoreTameableEntity : TameableEntity
    {
        public HerbivoreTameableEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Chartreuse;

            bool IsFood(Cell current)
            {
                return current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                             && current.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                                             false;
            }

            bool IsPartner(Cell current) => current.IsEntityHere() && current.GetEntityToReproduce(this) != null;

            Lifecycle = new HerbivoreLifecycleManager(this, Map.Field, IsFood, IsPartner);
            TameableLifecycle = new HerbivoreTameableLifecycleManager(this, Map.Field, IsFood, IsPartner);
            DetermineFoodInInventory = item => item is IEatableForHerbivore;
        }

        protected override void LookAroundForFood()
        {
            if (Cell.IsPlantHere() && Cell.GetPlant() is IEatableForHerbivore food)
            {
                DoFoodFoundAction(food);
            }
        }
    }
}