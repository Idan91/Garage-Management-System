using System;

namespace Ex03.GarageLogic
{
    internal class Energy
    {
        private float m_CurrentAmount;
        private float m_Capacity;
        private const int k_MinimumAmount = 0;// define

        public float EnergyStatus
        {
            get
            {
                return (m_CurrentAmount / m_Capacity) * 100; /// 0 - 100 %
            }
        }

        public float Capacity
        {
            get
            {
                return m_Capacity;
            }
            set
            {
                m_Capacity = value;
            }
        }

        public float CurrentAmount
        {
            get
            {
                return m_CurrentAmount;
            }
            set
            {
                m_CurrentAmount = value;
            }
        }

        public virtual bool FillEnergy(float i_EnergyToFill,
            Garage.Elements.eFuelTypes i_FuelTypes = Garage.Elements.eFuelTypes.None)
        {
            bool v_EnergyAdded = false;
            if(checkIfInRange(i_EnergyToFill))
            {
                v_EnergyAdded = true; // if exception wasn't thrown
            }
            else
            {
                throw new ValueOutOfRangeException(new Exception(), k_MinimumAmount, m_Capacity);// out of range
            }
            return v_EnergyAdded;
        }

        private bool checkIfInRange(float i_EnergyToFill)
        {
            bool v_EnergyAdded = false;
            if (m_CurrentAmount + i_EnergyToFill <= m_Capacity)
            {
                m_CurrentAmount += i_EnergyToFill;
                v_EnergyAdded = true; // if exception wasn't thrown
            }
            return v_EnergyAdded;
        }
    }
}
