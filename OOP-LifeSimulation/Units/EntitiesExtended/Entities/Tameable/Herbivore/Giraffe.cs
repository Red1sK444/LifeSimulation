using System.Drawing;
using System.Windows.Forms;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Herbivore
{
    public class Giraffe : HerbivoreTameableEntity, IReactingToSeasonChange
    {
        public Giraffe(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Gold;
            Lifecycle.SetMovementMethods(new LadderMovement(Map.Field),
                new AllRandomMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\giraffe.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Giraffe(map, cell);
        }

        protected override void DoTameableAction()
        {
            if (owner.StateCheck() == EntityState.Dying) BecomeEaten(owner);
            base.DoTameableAction();
        }
    }
}