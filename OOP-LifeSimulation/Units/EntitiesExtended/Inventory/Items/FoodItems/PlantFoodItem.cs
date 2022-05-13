using System.Drawing;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Inventory
{
    public class PlantFoodItem : FoodItem, IEatableForHerbivore
    {
        private readonly bool _toxic;
        private readonly int _hpToApply;
        public PlantFoodItem(Inventory inventory, IEatableForHerbivore owner) : base(inventory, owner)
        {
            _hpToApply = owner.GetHpToApply();
            _toxic = owner.IsToxic();
        }

        public override void BecomeEaten(Entity eater)
        {
            if (_toxic)
            {
                eater.HP.Decrease(GetHpToApply());
            }
            else
            {
                base.BecomeEaten(eater);    
            }
        }

        public bool IsToxic()
        {
            return _toxic;
        }

        public int GetHpToApply()
        {
            return _hpToApply;
        }
    }
}