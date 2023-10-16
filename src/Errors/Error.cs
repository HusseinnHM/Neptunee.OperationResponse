namespace Neptunee.OResponse;

public record Error(string Description)
{
    public static implicit operator Error(string description) => new(description);
    public override string ToString()
    {
        return Description;
    }
}