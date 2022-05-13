using System.Runtime.Remoting.Messaging;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;

namespace OOP_LifeSimulation.Actions
{
    public class ShepherdActions
    {
        private Human _owner;

        public ShepherdActions(Human owner)
        {
            _owner = owner;
        }

        public void TameAction()
        {
            for (var i = -5; i <= 5; i++)
            {
                for (var j = -5; j <= 5; j++)
                {
                    var newCords = new Coords(_owner.Cell.Position.X + j,
                        _owner.Cell.Position.Y + i);
                    if (0 <= newCords.X && newCords.X < _owner.Map.MapSize && 0 <= newCords.Y &&
                        newCords.Y < _owner.Map.MapSize)
                    {
                        if (_owner.CanAddPet())
                        {
                            var possiblePet = _owner.Map.Field[newCords.Y, newCords.X].GetEntityToTame(_owner);
                            if (possiblePet != null)
                            {
                                _owner.AddPet(possiblePet);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}