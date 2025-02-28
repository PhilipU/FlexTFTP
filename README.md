# FlexTFTP

![GitHub Release](https://img.shields.io/github/v/release/PhilipU/FlexTFTP)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/PhilipU/FlexTFTP/build_and_test.yml)
[![Renovate enabled](https://img.shields.io/badge/renovate-enabled-brightgreen.svg)](https://renovatebot.com/)

![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=flat=csharp&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=flat&logo=.net&logoColor=white)
![Windows](https://img.shields.io/badge/Windows-0078D6?style=flat&logo=windows&logoColor=white)
![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=flat&logo=visual-studio&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-ff8000?style=flat&logo=windows&logoColor=white)

TFTP Firmware Update Tool with support of special SRecord (*.s19) flash files.

![FlexTFTP UI](docs/screenshot-1.png)

## Update

Integrated auto-update via GitHub releases.

## Hotkeys

Enter - Start transfer \
ESC   - Stop transfer

## Command line parameters

FlexTFTP.exe

    [Path | "auto"]
    [IP | "last"]
    [Port | "last"]
    [Afterwards action "close"]

### TargetFile

    File path to file which should 
    be flashed

### Path

    Target path 
    (e.g. "cpu/application"). 
    Set to "auto" if auto-path option 
    should be used.

### IP

    Target IPv4 address.
    Set to "last" if last saved IP 
    should be used.

### Port

    Target port (e.g. 69)
    Set to "last" if last saved port 
    should be used.

### Action

    Action which should be performed
    after transfer.
    This action is performed even 
    if transfer failed.
    "notclose" = Application will not be closed.
    "close" = Application will be closed 
    without delay.
    Default behavior will close application
    after a few seconds.

#### Example

    FlexTFTP "myFile.s19"
    FlexTFTP "myFile.s19" auto last last close
    FlexTFTP "myFile.s19" cpu/preloader
    FlexTFTP "myFile.s19" auto 10.50.0.10 69 close

## Alias File

Create 'alias.flextftp' file within
flash file folder and add path alias
as text.