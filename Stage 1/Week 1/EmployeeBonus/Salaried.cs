using System;

namespace EmployeeBonus
{
    class Salaried : Employee
    {
        public decimal Salary
            { get; set; }
        
        public Salaried() : base()
        {
            EmployeeType = "S";
            Salary = 0.00m;
        }

        public Salaried(string first, string last, decimal salary)
        {
            FirstName = first;
            LastName = last;
            EmployeeType = "S";
            Salary = salary;
        }

        public override decimal BonusCalculation()
        {
            decimal bonus = Math.Round((.1m * Salary), 2);
            return Convert.ToDecimal(bonus);
        }
    } //end class
}  //end namespace