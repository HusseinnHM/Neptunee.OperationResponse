namespace Neptunee.OResponse.ValidationErrors;

public record ValidationError(string Description)
{
    public static implicit operator ValidationError(string description) => new(description);
    public override string ToString()
    {
        return Description;
    }
}