using System.Drawing;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous
{
    public class HoneyBadger : OmnivorousTameableEntity
    {
        public HoneyBadger(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Brown;
            Lifecycle.SetMovementMethods(new LadderMovement(Map.Field),
                new AllRandomMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\honeybadger.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new HoneyBadger(map, cell);
        }

        protected override void DoTameableAction()
        {
            if (owner.StateCheck() == EntityState.Dying)
            {
                for (var i = -3; i <= 3; i++)
                {
                    for (var j = -3; j <= 3; j++)
                    {
                        var newCords = new Coords(owner.Cell.Position.X + j,
                            owner.Cell.Position.Y + i);
                        if (0 <= newCords.X && newCords.X < Map.MapSize && 0 <= newCords.Y && newCords.Y < Map.MapSize)
                        {
                            if ((i != 0 || j != 0)
                                && Map.Field[newCords.Y, newCords.X].GetOtherEntityByType(this) != null)
                            {
                                DoFoodFoundAction(Map.Field[newCords.Y, newCords.X].GetOtherEntityByType(this));
                                Step(Map.Field[newCords.Y, newCords.X]);
                            }
                        }
                    }
                }
            }
            base.DoTameableAction();
        }
    }
}