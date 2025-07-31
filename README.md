# Enigma Machine Simulator

A C# console application that simulates the historical Enigma encryption machine, with support for customizable rotors, reflector, ring settings, plugboard, and live encryption as you type.

## Features

- **Interactive Console UI**  
  Change rotors, ring settings, plugboard, and reflector on the fly.
- **Historical Rotor/Reflector Support**  
  Rotors I–V, UKW-B and UKW-C reflectors.
- **Ring Settings & Positions**  
  Handling ringstellung and rotor positions, including double-stepping.
- **Plugboard**  
  Support for custom plugboard (Steckerbrett) wiring pairs.
- **Configuration Management**  
  Save/restore/reset configurations.
- **Live Encryption**  
  See output as you type.

## How to Use

1. **Build and Run**  
   - Open in Visual Studio (or run `dotnet run` if using .NET CLI).

3. **Configuration Options:**  
   - **Rotors:** Choose from I, II, III, IV, V  
   - **Rotor Positions:** A–Z for each rotor  
   - **Ring Settings:** A–Z for each rotor  
   - **Plugboard:** Enter pairs (e.g., `AB CD EF`)  
   - **Reflector:** UKW-B or UKW-C

## Example
```
Enigma Machine

Commands:
  Machine Configuration (F1-F5)   | State Management (F6-F8)
  -----------------------------   | ----------------------------
  [F1] Set Rotors (Types & Pos)   | [F6] Save Current as Default
  [F2] Set Positions Only         | [F7] Reset to Custom Default
  [F3] Set Ring Settings          | [F8] Reset to Initial Default
  [F4] Set Plugboard              |
  [F5] Set Reflector              | [F9] Clear Text 

[ESC] Exit | Type to encrypt

Rotor Types: I II III
Rotor Positions: I-[A] II-[A] III-[G]
Ring Settings: I-[A] II-[A] III-[A]
Reflector: UKW-B
Plugboard Connections:
Configuration: Default | Starting Key: AAA

Plaintext: ENIGM A
Encrypted: FQGAH W
```

