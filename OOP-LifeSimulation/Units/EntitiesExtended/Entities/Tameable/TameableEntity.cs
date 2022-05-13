using System;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.EntitiesExtended.LifecycleManagers;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation
{
    public class TameableEntity : Entity
    {
        public Human owner;
        protected TameableLifecycleManager TameableLifecycle;
        public Func<Item, bool> DetermineFoodInInventory;
        private const int TameableActionCDDuration = 100;
        private int _tameableActionCD = 100;

        public TameableEntity(Map map, Cell cell) : base(map, cell)
        {
        }

        protected virtual void DoTameableAction()
        {
            _tameableActionCD = TameableActionCDDuration;
        }

        protected override void IsNotStarvingActions()
        {
            base.IsNotStarvingActions();
            if (_tameableActionCD <= 0 && owner != null)
            {
                DoTameableAction();
            }
        }

        protected override void AfterStepCheck()
        {
            base.AfterStepCheck();
            _tameableActionCD--;
        }

        public override void Die()
        {
            base.Die();
            owner?.RemovePet(this);
        }

        protected override void Move()
        {
            if (owner != null)
            {
                Step(TameableLifecycle.MoveTo(IsStarving(), Cell));
            }
            else
            {
                base.Move();
            }
        }

        public override void CheckToEat()
        {
            if (owner != null && owner.Cell == Cell && owner.Inventory.FindItem(DetermineFoodInInventory) != null)
            {
                DoFoodFoundAction(
                    (IEatable) owner.Inventory.FetchItem(owner.Inventory.FindItem(DetermineFoodInInventory)));
            }
            else
            {
                LookAroundForFood();
            }
        }
    }
}