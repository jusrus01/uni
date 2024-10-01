========================================================================
    netCard_s1 Wizard : "netCard_s1" Project Overview
========================================================================

netCard_s1 Wizard has created this "netCard_s1" project for you as a starting point.

This file contains a summary of what you will find in each of the files that make up your project.

netCard_s1.csproj
    This is the main project file of your server application.

MyServer.cs
    This is the main class of your server.
    Purpose of this class is to register the remote object MyServices during execution of the main.    

MyServices.cs
    This class is the exposed remote object: your server services.
    
AssemblyInfo.cs
    Because the .NetCard will accept only Strong-Name signed applications/libraries, we've provided
    a dummy keypair and activated the assembly signing.
    
DummyKeyPair.snk
    Keypair created for development purpose. Note that this wizard always provides the same keypair.
    (You can create your own keypair using the sn tool)

nant.build
    This file is an xml based script using NAnt [http://nant.sourceforge.net/] tasks. .Net Smartcard
    framework comes with custom tasks which helps in automation of interaction with card & provide
    unified command line interface.
    If you are using explorer as an AddIn in VS.NET, clicking on button with Ant icon will execute this
    script. For standalone CardExplorer, clicking on button with Ant icon will show a dialog to select the
    script file and then execute it.
    This script can used separately from command line.