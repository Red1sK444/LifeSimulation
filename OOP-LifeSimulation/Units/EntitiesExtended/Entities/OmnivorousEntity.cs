using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class OmnivorousEntity : Entity
    {
        public OmnivorousEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.IndianRed;
            Lifecycle = new OmnivorousLifecycleManager(this, Map.Field, delegate(Cell current)
            {
                if ((current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                           && current.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                                           false) || current.IsEntityHere() &&
                    current.GetOtherEntityByType(this) != null)
                {
                    return true;
                }

                return false;
            }, current => current.IsEntityHere() && current.GetEntityToReproduce(this) != null);
        }

        protected override void LookAroundForFood()
        {
            base.LookAroundForFood();
            if (Cell.IsPlantHere() && Cell.GetPlant() is IEatableForHerbivore food)
            {
                DoFoodFoundAction(food);
            }
        }
    }
}