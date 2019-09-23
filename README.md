# Inconsistency demo

## A small set of applications that reproduces a weird bug we're having in our production environment.

## Background

In our production system, we found out that even though some messages failed during processing, some crud operations did not entirely roll back their transactions.

## Prerequisites

You need this installed on your machine

- SqlServer (I happen to have 2017 developer edition)
- MSDTC running
- MSMQ running
- Visual Studio (msbuild.exe)

## How to run

Run the `Runme.ps1` in the root of the repository. This will

1.	Create the database
2.	Nuget restore and build the applications
3.	Start all 3 applications

Once all three applications are running, you can start the processing by sending 100 messages to the `DemoService` input queue by pressing `enter` in the `MessageCreator` console window. Now verify that the DemoService is processing messages. It only takes a few minutes for inconsistencies to appear on my developer machine. When the `Trancount` property is 0 we cannot roll back crud operations.

![Console applications running](https://raw.githubusercontent.com/samegutt/InconsistencyDemo/master/doc/Inconsistency.console.png)

Verify using SQL:

![Transaction is not rolled back](https://raw.githubusercontent.com/samegutt/InconsistencyDemo/master/doc/Inconsistency.ssms.png)


We have not been able to reproduce without the transactional WCF call. In this example we also instruct NServiceBus to NOT to create a transaction. 

We have trimmed the example to the smallest amount of code possibly, only leaving logging and some database cruds for demo purposes. We have also disabled NServiceBus transaction handling, only leaving our own TransactionScope.

Can you please take a look at the code, and see if you can see anything else we might have missed? All the important bits are in the `DemoService\Handlers\DostuffHandler.cs` and `DemoService\Infrastructure\EndpointConfig.cs` files.
