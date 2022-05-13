namespace OOP_LifeSimulation.PlantsExtended
{
    public abstract class EatablePlant : SpreadingPlant, IEatableForHerbivore
    {
        private const int SatietyToRegen = 5;
        private const int HpToRegen = 10;
        private const int HpToApply = 3;
        
        protected EatablePlant(Map map, Cell cell) : base(map, cell){}
        
        public void BecomeEaten(Entity eater)
        {
            if (IsToxic() == false)
            {
                eater.HP.Increase(GetHpToRegen());
                eater.Satiety.Increase(GetSatietyToRegen());
                Die();
            }
            else
            {
                eater.HP.Decrease(HpToApply);
                Die();
            }
        }

        public int GetHpToRegen()
        {
            return HpToRegen;
        }

        public int GetSatietyToRegen()
        {
            return SatietyToRegen;
        }

        public abstract bool IsToxic();
        public int GetHpToApply()
        {
            return HpToApply;
        }
    }
}