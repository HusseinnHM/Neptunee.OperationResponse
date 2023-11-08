using System.Runtime.InteropServices.JavaScript;

namespace Neptunee.OperationResponse.Test;

public static class MockValidationErrors
{
    public static readonly Error MockError = new("MockError");
    public abstract class Errors
    {
        public static readonly Error Age = new("Age must be 18 or older");
        public static readonly Error AmountOverMax = new("Amount is over maximum");
        public static readonly Error AmountUnderMin = new("Amount is under minimum");
    }

    public abstract class SpecificErrors
    {
        public static readonly SpecificError Age = new("Age", "must be 18 or older");
        public static readonly SpecificError AmountOverMax = new("Amount", "over maximum");
        public static readonly SpecificError AmountUnderMin = new("Amount", "under minimum");
    }
}