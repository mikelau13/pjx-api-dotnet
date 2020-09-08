
I added some simple codes to demostrate my programming knowledge of few topics that we talked about duiring our talks, such that Dependency Injection, test automation.


## What is this library trying to do?

The codes I wrote here is to simulate a hospital complex staffing software, there were many rules to help the hospital administrators to make decisions when assigning events to staffs, such that check for overlapping events, control overtime paying, balance shift assignment (for example - avoid assigning too many weekend shifts to staffs, etc) and many others.

For time being, I developed only one rule and very few test cases; coming soon, I will construct a more complex conflict check chain using DI Container (instead of hardcoding) in runtime, because different hostipals might use different set of rules and configurations.


## About the codes and patterns

Entity Framework Core with Repository Pattern and Unit of Work Pattern, example [here](https://github.com/mikelau13/pjx-api-dotnet/tree/master/src/Pjx_Api/Data/UnitOfWork.cs), so that I can easily inject any repositories and entities when needed.

For the Dependency Injection, [Autofac](https://autofac.org/) IoC container is being used to mock the repository in TDD, see example codes in [OverlappingCheckTests.cs](https://github.com/mikelau13/pjx-api-dotnet/tree/master/src/Pjx.Calendar_Test/ConflictChecks/OverlappingCheckTests.cs).

<br/><img src="/images/api_di_unittest.png" alt="Autofac" />

Chain of Responsibility design pattern is applied to manage the rules, see example class [EventCheckAbstract.cs](https://github.com/mikelau13/pjx-api-dotnet/tree/master/src/Pjx.CalendarLibrary/ConflictChecks/EventCheckAbstract.cs)

<br/><img src="/images/api_chain_conflictcheck.png" alt="Chain of Responsibility design pattern" />

To read the codes and projects, they are located in this folder - https://github.com/mikelau13/pjx-api-dotnet/tree/master/src/ 
