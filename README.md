# Registry DLL

A direct link library for editing the windows registry.
As the standard .Net framework registry implementation has a poor performance in reading and writing big REG_BINARY, REG_MULTI_SZ, or REG_EXPAND_SZ values, I created this dll using advapi32.dll to access the registry.
It's up to 16 times faster than the standard implementation.

This repository contains the visual studio class library project and a corresponding test class.

## Documentation

Shame on me it's not yet finished.
Examples for the use of the registry dll can be found in the test class.
