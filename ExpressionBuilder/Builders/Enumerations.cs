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
        EqualTo,

        [NumberOfValues(1)]
        Contains,

        [NumberOfValues(1)]
        StartsWith,

        [NumberOfValues(1)]
        EndsWith,

        [NumberOfValues(1)]
        NotEqualTo,

        [NumberOfValues(1)]
        GreaterThan,

        [NumberOfValues(1)]
        GreaterThanOrEqualTo,

        [NumberOfValues(1)]
        LessThan,

        [NumberOfValues(1)]
        LessThanOrEqualTo,

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
        [SupportedOperations(Operation.EqualTo, Operation.NotEqualTo)]
        Default,

        [SupportedOperations(Operation.Contains, Operation.EndsWith, Operation.EqualTo,
                             Operation.IsEmpty, Operation.IsNotEmpty, Operation.IsNotNull, Operation.IsNotNullNorWhiteSpace,
                             Operation.IsNull, Operation.IsNullOrWhiteSpace, Operation.NotEqualTo, Operation.StartsWith)]
        Text,

        [SupportedOperations(Operation.Between, Operation.EqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo,
                             Operation.LessThan, Operation.LessThanOrEqualTo, Operation.NotEqualTo)]
        Number,

        [SupportedOperations(Operation.EqualTo, Operation.NotEqualTo)]
        Boolean,

        [SupportedOperations(Operation.Between, Operation.EqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo,
                             Operation.IsNotNull, Operation.IsNull, Operation.LessThan, Operation.LessThanOrEqualTo, Operation.NotEqualTo)]
        Date,

        [SupportedOperations(Operation.IsNotNull, Operation.IsNull)]
        Nullable
    }
}
