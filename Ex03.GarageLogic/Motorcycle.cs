namespace Ex03.GarageLogic
{
    internal class Motorcycle : Vehicle
    {
        private Garage.Elements.eMotorcycleLicenseType m_LicenseType;
        private int m_EngineVolume;
        public const int NumOfWheels = 2;
        public const float MaxAirPressure = 33;
        public const float MaxFuelCapacity = 8; // define
        public const float MaxBatteryCapacity = 1.4f;

        public Motorcycle(Vehicle i_Vehicle, bool i_IsElectricVehicle,int i_NumOfWheels,
           float i_CurrentAirPressure, float i_MaxAirPressure, string i_ManufacturerName, float i_CurrentEnergy,
            float i_MaxEnergyCapacity, Garage.Elements.eFuelTypes i_FuelType)
        : base(i_Vehicle)
        {
            this.SetTires(i_NumOfWheels, i_CurrentAirPressure, i_MaxAirPressure, i_ManufacturerName);
            this.SetEngineType(i_IsElectricVehicle, i_CurrentEnergy, i_MaxEnergyCapacity, i_FuelType);
        }

        public Garage.Elements.eMotorcycleLicenseType LicenseType
        {
            get
            {
                return m_LicenseType;       
            }
            set
            {
                m_LicenseType = value;
            }
        }

        public int EngineVolume
        {
            get
            {
                return m_EngineVolume;
            }
            set
            {
                m_EngineVolume = value;
            }
        }
    }
}
