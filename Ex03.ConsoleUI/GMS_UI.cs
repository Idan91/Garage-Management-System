using System;
using System.Collections.Generic;
using System.Text;
using Ex03.GarageLogic;

namespace B19_Ex03
{
    public class GMS_UI
    {
        #region Members
        private static Garage m_Garage = new Garage();
        
        enum ePages :byte
        {
            Home = (int)'a', //Main menu page
            VehicleRegister, //Main menu page
            Help, //Main menu page - Main Menu cutoff value
            AddVehicle, //SubMenu page
            VehicleOperations, //SubMenu page
        }

        private ePages m_CurrentPage;
        private int m_NumOfPages = Enum.GetNames(typeof(ePages)).Length;
        private int m_FirstPageValue = (int)ePages.Home;
        private string m_GarageName;
        private bool v_FilterStatus = false;
        private readonly int m_NoFilter = 0;
        private readonly string m_PositiveAnswer = "Y";
        private readonly string m_NegativeAnswer = "N";
        private readonly char m_QuitChar = 'Q';
        private List<string> m_VehicleTypes = m_Garage.GetSupportedVehicleTypes();
        private int m_NumOfVehicleTypes = m_Garage.GetSupportedVehicleTypes().Count;
        #endregion

        #region Initialization
        public GMS_UI()
        {
            printHeader();
            initialStartup();
            m_CurrentPage = ePages.Home;
            switchPage(m_CurrentPage);
        }

        private void initialStartup()
        {
            string welcome = String.Format(@"Welcome to GMS!
To set up your garage manager, please enter your garage name: ");
            Console.Write(welcome);
            m_GarageName = stringInput();
            Console.WriteLine("Welcome to " + m_GarageName + "!" + Environment.NewLine);
        }
        #endregion

        #region General UI Elements

        private void clearAndPrintTopOfPage()
        {
            Console.Clear();
            printHeader();
        }

        private void switchPage(ePages page) 
        {
            m_CurrentPage = page;
            v_FilterStatus = false;

            switch (page)
            {
                case ePages.Home:
                    clearAndPrintTopOfPage();
                    printMainMenu();
                    homePage();
                    break;
                case ePages.VehicleRegister:
                    clearAndPrintTopOfPage();
                    printMainMenu();
                    vehicleRegisterPage();
                    break;
                case ePages.Help:
                    clearAndPrintTopOfPage();
                    printMainMenu();
                    helpPage();
                    break;
                case ePages.AddVehicle:
                    clearAndPrintTopOfPage();
                    addVehiclePage();
                    break;
                case ePages.VehicleOperations:
                    vehicleOperations();
                    break;
                default:
                    invalidInputMsg();
                    mainUserInput();
                    break;
            }
        }
       
        private void printHeader()
        {
            Console.WriteLine("GMS - Garage Management System" + Environment.NewLine);
        }

        private void printMainMenu()
        {
            int separatorPadding = 0;

            foreach(ePages page in Enum.GetValues(typeof(ePages)))
            {
                ePages p = page;
                Console.Write("{0}. {1}     ", (char)page, UIUtils.EnumDisplayName(page));
                separatorPadding += 7 + page.ToString().Length;
                if(p.Equals(ePages.Help)) //Main Menu end
                {
                    break;
                }
            }
            Console.Write(Environment.NewLine);
            for(int i = 0; i <= separatorPadding; i++)
            {
                Console.Write("=");
            }
            Console.Write(Environment.NewLine);

        }

        private void invalidInputMsg()
        {
            Console.WriteLine("Invalid Input! Please try again: ");
        }

        private void mainUserInputRequestMsg()
        {
            Console.Write("! Enter the corresponding menu character to proceed (enter Q to quit): ");
        }

        private void mainUserInput()
        {
            Console.WriteLine(Environment.NewLine);
            mainUserInputRequestMsg();
            string input = "";
            int inputINT = 0;
            try
            {
                input = stringInput();
            }
            catch (FormatException)
            {
                invalidInputMsg();
                mainUserInput();
            }
            try
            {
                inputINT = (int)input[0];
            }
            catch(Exception e)
            {
                if(e is IndexOutOfRangeException || e is FormatException)
                {
                    invalidInputMsg();
                    mainUserInput();
                }
            }

            if(input == m_QuitChar.ToString() || input == (char.ToLower(m_QuitChar)).ToString()) //Quit input
            {
                quitProgram();
            }
            if (m_CurrentPage != ePages.VehicleRegister) //Sub-menu inaccessible when not present
            {
                while(inputINT > (int)ePages.Help && inputINT <= m_FirstPageValue + m_NumOfPages - 1)
                {
                    invalidInputMsg();
                    mainUserInput();
                }
                ePages page = (ePages)inputINT;
                switchPage(page);
            }
            else if(m_CurrentPage == ePages.VehicleRegister) //Allow sub-menu access + status filtering
            {
                if (inputINT < m_FirstPageValue || inputINT >= m_FirstPageValue + m_NumOfPages)
                {
                    inputINT = int.Parse(input[0].ToString());
                    if(inputINT < (int)Garage.Elements.eVehicleStatus.InProgress || inputINT > (int)Garage.Elements.eVehicleStatus.Paid)
                    {
                        invalidInputMsg();
                        mainUserInput();
                    }
                    else
                    {
                        if (!v_FilterStatus)
                        {
                            filterBy(inputINT);
                        }
                        else
                        {
                            while (inputINT != 1)
                            {
                                invalidInputMsg();
                                mainUserInput();
                            }
                            switchPage(ePages.VehicleRegister);
                        }
                    }                   
                }
                else
                {
                    ePages page = (ePages)inputINT;
                    switchPage(page);
                }
            }
        }

        private void quitProgram()
        {
            Console.WriteLine(string.Format(
@"Thank you for using GMS!
Quitting program..."));
            Environment.Exit(1);
        }
        #endregion

        #region Home Page 
        //The Home page is your first main page of the system. It displays a general overview of your current garage status
        private void homePage()
        {
            Console.WriteLine("Welcome to the " + m_GarageName + " garage manager!" + Environment.NewLine);
            int[] typeCount = vehicleTypeCounter();

            //OVERVIEW
            Console.WriteLine("Overview:" + Environment.NewLine);
            if (m_Garage.TotalNumOfVehicles == 0)
            {
                Console.WriteLine("There are currently 0 vehicles in your garage.");
            }
            else if(m_Garage.TotalNumOfVehicles == 1)
            {
                Console.WriteLine(string.Format(
@"There is currently 1 vehicle in your garage.
This vehicle's type is: " + UIUtils.EnumDisplayName(m_VehicleTypes[(m_Garage.GetVehicleType(m_Garage.GetAllVehiclePlateNumbers()[0]))-1]) ));
            }
            else
            {
            string[] argOverview = new string[(m_NumOfVehicleTypes)+3];
            argOverview[0] = m_Garage.TotalNumOfVehicles.ToString();
            argOverview[1] = (typeCount[0] + typeCount[1]).ToString(); //Total # of motorcycles
            argOverview[2] = typeCount[0].ToString(); //Fuel motorcycle
            argOverview[3] = typeCount[1].ToString(); //Electric motorcycle
            argOverview[4] = (typeCount[2] + typeCount[3]).ToString(); //Total # of motorcycles
            argOverview[5] = typeCount[2].ToString(); //Fuel car
            argOverview[6] = typeCount[3].ToString(); //Electric car
            argOverview[7] = typeCount[4].ToString(); //Trucks


            Console.Write(string.Format(
@"In your garage, there are currently {0} vehicles of which there are:
- {1} Motorcycles ({2} fuel-powered, {3} electric)
- {4} Cars ({5} fuel-powered, {6} electric)
- {7} Trucks
", argOverview));
            }

            mainUserInput();
        
        }

        private int[] vehicleTypeCounter()
        {
            int[] vehicleTypeCount = new int[m_NumOfVehicleTypes];
            List<string> allVehicles = m_Garage.GetAllVehiclePlateNumbers();

            for(int i = 0; i < m_NumOfVehicleTypes; i++)
            {
                for(int j = 0; j < m_Garage.TotalNumOfVehicles; j++)
                {
                    if(m_Garage.GetVehicleType(allVehicles[j]) == i+1)
                    {
                        vehicleTypeCount[i]++;
                    }
                }
            }

            return vehicleTypeCount;
        }

        #endregion

        #region Vehicle Register Page
        //The Vehicle Register page displays all the vehicles in the garage.
        //You can filter based on the current status and proceed to add vehicle or perform action on a selected vehicle
        private void vehicleRegisterPage()
        {
            printRegisterSubMenu();
            Console.Write(Environment.NewLine);
            Console.WriteLine(UIUtils.EnumDisplayName(ePages.VehicleRegister) + ":" + Environment.NewLine);

            Console.Write("Filter by status: ");
            for (int i = 1; i <= Enum.GetNames(typeof(Garage.Elements.eVehicleStatus)).Length; i++)
            {
                Garage.Elements.eVehicleStatus status = (Garage.Elements.eVehicleStatus)i;
                if (i != 1)
                {
                    Console.Write("  ");
                }
                Console.Write(i + ". " + UIUtils.EnumDisplayName(status));
            }
            Console.WriteLine(Environment.NewLine);

            printRegisterTable(m_NoFilter);
            mainUserInput();
        }

        private void printRegisterSubMenu()
        {
            int separatorPadding = 0;

            foreach (ePages page in Enum.GetValues(typeof(ePages)))
            {
                int p = (int)page;
                if (p <= (int)ePages.Help) //Main Menu end
                {
                    continue;
                }
                Console.Write("{0}. {1}     ", (char)page, UIUtils.EnumDisplayName(page));
                separatorPadding += 7 + page.ToString().Length;
            }
            Console.Write(Environment.NewLine);
            for (int i = 0; i <= separatorPadding; i++)
            {
                Console.Write("-");
            }
            Console.Write(Environment.NewLine);
        }

        enum eRegisterTableFields : byte
        {
            Num = 0,
            LicensePlateNum,
            Status,
            VehicleType,
            Model,
            OwnerPhone,
            OwnerName
        }

        private void printRegisterTable(int i_Filter)
        {
            int[] padding = new int[Enum.GetNames(typeof(eRegisterTableFields)).Length] ;
            printRegisterTableHeaders(ref padding);
            printRegisterTableAttributes(i_Filter,padding);

        }
        private void printRegisterTableHeaders(ref int[] io_Padding)
        {
            int separatorPadding = 0;
            int vehicleTypeFieldPadding = 21;
            int modelFieldPadding = 15;

            for(int i = 0; i < Enum.GetNames(typeof(eRegisterTableFields)).Length ; i++)
            {
                eRegisterTableFields field = (eRegisterTableFields)i;
                string fieldTitle = UIUtils.EnumDisplayName(field);
                if (field == eRegisterTableFields.VehicleType)
                {
                    Console.Write("| " + UIUtils.PadBothSides(fieldTitle, vehicleTypeFieldPadding) + " ");
                    separatorPadding += 3 + vehicleTypeFieldPadding;
                    io_Padding[i] = vehicleTypeFieldPadding;
                }
                else if (field == eRegisterTableFields.Model)
                {
                    Console.Write("| " + UIUtils.PadBothSides(fieldTitle, modelFieldPadding) + " ");
                    separatorPadding += 3 + modelFieldPadding;
                    io_Padding[i] = modelFieldPadding;
                }
                else if(field != eRegisterTableFields.Num && fieldTitle.Length < 13)
                {
                    Console.Write("| " + UIUtils.PadBothSides(fieldTitle,13) + " ");
                    separatorPadding += 3 + 13;
                    io_Padding[i] = 13;
                }
                else
                {
                    Console.Write("| " + fieldTitle + " ");
                    separatorPadding += 3 + fieldTitle.Length;
                    io_Padding[i] = fieldTitle.Length;
                }
            }
            Console.Write(Environment.NewLine);
            for (int i = 0; i <= separatorPadding; i++)
            {
                Console.Write("-");
            }
            Console.Write(Environment.NewLine);
        }

        private void printRegisterTableAttributes(int i_Filter, int[] i_Padding)
        {
            if(m_Garage.TotalNumOfVehicles != 0)           
            {
                for (int i = 1; i <= m_Garage.TotalNumOfVehicles; i++)
                {
                    
                    if(m_Garage.GetVehicleStatus(m_Garage.GetVehicleLicensePlateNum(i)) == (Garage.Elements.eVehicleStatus)i_Filter || i_Filter == m_NoFilter)
                    {
                        int registerTableFieldCount = Enum.GetNames(typeof(eRegisterTableFields)).Length;
                        string[] printableVehicleList = new string[registerTableFieldCount];
                        int vehicleTypeIndex = m_Garage.GetVehicleType(m_Garage.GetVehicleLicensePlateNum(i)) - 1;

                        printableVehicleList[(byte)eRegisterTableFields.Num] = i.ToString();
                        printableVehicleList[(byte)eRegisterTableFields.LicensePlateNum] = m_Garage.GetVehicleLicensePlateNum(i);
                        printableVehicleList[(byte)eRegisterTableFields.Status] = UIUtils.EnumDisplayName(m_Garage.GetVehicleStatus(m_Garage.GetVehicleLicensePlateNum(i)));
                        printableVehicleList[(byte)eRegisterTableFields.VehicleType] = UIUtils.EnumDisplayName(m_VehicleTypes[vehicleTypeIndex]);
                        printableVehicleList[(byte)eRegisterTableFields.Model] =  m_Garage.GetVehicleModel(m_Garage.GetVehicleLicensePlateNum(i));
                        printableVehicleList[(byte)eRegisterTableFields.OwnerPhone] = m_Garage.GetOwnerPhoneNumber(m_Garage.GetVehicleLicensePlateNum(i));
                        printableVehicleList[(byte)eRegisterTableFields.OwnerName] = m_Garage.GetOwnerName(m_Garage.GetVehicleLicensePlateNum(i));

                        for (int j = 0; j < registerTableFieldCount; j++)
                        {
                            
                            Console.Write("| " + UIUtils.PadBothSides(printableVehicleList[j], i_Padding[j]) + " ");                          
                        }
                        Console.Write(Environment.NewLine);
                    }
                    
                }
            }
            
        }

        private void filterBy(int i_FilterType)
        {
            v_FilterStatus = true;
            clearAndPrintTopOfPage();
            printRegisterSubMenu();
            Console.Write(Environment.NewLine);
            Console.WriteLine(UIUtils.EnumDisplayName(ePages.VehicleRegister) + ":" + Environment.NewLine);

            Garage.Elements.eVehicleStatus status = (Garage.Elements.eVehicleStatus)i_FilterType;

            Console.WriteLine("Status filter = " + UIUtils.EnumDisplayName(status) + ":    1. Clear filter" + Environment.NewLine);
            printRegisterTable(i_FilterType);

            mainUserInput();
        }
        #endregion
        
        #region Add Vehicle Page
        private void addVehiclePage()
        {
            Console.WriteLine(UIUtils.EnumDisplayName(ePages.AddVehicle) + ":" + Environment.NewLine);

            addVehicleInput();

            string anotherVehicle;
            anotherVehicleMsg();
            anotherVehicle = stringInput();
            while(neitherConditionIsTrue(anotherVehicle, m_PositiveAnswer, m_NegativeAnswer))
            {
                invalidInputMsg();
                anotherVehicleMsg();
                anotherVehicle = Console.ReadLine();
            }
            if (stringConditionIsTrue(anotherVehicle, m_PositiveAnswer))
            {
                switchPage(ePages.AddVehicle);
            }
            else
            {
                switchPage(ePages.VehicleRegister);
            }
        }

        private bool stringConditionIsTrue(string i_InputString, string i_Condition)
        {
            bool v_conditionIsTrue = false;

            if(i_Condition.Length ==1) //condition is 1 char long
            {
                char conditionChar = char.Parse(i_Condition);

                if(char.IsLetter(conditionChar))
                {
                    conditionChar = char.ToUpper(conditionChar);

                    if(i_InputString == conditionChar.ToString() || i_InputString == (char.ToLower(conditionChar)).ToString())
                    {
                        v_conditionIsTrue = true;
                    }
                }
            }
            else
            {
                if(i_InputString == i_Condition)
                {
                    v_conditionIsTrue = true;
                }
            }

            return v_conditionIsTrue;
        }

        private bool neitherConditionIsTrue(string i_InputString, string i_ConditionOne, string i_ConditionTwo)
        {
            bool v_neitherCondition = false;

            if(i_ConditionOne.Length == 1 && i_ConditionTwo.Length == 1) //both conditions are 1 char long
            {
                char conditionOneChar = char.Parse(i_ConditionOne);
                char conditionTwoChar = char.Parse(i_ConditionTwo);

                if (char.IsLetter(conditionOneChar) && char.IsLetter(conditionTwoChar)) //both condition are letters
                {
                    if(!stringConditionIsTrue(i_InputString, i_ConditionOne) && !stringConditionIsTrue(i_InputString, i_ConditionTwo))
                    {
                        v_neitherCondition = true;
                    }

                }
            }
            else
            {
                if(i_InputString != i_ConditionOne && i_InputString != i_ConditionTwo)
                {
                    v_neitherCondition = true;
                }
            }
            
           

            return v_neitherCondition;
        }

        private void anotherVehicleMsg()
        {
            Console.Write("Would you like to add another vehicle? (Y/N): ");
        }

        #region Input Methods

        private string tryReadingInput()
        {
            string str = null;

            try
            {
                str = Console.ReadLine();
            }
            catch (FormatException)
            {
                invalidInputMsg();
            }

            return str;
        }
        private string stringInput()
        {
            string inputStr = null;
            inputStr = tryReadingInput();
            while (inputStr is null || inputStr == "" || inputStr.Length == 0)
            {
                invalidInputMsg();
                inputStr = tryReadingInput();
            }

            return inputStr;
        }

        private bool isNameValid(string i_Name)
        {
            bool v_validName = true; //intialized to true in this case

            for (int i = 0; i < i_Name.Length; i++)
            {
                if (!char.IsLetter(i_Name[i]) && i_Name[i] != ' ')
                {
                    v_validName = false;
                    break;
                }
            }

            return v_validName;
        }

        private string personNameInput()
        {
            string personName = stringInput();

            while(!isNameValid(personName))
            {
                invalidInputMsg();
                personName = stringInput();
            }
            StringBuilder formattedName = new StringBuilder();
            for(int i=0 ; i < personName.Length ; i++)
            {
                if(i == 0 || personName[i-1] == ' ')
                {
                    if(char.IsLower(personName[i]))
                    {
                        formattedName.Append(char.ToUpper(personName[i]));
                    }
                    else
                    {
                        formattedName.Append(personName[i]);
                    }
                }
                else
                {
                    formattedName.Append(personName[i]);
                }
            }

            return formattedName.ToString();
        }

        private string phoneNumberInput(int i_NumOfDigits)
        {
            string phoneNumber = stringInput();

            while(!isPhoneNumberValid(phoneNumber, i_NumOfDigits))
            {
                invalidInputMsg();
                phoneNumber = stringInput();
            }

            StringBuilder formattedNumber = new StringBuilder();
            for(int i = 0; i < phoneNumber.Length; i++)
            {
                if(i != 3)
                {
                    formattedNumber.Append(phoneNumber[i]);
                }
                else
                {
                    formattedNumber.Append('-');
                    formattedNumber.Append(phoneNumber[i]);
                }
            }

            return formattedNumber.ToString();
        }

        private bool isPhoneNumberValid(string i_PhoneNumber, int i_NumOfDigits)
        {
            bool v_validNumber = true; //intialized to true in this case

            if(i_PhoneNumber.Length != i_NumOfDigits)
            {
                v_validNumber = false;
            }
            else
            {
                for (int i = 0; i < i_PhoneNumber.Length; i++)
                {
                    if (!char.IsDigit(i_PhoneNumber[i]))
                    {
                        v_validNumber = false;
                        break;
                    }
                }
            }
            
            return v_validNumber;
        }

        private string licensePlateNumberInput(int i_MinNumOfChars, int i_MaxNumOfChars)
        {
            string plateNum = stringInput();

            while(!isLicensePlateNumberValid(plateNum, i_MinNumOfChars, i_MaxNumOfChars))
            {
                invalidInputMsg();
                plateNum = stringInput();
            }

            StringBuilder formattedPlateNum = new StringBuilder();
            for(int i=0;i<plateNum.Length;i++)
            {
                if(plateNum.Length == i_MinNumOfChars)
                {
                    if (i == 2 || i == 5) //Dash locations for 7 digit plate numbers
                    {
                        formattedPlateNum.Append('-');
                        formattedPlateNum.Append(plateNum[i]);
                    }
                    else
                    {
                        formattedPlateNum.Append(plateNum[i]);
                    }
                }
                else // = i_MaxNumOfChars
                {
                    if (i == 3 || i == 5) //Dash locations for 8 digit plate numbers
                    {
                        formattedPlateNum.Append('-');
                        formattedPlateNum.Append(plateNum[i]);
                    }
                    else
                    {
                        formattedPlateNum.Append(plateNum[i]);
                    }
                }
            }

            return formattedPlateNum.ToString();

        }

        private bool isLicensePlateNumberValid(string i_LicensePlateNum, int i_MinNumOfChars, int i_MaxNumOfChars)
        {
            bool v_validPlateNum = true; //initialized to true
          
            if(i_LicensePlateNum.Length < i_MinNumOfChars || i_LicensePlateNum.Length > i_MaxNumOfChars)
            {
                v_validPlateNum = false;
            }
            else
            {
                for (int i = 0; i < i_LicensePlateNum.Length; i++)
                {
                    if (!char.IsLetterOrDigit(i_LicensePlateNum[i])) //Diplomatic license plates have letters
                    {
                        v_validPlateNum = false;
                        break;
                    }
                }
            }

            return v_validPlateNum;

        }

        private byte tryReadingEnumByteValue()
        {
            byte enumIntValue = 0;
            try
            {
                enumIntValue = byte.Parse(stringInput());
            }
            catch (Exception ex)
            {
                if(ex is FormatException || ex is OverflowException)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return enumIntValue;
        }
        private byte selectEnumByteValue<T>(T i_Value)
        {
            byte enumByteValue = 0; //default value in case of exception
            byte firstEnumValue = (byte)Enum.GetValues(typeof(T)).GetValue(0);
            byte lastEnumValue = (byte)Enum.GetValues(typeof(T)).GetValue((Enum.GetValues(typeof(T)).Length)-1);
            Type type = Enum.GetUnderlyingType(typeof(T));

            enumByteValue = tryReadingEnumByteValue();

            while (enumByteValue < firstEnumValue || enumByteValue > lastEnumValue)
            {
                invalidInputMsg();
                enumByteValue = tryReadingEnumByteValue();
            }

            return enumByteValue;
        }

        private float tryParsingFloat()
        {
            float f = -1;
            try
            {
                f = float.Parse(stringInput());
            }
            catch (FormatException)
            {
                invalidInputMsg();
            }

            return f;
        }

        private float floatInput()
        {
            float flt;

            flt = tryParsingFloat();
            while (flt == -1)
            {
                flt = tryParsingFloat();
            }

            return flt;
        }

        private int tryParsingInt()
        {
            int intValue = -1;
            try
            {
                intValue = int.Parse(stringInput());
            }
            catch (FormatException)
            {
                invalidInputMsg();
            }

            return intValue;
        }

        private int intInput()
        {
            int intValue;

            intValue = tryParsingInt();
            while (intValue == -1)
            {
                intValue = tryParsingInt();
            }

            return intValue;
        }
        #endregion

        private void inputDefaultRangedValues(ref float io_CurrentAirPressure, ref float io_CurrentEnergy, int i_VehicleType)
        {
            Console.Write("- Current Air Pressure (Maximum - " + m_Garage.GetVehicleMaxAirPressureCapacity(i_VehicleType) + "): ");
            io_CurrentAirPressure = floatInput();

            Console.Write(Environment.NewLine);

            string selectedType = m_VehicleTypes[i_VehicleType - 1];

            if (selectedType.Contains("Electric")) //if vehicle is electric
            {
                Console.Write("- Current Battery Time Remaining - Hours (Maximum - " + m_Garage.GetVehicleMaxEnergyCapacity(i_VehicleType) + "): ");
                io_CurrentEnergy = floatInput();
                Console.Write(Environment.NewLine);
            }
            else
            {
                Console.Write("- Current Fuel Amount - Liters (Maximum - " + m_Garage.GetVehicleMaxEnergyCapacity(i_VehicleType) + "): ");
                io_CurrentEnergy = floatInput();
                Console.Write(Environment.NewLine);
            }
        }

        private bool setLicensePlateNumber(ref string i_LicensePlateNum, int i_MinNumOfChars, int i_MaxNumOfChars)
        {
            bool v_reEntry = false;
            Console.Write("- License Plate Number (" + i_MinNumOfChars + "-" + i_MaxNumOfChars + " characters, no dashes!): ");
            i_LicensePlateNum = licensePlateNumberInput(i_MinNumOfChars, i_MaxNumOfChars);
            if (m_Garage.TotalNumOfVehicles > 0)
            {
                if (m_Garage.SetVehicleStatus(i_LicensePlateNum, Garage.Elements.eVehicleStatus.InProgress))
                {
                    Console.WriteLine("License plate already exists! Status has been changed to In Progress!");
                    v_reEntry = true;
                }
            }

            return v_reEntry;
        }

        private void inputOwnerDetails(ref string io_OwnerName, ref string io_OwnerPhoneNum, int i_PhoneNumberNumOfDigits)
        {
            Console.Write("- Owner Name: ");
            io_OwnerName = personNameInput();
            Console.Write(Environment.NewLine);

            Console.Write("- Owner Phone Number (" + i_PhoneNumberNumOfDigits + " digits, no dashes!): ");
            io_OwnerPhoneNum = phoneNumberInput(i_PhoneNumberNumOfDigits);
            Console.Write(Environment.NewLine);
        }

        private int selectVehicleType()
        {
            int vehicleType = 0;
            Console.Write("- Vehicle Type" + Environment.NewLine + "  ");
            for (int i = 0; i < m_NumOfVehicleTypes; i++)
            {
                int printedIndex = i + 1;
                if (i != 0)
                {
                    Console.Write("|  ");
                }
                Console.Write(printedIndex + ". " + UIUtils.EnumDisplayName(m_VehicleTypes[i]) + "  ");
            }
            Console.Write(Environment.NewLine + "  Select type number: ");
            vehicleType = intInput();
            while (vehicleType < 1 || vehicleType > m_NumOfVehicleTypes)
            {
                invalidInputMsg();
                vehicleType = intInput();
            }


            return vehicleType;
        }

        private void setMotorcycleDetails(string i_LicensePlateNum)
        {
            Garage.Elements.eMotorcycleLicenseType licenseType = (Garage.Elements.eMotorcycleLicenseType)1; //defualt assignment

            Console.WriteLine("- License Type: ");
            UIUtils.PrintEnumSelection((Garage.Elements.eMotorcycleLicenseType)m_Garage.GetMotorcycleLicenseType(i_LicensePlateNum));
            licenseType = ((Garage.Elements.eMotorcycleLicenseType)selectEnumByteValue(licenseType));
            m_Garage.SetMotorcycleLicenseType(i_LicensePlateNum, licenseType);

            Console.Write(Environment.NewLine);
            Console.Write("- Engine Volume: ");
            int engineVolume = intInput();
            m_Garage.SetMotorcycleEngineVolume(i_LicensePlateNum, engineVolume);
            Console.Write(Environment.NewLine);
        }

        private void setCarDetails(string i_LicensePlateNum)
        {
            Garage.Elements.eCarColors carColor = (Garage.Elements.eCarColors)3; //defualt assignment
            Garage.Elements.eCarDoors numOfDoors = (Garage.Elements.eCarDoors)4; //defualt assignment

            Console.WriteLine("- Car Color: ");
            UIUtils.PrintEnumSelection((Garage.Elements.eCarColors)m_Garage.GetCarColor(i_LicensePlateNum));
            carColor = (Garage.Elements.eCarColors)selectEnumByteValue(carColor);
            bool v_setCarColor = m_Garage.SetCarColor(i_LicensePlateNum, carColor);
            if (!v_setCarColor)
            {
                Console.WriteLine("Car color selection unsuccessful");
            }
            Console.Write(Environment.NewLine);

            Console.WriteLine("- Number of Doors: ");
            UIUtils.PrintEnumSelection((Garage.Elements.eCarDoors)m_Garage.GetCarNumberOfDoors(i_LicensePlateNum));
            numOfDoors = (Garage.Elements.eCarDoors)selectEnumByteValue(numOfDoors);
            bool v_setNumOfDoors = m_Garage.SetCarNumberOfDoors(i_LicensePlateNum, numOfDoors);
            if (!v_setNumOfDoors) 
            {
                Console.WriteLine("Number of Doors selection unsuccessful");
            }
            Console.Write(Environment.NewLine);
        }

        private void setTruckDetails(string i_LicensePlateNum)
        {
            Console.Write("- Carries Hazerdous Materials? (Y/N)");
            string hazerdousMaterials = stringInput();
            while (neitherConditionIsTrue(hazerdousMaterials, m_PositiveAnswer, m_NegativeAnswer))
            {
                invalidInputMsg();
                hazerdousMaterials = Console.ReadLine();
            }
            bool v_setHazerdous = false;
            if (stringConditionIsTrue(hazerdousMaterials, m_PositiveAnswer))
            {
                v_setHazerdous = m_Garage.SetCarriesHazardousMaterials(i_LicensePlateNum, true);
                if (!v_setHazerdous)
                {
                    Console.WriteLine("Hazerdous material setting unsuccessful");
                }
            }
            else
            {
                v_setHazerdous = m_Garage.SetCarriesHazardousMaterials(i_LicensePlateNum, false);
                if (!v_setHazerdous)
                {
                    Console.WriteLine("Hazerdous material setting unsuccessful");
                }
            }

            Console.Write(Environment.NewLine);
            Console.Write("- Cargo Volume:");
            float cargoVolume = floatInput();
            bool v_SetCargoVolume = m_Garage.SetTruckCargoVolume(i_LicensePlateNum, cargoVolume);
            if (!v_SetCargoVolume)
            {
                Console.WriteLine("Cargo volume setting unsuccessful");
            }
            Console.Write(Environment.NewLine);
        }

        private void addVehicleInput()
        {
            const int plateMinNumOfChars = 7;
            const int plateMaxNumOfChars = 8;
            string plateNum = "";
            string ownerName = "";
            const int phoneNumberNumOfDigits = 10;
            string ownerNumber = "";
            int vehicleType = 0;
            string vehicleModel = "";
            string manufacturerName = "";
            float currentAirPressure = 0f;
            float currentEnergy = 0f;
            bool v_reEntry = false;

            v_reEntry = setLicensePlateNumber(ref plateNum, plateMinNumOfChars, plateMaxNumOfChars);
            Console.Write(Environment.NewLine);

            if(!v_reEntry)
            {
                inputOwnerDetails(ref ownerName, ref ownerNumber, phoneNumberNumOfDigits);

                vehicleType = selectVehicleType();
                string selectedType = m_VehicleTypes[vehicleType - 1];
                Console.Write(Environment.NewLine);

                Console.Write("- Model Name: ");
                vehicleModel = stringInput();
                Console.Write(Environment.NewLine);

                Console.Write("- Tire Manufacturer: ");
                manufacturerName = stringInput();
                Console.Write(Environment.NewLine);

                inputDefaultRangedValues(ref currentAirPressure, ref currentEnergy , vehicleType);

                bool v_SuccessfulInput = m_Garage.AddVehicleToGarage(ownerName, ownerNumber, vehicleType, vehicleModel, plateNum, manufacturerName, currentAirPressure, currentEnergy);
                while(!v_SuccessfulInput)
                {
                    Console.WriteLine("One of the values is out of range, please try again:");
                    inputDefaultRangedValues(ref currentAirPressure, ref currentEnergy, vehicleType);
                    v_SuccessfulInput = m_Garage.AddVehicleToGarage(ownerName, ownerNumber, vehicleType, vehicleModel, plateNum, manufacturerName, currentAirPressure, currentEnergy);
                }

                if (selectedType.Contains("Motorcycle"))
                {
                    setMotorcycleDetails(plateNum);
                }
                else if (selectedType.Contains("Car"))
                {
                    setCarDetails(plateNum);
                }
                else //vehicle is a truck
                {
                    setTruckDetails(plateNum);
                }
            }           

        }
        #endregion

        #region Vehicle Operations Page
        private void vehicleOperations()
        {
            bool v_anotherAction = true;

            if (m_Garage.TotalNumOfVehicles == 0) //there are no vehicles in the garage's register
            {
                Console.WriteLine(Environment.NewLine + "There are no vehicles in your garage's register! Add a vehicle to enable vehicle operations.");
                Console.WriteLine("Press ANY KEY to continue...");
                Console.ReadKey();
                switchPage(ePages.VehicleRegister);
            }
            else
            {
                bool v_firstSelectionIteration = true;
                int selectedVehicle = -1;
                string plateNum = null;
                int vehicleIndex = 0;
                while(vehicleIndex != selectedVehicle)
                {
                    if(!v_firstSelectionIteration)
                    {
                        invalidInputMsg();
                    }
                    selectedVehicle = selectVehicleToWorkOn();
                    try
                    {
                        plateNum = m_Garage.GetVehicleLicensePlateNum(selectedVehicle);
                                            }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                    try
                    {
                        vehicleIndex = m_Garage.GetVehicleIndex(plateNum) + 1;
                    }
                    catch(ArgumentException)
                    {

                    }
                    v_firstSelectionIteration = false;
                }
                vehicleOperationsPage(plateNum);
                while (v_anotherAction)
                {
                    string anotherAction;
                    anotherActionMsg();
                    anotherAction = stringInput();
                    while (neitherConditionIsTrue(anotherAction, m_PositiveAnswer, m_NegativeAnswer))
                    {
                        invalidInputMsg();
                        anotherActionMsg();
                        anotherAction = stringInput();
                    }
                    if (stringConditionIsTrue(anotherAction, m_PositiveAnswer))
                    {
                        vehicleOperationsPage(plateNum);
                    }
                    else
                    {
                        v_anotherAction = false;
                        switchPage(ePages.VehicleRegister);
                    }
                }
            }


        }

        private int selectVehicleToWorkOn()
        {
            Console.Write("Select the serial number of the vehicle you'd like to work on: ");
            int selectedVehicle = intInput();

            return selectedVehicle;
        }

        private void anotherActionMsg()
        {
            Console.Write(Environment.NewLine + "Would you like to perform another action? (Y/N): ");
        }

        private void vehicleOperationsPage(string i_LicensePlateNum)
        {
            clearAndPrintTopOfPage();
            Console.WriteLine(UIUtils.EnumDisplayName(ePages.VehicleOperations) + ":" + Environment.NewLine);
            printVehicleDetails(i_LicensePlateNum);
            vehicleOpInput(i_LicensePlateNum);
        }

        private void printVehicleDetails(string i_LicensePlateNum)
        {
            int serialNum = (int)m_Garage.GetVehicleIndex(i_LicensePlateNum);
            serialNum++;
            string vehicleType = m_VehicleTypes[(m_Garage.GetVehicleType(i_LicensePlateNum)) - 1];

            Console.WriteLine(Environment.NewLine + "Vehicle Details: ");
            Console.WriteLine("- Serial Number: " + serialNum);
            Console.WriteLine("- Status: " + UIUtils.EnumDisplayName(m_Garage.GetVehicleStatus(i_LicensePlateNum)));
            Console.WriteLine("- Owner Name: " + m_Garage.GetOwnerName(i_LicensePlateNum));
            Console.WriteLine("- Owner Phone Number: " + m_Garage.GetOwnerPhoneNumber(i_LicensePlateNum));
            Console.WriteLine("- License Plate Number: " + i_LicensePlateNum);
            Console.WriteLine("- Vehicle Type: " + UIUtils.EnumDisplayName(vehicleType));
            Console.WriteLine("- Model: " + m_Garage.GetVehicleModel(i_LicensePlateNum));
            Console.WriteLine("- Tire Manufacturer: " + m_Garage.GetVehicleTireManufacturer(i_LicensePlateNum));
            Console.WriteLine("- Current Air Pressure (Maximum - " + m_Garage.GetVehicleMaxAirPressureCapacity(i_LicensePlateNum) + "): " + m_Garage.GetVehicleCurrentAirPressure(i_LicensePlateNum));
            printSpecificVehicleDetails(i_LicensePlateNum);


            Console.WriteLine(Environment.NewLine);

        }

        private void printSpecificVehicleDetails(string i_LicensePlateNum)
        {
            string type = m_VehicleTypes[(m_Garage.GetVehicleType(i_LicensePlateNum))-1];
            switch (type)
            {
                case "FuelMotorcycle":
                    motorcycleDetails(i_LicensePlateNum);
                    fuelVehicleDetails(i_LicensePlateNum);
                    break;
                case "ElectricMotorcycle":
                    motorcycleDetails(i_LicensePlateNum);
                    electricVehicleDetails(i_LicensePlateNum);
                    break;
                case "FuelCar":
                    carDetails(i_LicensePlateNum);
                    fuelVehicleDetails(i_LicensePlateNum);
                    break;
                case "ElectricCar":
                    carDetails(i_LicensePlateNum);
                    electricVehicleDetails(i_LicensePlateNum);
                    break;
                case "Truck":
                    truckDetails(i_LicensePlateNum);
                    fuelVehicleDetails(i_LicensePlateNum);
                    break;
                default:
                    break;
            }

        }
        private void fuelVehicleDetails(string i_LicensePlateNum)
        {
            float maxEnergy = m_Garage.GetVehicleMaxEnergyCapacity(i_LicensePlateNum);
            float currentEnergyPerc = m_Garage.GetEnergyStatus(i_LicensePlateNum);
            float currentEnergy = (currentEnergyPerc / 100) * maxEnergy;

            Console.WriteLine("- Fuel Type: " + UIUtils.EnumDisplayName(m_Garage.GetVehicleFuelType(i_LicensePlateNum)));
            Console.WriteLine(string.Format("- Current Fuel Level (Liters / %): {0:F1}L / {1:F1}%", currentEnergy, currentEnergyPerc));
            Console.WriteLine("- Maximum Fuel Capacity (Liters): " + maxEnergy + "L");
        }

        private void electricVehicleDetails(string i_LicensePlateNum)
        {
            float maxEnergy = m_Garage.GetVehicleMaxEnergyCapacity(i_LicensePlateNum);
            float currentEnergyPerc = m_Garage.GetEnergyStatus(i_LicensePlateNum);
            float currentEnergy = (currentEnergyPerc/100) * maxEnergy;

            Console.WriteLine(string.Format("- Charge Level (%): {0:F1}", currentEnergyPerc));
            Console.WriteLine("- Battery Time Remaining (Hours): " + currentEnergy);
            Console.WriteLine("- Maximum Battery Time (Hours): " + maxEnergy);
        }

        private void motorcycleDetails(string i_LicensePlateNum)
        {
            Console.WriteLine("- License Type: " + m_Garage.GetMotorcycleLicenseType(i_LicensePlateNum));
            Console.WriteLine("- Engine Volume: " + m_Garage.GetMotorcycleEngineVolume(i_LicensePlateNum));
        }
        private void carDetails(string i_LicensePlateNum)
        {
            Console.WriteLine("- Color: " + UIUtils.EnumDisplayName(m_Garage.GetCarColor(i_LicensePlateNum)));
            Console.WriteLine("- Number of Doors: " + m_Garage.GetCarNumberOfDoors(i_LicensePlateNum));
        }
        private void truckDetails(string i_LicensePlateNum)
        {
            Console.WriteLine("- Carries Hazardous Materials? " + m_Garage.GetCarriesHazardousMaterials(i_LicensePlateNum));
            Console.WriteLine("- Cargo Volume: " + m_Garage.GetTruckCargoVolume(i_LicensePlateNum));
        }

        private void performActionMsg()
        {
            Console.Write(Environment.NewLine + "Would you like to perform one of the above actions? (Y/N) ");
        }

        private void selectActionMsg()
        {
            Console.Write("Which action would you like to perform? ");
        }

        private void vehicleOpInput(string i_LicensePlateNum)
        {
            int action = 0;
            string type = m_VehicleTypes[(m_Garage.GetVehicleType(i_LicensePlateNum))-1];

            if (type.Contains("Electric"))
            {
                Console.WriteLine("Actions: 1. Change Status    2. Inflate Tires    3. Recharge    4. Go Back");
            }
            else
            {
                Console.WriteLine("Actions: 1. Change Status    2. Inflate Tires    3. Refuel    4. Go Back");
            }
            selectActionMsg();
            action = intInput();
            Console.Write(Environment.NewLine);
            while (action < 1 || action > 4)
            {
                invalidInputMsg();
                selectActionMsg();
                action = intInput();
            }
            vehicleActions(i_LicensePlateNum, action);
        }

        private void vehicleActions(string i_LicensePlateNum, int i_Action)
        {
            switch (i_Action)
            {
                case 1:
                    Garage.Elements.eVehicleStatus currentStatus = m_Garage.GetVehicleStatus(i_LicensePlateNum);
                    Console.WriteLine(string.Format(
@"Current status: {0}
Select new status for your selected vehicle: 1. {1}    2. {2}    3. {3}", UIUtils.EnumDisplayName(currentStatus),
UIUtils.EnumDisplayName(Garage.Elements.eVehicleStatus.InProgress), UIUtils.EnumDisplayName(Garage.Elements.eVehicleStatus.Repaired),
UIUtils.EnumDisplayName(Garage.Elements.eVehicleStatus.Paid)));
                    int newStatus = intInput();
                    while (newStatus < 1 || newStatus > 3)
                    {
                        invalidInputMsg();
                        newStatus = intInput();
                    }
                    bool v_setStatus = m_Garage.SetVehicleStatus(i_LicensePlateNum, (Garage.Elements.eVehicleStatus)newStatus);
                    if(!v_setStatus)
                    {
                        Console.WriteLine("Status change failed!");
                    }
                    else
                    {
                        currentStatus = m_Garage.GetVehicleStatus(i_LicensePlateNum);
                        Console.WriteLine("New current status: " + UIUtils.EnumDisplayName(currentStatus));
                    }
                    break;
                case 2:
                    Console.WriteLine("Inflate Tires:");
                    Console.WriteLine("Current air pressure (maximum - " + m_Garage.GetVehicleMaxAirPressureCapacity(i_LicensePlateNum) +"): " + m_Garage.GetVehicleCurrentAirPressure(i_LicensePlateNum));
                    inflateTiresMsg();
                    string inflateTires = stringInput();
                    while (neitherConditionIsTrue(inflateTires, m_PositiveAnswer, m_NegativeAnswer))
                    {
                        invalidInputMsg();
                        inflateTiresMsg();
                        inflateTires = Console.ReadLine();
                    }
                    if (stringConditionIsTrue(inflateTires, m_PositiveAnswer))
                    {
                        bool v_infaltion = m_Garage.InflateTiresToMax(i_LicensePlateNum);
                        if(!v_infaltion)
                        {
                            Console.WriteLine("Tire inflation failed!");
                        }
                        else
                        {
                            Console.WriteLine("Tire inflation successful!");
                        }
                    }                
                    break;
                case 3:
                    string type = m_VehicleTypes[(m_Garage.GetVehicleType(i_LicensePlateNum))-1];
                    if (type.Contains("Electric"))
                    {
                        float maxEnergy = m_Garage.GetVehicleMaxEnergyCapacity(i_LicensePlateNum);
                        float currentEnergyPerc = m_Garage.GetEnergyStatus(i_LicensePlateNum);
                        float currentEnergy = (currentEnergyPerc / 100) * maxEnergy;
                        bool v_Recharge = false;

                        Console.WriteLine("Recharge:");
                        Console.WriteLine("- Maximum Battery Time (Hours): " + maxEnergy);
                        Console.WriteLine(string.Format("- Charge Level (%): {0:F1}", currentEnergyPerc));
                        Console.Write(Environment.NewLine + "To recharge, enter desired recharge time (hours): ");
                        float rechargeValue = floatInput();
                        try
                        {
                            v_Recharge = m_Garage.Recharge(i_LicensePlateNum, rechargeValue);

                        }
                        catch (ValueOutOfRangeException e)
                        {
                            Console.WriteLine(e.Message + " Recharge Unsuccesful!");
                        }
                        if (v_Recharge)
                        {
                            Console.WriteLine("Recharge Succesful!");

                        }

                    }
                    else //vehicle isn't electric
                    {

                        float maxEnergy = m_Garage.GetVehicleMaxEnergyCapacity(i_LicensePlateNum);
                        float currentEnergyPerc = m_Garage.GetEnergyStatus(i_LicensePlateNum);
                        float currentEnergy = (currentEnergyPerc / 100) * maxEnergy;
                        Garage.Elements.eFuelTypes fuelType = m_Garage.GetVehicleFuelType(i_LicensePlateNum);
                        bool v_Refuel = false;

                        Console.WriteLine("Refuel:");
                        Console.WriteLine("- Maximum Fuel Capacity (Liters): " + maxEnergy + "L");
                        Console.WriteLine(string.Format("- Current Fuel Level (Liters / %): {0:F1}L / {1:F1}%", currentEnergy, currentEnergyPerc));
                        Console.Write(Environment.NewLine + "To refuel, enter desired refuel value (liters): ");
                        float refuelValue = floatInput();
                        try
                        {
                            v_Refuel = m_Garage.Refuel(i_LicensePlateNum, refuelValue, fuelType);
                        }
                        catch (ValueOutOfRangeException e)
                        {
                            Console.WriteLine(e.Message + " Refuel Unsuccesful!");
                        }
                        if(v_Refuel)
                        {
                            Console.WriteLine("Refuel Succesful!");

                        }
                    }
                    break;
                case 4:
                    switchPage(ePages.VehicleRegister);
                    break;
            }
        }

        private void inflateTiresMsg()
        {
            Console.WriteLine("Would you like to inflate to the maximum level of air pressure? (Y/N)");
        }

        #endregion
        
        #region Help Page
        // The Help page provides assistance & instructions should the user need any to use the UI
        private void helpPage()
        {
            Console.WriteLine(Environment.NewLine + "Help:" + Environment.NewLine);

            Console.Write(string.Format(
@"Welcome to the GMS help page!

Below are a few pointers to help you navigate the system:

- When the input message appears at the bootom of your screen:
  (")); mainUserInputRequestMsg(); Console.Write(string.Format(@")
  you may enter the corresponding character on screen, whether it's a menu page(e: a. Home) or 
  a page-specific one (e: 1. Clear Filter), to proceed/perform the action.
"));

            Console.Write(Environment.NewLine);
            mainUserInput();
        }
        #endregion        
    }
}
