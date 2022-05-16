# SharpAwareness
## In Development
This project is still in development. Feel free to send pull requeests or report issues.
Development roadmap (in order):
1. Complete host enum
    - RDP check
    - Network shares
    - EDRs installed
    - Local admins and groups
    - Remake process list - add parent child relationships
2. Add basic active directory recon - very minimal to avoid noise
    - Domain name
    - Domain Admins
    - Domain controller name
3. Add user awareness
    - ~~Foreground window~~
    - Logon logoff times
    - Screenshot of desktop

## Overview  
This is intended to be a light and more opsec friendly way for red teamers to gain quick situational awareness of both the host and the user. This is more oriented towards red team engagements than privilege escalation challenges, as it has a focus on the user and situational aspects of the system.  
The goals of executing this tool are to understand:  
- What is this host used for?
- What is the current user doing on this host?
- What kind of user is the current user?
- What kind of environment is this? (in general)

  
This .NET assembly is intended to be compatible with Beacon's execute-assembly builtin command as well as other .NET execution BOFs, such as inlineExecute-Assembly. I try to handle as much errors as I can, to avoid killing your beacons :)


## Things to note  
- This is NOT a privesc/enum tool, it just lets you get a rough idea of what goes on in the host.
- There is NO built in evasion/obfuscation techniques. I believe that evasion should be left to the operator, as it changes on a per-target basis. You are advised to be aware of things like AMSI, ETW, API hooks etc.
- This avoids things that require the use of commands, for OPSEC reasons. Everything here should be done via C# builtins or kernel32 API.

## Known visible events produced
- Registry queries
- Directory listing

I try to be as light as possible, but consider patching ETW.

## Information checked for
- 


## Credits
- https://github.com/threatexpress/red-team-scripts/blob/master/HostEnum.ps1
- https://gist.github.com/HarmJ0y/fe676e3ceba74f22a28bd1b121182db7

This was made as a quick utility while learning endpoint recon tradecraft.