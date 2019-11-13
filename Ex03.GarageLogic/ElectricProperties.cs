namespace Ex03.GarageLogic
{
    internal class ElectricProperties : Energy
    {
        public ElectricProperties(float i_CurrentEnergy, float i_MaxBatteryTime) 
        {
            base.CurrentAmount = i_CurrentEnergy;
            base.Capacity = i_MaxBatteryTime;
        }
    }
}
