using System;

namespace EmployeeBonusAbstract
{
    class Hourly : Employee
    {
        public decimal HourlyRate
            { get; set; }

        public Hourly() : base()
        {
            EmployeeType = "H";
            HourlyRate = 0.00m;
        }

       // public Hourly(string first, string last, string empType, decimal rate)
        public Hourly(string first, string last, decimal rate) : base(first, last)
        {
           // FirstName = first;
           // LastName = last;
            EmployeeType = "H";
            HourlyRate = rate;
        }

        public override decimal BonusCalculation()
        {
            return HourlyRate * 80;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return " ";
            else
                return $"{FirstName} {LastName} {EmployeeType}-type bonus for ${HourlyRate}/hr is ${BonusCalculation():F2}";
        }
    } //end class
}  //end namespace