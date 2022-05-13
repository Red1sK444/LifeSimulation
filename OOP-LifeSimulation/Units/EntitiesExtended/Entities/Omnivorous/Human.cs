using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using OOP_LifeSimulation.Buildings;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntitiesExtended.Entities.LifecycleManagers;
using OOP_LifeSimulation.EntityMovement.FreeMovement;
using OOP_LifeSimulation.EntityMovement.TargetMovement;
using OOP_LifeSimulation.Inventory;
using OOP_LifeSimulation.Inventory.Resources;
using OOP_LifeSimulation.Inventory.Resources.ResTypes;
using OOP_LifeSimulation.PlantsExtended;
using OOP_LifeSimulation.Villages;

namespace OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human
{
    public class Human : OmnivorousEntity
    {
        private bool IsPartner(Cell current) => current.IsEntityHere() && current.GetEntityToReproduce(this) != null;

        private bool IsFood(Cell current) => current.IsPlantHere() && current.GetPlant() is IEatableForHerbivore
                                                                   && current.GetPlant().GrowthState
                                                                       .Equals(PlantGrowthState.Seed) ==
                                                                   false || current.IsEntityHere() &&
            current.GetOtherEntityByType(this) != null &&
            !(current.GetOtherEntityByType(this) is TameableEntity entity && _pets.Contains(entity));

        public Inventory.Inventory Inventory;
        public House House;
        private List<TameableEntity> _pets = new List<TameableEntity>(10);
        public Action CurrentAction;
        public HumanLifecycleManager ProfessionLifecycle;

        public Human(Map map, Cell cell) : base(map, cell)
        {
            Inventory = new Inventory.Inventory(this);
            Lifecycle = new HumanLifecycleManager(this, Map.Field, IsFood, IsPartner);
            Lifecycle.SetMovementMethods(new StraightMovement(Map.Field),
                new AntMovement(Map.Field, Lifecycle.UpdatePrevMove));
            Sprite = Sex == EntitySex.Male
                ? new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\man.png"))
                : Sprite = new Bitmap(Image.FromFile("D:\\LifeSimulationSprites\\woman.png"));
        }

        public bool RemovePet(TameableEntity pet)
        {
            return _pets.Remove(pet);
        }

        public void AddPet(TameableEntity pet)
        {
            Inventory.FindItem(pet.DetermineFoodInInventory).Use(pet);
            pet.owner = this;
            _pets.Add(pet);
        }

        public bool CanAddPet()
        {
            return _pets.Count < _pets.Capacity;
        }

        protected override Entity GetChild(Map map, Cell cell)
        {
            return new Human(map, cell);
        }

        protected override void DoFoodFoundAction(IEatable food)
        {
            if (Inventory.CanAddItem())
            {
                if (food is IEatableForCarnivore carnivore)
                {
                    Inventory.AddItem(new MeatFoodItem(Inventory, carnivore));
                }
                else
                {
                    Inventory.AddItem(new PlantFoodItem(Inventory, (IEatableForHerbivore) food));
                }

                ((Unit) food).Die();
            }
        }

        protected override bool ForgetPartner()
        {
            return Partner.StateCheck().Equals(EntityState.Dead);
        }

        protected override void AfterStepCheck()
        {
            if (CurrentAction != null)
            {
                CurrentAction.Invoke();
                CurrentAction = null;
            }
            else
            {
                CheckToEat();
            }
        }

        public void SetHouse(House house)
        {
            if (house == null)
            {
                return;
            }

            House = house;
            House.AddOwner(this);
        }

        protected override void Move()
        {
            if (ProfessionLifecycle != null)
            {
                Step(ProfessionLifecycle.MoveTo(IsStarving(), Cell));
            }
            else
            {
                base.Move();
            }
        }

        public void PerformVillageAppearsAction()
        {
            if (Sex == EntitySex.Male)
            {
                switch (Utils.GetRandomInt(3))
                {
                    case 0:
                        ProfessionLifecycle = new HunterLifecycleManager(this, Map.Field, IsFood, IsPartner);
                        break;

                    case 1:
                        ProfessionLifecycle = new BuilderLifecycleManager(this, Map.Field, IsFood, IsPartner);
                        break;

                    case 2:
                        ProfessionLifecycle = new ShepherdLifecycleManager(this, Map.Field, IsFood, IsPartner);
                        break;
                }
            }
            else
            {
                switch (Utils.GetRandomInt(2))
                {
                    case 0:
                        ProfessionLifecycle = new CollectorLifecycleManager(this, Map.Field, IsFood, IsPartner);
                        break;

                    case 1:
                        ProfessionLifecycle = new ShepherdLifecycleManager(this, Map.Field, IsFood, IsPartner);
                        break;
                }
            }
        }

        protected override void AfterReproduceActions(Entity entity)
        {
            (entity as Human)?.SetHouse(House);
        }

        public override void Die()
        {
            base.Die();
            foreach (var pet in _pets)
            {
                pet.owner = null;
            }
        }

        public override string SendInfo()
        {
            var type = $"Type = {GetType().Name}";
            var profession = $"Profession = {ProfessionLifecycle?.GetType().Name}";
            var coords = $"Coordinates: X={Cell.Position.X};Y={Cell.Position.Y}";
            var hp = $"HP = {HP.Value}/{HP.MaxValue}";
            var satiety = $"Satiety = {Satiety.Value}/{Satiety.MaxValue}";
            var category = $"Category = {nameof(OmnivorousEntity)}";
            var partnerCoords =
                $"Partner Coords: {(Partner != null ? $"X={Partner.Cell.Position.X.ToString()};Y={Partner.Cell.Position.Y.ToString()}" : "No Partner")}";

            return
                $"\n{type}\n{coords}\n{hp}\n{satiety}\n{category}\n{partnerCoords}\n{profession}\n{Inventory.SendInfo()}\n{CollectPetsInfo()}";
        }

        private string CollectPetsInfo()
        {
            var pets = "Pets:\n";
            foreach (var pet in _pets)
            {
                pets += $"\t{pet.GetType().Name} - Coords: X={pet.Cell.Position.X};Y={pet.Cell.Position.Y}\n";
            }

            return pets;
        }
    }
}