Use **LambdaExpressionBuilder** to easily create lambda expressions to filter lists and database queries. It can be useful to turn WebApi requests parameters into expressions, create advanced search screens with the capability to save and re-run those filters, among other things.

# Features:
* Ability to reference properties by their names
* Ability to reference properties from a property
* Ability to reference properties from list items
* Built-in null-checks
* Built-in XML serialization
* Globalization support [not available in .NetStandard 2.0 / .NetCore 2.0]
* Support for complex expressions (those that group up statements within parenthesis)
* Ability to create your own custom operations or overwrite the behaviour of the default operations

## New on version 2.1:
* Added support for .NetStandard 2.0 (which should include support for .Net Core 2.0) (huge thanks to Joris Labie @labiej)
* `FilterFactory` class added to offer a ["non-generics" approach for creating filters](https://github.com/dbelmont/ExpressionBuilder/issues/25)
* Improved support for nested properties (issues [#26](https://github.com/dbelmont/ExpressionBuilder/issues/26) and [#29](https://github.com/dbelmont/ExpressionBuilder/issues/29))
* Added new ['NotIn' operator](https://github.com/dbelmont/ExpressionBuilder/issues/36)
* [Fixed bug](https://github.com/dbelmont/ExpressionBuilder/issues/37) that used to throw an exception when using the `In` operator over a nullable property

## New on version 2:
* Custom operations: create your own operations or overwrite the behaviour of the default operations
* Full support to [Properties and Fields](https://github.com/dbelmont/ExpressionBuilder/issues/14)
* Enum renaming: **FilterStatementConnector** has changed to just **Connector**
* Other minor improvements

### Upgrading to version 2:
Below are a few notes on things you must take into account when upgrading from version 1 to version 2:
* The `Operation` enum was substituted by a class. So, you'll need to add a reference to the new namespace in order to use it: `using ExpressionBuilder.Operations;`
* Obtaining operations by their names:
<br />*Before:* `(Operation)Enum.Parse(typeof(Operation), "EqualTo")`
<br />*Now:* `Operation.GetByName("EqualTo")`
* Getting the number of values expected by an operation:
<br />*Before:* `new OperationHelper().NumberOfValuesAcceptable(Operation.EqualTo)`
<br />*Now:* `Operation.EqualTo.NumberOfValues`
* Connecting filter statements:
<br />*Before:* `FilterStatementConnector.And`
<br />*Now:* `Connector.And`

## New on version 1.1.2:
* New operation added: DoesNotContain
* [Support for complex expressions](https://github.com/dbelmont/ExpressionBuilder/issues/10) (those that group up statements within parenthesis)
* Added tests using LINQ to SQL (along with a [bug fix regarding the library usage with LINQ to SQL](https://github.com/dbelmont/ExpressionBuilder/issues/12))