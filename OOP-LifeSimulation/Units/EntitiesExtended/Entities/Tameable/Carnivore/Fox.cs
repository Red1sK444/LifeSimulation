using System.Drawing;
using System.Drawing.Text;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Carnivore
{
    public class Fox : CarnivoreTameableEntity
    {
        public Fox(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Orange;
            Lifecycle.SetMovementMethods(new LadderMovement(Map.Field),
                new AllRandomMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\fox.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Fox(map, cell);
        }

        protected override void DoTameableAction()
        {
            owner.HP.Increase(HP.Value/10);
            HP.Decrease(HP.Value/10);
            base.DoTameableAction();
        }
    }
}