using System;
using OOP_LifeSimulation.Actions;
using OOP_LifeSimulation.EntityMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers
{
    public class ShepherdLifecycleManager : HumanLifecycleManager
    {
        private ShepherdActions _shepherdActions;
        
        public ShepherdLifecycleManager(Entity entity, Cell[,] field, Func<Cell, bool> isFood,
            Func<Cell, bool> isPartner) : base(entity, field, isFood, isPartner)
        {
            _shepherdActions = new ShepherdActions(Entity);
        }

        protected override Cell DefaultActions(Cell current)
        {
            Entity.CurrentAction = _shepherdActions.TameAction;
            return DefaultMove(current);
        }
    }
}