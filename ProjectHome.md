Provides a custom runner for TeamCity which can run C# code supplied inline, in a way similar to what [LINQPad](http://www.linqpad.net) does.

Packages ready to use can be found [here](http://teamcity.jetbrains.com/project.html?projectId=project44&guest=1).

Deploying requires copying the .zip file to the TeamCity data directory under the plugins folder and restarting the server. The C# runner appears among the other runners.

Use just like you use LINQPad (without support for DataContexts, of course). You can .Dump objects, and the output appears on the fly on a custom tab.

You can also send service messages to TeamCity as described [here](http://confluence.jetbrains.net/display/TCD5/Build+Script+Interaction+with+TeamCity) with the extension methods available [here](http://code.google.com/p/teamcitycsharprunner/source/browse/trunk/externalProcess/CSharpCompiler/Runtime/Messages/TeamCityServiceMessagesExtensions.cs).