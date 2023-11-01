using System.Collections.ObjectModel;

namespace Neptunee.OperationResponse;

public class ExternalProps : ReadOnlyDictionary<string, string>
{
    public ExternalProps() : base(new Dictionary<string, string>())
    {
    }

    public ExternalProps(IDictionary<string, string> dictionary) : base(dictionary)
    {
    }

    internal void TryAdd<TValue>(string key, TValue value)
    {
        Dictionary!.TryAdd(key, value!.ToString());
    }
}