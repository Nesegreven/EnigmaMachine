# Enigma Machine Simulator

A C# console application that simulates the historical Enigma encryption machine, with support for customizable rotors, reflector, ring settings, plugboard, and live encryption as you type.

## Features

- **Interactive Console UI**  
  Change rotors, ring settings, plugboard, and reflector on the fly.
- **Historical Rotor/Reflector Support**  
  Rotors I–V, UKW-B and UKW-C reflectors.
- **Ring Settings & Positions**  
  Correct handling of ringstellung and rotor positions, including double-stepping.
- **Plugboard**  
  Support for custom plugboard (Steckerbrett) wiring pairs.
- **Configuration Management**  
  Save/restore/reset configurations.
- **Live Encryption**  
  See output as you type.
- **Accurate Simulation**  
  Carefully follows historical stepping and encryption logic.

## How to Use

1. **Build and Run**  
   - Open in Visual Studio (or run `dotnet run` if using .NET CLI).

2. **Controls:**  
   - `ESC` – Exit program  
   - `F1` – Reset to default configuration  
   - `F2` – Set rotors and positions  
   - `F3` – Set plugboard  
   - `F4` – Set ring settings  
   - `F5` – Set reflector  
   - `F6` – Save current as default  
   - `F7` – Reset to initial configuration  
   - `F8` – Clear input/output text  
   - **Type letters** – Encrypt letter by letter

3. **Configuration Options:**  
   - **Rotors:** Choose from I, II, III, IV, V  
   - **Rotor Positions:** A–Z for each rotor  
   - **Ring Settings:** A–Z for each rotor  
   - **Plugboard:** Enter pairs (e.g., `AB CD EF`)  
   - **Reflector:** UKW-B or UKW-C

## Example
Rotor Types: I II III  
Rotor Positions: I-[A] II-[A] III-[G]  
Ring Settings: I-[A] II-[A] III-[A]  
Reflector: UKW-B  
Plugboard Connections:  
Configuration: Default | Initial Positions: AAA  
Plaintext: ENIGMA  
Encrypted: FQGAHW

## Notes

- Double-stepping and ring setting behaviors are implemented as per the historical machines.
- Plugboard swaps are symmetric (A–B means B–A).
- All state and configuration changes are reflected live.

