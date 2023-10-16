namespace Neptunee.OResponse;

public record SpecificError(string Code, string Description) : Error(Description)
{
    public override string ToString()
    {
        return $"{Code} : {Description}";
    }
}