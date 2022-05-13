using System.Drawing;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Entities
{
    public class OmnivorousTameableEntity : TameableEntity
    {
        public OmnivorousTameableEntity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.IndianRed;

            bool IsFood(Cell current)
            {
                return current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                             && current.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                                             false || current.IsEntityHere() &&
                    current.GetOtherEntityByType(this) != null && current.GetOtherEntityByType(this) != owner;
            }

            bool IsPartner(Cell current) => current.IsEntityHere() && current.GetEntityToReproduce(this) != null;

            Lifecycle = new OmnivorousLifecycleManager(this, Map.Field, IsFood, IsPartner);
            TameableLifecycle =
                new OmnivorousTameableLifecycleManager(this, Map.Field, IsFood, IsPartner);
            DetermineFoodInInventory = item => item is IEatable;
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