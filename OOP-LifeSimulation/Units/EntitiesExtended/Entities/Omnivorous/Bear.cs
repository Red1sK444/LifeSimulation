using System.Drawing;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous
{
    public class Bear : OmnivorousEntity, IReactingToSeasonChange
    {
        public Bear(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Plum;
            Lifecycle.SetMovementMethods(new StraightMovement(map.Field),
                new AntMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\bear.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Bear(map, cell);
        }
    }
}