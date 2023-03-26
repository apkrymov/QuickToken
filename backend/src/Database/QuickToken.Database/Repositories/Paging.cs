namespace QuickToken.Database.Repositories;

public readonly struct Paging
{
    public Paging(int count, int shift)
    {
        Count = count;
        Shift = shift;
    }
    
    public Paging(int count) : this(count, 0)
    {
    }
    
    public Paging() : this(100, 0)
    {
    }

    public readonly int Count;

    public readonly int Shift;
}