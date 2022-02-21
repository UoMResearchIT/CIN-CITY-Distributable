# CIN-CITY-Distributable
Civic InnovatioN in CommunITY (CIN-CITY): An open source, distributable version of the mobile app originally written by Research IT's Mobile Development Service. The private repository can be found [here](https://github.com/UoMResearchIT/CIN_CITY).

## Getting Started

### Technology
The app is written as a Visual Studio 2019 solution in C# using a .NET technology stack. It may be deployed for Android at present but it is written using Microsoft's cross-platform Xamarin.Forms framework which means it can easily be extended to compile for iOS with little additional effort since most of the code lives in a .NET Standard project within the solution which is shared between platforms. For more information on Xamarin.Forms, and how to get started with the technology, see the [online documentation](https://dotnet.microsoft.com/en-us/apps/xamarin/xamarin-forms). _Note that from 2022, Xamarin.Forms is evolving into .NET MAUI, Microsoft's next generation cross-platform mobile framework so this app may be migrated to the new iteration if necessary. Otherwise, it can still be used in its currnt form but new updates for Xamarin.Forms will stop from November._

### Backend
The app is configured to send data from participants to a backend data system. This public version of the software has no backend configured or any data communication API defined to allow you to link the app to whatever backend you choose. Instead, the code will throw an Exception when it expects to invoke a method to the backend and it is up to you to address these in your own version.

### Languages
The app may be translated into different languages using Xamarin.Forms localisation patterns. This version of the app is available in both English and Czech to illustrate this. These translations are coupled to Visual Studio build configurations which allows different translations to be built from the same code base.
