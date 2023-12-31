﻿using Nest;

//public abstract class BaseChildDocument
//{
//    //child need to know how to join the parent
//    public int Parent { get; set; }

//    public JoinField JoinField { get; set; }
//}

//public abstract class BaseParentDocument
//{
//    //child don't need to have id
//    public virtual int Id { get; set; }

//    public JoinField JoinField { get; set; }
//}

//public abstract class BaseDocument
//{
//    public int Parent { get; set; }

//    public JoinField JoinField { get; set; }
//}

public class BaseDocument
{
    public virtual int Id { get; set; }
    public int Parent { get; set; }

    public JoinField JoinField { get; set; }

    public BaseDocument()
    {
    }
}