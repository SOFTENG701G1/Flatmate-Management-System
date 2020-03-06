# C# Coding Guidelines

Coding conventions serve the following purposes:  
  
- They create a consistent look to the code, so that readers can focus on content, not layout.  
  
- They enable readers to understand the code more quickly by making assumptions based on previous experience.  
  
- They facilitate copying, changing, and maintaining the code.  
  
- They demonstrate C# best practices.  

The guidelines in this article are used by Microsoft to develop samples and documentation.  
  
## Naming Conventions  
  
- In short examples that do not include [using directives](../../language-reference/keywords/using-directive.md), use namespace qualifications. If you know that a namespace is imported by default in a project, you do not have to fully qualify the names from that namespace. Qualified names can be broken after a dot (.) if they are too long for a single line, as shown in the following example.  
    
- You do not have to change the names of objects that were created by using the Visual Studio designer tools to make them fit other guidelines.  
  
## Layout Conventions  

Good layout uses formatting to emphasize the structure of your code and to make the code easier to read. Microsoft examples and samples conform to the following conventions:  
  
- Use the default Code Editor settings (smart indenting, four-character indents, tabs saved as spaces). For more information, see [Options, Text Editor, C#, Formatting](/visualstudio/ide/reference/options-text-editor-csharp-formatting).  
  
- Write only one statement per line.  
  
- Write only one declaration per line.  
  
- If continuation lines are not indented automatically, indent them one tab stop (four spaces).  
  
- Add at least one blank line between method definitions and property definitions.  
  
- Use parentheses to make clauses in an expression apparent, as shown in the following code.  
  
    
## Commenting Conventions  
  
- Place the comment on a separate line, not at the end of a line of code.  
  
- Begin comment text with an uppercase letter.  
  
- End comment text with a period.  
  
- Insert one space between the comment delimiter (//) and the comment text, as shown in the following example.  
  
    
- Do not create formatted blocks of asterisks around comments.  
  
## Language Guidelines  

The following sections describe practices that the C# team follows to prepare code examples and samples.  
  
### String Data Type  
  
- Use [string interpolation](../../language-reference/tokens/interpolated.md) to concatenate short strings, as shown in the following code.  
  
    
- To append strings in loops, especially when you are working with large amounts of text, use a <xref:System.Text.StringBuilder> object.  
  
    
### Implicitly Typed Local Variables  
  
- Use [implicit typing](../classes-and-structs/implicitly-typed-local-variables.md) for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important.  
  
    
- Do not use [var](../../language-reference/keywords/var.md) when the type is not apparent from the right side of the assignment.  
  
    
- Do not rely on the variable name to specify the type of the variable. It might not be correct.  
  
    
- Avoid the use of `var` in place of [dynamic](../../language-reference/builtin-types/reference-types.md).  
  
- Use implicit typing to determine the type of the loop variable in [for](../../language-reference/keywords/for.md) loops.  
  
     The following example uses implicit typing in a `for` statement.  
  
    
- Do not use implicit typing to determine the type of the loop variable in [foreach](../../language-reference/keywords/foreach-in.md) loops.

     The following example uses explicit typing in a `foreach` statement.

    
     > [!NOTE]
     > Be careful not to accidentally change a type of an element of the iterable collection. For example, it is easy to switch from <xref:System.Linq.IQueryable?displayProperty=nameWithType> to <xref:System.Collections.IEnumerable?displayProperty=nameWithType> in a `foreach` statement, which changes the execution of a query.

### Unsigned Data Type  
  
In general, use `int` rather than unsigned types. The use of `int` is common throughout C#, and it is easier to interact with other libraries when you use `int`.  
  
### Arrays  
  
Use the concise syntax when you initialize arrays on the declaration line.  
  
### Delegates  
  
Use the concise syntax to create instances of a delegate type.  
  
### try-catch and using Statements in Exception Handling  
  
- Use a [try-catch](../../language-reference/keywords/try-catch.md) statement for most exception handling.  
  
    
- Simplify your code by using the C# [using statement](../../language-reference/keywords/using-statement.md). If you have a [try-finally](../../language-reference/keywords/try-finally.md) statement in which the only code in the `finally` block is a call to the <xref:System.IDisposable.Dispose%2A> method, use a `using` statement instead.  
  
    
### && and &#124;&#124; Operators  
  
To avoid exceptions and increase performance by skipping unnecessary comparisons, use [&&](../../language-reference/operators/boolean-logical-operators.md#conditional-logical-and-operator-) instead of [&](../../language-reference/operators/boolean-logical-operators.md#logical-and-operator-) and [&#124;&#124;](../../language-reference/operators/boolean-logical-operators.md#conditional-logical-or-operator-) instead of [&#124;](../../language-reference/operators/boolean-logical-operators.md#logical-or-operator-) when you perform comparisons, as shown in the following example.  
  
### New Operator  
  
- Use the concise form of object instantiation, with implicit typing, as shown in the following declaration.  
  
    
     The previous line is equivalent to the following declaration.  
  
    
- Use object initializers to simplify object creation.  
  
    
### Event Handling  
  
If you are defining an event handler that you do not need to remove later, use a lambda expression.  
  
### Static Members  
  
Call [static](../../language-reference/keywords/static.md) members by using the class name: *ClassName.StaticMember*. This practice makes code more readable by making static access clear.  Do not qualify a static member defined in a base class with the name of a derived class.  While that code compiles, the code readability is misleading, and the code may break in the future if you add a static member with the same name to the derived class.  
  
### LINQ Queries  
  
- Use meaningful names for query variables. The following example uses `seattleCustomers` for customers who are located in Seattle.  
  
    
- Use aliases to make sure that property names of anonymous types are correctly capitalized, using Pascal casing.  
  
    
- Rename properties when the property names in the result would be ambiguous. For example, if your query returns a customer name and a distributor ID, instead of leaving them as `Name` and `ID` in the result, rename them to clarify that `Name` is the name of a customer, and `ID` is the ID of a distributor.  
  
    
- Use implicit typing in the declaration of query variables and range variables.  
  
    
- Align query clauses under the [from](../../language-reference/keywords/from-clause.md) clause, as shown in the previous examples.  
  
- Use [where](../../language-reference/keywords/where-clause.md) clauses before other query clauses to ensure that later query clauses operate on the reduced, filtered set of data.  
  
    
- Use multiple `from` clauses instead of a [join](../../language-reference/keywords/join-clause.md) clause to access inner collections. For example, a collection of `Student` objects might each contain a collection of test scores. When the following query is executed, it returns each score that is over 90, along with the last name of the student who received the score.  
  
  
## Sources and Additional Notes

- [C# Conventions](https://github.com/dotnet/docs/blob/master/docs/csharp/programming-guide/inside-a-program/coding-conventions.md)
- [Visual Basic Coding Conventions](../../../visual-basic/programming-guide/program-structure/coding-conventions.md)
- [Secure Coding Guidelines](../../../standard/security/secure-coding-guidelines.md)
