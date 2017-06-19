namespace ExpressionBuilder.Builders
{
	public enum FilterStatementConnector
    {
        And,
        Or
    }
	
	public enum Operation
	{
        Equals,
        Contains,
        StartsWith,
        EndsWith,
        NotEquals,
        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals
	}

    public enum TypeGroup
    {
        Default,
        Text,
        Number,
        Boolean,
        Date
    }
}
