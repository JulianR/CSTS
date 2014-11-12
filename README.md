CSTS
====

A simple library that can convert .NET types to TypeScript types. The output can be both a .ts file with classes as well as a .d.ts file with only the interface definitions.

### Installation

A nuget package is available:

    Install-Package CSTS 

### Basic usage:


```C#
var generator = new Generator(typeof(MyType).Assembly.GetExportedTypes());

 // generate classes and interfaces for a .ts file
string types = generator.GenerateTypes();

// only generate interfaces for a .d.ts file
string interfaceDefinitions = generator.GenerateInterfaceDefinitions(); 
```

By default, it will only generate TypeScript code for types that are in the same assembly as the assemblies of the types that you passed in. To change this behavior, pass in a `Func<Type, bool>` for the `processType` parameter of the `Generator` constructor that returns whether or not to process the type that it encountered. The same holds true for the `processBaseType` parameter; this should return whether or not the base type of a type should also be turned into TypeScript code. By default it only does this if the base type is part of the same assembly as the types you passed into the constructor. Additionally, the `moduleNameGenerator` function returns the name of the module that a given type should be part of. It defaults to a type's `Namespace` property.
