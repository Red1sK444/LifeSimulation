using System.Drawing;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Carnivore
{
    public class Crocodile : CarnivoreEntity, IReactingToSeasonChange
    {
        public Crocodile(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Olive;
            Lifecycle.SetMovementMethods(new StraightMovement(map.Field),
                new AntMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\crocodile.png"));
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Crocodile(map, cell);
        }
    }
}