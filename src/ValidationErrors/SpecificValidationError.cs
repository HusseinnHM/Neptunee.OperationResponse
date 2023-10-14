namespace Neptunee.OResponse.ValidationErrors;

public record SpecificValidationError(string Code, string Description) : ValidationError(Description)
{
    public override string ToString()
    {
        return $"{Code} : {Description}";
    }
}