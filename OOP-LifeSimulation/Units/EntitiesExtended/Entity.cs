using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using OOP_LifeSimulation.Entities;
using OOP_LifeSimulation.EntitiesExtended.Entities.Carnivore;
using OOP_LifeSimulation.EntitiesExtended.Entities.Herbivore;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous;
using OOP_LifeSimulation.EntitiesExtended.Entities.Omnivorous.Human;
using OOP_LifeSimulation.EntitiesExtended.EntityMovement.MovementManagers;
using OOP_LifeSimulation.EntityMovement;
using OOP_LifeSimulation.PlantsExtended;

namespace OOP_LifeSimulation
{
    public class Entity : Unit, IEatableForCarnivore
    {
        private int HpToRegen = 30;
        private int SatietyToRegen = 50;
        private const int SatietyToStarve = 30;
        public static int StarvingPerTick = 1; //1
        private const int WastingHealthPerTick = 1; //1
        private const int PassiveHealing = 3;
        private const int ReproductionCDDuration = 70;
        public Entity Partner;
        public int ReproductionCD;
        public readonly Indicator HP = new Indicator(0, 100);
        public readonly Indicator Satiety = new Indicator(0, 100);
        protected LifecycleManager Lifecycle;
        private List<Cell> _foodRoute = new List<Cell>();
        public readonly EntitySex Sex = Utils.GetRandomInt(2) == 1 ? EntitySex.Male : EntitySex.Female;
        private int _ticksToDie = 10000;

        public Entity(Map map, Cell cell) : base(map, cell)
        {
            Color = Color.Crimson;
            //_movement = new Movement(Map.Field);
            Lifecycle = new LifecycleManager(this, Map.Field, delegate(Cell current)
            {
                if (current.IsPlantHere() && current.GetPlant() is IEatable
                                          && current.GetPlant().GrowthState.Equals(PlantGrowthState.Seed) ==
                                          false)
                {
                    return true;
                }

                return false;
            }, delegate(Cell current) { return false; });
        }

        public static Entity GetRandomEntity(Map map, Cell cell)
        {
            var randomInteger = Utils.GetRandomInt(25);
            //return new Human(map, cell);
            return randomInteger switch
            {
                0 => new Wolf(map, cell),
                1 => new Fox(map, cell),
                2 => new Crocodile(map, cell),
                3 => new Donkey(map, cell),
                4 => new Giraffe(map, cell),
                5 => new Rabbit(map, cell),
                6 => new Bear(map, cell),
                7 => new HoneyBadger(map, cell),
                8 => new Sloth(map, cell),
                _ => new Human(map, cell)
            };
        }

        public override string SendInfo()
        {
            var type = $"Type = {GetType().Name}";
            var coords = $"Coordinates: X={Cell.Position.X};Y={Cell.Position.Y}";
            var hp = $"HP = {HP.Value}/{HP.MaxValue}";
            var satiety = $"Satiety = {Satiety.Value}/{Satiety.MaxValue}";
            var category =
                $"Category = {(this is HerbivoreEntity || this is HerbivoreTameableEntity ? nameof(HerbivoreEntity) : this is CarnivoreEntity || this is CarnivoreTameableEntity ? nameof(CarnivoreEntity) : nameof(OmnivorousEntity))}";
            return $"\n{type}\n{coords}\n{hp}\n{satiety}\n{category}";
        }

        public virtual void CheckToEat()
        {
            LookAroundForFood();
        }

        protected virtual void LookAroundForFood()
        {
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    var newCords = new Coords(Cell.Position.X + j,
                        Cell.Position.Y + i);
                    if (0 <= newCords.X && newCords.X < Map.MapSize && 0 <= newCords.Y && newCords.Y < Map.MapSize)
                    {
                        if (Map.Field[newCords.Y, newCords.X].Biome.Name != BiomesEnum.Lake
                            && Lifecycle.food is IEatableForCarnivore food && Map
                                .Field[newCords.Y, newCords.X]
                                .HasThisUnit((Entity) Lifecycle.food))
                        {
                            if (i == 0 && j == 0)
                            {
                                DoFoodFoundAction(food);
                                Lifecycle.food = null;
                            }
                            else
                            {
                                Step(Map.Field[newCords.Y, newCords.X]);
                            }

                            return;
                        }
                    }
                }
            }
        }

        protected virtual void DoFoodFoundAction(IEatable food)
        {
            food.BecomeEaten(this);
        }

        protected virtual Entity GetChild(Map map, Cell cell)
        {
            return new Entity(map, cell);
        }

        protected void Step(Cell moveTo)
        {
            if (moveTo != Cell)
            {
                Map.Field[Cell.Position.Y, Cell.Position.X].RemoveEntity(this);
                Map.Field[moveTo.Position.Y, moveTo.Position.X].UnitList.Add(this);

                Map.ChangedCells.Add(moveTo);
                Map.ChangedCells.Add(Cell);

                Cell = moveTo;
            }

            AfterStepCheck();
        }

        protected virtual void AfterStepCheck()
        {
            if (IsStarving())
            {
                CheckToEat();
            }
            else
            {
                IsNotStarvingActions();
            }
        }

        protected virtual void IsNotStarvingActions()
        {
            ReproduceAction();
        }

        public void ReproduceAction()
        {
            if (Partner != null && Cell == Partner.Cell && ReproductionCD <= 0 && Partner.ReproductionCD <= 0)
            {
                var entity = GetChild(Map, Cell);
                Cell.UnitList.Add(entity);
                Map.EntityList.Add(entity);
                Map.ChangedCells.Add(Cell);
                ReproductionCD = ReproductionCDDuration;
                Partner.ReproductionCD = ReproductionCDDuration;
                AfterReproduceActions(entity);
                if (!ForgetPartner()) return;
                Partner.Partner = null;
                Partner = null;
            }
        }

        protected virtual void AfterReproduceActions(Entity entity) {}

        protected virtual bool ForgetPartner()
        {
            return true;
        }

        protected virtual void Move()
        {
            Step(Lifecycle.MoveTo(IsStarving(), Cell));
        }

        protected bool IsStarving()
        {
            return Satiety.Value < SatietyToStarve;
        }

        private void ApplyDamage(int damageSize)
        {
            HP.Decrease(damageSize);
        }

        public override void Die()
        {
            Cell.RemoveEntity(this);
            Map.EntityList.Remove(this);
            Map.ChangedCells.Add(Cell);
            HP.Decrease(HP.MaxValue);
        }

        public override void Draw(Drawer drawer)
        {
            drawer.Draw(Cell, Sprite);
        }

        public EntityState StateCheck()
        {
            if (HP.Value <= HP.MinValue)
            {
                return EntityState.Dead;
            }

            if (Map.Season == SeasonEnum.Winter && this is IReactingToSeasonChange)
            {
                return EntityState.Sleeping;
            }

            if (Satiety.Value >= SatietyToStarve)
            {
                return EntityState.Healthy;
            }

            if (Satiety.Value > Satiety.MinValue)
            {
                return EntityState.Starving;
            }

            if (HP.Value > HP.MinValue)
            {
                return EntityState.Dying;
            }

            return EntityState.Dead;
        }

        public override void TimeTick(int? timeTickCount = null)
        {
            _ticksToDie--;
            ReproductionCD--;
            if (_ticksToDie <= 0)
            {
                Die();
            }

            switch (StateCheck())
            {
                case EntityState.Healthy:
                    HP.Increase(PassiveHealing);
                    Satiety.Decrease(StarvingPerTick);
                    break;
                case EntityState.Starving:
                    Satiety.Decrease(StarvingPerTick);
                    ApplyDamage(WastingHealthPerTick);
                    break;
                case EntityState.Dying:
                    ApplyDamage(WastingHealthPerTick);
                    if (StateCheck() == EntityState.Dead)
                    {
                        Die();
                        return;
                    }

                    break;
                case EntityState.Dead:
                    return;
                case EntityState.Sleeping:
                    HP.Increase(HP.MaxValue);
                    return;
            }

            Move();
        }

        public void BecomeEaten(Entity eater)
        {
            eater.Satiety.Increase(GetSatietyToRegen());
            eater.HP.Increase(GetHpToRegen());
            Die();
        }

        public int GetHpToRegen()
        {
            return HpToRegen;
        }

        public int GetSatietyToRegen()
        {
            return SatietyToRegen;
        }
    }
}