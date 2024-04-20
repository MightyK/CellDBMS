# Alternative Language Project
Using an unfamiliar language to do operations on a csv file.
### Which programming language and version did you pick?
    I am using C# for this project. The language is currently on version #12, so I'm using that.

### How your programming language chosen handles: object-oriented programming, file ingestion, conditional statements, assignment statements, loops, subprograms (functions/methods), unit testing and exception handling. If one or more of these are not supported by your programming language, indicate it as so. 


    1. Object-Oriented Programming (OOP):
    C# fully supports OOP principles such as encapsulation, inheritance, and polymorphism. You can define classes, create objects, and use features like inheritance, interfaces, and access modifiers to organize and structure your code in an object-oriented manner.

    2. File Ingestion:
    C# provides libraries and classes for file handling and ingestion. You can use classes like `FileStream`, `StreamReader`, `StreamWriter`, `File`, `Directory`, etc., to read from and write to files. Additionally, C# supports modern techniques like asynchronous file I/O for efficient file handling operations.

    3. Conditional Statements:
    C# supports common conditional statements like `if`, `else`, `else if`, and `switch`. These statements allow you to control the flow of your program based on different conditions.

    4. Assignment Statements:
    Assignment statements in C# are straightforward. You can assign values to variables using the assignment operator `=`. C# also supports compound assignment operators like `+=`, `-=`, `*=`, `/=`, etc., for shorthand assignments.

    5. Loops:
    C# supports various types of loops including `for`, `foreach`, `while`, and `do-while`. These loops allow you to iterate over collections, execute code repeatedly until a condition is met, or perform iterative tasks.

    6. Subprograms (Functions/Methods):
    C# uses methods (or functions) to encapsulate reusable code. You can define methods within classes, which can then be called from other parts of the program. C# also supports method overloading, allowing you to define multiple methods with the same name but different parameter lists.

    7. Unit Testing:
    C# provides robust support for unit testing through frameworks like NUnit, MSTest, and xUnit. These frameworks allow developers to write test cases for their code to ensure its correctness, maintainability, and reliability. Visual Studio, the primary IDE for C#, offers built-in support for unit testing.

    8. Exception Handling:
    Exception handling in C# is done using `try`, `catch`, `finally`, and `throw` blocks. You can use these constructs to handle both anticipated and unexpected errors gracefully. C# also supports custom exception classes for defining and throwing custom exceptions.


### List out 3 libraries you used from your programming language (if applicable) and explain what they are, why you chose them and what you used them for.

    I only used two Libraries during this project. The first is the System Library (which I used extensively), and the second is the Microsoft Library.

    1. System: Used for encoding, decoding, and string manipulation. `System.Text.StringBuilder` is utilized for efficient string building due to its mutable nature, which is advantageous over using the `+` operator for string concatenation. Employed for working with regular expressions, enabling pattern matching and manipulation of strings based on specific patterns. I used regular expressions extensively, such as for cleaning and extracting data from CSV fields and matching patterns in launch announcements and statuses. Provides generic collection classes for storing and managing data in a type-safe manner. I used `System.Collections.Generic.Dictionary` for storing cell phone data, where the key is an integer index representing the cell's position, and the value is an instance of the `Cell` class. Additionally, `List<Cell>` is used for storing cells with specific features and for statistical analysis.

    2. Microsoft: I used this library for accessing the `Microsoft.VisualBasic.FileIO.TextFieldParser` class. This class is part of the Microsoft.VisualBasic namespace, which provides functionality for working with basic file I/O operations in Visual Basic .NET. I used `TextFieldParser` in the `GenerateCellData` method of the `CellData` class. This method parses the CSV file data using `TextFieldParser`, which allows for efficient parsing of delimited text files, such as CSV files, by automatically handling issues like quoted fields, escaped characters, and delimiter-based separation. By using `TextFieldParser`, I was able to easily read and parse the CSV file, extracting the data needed to populate the cell database without having to manually implement the parsing logic. This enhanced readability, maintainability, and reduced the potential for errors.


### What company (oem) has the highest average weight of the phone body?
    The company with the highest average weight was HP with an average weight of 453.6 (g).

![Screenshot of MaxAvgWeightCell function output](<Screenshot 2024-04-20 at 3.50.26 PM.png>)


### Was there any phones that were announced in one year and released in another? What are they? Give me the oem and models.
    There were 4 Cell devices that were announced in one year and released in another:
        Motorola One Hyper
        Motorola Razr 2019
        Xiaomi Redmi K30 5G
        Xiaomi Mi Mix Alpha

![Screenshot of DelayedCells function output](<Screenshot 2024-04-20 at 3.49.38 PM.png>)


### How many phones have only one feature sensor?
    432

![Screenshot of OneFeatureCells function output](<Screenshot 2024-04-20 at 4.07.05 PM.png>)


### What year had the most phones launched in any year later than 1999? 
    2019

![Screenshot of PrintYearlyReleases subprogram output](<Screenshot 2024-04-20 at 3.50.53 PM.png>)

### Screenshot of Results: 
![Screenshot of function outputs for report](<Screenshot 2024-04-20 at 4.15.51 PM.png>)