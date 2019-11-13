using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class Vehicle
    {
        #region Members
        private string m_VehicleModel;
        private readonly string m_LicensePlateNum;
        private bool m_IsElectricVehicle = false;
        GenerateNewVehicle.eVehicleTypes m_VehicleType;
        private Energy m_EngineEnergy;
        private Tire[] m_Tires;
        
        #endregion

        #region Constructors
        public Vehicle(string i_VehicleModel, string i_LicensePlateNum, GenerateNewVehicle.eVehicleTypes eVehiclesTypes)
        {    
            m_VehicleModel = i_VehicleModel;
            m_LicensePlateNum = i_LicensePlateNum;
            m_VehicleType = eVehiclesTypes;
        }

        public Vehicle(Vehicle i_Vehicle)
        {
            m_VehicleModel = i_Vehicle.m_VehicleModel;
            m_LicensePlateNum = i_Vehicle.m_LicensePlateNum;
            m_VehicleType = i_Vehicle.VehicleType;
        }
        #endregion

        #region Properties
        public void SetTires(int i_NumberOfWheels, float i_CurrentAirPressure,
            float i_MaxWheelAirPressure, string i_ManufacturerName)
        {
            m_Tires = new Tire[i_NumberOfWheels];
            for (int i = 0; i < i_NumberOfWheels; i++)
            {
                m_Tires[i] = new Tire(i_ManufacturerName, i_CurrentAirPressure, i_MaxWheelAirPressure);
            }
        }

        public void SetEngineType(bool i_IsElectricVehicle, float i_CurrentEnergy,
            float i_MaxEnergyCapacity, Garage.Elements.eFuelTypes i_FuelType = Garage.Elements.eFuelTypes.None)
        {
            if (i_IsElectricVehicle)
            {
                IsElectricVehicle = i_IsElectricVehicle;
                m_EngineEnergy = new ElectricProperties(i_CurrentEnergy, i_MaxEnergyCapacity);
            }
            else
            {
                m_EngineEnergy = new FuelProperties(i_CurrentEnergy, i_MaxEnergyCapacity, i_FuelType);
            }
        }

        public Vehicle(string i_LicensePlateNum)
        {
            m_LicensePlateNum = i_LicensePlateNum;
        }

        public bool IsElectricVehicle
        {
            get
            {
                return m_IsElectricVehicle;
            }
            set
            {
                m_IsElectricVehicle = value;
            }
        }

        public string VehicleModel
        {
            get
            {
                return m_VehicleModel;
            }
            set
            {
                m_VehicleModel = value;
            }
        }

        public string LicensePlateNum
        {
            get
            {
                return m_LicensePlateNum;
            }
        }

        public float EnergyStatus
        {
            get
            {
                return m_EngineEnergy.EnergyStatus; 
            }
        }

        public GenerateNewVehicle.eVehicleTypes VehicleType
        {
            get
            {
                return m_VehicleType;
            }
        }

        public string TireManufacturer
        {
            get
            {
                return m_Tires[0].Manufacturer;
            }
        }
        #endregion

        #region Methods
        public bool FillEnergy(float i_EnergyToFill, Garage.Elements.eFuelTypes i_FuelType = Garage.Elements.eFuelTypes.None)
        {
            bool v_Successfully = false;
            v_Successfully = m_EngineEnergy.FillEnergy(i_EnergyToFill, i_FuelType);
            return v_Successfully;
        }

        public void InflateToMax()
        {
            foreach(Tire tireToInflat in m_Tires)
            {
                tireToInflat.MaxAirPressure();
            }
        }

        public float EnergyCapacity
        {
            get
            {
                return m_EngineEnergy.Capacity;
            }
        }
        
        public float AirPressureCapacity
        {
            get
            {
                return m_Tires[0].MaximumAirPressure;
            }
        }

        public float AirPressureStatus
        {
            get
            {
                return m_Tires[0].CurrentAirPressure;
            }
        }

        public Garage.Elements.eFuelTypes FuelType
        {
            get
            {
                Garage.Elements.eFuelTypes fuelType = Garage.Elements.eFuelTypes.None;
                if (m_EngineEnergy is FuelProperties)
                {
                    fuelType = ((FuelProperties)m_EngineEnergy).FuelType;
                }
                return fuelType;
            }
        }
        #endregion

        #region Wheels
        private class Tire
        {
            private string m_ManufacturerName;
            private float m_MaximumAirPressure;
            private float m_CurrentAirPressure;

            public Tire(string i_ManufacturerName, float i_CurrentAirPressure, float i_MaximumAirPressure)
            {
                m_ManufacturerName = i_ManufacturerName;
                m_CurrentAirPressure = i_CurrentAirPressure;
                m_MaximumAirPressure = i_MaximumAirPressure;
            }

            public void MaxAirPressure()
            {
                m_CurrentAirPressure = m_MaximumAirPressure;
            }

            public string Manufacturer
            {
                get
                {
                    return m_ManufacturerName;
                }
            }

            public float MaximumAirPressure
            {
                get
                {
                    return m_MaximumAirPressure;
                }
            }

            public float CurrentAirPressure
            {
                get
                {
                    return m_CurrentAirPressure;
                }
            }
        }
        #endregion
    }
}
