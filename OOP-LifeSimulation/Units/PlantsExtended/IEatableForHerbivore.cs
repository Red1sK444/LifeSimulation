namespace OOP_LifeSimulation.PlantsExtended
{
    public interface IEatableForHerbivore : IEatable
    {
        bool IsToxic();
        int GetHpToApply();
    }
}