using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SwapDataUCtr
{
    public class NumValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int _out;
            if (!int.TryParse(value.ToString(),out _out))
                return new System.Windows.Controls.ValidationResult(false,"请输入整数!");
            if(_out<0)
                return new System.Windows.Controls.ValidationResult(false, "值不能小于0!");
            return ValidationResult.ValidResult;
        }
    }
    public class Num1Validation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int _out;
            if (!int.TryParse(value.ToString(), out _out))
                return new System.Windows.Controls.ValidationResult(false, "请输入整数!");
            if (_out < 1)
                return new System.Windows.Controls.ValidationResult(false, "值要大于0!");
            return ValidationResult.ValidResult;
        }
    }
}
