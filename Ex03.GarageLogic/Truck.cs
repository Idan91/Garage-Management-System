namespace Ex03.GarageLogic
{
    internal class Truck : Vehicle
    {
        private bool m_CarriesHazardousMaterials;
        private float m_CargoVolume;
        public const int NumOfWheels = 12;
        public const float MaxAirPressure = 26;
        public const float MaxFuelCapacity = 110;

        public Truck(Vehicle i_Vehicle, bool i_IsElectricVehicle, int i_NumOfWheels,
            float i_CurrentAirPressure, float i_MaxAirPressure, string i_ManufacturerName, float i_CurrentEnergy,
            float i_MaxEnergyCapacity, Garage.Elements.eFuelTypes i_FuelType)
        : base(i_Vehicle)
        {
            this.SetTires(i_NumOfWheels, i_CurrentAirPressure, i_MaxAirPressure, i_ManufacturerName);
            this.SetEngineType(i_IsElectricVehicle, i_CurrentEnergy, i_MaxEnergyCapacity, i_FuelType);
        }

        public bool CarriesHazardousMaterials
        {
            get
            {
                return m_CarriesHazardousMaterials;
            }
            set
            {
                m_CarriesHazardousMaterials = value;
            }
        }

        public float CargoVolume
        {
            get
            {
                return m_CargoVolume;
            }
            set
            {
                m_CargoVolume = value;
            }
        }

    }
}
