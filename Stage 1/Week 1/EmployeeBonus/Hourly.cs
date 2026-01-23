using System;

namespace EmployeeBonus
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
        public Hourly(string first, string last, decimal rate)
        {
            FirstName = first;
            LastName = last;
            EmployeeType = "H";
            HourlyRate = rate;
        }

        public override decimal BonusCalculation()
        {
            return HourlyRate * 80;
        }
    } //end class
}  //end namespace