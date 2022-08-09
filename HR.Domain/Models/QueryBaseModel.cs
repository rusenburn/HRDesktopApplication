namespace HR.Domain.Models;
public record QueryBaseModel
{
    public int Skip { get; init; } = 0;
    public int Limit { get; init; } = int.MaxValue;

    public virtual Dictionary<string, string?> GetDict()
    {
        Dictionary<string, string?> dict = new Dictionary<string, string?>();
        var props = this.GetType().GetProperties();
        foreach(var p in props)
        {
            dict.Add(p.Name,p.GetValue(this)?.ToString()) ;
        }
        return dict;
    }
}


