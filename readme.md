# OctoSearch

[![NuGet](https://img.shields.io/nuget/v/OctoSearch.svg?maxAge=2592000)](https://www.nuget.org/packages/OctoSearch/) [![Gitter](https://img.shields.io/gitter/room/nwjs/nw.js.svg?maxAge=2592000)](https://gitter.im/OctoSearch/Lobby)

OctoSearch is a command line tool that downloads variable sets from Octopus, caches them locally and
allows you to search through the command line or a generated html single page application.

# Installation

OctoSearch itself is a package available from [nuget](https://www.nuget.org/packages/OctoSearch/). It has been compiled 
down to a native executable so a dotnet core installation is not needed.

You can also download a zip from the [releases](https://github.com/naeemkhedarun/OctoSearch/releases) page.

```
> nuget install OctoSearch
```

You can access the executable in the tools folder.

```
> cd .\OctoSearch*\tools
> .\OctoSearch.exe
```

## Building from source

You can clone this repository and create your own package using the build script.

This application is built with [dotnet core](https://www.microsoft.com/net/core) so you will need
to install the appropriate tools for your platform.

```
> cd build
> .\build.ps1
```

# Getting Started

## Login to Octopus

The first step is to login with the octopus server so we can create, download and cache an API token. This will be used
for subsequent calls to octopus. 

```
> .\OctoSearch.exe login -l https://octopus/ -u username
Please enter your pasword...
*********
Successfully logged in.
```

## Cache Variables locally

Now that we're authenticated we can download and cache the variable sets and their variable collections. This cache will
be used for our searches to reduce the load on the Octopus server. Variables marked as sensitive won't have their values 
downloaded or cached; their variable names will be searchable but not their values.

```
> .\OctoSearch.exe cache
Saved LibraryVariableSet1.
Saved LibraryVariableSet2.
...
```

## Search

With the variables cached locally you can run fast searches and regenerate them into either Json or Html documents. To run a basic
command line search you can use the `search` verb. It takes a regex so you can pass in basic text or more advanced text searches when
you need to.

```
> .\OctoSearch.exe search --regex connectionstring
Database.ConnectionString            ConnectionStringOne
ServiceBus.ConnectionString          ConnectionStringTwo
```

To output the search results into a text file you can do:

```
> .\OctoSearch.exe search --regex connectionstring --output-file results.txt
```

If you would prefer it in Json:

```
> .\OctoSearch.exe search --regex connectionstring --output-file results.json --output-format json
```

To display all the variables in a html report we omit the regex to default to a greedy regex `\w.`. The html report 
has a client side search facility to filter variables for easier exploration.

```
> .\OctoSearch.exe search --output-file results.html --output-format html
```

# Issues and feature requests

Please start raise an [issue](https://github.com/naeemkhedarun/OctoSearch/issues) on github for any bugs or 
features you would like to see.