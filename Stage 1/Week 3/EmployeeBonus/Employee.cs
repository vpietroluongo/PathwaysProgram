using System;

namespace EmployeeBonus
{
    class Employee
    {
       public string FirstName
            { get; set; }

        public string LastName
            { get; set; } 

        public string EmployeeType
            { get; set; }

        public Employee()
        {
            FirstName = " ";
            LastName = " ";
            EmployeeType = " ";    
        }

        public Employee(string first, string last)
        {
            FirstName = first;
            LastName = last;
            EmployeeType = "O";
        }

        public virtual decimal BonusCalculation()
        {
            return 0.00m;
        }
        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return " ";
            else
                return $"{FirstName} {LastName} {EmployeeType}-type bonus is ${BonusCalculation()}";
        }
    } //end class
}  //end namespace