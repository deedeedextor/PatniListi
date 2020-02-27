namespace PatniListi.Data.Common.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MyDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            return date >= DateTime.UtcNow;
        }
    }
}
