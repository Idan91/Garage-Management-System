using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    internal class VehicleInGarage
    {
        #region Members
        private Vehicle m_Vehicle;
        private string m_OwnerName;
        private string m_OwnerPhoneNumber;
        private Garage.Elements.eVehicleStatus m_VehicleInGarageStatus;
        
        #endregion

        #region Constructors
        public VehicleInGarage(string i_OwnerName, string i_OwnerPhoneNumber,
            float i_CurrentEnergy, string i_ManufacturerName, float i_CurrentAirPressure, Vehicle i_VehicleToCopy)
        {
            m_OwnerName = i_OwnerName;
            m_OwnerPhoneNumber = i_OwnerPhoneNumber;
            m_VehicleInGarageStatus = Garage.Elements.eVehicleStatus.InProgress;
            GenerateNewVehicle.CreateVehicle(ref m_Vehicle, i_CurrentEnergy,
                i_ManufacturerName, i_CurrentAirPressure, i_VehicleToCopy);
        }
        public VehicleInGarage()
        {

        }
        #endregion

        #region Properties
        public Vehicle Vehicle
        {
            get
            {
                return m_Vehicle;
            }
            set
            {
                m_Vehicle = value;
            }
        }

        public string OwnerName
        {
            get
            {
                ;
                return m_OwnerName;
            }
            set
            {
                m_OwnerName = value;
            }
        }

        public string OwnerPhoneNumber
        {
            get
            {
                ;
                return m_OwnerPhoneNumber;
            }
            set
            {
                m_OwnerPhoneNumber = value;
            }
        }

        public Garage.Elements.eVehicleStatus VehicleInGarageStatus
        {
            get
            {
                ;
                return m_VehicleInGarageStatus;
            }
            set
            {
                m_VehicleInGarageStatus = value;
            }
        }
        #endregion

        #region Methods
        public bool Refuel(float i_AmountToFuel, Garage.Elements.eFuelTypes i_FuelType)
        {
            bool v_Successfully = false;
            if (!m_Vehicle.IsElectricVehicle)
            {
                v_Successfully = m_Vehicle.FillEnergy(i_AmountToFuel, i_FuelType);
            }
            return v_Successfully;
        }

        public bool Recharge(float i_AmountToCharge)
        {
            bool v_Successfully = false;
            if (m_Vehicle.IsElectricVehicle)
            {
                v_Successfully = m_Vehicle.FillEnergy(i_AmountToCharge);
            }
            return v_Successfully;
        }

        public override bool Equals(object obj)
        {
            bool v_IsEquals = false;
            VehicleInGarage ToCompareTo = obj as VehicleInGarage;
            if (ToCompareTo != null)
            {
                v_IsEquals = this.m_Vehicle.LicensePlateNum == ToCompareTo.m_Vehicle.LicensePlateNum;
            }
            return v_IsEquals;
        }

        public override int GetHashCode()
        {
            return this.m_Vehicle.LicensePlateNum.GetHashCode();
        }
        #endregion
    }
}