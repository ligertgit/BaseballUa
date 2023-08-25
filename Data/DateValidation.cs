using System.ComponentModel.DataAnnotations;

namespace BaseballUa.Data
{
    public class Within2000To2050Attribute : ValidationAttribute
    {
        //public class CustomDateAttribute : RangeAttribute
        //{
        //    public CustomDateAttribute()
        //      : base(typeof(DateTime),
        //              DateTime.Now.AddYears(-6).ToShortDateString(),
        //              DateTime.Now.ToShortDateString())
        //    { }
        //}



        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            value = (DateTime)value;
            if (((new DateTime(2000, 1, 1)).CompareTo(value) <= 0) && ((new DateTime(2050, 12, 31)).CompareTo(value) >= 0))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date must be within the last six years!");
            }
        }

        //public class ValidateDateRange : ValidationAttribute
        //{
        //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //    {
        //        // your validation logic
        //        if (value >= Convert.ToDateTime("01/10/2008") && value <= Convert.ToDateTime("01/12/2008"))
        //        {
        //            return ValidationResult.Success;
        //        }
        //        else
        //        {
        //            return new ValidationResult("Date is not in given range.");
        //        }
        //    }
        //}
    }
}
