namespace PatniListi.Common
{
    public static class AttributesErrorMessages
    {
        public const string RequiredErrorMessage = " Моля, попълнете \"{0}\".";

        public const string StringLengthErrorMessage = "\"{0}\" трябва да бъде между {2} и {1} символа.";

        public const string MaxLengthErrorMessage = "\"{0}\" не може да бъде повече от {1} символа.";

        public const string RangeErrorMessage = "\"{0}\" трябва да бъде между {1} и {2}.";

        public const string DateТimeErrorMessage = "Не може да въвеждате предходна дата.";

        public const string InvalidBulstatErrorMessage = "Невалиден булстат номер.";

        public const string InvalidPhoneNumberErrorMessage = "Невалиден телефонен номер.";
    }
}
