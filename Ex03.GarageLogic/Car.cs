namespace Ex03.GarageLogic
{
    internal class Car : Vehicle
    {
        private Garage.Elements.eCarColors m_CarColor;
        private Garage.Elements.eCarDoors m_NumberOfDoors;
        public const int NumOfWheels = 4;
        public const float MaxAirPressure = 31;
        public const float MaxFuelCapacity = 55;
        public const float MaxBatteryCapacity = 1.8f;

        public Car(Vehicle i_Vehicle, bool i_IsElectricVehicle, int i_NumOfWheels,
            float i_CurrentAirPressure, float i_MaxAirPressure, string i_ManufacturerName, float i_CurrentEnergy,
             float i_MaxEnergyCapacity, Garage.Elements.eFuelTypes i_FuelType)
        : base(i_Vehicle)
        {
            this.SetTires(i_NumOfWheels, i_CurrentAirPressure, i_MaxAirPressure, i_ManufacturerName);
            this.SetEngineType(i_IsElectricVehicle, i_CurrentEnergy, i_MaxEnergyCapacity, i_FuelType);
        }

        public Garage.Elements.eCarDoors NumberOfDoors
        {
            get
            {
                return m_NumberOfDoors;
            }
            set
            {
                m_NumberOfDoors = value;
            }
        }

        public Garage.Elements.eCarColors CarColor
        {
            get
            {
                return m_CarColor;
            }
            set
            {
                m_CarColor = value;
            }
        }
    }
}
