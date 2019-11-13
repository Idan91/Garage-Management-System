using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B19_Ex03
{
    public class Temp_GarageLogic
    {

        public static int m_TotalNumOfVehicles = 0;
        public int m_NumOfVehicleTypes = Enum.GetNames(typeof(eVehicleTypes)).Length;
        
        public enum eVehicleTypes
        {
            Motorcylce_Fuel = 1,
            Motorcycle_Electric,
            Car_Fuel,
            Car_Electric,
            Truck
        }

        public enum eStatus
        {
            InProgress = 1,
            Repaired,
            Paid
        }

        public Temp_GarageLogic()
        {

        }

   
        public class Vehicle
        {
            public int m_SerialNum;
            public string m_LicensePlateNum;
            public eStatus m_Status;
            public eVehicleTypes m_VehicleType;
            public string m_OwnerPhoneNumber;
            public string m_OwnerName;

            public Vehicle(string i_plateNum)
            {
                m_LicensePlateNum = i_plateNum;
                m_Status = (eStatus)1;
                m_SerialNum = ++m_TotalNumOfVehicles;
                m_OwnerPhoneNumber = "-";
                m_OwnerName = "-";

            }

        }

        public List<Vehicle> vehicleList = new List<Vehicle>();

        public void AddVehicleToList(string i_plateNum)
        {
            Vehicle v = new Vehicle(i_plateNum);
            vehicleList.Add(v);
        }
    }
}
