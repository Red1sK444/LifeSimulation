using System.Drawing;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous
{
    public class Sloth : OmnivorousEntity
    {
        public Sloth(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Aquamarine;
            Lifecycle.SetMovementMethods(new BurstMovement(map.Field),
                new AreaAntMovement(map.Field, Lifecycle.UpdatePrevMove, cell.Position));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\sloth.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Sloth(map, cell);
        }
    }
}