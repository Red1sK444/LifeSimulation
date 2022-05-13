using System;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement.TargetMovement;
using OOP_LifeSimulation.Inventory;

namespace OOP_LifeSimulation.EntitiesExtended.LifecycleManagers
{
    public class TameableLifecycleManager : LifecycleManager
    {
        private TameableEntity _pet;
        private Func<Item, bool> _determineFoodInInventory;

        public TameableLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) :
            base(entity, field, isFood, isPartner)
        {
            _pet = (TameableEntity) Entity;
            PartnerMovement = new LadderMovement(field);
        }

        protected override Cell IsStarvingActions(Cell current)
        {
            var foodFromOwner = _pet.owner.Inventory.FindItem(_pet.DetermineFoodInInventory);
            if (foodFromOwner != null)
            {
                if (Math.Abs(current.Position.X - _pet.owner.Cell.Position.X) <= 2
                    && Math.Abs(current.Position.Y - _pet.owner.Cell.Position.Y) <= 2)
                {
                    return _pet.owner.Cell;
                }

                return FoodMovement.MoveByWay(current, _pet.owner.Cell);
            }

            return base.IsStarvingActions(current);
        }

        protected override Cell IsNotStarvingActions(Cell current)
        {
            return Entity.ReproductionCD <= 0
                ? base.IsNotStarvingActions(current)
                : PartnerMovement.MoveByWay(current, _pet.owner.Cell);
        }

        protected override Cell DefaultMove(Cell current)
        {
            return PartnerMovement.MoveByWay(current, _pet.owner.Cell);
        }
    }
}