using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class HerbivoreEntity : Entity
    {
        public HerbivoreEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Chartreuse;
            Lifecycle = new HerbivoreLifecycleManager(this, Map.Field, delegate(Cell current)
            {
                if (current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                          && current.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                                          false)
                {
                    return true;
                }

                return false;
            }, current => current.IsEntityHere() && current.GetEntityToReproduce(this) != null);
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