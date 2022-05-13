using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Inventory
{
    public class MeatFoodItem : FoodItem, IEatableForCarnivore
    {
        public MeatFoodItem(Inventory inventory, IEatableForCarnivore owner) : base(inventory, owner)
        {
        }
    }
}