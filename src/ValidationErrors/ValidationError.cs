namespace Neptunee.OResponse.ValidationErrors;

public record ValidationError(string Description);

public record SpecificValidationError(string Code,string Description) : ValidationError(Description);