using System;
using System.Collections.Generic;

namespace Ex03.GarageLogic
{
    public class Garage
    {
        #region Fields
        private List<VehicleInGarage> m_GarageStorageVehicles = new List<VehicleInGarage>();
        #endregion

        #region Methods
        public bool AddVehicleToGarage(string i_OwnerName, string i_OwnerPhoneNumber, int i_VehicleType,
            string i_VehicleModel, string i_LicensePlateNum, string i_ManufacturerName,
            float i_CurrentAirPressure, float i_CurrentEnergy)
        {
            bool v_AddedSuccessfully = false;
            if (vehicleInputValidation(i_VehicleType, i_CurrentAirPressure, i_CurrentEnergy))
            {
                Vehicle vehicleToAdd = new Vehicle(i_VehicleModel, i_LicensePlateNum,
                    (GenerateNewVehicle.eVehicleTypes)i_VehicleType);
                m_GarageStorageVehicles.Add(new VehicleInGarage(i_OwnerName, i_OwnerPhoneNumber,
                    i_CurrentEnergy, i_ManufacturerName, i_CurrentAirPressure, vehicleToAdd));
                v_AddedSuccessfully = true;
            }
            return v_AddedSuccessfully;
        }

        public int TotalNumOfVehicles
        {
            get
            {
                return m_GarageStorageVehicles.Count;
            }
        }
        
        public bool Refuel(string i_LicensePlateNum, float i_LitersToAdd, Elements.eFuelTypes i_FuelType)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                v_Successfully = m_GarageStorageVehicles[vehicleIndex].Refuel(i_LitersToAdd, i_FuelType);
            }
            return v_Successfully;
        }

        public bool Recharge(string i_LicensePlateNum, float i_HoursToCharge)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                v_Successfully = m_GarageStorageVehicles[vehicleIndex].Recharge(i_HoursToCharge);
            }
            return v_Successfully;
        }

        public bool SetVehicleStatus(string i_LicensePlateNum, Elements.eVehicleStatus i_VehicleStatus)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                v_Successfully = true;
                m_GarageStorageVehicles[vehicleIndex].VehicleInGarageStatus = i_VehicleStatus;
            }
            return v_Successfully;
        }

        public bool InflateTiresToMax(string i_LicensePlateNum)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                v_Successfully = true;
                m_GarageStorageVehicles[vehicleIndex].Vehicle.InflateToMax();
            }
            return v_Successfully;
        }

        public string GetOwnerName(string i_LicensePlateNum)
        {
            string ownerName = string.Empty;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                ownerName = m_GarageStorageVehicles[vehicleIndex].OwnerName;
            }
            return ownerName;
        }

        public int GetVehicleType(string i_LicensePlateNum)
        {
            int vehicleType = 0; // default
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleType = (int)m_GarageStorageVehicles[vehicleIndex].Vehicle.VehicleType;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleType;
        }

        public string GetOwnerPhoneNumber(string i_LicensePlateNum)
        {
            string ownerPhoneNumber = string.Empty;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                ownerPhoneNumber = m_GarageStorageVehicles[vehicleIndex].OwnerPhoneNumber;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return ownerPhoneNumber;
        }

        public string GetVehicleTireManufacturer(string i_LicensePlateNum)
        {
            string vehicleTireManufacturer = string.Empty;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleTireManufacturer = m_GarageStorageVehicles[vehicleIndex].Vehicle.TireManufacturer;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleTireManufacturer;
        }

        public string GetVehicleModel(string i_LicensePlateNum)
        {
            string getVehicleModel = string.Empty;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                getVehicleModel = m_GarageStorageVehicles[vehicleIndex].Vehicle.VehicleModel;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return getVehicleModel;
        }

        public float GetVehicleMaxEnergyCapacity(string i_LicensePlateNum)
        {
            float vehicleMaxEnergyCapacity = 0;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleMaxEnergyCapacity = m_GarageStorageVehicles[vehicleIndex].Vehicle.EnergyCapacity;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleMaxEnergyCapacity;
        }

        public float GetVehicleMaxAirPressureCapacity(string i_LicensePlateNum)
        {
            float vehicleMaxAirPressureCapacity = 0;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleMaxAirPressureCapacity = m_GarageStorageVehicles[vehicleIndex].Vehicle.AirPressureCapacity;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleMaxAirPressureCapacity;
        }

        public float GetVehicleCurrentAirPressure(string i_LicensePlateNum)
        {
            float vehicleCurrentAirPressure = 0;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleCurrentAirPressure = m_GarageStorageVehicles[vehicleIndex].Vehicle.AirPressureStatus;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleCurrentAirPressure;
        }

        public Elements.eFuelTypes GetVehicleFuelType(string i_LicensePlateNum)
        {
            Elements.eFuelTypes vehicleFuelType = Elements.eFuelTypes.None;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleFuelType = m_GarageStorageVehicles[vehicleIndex].Vehicle.FuelType;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleFuelType;
        }

        public Elements.eVehicleStatus GetVehicleStatus(string i_LicensePlateNum)
        {
            Elements.eVehicleStatus vehicleStatus = Elements.eVehicleStatus.InProgress;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                vehicleStatus = m_GarageStorageVehicles[vehicleIndex].VehicleInGarageStatus;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return vehicleStatus;
        }

        public float GetEnergyStatus(string i_LicensePlateNum)
        {
            float energyStatus = 0;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                energyStatus = m_GarageStorageVehicles[vehicleIndex].Vehicle.EnergyStatus;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return energyStatus;
        }

        public bool SetCarColor(string i_LicensePlateNum, Elements.eCarColors i_ColorToPaint)
        {
            bool v_Successfully = false;
            bool v_ColorInRange = (Elements.eCarColors.Red <= i_ColorToPaint
                && i_ColorToPaint <= Elements.eCarColors.Gray);
            if (v_ColorInRange)
            {
                int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
                if (vehicleIndex != -1)
                {
                    if(m_GarageStorageVehicles[vehicleIndex].Vehicle is Car)
                    {
                        v_Successfully = true;
                        ((Car)m_GarageStorageVehicles[vehicleIndex].Vehicle).CarColor = i_ColorToPaint;
                    }
                }
                else
                {
                    throwExceptionVehicleNotFound();
                }
            }
            return v_Successfully;
        }

        public Elements.eCarColors GetCarColor(string i_LicensePlateNum)
        {
            Elements.eCarColors carColor = Elements.eCarColors.Red;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Car)
                {
                    carColor = ((Car)m_GarageStorageVehicles[vehicleIndex].Vehicle).CarColor;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return carColor;
        }

        public bool SetCarNumberOfDoors(string i_LicensePlateNum, Garage.Elements.eCarDoors i_NumberOfDoors)
        {
            bool v_Successfully = false;
            bool v_NumberOfDoorsInRange = (Garage.Elements.eCarDoors.Two <= i_NumberOfDoors &&
                i_NumberOfDoors <= Garage.Elements.eCarDoors.Five);
            if (v_NumberOfDoorsInRange)
            {
                int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
                if (vehicleIndex != -1)
                {
                    if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Car)
                    {
                        v_Successfully = true;
                        ((Car)m_GarageStorageVehicles[vehicleIndex].Vehicle).NumberOfDoors = i_NumberOfDoors;
                    }
                }
                else
                {
                    throwExceptionVehicleNotFound();
                }
            }
            return v_Successfully;
        }

        public Garage.Elements.eCarDoors GetCarNumberOfDoors(string i_LicensePlateNum)
        {
            Garage.Elements.eCarDoors numberOfDoors = Garage.Elements.eCarDoors.Two;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Car)
                {
                    numberOfDoors = ((Car)m_GarageStorageVehicles[vehicleIndex].Vehicle).NumberOfDoors;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return numberOfDoors;
        }

        public bool SetMotorcycleLicenseType(string i_LicensePlateNum, Elements.eMotorcycleLicenseType i_LicenseType)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Motorcycle)
                {
                    v_Successfully = true;
                    ((Motorcycle)m_GarageStorageVehicles[vehicleIndex].Vehicle).LicenseType = i_LicenseType;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return v_Successfully;
        }

        public Elements.eMotorcycleLicenseType GetMotorcycleLicenseType(string i_LicensePlateNum)
        {
            Elements.eMotorcycleLicenseType licenseType = Elements.eMotorcycleLicenseType.A;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Motorcycle)
                {
                    licenseType = ((Motorcycle)m_GarageStorageVehicles[vehicleIndex].Vehicle).LicenseType;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return licenseType;
        }

        public bool SetMotorcycleEngineVolume(string i_LicensePlateNum, int i_EngineVolume)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Motorcycle)
                {
                    v_Successfully = true;
                    ((Motorcycle)m_GarageStorageVehicles[vehicleIndex].Vehicle).EngineVolume = i_EngineVolume;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return v_Successfully;
        }

        public bool GetCarriesHazardousMaterials(string i_LicensePlateNum)
        {
            bool v_CarriesHazardousMaterials = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Truck)
                {
                    v_CarriesHazardousMaterials = ((Truck)m_GarageStorageVehicles[vehicleIndex].Vehicle).CarriesHazardousMaterials;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return v_CarriesHazardousMaterials;
        }

        public bool SetCarriesHazardousMaterials(string i_LicensePlateNum, bool i_CarriesHazardousMaterials)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Truck)
                {
                    v_Successfully = true;
                    ((Truck)m_GarageStorageVehicles[vehicleIndex].Vehicle).CarriesHazardousMaterials =
                        i_CarriesHazardousMaterials;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return v_Successfully;
        }

        public float GetTruckCargoVolume(string i_LicensePlateNum)
        {
            float cargoVolume = 0;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Truck)
                {
                    cargoVolume = ((Truck)m_GarageStorageVehicles[vehicleIndex].Vehicle).CargoVolume;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return cargoVolume;
        }

        public bool SetTruckCargoVolume(string i_LicensePlateNum, float i_CargoVolume)
        {
            bool v_Successfully = false;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Truck)
                {
                    v_Successfully = true;
                    ((Truck)m_GarageStorageVehicles[vehicleIndex].Vehicle).CargoVolume = i_CargoVolume;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return v_Successfully;
        }

        public int GetMotorcycleEngineVolume(string i_LicensePlateNum)
        {
            int engineVolume = -1;
            int vehicleIndex = GetVehicleIndex(i_LicensePlateNum);
            if (vehicleIndex != -1)
            {
                if (m_GarageStorageVehicles[vehicleIndex].Vehicle is Motorcycle)
                {
                    engineVolume = ((Motorcycle)m_GarageStorageVehicles[vehicleIndex].Vehicle).EngineVolume;
                }
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return engineVolume;
        }

        public string GetVehicleLicensePlateNum(int i_Index)
        {
            string licenseNumber = string.Empty;
            if (m_GarageStorageVehicles[i_Index - 1] != null)
            {
                licenseNumber = m_GarageStorageVehicles[i_Index - 1].Vehicle.LicensePlateNum;
            }
            else
            {
                throwExceptionVehicleNotFound();
            }
            return licenseNumber;
        }

        public List<string> GetAllVehiclePlateNumbers()
        {
            List<string> allVehiclesPlates = new List<string>();
            foreach (VehicleInGarage vehicles in m_GarageStorageVehicles)
            {
                allVehiclesPlates.Add(vehicles.Vehicle.LicensePlateNum);
            }
            return allVehiclesPlates;
        }

        public List<string> GetSupportedVehicleTypes()
        {
            List<string> allVehiclesTypes = new List<string>();
            foreach (GenerateNewVehicle.eVehicleTypes types in Enum.GetValues(typeof(GenerateNewVehicle.eVehicleTypes)))
            {
                allVehiclesTypes.Add(types.ToString());
            }
            return allVehiclesTypes;
        }

        public int GetVehicleIndex(string i_LicensePlateNum)
        {
            VehicleInGarage toLookFor = new VehicleInGarage();
            toLookFor.Vehicle = new Vehicle(i_LicensePlateNum);
            int vehicleIndex = m_GarageStorageVehicles.IndexOf(toLookFor);
            return vehicleIndex;
        }

        // Used to allow input validation using the max value. Does not hurt the "generic" character of the system
        public float GetVehicleMaxEnergyCapacity(int i_VehicleType) 
        {
            float maxEnergyCapacity = 0;
            if ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.FuelMotorcycle)
            {
                maxEnergyCapacity = Motorcycle.MaxFuelCapacity;
            }
            else if ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.ElectricMotorcycle)
            {
                maxEnergyCapacity = Motorcycle.MaxBatteryCapacity;
            }
            else if ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.FuelCar)
            {
                maxEnergyCapacity = Car.MaxFuelCapacity;
            }
            else if ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.ElectricCar)
            {
                maxEnergyCapacity = Car.MaxBatteryCapacity;
            }
            else if ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.Truck)
            {
                maxEnergyCapacity = Truck.MaxFuelCapacity;
            }
            return maxEnergyCapacity;
        }

        // Used to allow input validation using the max value. Does not hurt the "generic" character of the system
        public float GetVehicleMaxAirPressureCapacity(int i_VehicleType)
        {
            float maxAirPressure = 0;
            bool v_IsMotorcycle = ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.FuelMotorcycle ||
                (GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.ElectricMotorcycle);
            bool v_IsCar = ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.FuelCar ||
                (GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.ElectricCar);
            bool v_IsTruck = ((GenerateNewVehicle.eVehicleTypes)i_VehicleType == GenerateNewVehicle.eVehicleTypes.Truck);
            if (v_IsMotorcycle)
            {
                maxAirPressure = Motorcycle.MaxAirPressure;
            }
            else if (v_IsCar)
            {
                maxAirPressure = Car.MaxAirPressure;
            }
            else if (v_IsTruck)
            {
                maxAirPressure = Truck.MaxAirPressure;
            }
            return maxAirPressure;
        }

        private bool vehicleInputValidation(int i_VehicleType, float i_AirPressureStatus, float i_EnergyStatus)
        {
            bool v_ValuesInRange = false;
            float vehicleMaxEnergyCapacity = GetVehicleMaxEnergyCapacity(i_VehicleType);
            float vehicleMaxAirPressure = GetVehicleMaxAirPressureCapacity(i_VehicleType);
            if (i_EnergyStatus >= 0 && i_EnergyStatus <= vehicleMaxEnergyCapacity)
            {
                if (i_AirPressureStatus >= 0 && i_AirPressureStatus <= vehicleMaxAirPressure)
                {
                    v_ValuesInRange = true;
                }
            }
            return v_ValuesInRange;
        }

        // throw Exception whenever the desired vehicle license plate number isn't in the collection
        private void throwExceptionVehicleNotFound()
        {
            throw new ArgumentException("Vehicle is not found in the garage!");
        }
        #endregion

        // The Elements region includes enums that would sync with all parts of the system
        // The objects in the logic portion that use these Enums are not revealed to the UI portion of the system.
        // The Vehicle Types Enum DOES NOT appear here and is not accessible to the user in order to maintain the generic ability to add vehicles and more vehicle types in the future.
        #region Elements
        public class Elements
        {
            public enum eVehicleStatus : byte
            {
                InProgress = 1,
                Repaired,
                Paid
            }

            public enum eCarColors : byte
            {
                Red = 1,
                Blue,
                Black,
                Gray
            }

            public enum eCarDoors : byte
            {
                Two = 2,
                Three,
                Four,
                Five
            }

            public enum eMotorcycleLicenseType : byte
            {
                A = 1,
                A1,
                A2,
                B
            }

            public enum eFuelTypes : byte
            {
                None = 1,
                Octane95,
                Octane96,
                Octane98,
                Soler,
            }
        }
        #endregion
    }
}



