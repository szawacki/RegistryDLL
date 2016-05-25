# Registry DLL

A direct link library for editing the windows registry.
As the .Net registry implementation has a poor performance in reading and write big binary, multireg_sz, or expanded string values, I created this dll using advapi32.dll to access the registry.
It's up to 16 times faster the the .Net framework implementation.

This repository contains the visual studio class library project and a corresponding test class.

## Documentation

Shame on me it's not yet finished.
Examples for the use of the regostry ddl can be found in the test class.
