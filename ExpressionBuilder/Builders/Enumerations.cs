using ExpressionBuilder.Attributes;

namespace ExpressionBuilder.Builders
{
    public enum FilterStatementConnector
    {
        And,
        Or
    }

    public enum Operation
    {
        [NumberOfValues(1)]
        Equals,

        [NumberOfValues(1)]
        Contains,

        [NumberOfValues(1)]
        StartsWith,

        [NumberOfValues(1)]
        EndsWith,

        [NumberOfValues(1)]
        NotEquals,

        [NumberOfValues(1)]
        GreaterThan,

        [NumberOfValues(1)]
        GreaterThanOrEquals,

        [NumberOfValues(1)]
        LessThan,

        [NumberOfValues(1)]
        LessThanOrEquals,

        [NumberOfValues(2)]
        Between,

        [NumberOfValues(0)]
        IsNull,

        [NumberOfValues(0)]
        IsEmpty,

        [NumberOfValues(0)]
        IsNullOrWhiteSpace,

        [NumberOfValues(0)]
        IsNotNull,

        [NumberOfValues(0)]
        IsNotEmpty,

        [NumberOfValues(0)]
        IsNotNullNorWhiteSpace,

        [NumberOfValues(1)]
        In
	}

    public enum TypeGroup
    {
        [SupportedOperations(Operation.Equals, Operation.NotEquals)]
        Default,

        [SupportedOperations(Operation.Contains, Operation.EndsWith, Operation.Equals,
                             Operation.IsEmpty, Operation.IsNotEmpty, Operation.IsNotNull, Operation.IsNotNullNorWhiteSpace,
                             Operation.IsNull, Operation.IsNullOrWhiteSpace, Operation.NotEquals, Operation.StartsWith)]
        Text,

        [SupportedOperations(Operation.Between, Operation.Equals, Operation.GreaterThan, Operation.GreaterThanOrEquals,
                             Operation.LessThan, Operation.LessThanOrEquals, Operation.NotEquals)]
        Number,

        [SupportedOperations(Operation.Equals, Operation.NotEquals)]
        Boolean,

        [SupportedOperations(Operation.Between, Operation.Equals, Operation.GreaterThan, Operation.GreaterThanOrEquals,
                             Operation.IsNotNull, Operation.IsNull, Operation.LessThan, Operation.LessThanOrEquals, Operation.NotEquals)]
        Date,

        [SupportedOperations(Operation.IsNotNull, Operation.IsNull)]
        Nullable
    }
}
