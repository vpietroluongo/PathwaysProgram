using System;

namespace EmployeeBonusAbstract
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

        public Salaried(string first, string last, decimal salary) : base(first, last)
        {
           // FirstName = first;
           // LastName = last;
            EmployeeType = "S";
            Salary = salary;
        }

        public override decimal BonusCalculation()
        {
            //decimal bonus = Math.Round((.1m * Salary), 2);
            decimal bonus = .1m * Salary;
            return bonus;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                return " ";
            else
                return $"{FirstName} {LastName} {EmployeeType}-type bonus for ${Salary} salary is ${BonusCalculation():F2}";
        }
    } //end class
}  //end namespace