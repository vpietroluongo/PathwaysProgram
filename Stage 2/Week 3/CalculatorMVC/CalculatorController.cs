using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorMVC;

class CalculatorController
{
    private CalculatorView aView;
    private CalculatorModel aModel;
    public CalculatorController()
    {
        aView = new CalculatorView();
        aModel = new CalculatorModel();


        do
        {
            aView.CalculatorViewHeading();
            //aModel = new CalculatorModel(aView.GetNumber(), aView.GetNumber(), aView.GetOperator());
            aModel.Number1 = aView.GetNumber();
            aModel.Number2 = aView.GetNumber();
            aModel.Operator = aView.GetOperator();
            aView.Result = aModel.CalculateResult();
            aView.ShowResult();
        } while (aView.DetermineGoAgain() == false);
    }
}
