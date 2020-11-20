; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Wally"
#define MyAppVersion "3.0.0Beta1"
#define MyAppPublisher "ZSA Technology Labs"
#define MyAppURL "https://www.zsa.io"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{AD29FB2C-766F-4272-9BB1-BEAC40CA627E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
OutputBaseFilename=wally
SetupIconFile=..\Wally\Wally.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "Resources\*.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "Resources\*.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Run]
Filename: {app}\wdi-simple.exe; Flags: "runhidden"; Parameters: " -x --type 0 --name ""Keyboard in reset mode"" --vid 0x0483 --pid 0xDF11"; StatusMsg: "Installing driver files";
Filename: {win}\sysnative\pnputil.exe; Flags: "runhidden"; Parameters: " -i -a usb_device.inf"; WorkingDir: "{app}\usb_driver";