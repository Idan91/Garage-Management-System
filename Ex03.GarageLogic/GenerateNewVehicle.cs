namespace Ex03.GarageLogic
{
    internal class GenerateNewVehicle
    {
        private const bool m_IsElectric = true;

        public enum eVehicleTypes : byte
        {
            FuelMotorcycle = 1,
            ElectricMotorcycle,
            FuelCar,
            ElectricCar,
            Truck,
        }

        public static void CreateVehicle(ref Vehicle io_Vehicle, float i_CurrentEnergy,
            string i_ManufacturerName, float i_CurrentAirPressure, Vehicle i_VehicleToCopy)
        {
            eVehicleTypes vehicleType = (eVehicleTypes)i_VehicleToCopy.VehicleType;
            if (vehicleType == eVehicleTypes.FuelMotorcycle)
            {
                io_Vehicle = new Motorcycle(i_VehicleToCopy, !m_IsElectric, Motorcycle.NumOfWheels,
                           i_CurrentAirPressure, Motorcycle.MaxAirPressure, i_ManufacturerName,
                           i_CurrentEnergy, Motorcycle.MaxFuelCapacity, Garage.Elements.eFuelTypes.Octane96);
            }
            else if (vehicleType == eVehicleTypes.ElectricMotorcycle)
            {
                io_Vehicle = new Motorcycle(i_VehicleToCopy, m_IsElectric, Motorcycle.NumOfWheels,
                           i_CurrentAirPressure, Motorcycle.MaxAirPressure, i_ManufacturerName,
                           i_CurrentEnergy, Motorcycle.MaxBatteryCapacity, Garage.Elements.eFuelTypes.Octane96);
            }
            else if (vehicleType == eVehicleTypes.FuelCar)
            {
                io_Vehicle = new Car(i_VehicleToCopy, !m_IsElectric, Car.NumOfWheels,
                   i_CurrentAirPressure, Car.MaxAirPressure, i_ManufacturerName,
                   i_CurrentEnergy, Car.MaxFuelCapacity, Garage.Elements.eFuelTypes.Octane95);
            }
            else if (vehicleType == eVehicleTypes.ElectricCar)
            {
                io_Vehicle = new Car(i_VehicleToCopy, !m_IsElectric, Car.NumOfWheels,
                   i_CurrentAirPressure, Car.MaxAirPressure, i_ManufacturerName,
                   i_CurrentEnergy, Car.MaxBatteryCapacity, Garage.Elements.eFuelTypes.Octane95);
            }
            else if (vehicleType == eVehicleTypes.Truck)
            {
                io_Vehicle = new Truck(i_VehicleToCopy, !m_IsElectric, Truck.NumOfWheels,
                   i_CurrentAirPressure, Truck.MaxAirPressure, i_ManufacturerName,
                   i_CurrentEnergy, Truck.MaxFuelCapacity, Garage.Elements.eFuelTypes.Soler);
            }
        }
    }
}
