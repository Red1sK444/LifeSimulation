namespace OOP_LifeSimulation
{
    public class Indicator
    {
        public int Value { get; private set; }
        public int MinValue { get; }
        public int MaxValue { get; }

        public Indicator(int minValue, int maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = MaxValue;
        }

        public void Increase(int onValue)
        {
            Value += onValue;
            if (Value > MaxValue)
            {
                Value = MaxValue;
            }
        }

        public void Decrease(int onValue)
        {
            Value -= onValue;
            if (Value < MinValue)
            {
                Value = MinValue;
            }
        }

        public float GetPercent()
        {
            return (float) Value / MaxValue;
        }
    }
}