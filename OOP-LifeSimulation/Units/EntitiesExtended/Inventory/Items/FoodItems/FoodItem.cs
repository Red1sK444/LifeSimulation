using System;
using System.Windows.Forms;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation.Inventory
{
    public abstract class FoodItem : Item, IEatable
    {
        private readonly int _satietyToRegen;
        private readonly int _hpToRegen;

        public FoodItem(Inventory inventory, IEatable owner) : base(inventory)
        {
            _hpToRegen = owner.GetHpToRegen();
            _satietyToRegen = owner.GetSatietyToRegen();
        }

        public override void Use(Object useOn)
        {
            Inventory?.DeleteItem(this);
            if (useOn is Entity food)
            {
                BecomeEaten(food);
                return;
            }
            (useOn as IStorage<Item>)?.PutResource(this);
        }

        public virtual void BecomeEaten(Entity eater)
        {
            eater.HP.Increase(_hpToRegen);
            eater.Satiety.Increase(_satietyToRegen);
        }

        public int GetHpToRegen()
        {
            return _hpToRegen;
        }

        public int GetSatietyToRegen()
        {
            return _satietyToRegen;
        }
    }
}