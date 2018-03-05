﻿using System;
using System.Linq.Expressions;
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Base class for operations.
    /// </summary>
    public abstract class Operation : IOperation
    {
        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public TypeGroup TypeGroup { get; }
        /// <inheritdoc />
        public int NumberOfValues { get; }

        /// <inheritdoc />
        public bool Active { get; }

        /// <inheritdoc />
        public bool SupportsLists { get; }

        /// <inheritdoc />
        public bool ExpectNullValues { get; }

        /// <summary>
        /// Instantiates a new operation.
        /// </summary>
        /// <param name="name">Operations name.</param>
        /// <param name="numberOfValues">Number of values supported by the operation.</param>
        /// <param name="typeGroups">TypeGroup(s) which the operation supports.</param>
        /// <param name="active">Determines if the operation is active.</param>
        /// <param name="supportsLists">Determines if the operation supports arrays.</param>
        /// <param name="expectNullValues"></param>
        protected Operation(string name, int numberOfValues, TypeGroup typeGroups, bool active = true, bool supportsLists = false, bool expectNullValues = false)
        {
            Name = name;
            NumberOfValues = numberOfValues;
            TypeGroup = typeGroups;
            Active = active;
            SupportsLists = supportsLists;
            ExpectNullValues = expectNullValues;
        }

        /// <inheritdoc />
        public abstract Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2);

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }
    }
}