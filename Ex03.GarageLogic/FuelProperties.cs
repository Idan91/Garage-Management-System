
namespace Ex03.GarageLogic
{
    internal class FuelProperties : Energy
    {
        private Garage.Elements.eFuelTypes m_FuelType;
        

        public FuelProperties(float i_CurrentEnergy, float i_MaxFuelAmount, Garage.Elements.eFuelTypes i_FuelType)
        {
            m_FuelType = i_FuelType;
            base.CurrentAmount = i_CurrentEnergy;
            base.Capacity = i_MaxFuelAmount;
        }

        public override bool FillEnergy(float i_EnergyToFill, Garage.Elements.eFuelTypes i_FuelType)
        {
            bool v_FuelAdded = false;
            if (i_FuelType == m_FuelType)
            {
                v_FuelAdded = base.FillEnergy(i_EnergyToFill);
            }
            else
            {
                throw new System.ArgumentException("Invalid parameter used! (valid 'FuelType')");
            }
            return v_FuelAdded;
        }

        public Garage.Elements.eFuelTypes FuelType
        {
            get
            {
                return m_FuelType;
            }
        }
    }
}
