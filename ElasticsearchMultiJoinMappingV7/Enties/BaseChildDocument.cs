using Nest;

public abstract class BaseChildDocument
{
    //child need to know how to join the parent
    public int Parent { get; set; }

    public JoinField JoinField { get; set; }
}
