namespace OOP_LifeSimulation.PlantsExtended
{
    public interface IEatable
    {
        //bool IsToxic();

        void BecomeEaten(Entity eater);
        int GetHpToRegen();
        int GetSatietyToRegen();
    }
}