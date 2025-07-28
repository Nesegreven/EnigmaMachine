using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnigmaMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new Enigma machine with default settings
            var enigma = new Enigma(
                rotors: new[] { "I", "II", "III" },
                reflector: "UKW-B",
                ringSetting: "AAA",
                ringPosition: "AAA",
                plugboardPairs: ""
            );

            Console.Title = "Enigma Machine Simulator";

            // Setup colors for terminal
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            // Display initial interface
            DrawInterface(enigma);

            // Main input loop
            bool running = true;
            while (running)
            {
                // Get key input
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        running = false;
                        break;

                    case ConsoleKey.F1:
                        enigma.Reset();
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F2:
                        SetRotors(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F3:
                        SetPlugboard(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F4:
                        SetRingSettings(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F5:
                        SetReflector(enigma);
                        DrawInterface(enigma);
                        break;

                    default:
                        // Only process alphabetic characters
                        if (char.IsLetter(key.KeyChar))
                        {
                            // Process through enigma
                            var encryptedChar = enigma.EncryptChar(key.KeyChar);

                            // Update the interface
                            DrawInterface(enigma);
                        }
                        break;
                }
            }
        }

        static void DrawInterface(Enigma enigma)
        {
            Console.Clear();

            // Get current state
            var state = enigma.GetState();

            // Title
            Console.WriteLine("Enigma Machine\n");

            // Commands
            Console.WriteLine("Commands: [ESC] Exit | [F1] Reset | [F2] Set Rotors | [F3] Set Plugboard | [F4] Set Rings | [F5] Set Reflector | Type to encrypt\n");

            // Rotor display
            Console.Write("Rotor Types: ");
            for (int i = 0; i < state.RotorTypes.Length; i++)
            {
                Console.Write($"{state.RotorTypes[i]} ");
            }
            Console.WriteLine();

            Console.Write("Rotor Positions: ");
            for (int i = 0; i < state.RingPositions.Length; i++)
            {
                string rotorName = i == 0 ? "I" : i == 1 ? "II" : "III";
                Console.Write($"{rotorName}-[{state.RingPositions[i]}] ");
            }
            Console.WriteLine();

            Console.Write("Ring Settings: ");
            for (int i = 0; i < state.RingSettings.Length; i++)
            {
                string rotorName = i == 0 ? "I" : i == 1 ? "II" : "III";
                Console.Write($"{rotorName}-[{state.RingSettings[i]}] ");
            }
            Console.WriteLine();

            // Reflector type
            Console.WriteLine($"Reflector: {state.ReflectorType}");

            // Plugboard
            Console.Write("Plugboard Connections: ");
            var plugboardPairs = state.Plugboard
                .Where(kvp => kvp.Key < kvp.Value)
                .Select(kvp => $"{kvp.Key}{kvp.Value}");
            Console.WriteLine(string.Join(" ", plugboardPairs));

            // Text display areas
            Console.WriteLine($"\nPlaintext: {state.Plaintext}");
            Console.WriteLine($"Encrypted: {state.Ciphertext}");
        }

        static void SetRotors(Enigma enigma)
        {
            Console.Clear();
            Console.WriteLine("Set Rotor Types and Positions");
            Console.WriteLine("----------------------------");

            // Select rotor types
            string[] rotorTypes = new string[3];
            string[] positions = new string[3] { "I", "II", "III" };

            for (int i = 0; i < 3; i++)
            {
                bool validInput = false;
                while (!validInput)
                {
                    Console.Write($"Rotor {positions[i]} Type (I-V): ");
                    var input = Console.ReadLine().ToUpper();

                    if (input == "I" || input == "II" || input == "III" || input == "IV" || input == "V")
                    {
                        rotorTypes[i] = input;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid rotor type (I, II, III, IV, or V).");
                    }
                }
            }

            // Set rotor positions
            char[] rotorPositions = new char[3];

            for (int i = 0; i < 3; i++)
            {
                bool validInput = false;
                while (!validInput)
                {
                    Console.Write($"Rotor {positions[i]} Position (A-Z): ");
                    var input = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    if (char.IsLetter(input))
                    {
                        rotorPositions[i] = char.ToUpper(input);
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a letter A-Z.");
                    }
                }
            }

            // Apply new settings
            enigma.SetRotors(rotorTypes, new string(rotorPositions));
        }

        static void SetPlugboard(Enigma enigma)
        {
            Console.Clear();
            Console.WriteLine("Set Plugboard Connections");
            Console.WriteLine("------------------------");
            Console.WriteLine("Enter pairs of letters, space separated (e.g. AB CD EF)");
            Console.Write("> ");

            string input = Console.ReadLine().ToUpper();

            // Apply new settings
            enigma.SetPlugboard(input);
        }

        static void SetRingSettings(Enigma enigma)
        {
            Console.Clear();
            Console.WriteLine("Set Ring Settings");
            Console.WriteLine("----------------");

            // Set ring settings
            char[] ringSettings = new char[3];
            string[] positions = new string[3] { "I", "II", "III" };

            for (int i = 0; i < 3; i++)
            {
                bool validInput = false;
                while (!validInput)
                {
                    Console.Write($"Ring {positions[i]} Setting (A-Z): ");
                    var input = Console.ReadKey().KeyChar;
                    Console.WriteLine();

                    if (char.IsLetter(input))
                    {
                        ringSettings[i] = char.ToUpper(input);
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a letter A-Z.");
                    }
                }
            }

            // Apply new settings
            enigma.SetRingSettings(new string(ringSettings));
        }

        static void SetReflector(Enigma enigma)
        {
            Console.Clear();
            Console.WriteLine("Set Reflector Type");
            Console.WriteLine("-----------------");
            Console.WriteLine("Options: UKW-B or UKW-C");
            Console.Write("> ");

            string input = Console.ReadLine().ToUpper();

            if (input == "UKW-B" || input == "B")
            {
                enigma.SetReflector("UKW-B");
            }
            else if (input == "UKW-C" || input == "C")
            {
                enigma.SetReflector("UKW-C");
            }
            else
            {
                Console.WriteLine("Invalid option. Using UKW-B.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                enigma.SetReflector("UKW-B");
            }
        }
    }

    /// <summary>
    /// Enigma machine implementation with historical accuracy
    /// </summary>
    class Enigma
    {
        // Constants
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Rotor wirings (historical Enigma rotors I-V)
        private readonly Dictionary<string, string> rotorWirings = new Dictionary<string, string>
        {
            { "I", "EKMFLGDQVZNTOWYHXUSPAIBRCJ" },
            { "II", "AJDKSIRUXBLHWTMCQGZNPYFVOE" },
            { "III", "BDFHJLCPRTXVZNYEIWGAKMUSQO" },
            { "IV", "ESOVPZJAYQUIRHXLNFTGKDCMWB" },
            { "V", "VZBRGITYUPSDNHLXAWMJQOFECK" }
        };

        // Rotor notches (position where the next rotor steps)
        private readonly Dictionary<string, char> rotorNotches = new Dictionary<string, char>
        {
            { "I", 'Q' },    // Notch at Q
            { "II", 'E' },   // Notch at E
            { "III", 'V' },  // Notch at V
            { "IV", 'J' },   // Notch at J
            { "V", 'Z' }     // Notch at Z
        };

        // Reflector wirings
        private readonly Dictionary<string, Dictionary<char, char>> reflectorWirings = new Dictionary<string, Dictionary<char, char>>();

        // Configuration
        private string[] rotorTypes; // Which rotors are used (I-V)
        private string reflectorType; // Which reflector (UKW-B or UKW-C)
        private char[] ringSettings; // Ring settings for each rotor
        private char[] ringPositions; // Current position of each rotor

        // Plugboard connections
        private Dictionary<char, char> plugboard;

        // For tracking input/output
        private StringBuilder plaintext;
        private StringBuilder ciphertext;

        public Enigma(string[] rotors = null, string reflector = "UKW-B", string ringSetting = "AAA", string ringPosition = "AAA", string plugboardPairs = "")
        {
            // Set up reflector wirings
            SetupReflectors();

            // Initialize configuration
            rotorTypes = rotors?.ToArray() ?? new[] { "I", "II", "III" };
            reflectorType = reflector;
            ringSettings = ringSetting.ToCharArray();
            ringPositions = ringPosition.ToCharArray();

            plaintext = new StringBuilder();
            ciphertext = new StringBuilder();

            // Set up plugboard
            plugboard = new Dictionary<char, char>();
            SetPlugboard(plugboardPairs);
        }

        private void SetupReflectors()
        {
            // Set up UKW-B reflector (historical)
            var reflectorB = new Dictionary<char, char>();
            var reflectorBPairs = new[] {
                "AY", "BR", "CU", "DH", "EQ", "FS", "GL", "IP", "JX", "KN", "MO", "TZ", "VW"
            };

            foreach (var pair in reflectorBPairs)
            {
                reflectorB[pair[0]] = pair[1];
                reflectorB[pair[1]] = pair[0];
            }

            // Set up UKW-C reflector (historical)
            var reflectorC = new Dictionary<char, char>();
            var reflectorCPairs = new[] {
                "AF", "BV", "CP", "DJ", "EI", "GO", "HY", "KR", "LZ", "MX", "NW", "QT", "SU"
            };

            foreach (var pair in reflectorCPairs)
            {
                reflectorC[pair[0]] = pair[1];
                reflectorC[pair[1]] = pair[0];
            }

            reflectorWirings["UKW-B"] = reflectorB;
            reflectorWirings["UKW-C"] = reflectorC;
        }

        // Apply Caesar shift to a string (used for ring settings)
        private string CaesarShift(string input, int shift)
        {
            if (shift == 0) return input;

            var result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    int code = c - 'A';
                    code = (code + shift) % 26;
                    result.Append((char)(code + 'A'));
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        // Rotate rotors according to Enigma mechanics
        private void RotateRotors()
        {
            // Check for notch positions to implement double stepping
            bool rotorANotch = ringPositions[0] == rotorNotches[rotorTypes[0]];
            bool rotorBNotch = ringPositions[1] == rotorNotches[rotorTypes[1]];
            bool rotorCNotch = ringPositions[2] == rotorNotches[rotorTypes[2]];

            // Step the middle rotor if either:
            // 1. Right rotor (C) is at its notch position
            // 2. Middle rotor (B) is at its notch position (double-stepping)
            bool stepMiddle = rotorCNotch || rotorBNotch;

            // Step the left rotor (A) if the middle rotor (B) is at its notch position
            bool stepLeft = rotorBNotch;

            // Always step the right rotor (C)
            ringPositions[2] = (char)(((ringPositions[2] - 'A' + 1) % 26) + 'A');

            // Step the middle rotor (B) if triggered
            if (stepMiddle)
            {
                ringPositions[1] = (char)(((ringPositions[1] - 'A' + 1) % 26) + 'A');
            }

            // Step the left rotor (A) if triggered
            if (stepLeft)
            {
                ringPositions[0] = (char)(((ringPositions[0] - 'A' + 1) % 26) + 'A');
            }
        }

        // Process a single character through the Enigma machine
        private char ProcessChar(char input)
        {
            if (!char.IsLetter(input))
            {
                return input;
            }

            // Convert to uppercase
            char c = char.ToUpper(input);

            // Apply plugboard
            if (plugboard.TryGetValue(c, out char plugboardOutput))
            {
                c = plugboardOutput;
            }

            // Step the rotors
            RotateRotors();

            // Get effective rotor wirings considering ring settings
            string[] effectiveRotors = new string[3];
            for (int i = 0; i < 3; i++)
            {
                // Apply ring setting offset to rotor wiring
                int ringSetting = ringSettings[i] - 'A';

                if (ringSetting > 0)
                {
                    // Apply Caesar shift, then rotate the string
                    string shiftedRotor = CaesarShift(rotorWirings[rotorTypes[i]], ringSetting);
                    effectiveRotors[i] = shiftedRotor.Substring(26 - ringSetting) + shiftedRotor.Substring(0, 26 - ringSetting);
                }
                else
                {
                    effectiveRotors[i] = rotorWirings[rotorTypes[i]];
                }
            }

            // Get current offsets from rotor positions
            int offsetA = ringPositions[0] - 'A';
            int offsetB = ringPositions[1] - 'A';
            int offsetC = ringPositions[2] - 'A';

            // Convert letter to number (A=0, B=1, etc.)
            int pos = c - 'A';

            // Right to left through the rotors (C -> B -> A)

            // Through rotor C (right)
            int shifted = (pos + offsetC) % 26;
            char let = effectiveRotors[2][shifted];
            pos = let - 'A';
            pos = (pos - offsetC + 26) % 26;

            // Through rotor B (middle)
            shifted = (pos + offsetB) % 26;
            let = effectiveRotors[1][shifted];
            pos = let - 'A';
            pos = (pos - offsetB + 26) % 26;

            // Through rotor A (left)
            shifted = (pos + offsetA) % 26;
            let = effectiveRotors[0][shifted];
            pos = let - 'A';
            pos = (pos - offsetA + 26) % 26;

            // Through reflector
            char reflectorChar = ' ';
            var currentReflector = reflectorWirings[reflectorType];
            if (currentReflector.TryGetValue((char)(pos + 'A'), out reflectorChar))
            {
                pos = reflectorChar - 'A';
            }

            // Left to right through the rotors (A -> B -> C)

            // Through rotor A (left)
            shifted = (pos + offsetA) % 26;
            int index = effectiveRotors[0].IndexOf((char)(shifted + 'A'));
            pos = (index - offsetA + 26) % 26;

            // Through rotor B (middle)
            shifted = (pos + offsetB) % 26;
            index = effectiveRotors[1].IndexOf((char)(shifted + 'A'));
            pos = (index - offsetB + 26) % 26;

            // Through rotor C (right)
            shifted = (pos + offsetC) % 26;
            index = effectiveRotors[2].IndexOf((char)(shifted + 'A'));
            pos = (index - offsetC + 26) % 26;

            // Convert back to letter
            c = (char)(pos + 'A');

            // Apply plugboard again
            if (plugboard.TryGetValue(c, out plugboardOutput))
            {
                c = plugboardOutput;
            }

            return c;
        }

        // Process a string through the Enigma machine
        public string ProcessText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var result = new StringBuilder(text.Length);

            foreach (char c in text)
            {
                result.Append(ProcessChar(c));
            }

            return result.ToString();
        }

        // Encrypt a single character and update internal state
        public char EncryptChar(char input)
        {
            if (char.IsLetter(input))
            {
                plaintext.Append(char.ToUpper(input));
                char encrypted = ProcessChar(input);
                ciphertext.Append(encrypted);
                return encrypted;
            }
            else
            {
                plaintext.Append(input);
                ciphertext.Append(input);
                return input;
            }
        }

        // Get current state for display
        public (string[] RotorTypes, char[] RingPositions, char[] RingSettings, string ReflectorType, Dictionary<char, char> Plugboard, string Plaintext, string Ciphertext) GetState()
        {
            return (
                rotorTypes.ToArray(),
                ringPositions.ToArray(),
                ringSettings.ToArray(),
                reflectorType,
                new Dictionary<char, char>(plugboard),
                plaintext.ToString(),
                ciphertext.ToString()
            );
        }

        // Set or update rotors
        public void SetRotors(string[] types, string positions)
        {
            rotorTypes = types.ToArray();
            ringPositions = positions.ToUpper().ToCharArray();

            // Reset text
            plaintext.Clear();
            ciphertext.Clear();
        }

        // Set or update ring settings
        public void SetRingSettings(string settings)
        {
            ringSettings = settings.ToUpper().ToCharArray();

            // Reset text
            plaintext.Clear();
            ciphertext.Clear();
        }

        // Set or update plugboard
        public void SetPlugboard(string pairs)
        {
            plugboard.Clear();

            // Parse plugboard settings
            var connections = pairs.ToUpper().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in connections)
            {
                if (pair.Length == 2 &&
                    char.IsLetter(pair[0]) &&
                    char.IsLetter(pair[1]) &&
                    !plugboard.ContainsKey(pair[0]) &&
                    !plugboard.ContainsKey(pair[1]))
                {
                    plugboard[pair[0]] = pair[1];
                    plugboard[pair[1]] = pair[0];
                }
            }

            // Reset text
            plaintext.Clear();
            ciphertext.Clear();
        }

        // Set or update reflector
        public void SetReflector(string type)
        {
            if (reflectorWirings.ContainsKey(type))
            {
                reflectorType = type;

                // Reset text
                plaintext.Clear();
                ciphertext.Clear();
            }
        }

        // Reset the machine
        public void Reset()
        {
            rotorTypes = new[] { "I", "II", "III" };
            reflectorType = "UKW-B";
            ringSettings = new[] { 'A', 'A', 'A' };
            ringPositions = new[] { 'A', 'A', 'A' };

            plugboard.Clear();

            plaintext.Clear();
            ciphertext.Clear();
        }
    }
}

