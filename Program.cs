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
                    // --- Core Encryption ---
                    default:
                        if (char.IsLetter(key.KeyChar))
                        {
                            enigma.EncryptChar(key.KeyChar);
                            DrawInterface(enigma);
                        }
                        break;

                    // --- Machine Configuration ---
                    case ConsoleKey.F1: // Set Rotors (Types & Positions)
                        SetRotors(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F2: // Set Positions Only
                        SetRotorPositionsOnly(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F3: // Set Ring Settings
                        SetRingSettings(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F4: // Set Plugboard
                        SetPlugboard(enigma);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F5: // Set Reflector
                        SetReflector(enigma);
                        DrawInterface(enigma);
                        break;

                    // --- State Management ---
                    case ConsoleKey.F6: // Save Current as Default
                        enigma.SaveCurrentAsDefault();
                        Console.Clear();
                        Console.WriteLine("Current configuration saved as default!");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey(true);
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F7: // Reset to Custom Default
                        enigma.Reset();
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.F8: // Reset to Initial "Factory" Default
                        enigma.ResetToInitial();
                        DrawInterface(enigma);
                        break;

                    // --- Utilities & Program Control ---
                    case ConsoleKey.F9: // Clear Text
                        enigma.ClearText();
                        DrawInterface(enigma);
                        break;

                    case ConsoleKey.Escape: // Exit
                        running = false;
                        break;
                }
            }
        }

        static string FormatTextIntoGroups(string text, int groupSize = 5)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            var formattedText = new System.Text.StringBuilder();

            for (int i = 0; i < text.Length; i++)
            {
                formattedText.Append(text[i]);

                // Add a space after a group is complete, but not at the very end of the string
                if ((i + 1) % groupSize == 0 && (i + 1) < text.Length)
                {
                    formattedText.Append(' ');
                }
            }

            return formattedText.ToString();
        }
        static void DrawInterface(Enigma enigma)
        {
            Console.Clear();

            // Get current state
            var state = enigma.GetState();

            // Title
            Console.WriteLine("Enigma Machine\n");

            // Commands
            Console.WriteLine("Commands:");
            Console.WriteLine("  Machine Configuration (F1-F5)   | State Management (F6-F8)");
            Console.WriteLine("  -----------------------------   | ----------------------------");
            Console.WriteLine("  [F1] Set Rotors (Types & Pos)   | [F6] Save Current as Default");
            Console.WriteLine("  [F2] Set Positions Only         | [F7] Reset to Custom Default");
            Console.WriteLine("  [F3] Set Ring Settings          | [F8] Reset to Initial Default");
            Console.WriteLine("  [F4] Set Plugboard              | ");
            Console.WriteLine("  [F5] Set Reflector              | [F9] Clear Text ");
            Console.WriteLine("\n[ESC] Exit | Type to encrypt\n");

            // Configuration display
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

            // Configuration status
            Console.WriteLine($"Configuration: {(state.IsDefault ? "Default" : "Custom")} | Starting Key: {state.StartingKey}");

            // Text display areas
            Console.WriteLine($"\nPlaintext: {FormatTextIntoGroups(state.Plaintext)}");
            Console.WriteLine($"Encrypted: {FormatTextIntoGroups(state.Ciphertext)}");


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

        static void SetRotorPositionsOnly(Enigma enigma)
        {
            Console.Clear();
            Console.WriteLine("Set Rotor Positions");
            Console.WriteLine("------------------");

            char[] rotorPositions = new char[3];
            string[] positions = new string[3] { "I", "II", "III" };

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

            // Apply new settings using our new method
            enigma.SetRotorPositions(new string(rotorPositions));
        }

        static bool ValidatePlugboardInput(string input, out string errorMessage)
        {
            errorMessage = string.Empty;
            if (string.IsNullOrWhiteSpace(input))
            {
                return true; // Empty input is valid (no plugboard connections)
            }

            // Convert to uppercase and split by spaces
            var connections = input.ToUpper().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var usedLetters = new HashSet<char>();

            foreach (var pair in connections)
            {
                if (pair.Length != 2)
                {
                    errorMessage = $"Invalid pair '{pair}'. Each connection must be exactly two letters (e.g., 'AB').";
                    return false;
                }

                char char1 = pair[0];
                char char2 = pair[1];

                if (!char.IsLetter(char1) || !char.IsLetter(char2))
                {
                    errorMessage = $"Invalid characters in pair '{pair}'. Only letters A-Z are allowed.";
                    return false;
                }

                if (char1 == char2)
                {
                    errorMessage = $"Invalid pair '{pair}'. A letter cannot be connected to itself.";
                    return false;
                }

                if (usedLetters.Contains(char1))
                {
                    errorMessage = $"Letter '{char1}' is used multiple times in plugboard connections.";
                    return false;
                }
                if (usedLetters.Contains(char2))
                {
                    errorMessage = $"Letter '{char2}' is used multiple times in plugboard connections.";
                    return false;
                }

                usedLetters.Add(char1);
                usedLetters.Add(char2);
            }

            return true;
        }

        static void SetPlugboard(Enigma enigma)
        {
            bool validInput = false;
            string input = string.Empty;
            string validationError = string.Empty;

            while (!validInput)
            {
                Console.Clear();
                Console.WriteLine("Set Plugboard Connections");
                Console.WriteLine("------------------------");
                Console.WriteLine("Enter pairs of letters, space separated (e.g. AB CD EF)");
                Console.WriteLine("Leave blank for no connections.");

                if (!string.IsNullOrEmpty(validationError))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInput Error:");
                    Console.WriteLine(validationError);
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write("> ");
                input = Console.ReadLine().ToUpper();

                if (ValidatePlugboardInput(input, out validationError))
                {
                    validInput = true;
                }
                else
                {
                    // If input is invalid, the loop will continue, and the error will be displayed
                    // No need for Console.ReadKey here, as the prompt will reappear.
                }
            }

            // Apply new settings only if input is valid
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
                    Console.Write($"Ring {positions[i]} Setting (A-Z or 1-26): ");
                    string input = Console.ReadLine().Trim().ToUpper();

                    // Numeric input
                    if (int.TryParse(input, out int num))
                    {
                        if (num >= 1 && num <= 26)
                        {
                            ringSettings[i] = (char)('A' + (num - 1));
                            validInput = true;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid number (1-26) or letter (A-Z).");
                        }
                    }
                    // Single letter input
                    else if (input.Length == 1 && input[0] >= 'A' && input[0] <= 'Z')
                    {
                        ringSettings[i] = input[0];
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid number (1-26) or letter (A-Z).");
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
    /// Configuration structure to hold Enigma settings
    /// Encapsulates all machine parameters for easy state management
    /// </summary>
    public struct EnigmaConfiguration
    {
        public string[] RotorTypes;      // Which physical rotors are installed (I-V)
        public string ReflectorType;     // Which reflector is used (UKW-B or UKW-C)
        public char[] RingSettings;      // Ring settings (Ringstellung) - internal wiring offset
        public char[] RingPositions;     // Current rotor positions visible in windows
        public string PlugboardPairs;   // Plugboard cable connections as string pairs

        public EnigmaConfiguration(string[] rotorTypes, string reflectorType,
            char[] ringSettings, char[] ringPositions, string plugboardPairs)
        {
            RotorTypes = (string[])rotorTypes?.Clone() ?? new[] { "I", "II", "III" };
            ReflectorType = reflectorType ?? "UKW-B";
            RingSettings = (char[])ringSettings?.Clone() ?? new[] { 'A', 'A', 'A' };
            RingPositions = (char[])ringPositions?.Clone() ?? new[] { 'A', 'A', 'A' };
            PlugboardPairs = plugboardPairs ?? "";
        }

        public EnigmaConfiguration Clone()
        {
            return new EnigmaConfiguration(
                (string[])RotorTypes.Clone(),
                ReflectorType,
                (char[])RingSettings.Clone(),
                (char[])RingPositions.Clone(),
                PlugboardPairs
            );
        }
    }

    /// <summary>
    /// Enigma machine implementation with state management
    /// </summary>
    class Enigma
    {
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

        // Configuration management
        private EnigmaConfiguration initialConfiguration;  // Configuration at startup
        private EnigmaConfiguration defaultConfiguration;  // User-defined default
        private EnigmaConfiguration currentConfiguration;  // Current active config

        // Current state
        private string[] rotorTypes; // Which rotors are used (I-V)
        private string reflectorType; // Which reflector (UKW-B or UKW-C)
        private char[] ringSettings; // Ring settings for each rotor
        private char[] ringPositions; // Current position of each rotor
        private Dictionary<char, char> plugboard; // Plugboard connections

        private char[] messageKeyPositions; // Holds the starting key for the current message

        // For tracking input/output
        private StringBuilder plaintext;  // Accumulates original input text
        private StringBuilder ciphertext; // Accumulates encrypted output text

        public Enigma(string[] rotors = null, string reflector = "UKW-B",
            string ringSetting = "AAA", string ringPosition = "AAA", string plugboardPairs = "")
        {
            // Set up reflector wirings
            SetupReflectors();

            // Initialize configurations
            initialConfiguration = new EnigmaConfiguration(
                rotors ?? new[] { "I", "II", "III" },
                reflector,
                ringSetting.ToCharArray(),
                ringPosition.ToCharArray(),
                plugboardPairs
            );

            // Default starts as initial
            defaultConfiguration = initialConfiguration.Clone();

            // Apply initial configuration
            ApplyConfiguration(initialConfiguration);

            plaintext = new StringBuilder();
            ciphertext = new StringBuilder();
        }

        private void SetupReflectors()
        {
            // Set up UKW-B reflector (historical Enigma reflector wiring)
            // The reflector connects pairs of contacts, creating a symmetric substitution
            var reflectorB = new Dictionary<char, char>();
            var reflectorBPairs = new[] {
                "AY", "BR", "CU", "DH", "EQ", "FS", "GL", "IP", "JX", "KN", "MO", "TZ", "VW"
            };

            foreach (var pair in reflectorBPairs)
            {
                // Reflector connections are bidirectional (A->Y and Y->A)
                reflectorB[pair[0]] = pair[1];
                reflectorB[pair[1]] = pair[0];
            }

            // Set up UKW-C reflector (alternative historical reflector)
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

        private void ApplyConfiguration(EnigmaConfiguration config)
        {
            rotorTypes = (string[])config.RotorTypes.Clone();
            reflectorType = config.ReflectorType;
            ringSettings = (char[])config.RingSettings.Clone();
            ringPositions = (char[])config.RingPositions.Clone();
            messageKeyPositions = (char[])config.RingPositions.Clone();

            // Set up plugboard
            plugboard = new Dictionary<char, char>();
            SetupPlugboard(config.PlugboardPairs);

            // Update current configuration
            currentConfiguration = config.Clone();
        }

        private void SetupPlugboard(string pairs)
        {
            plugboard.Clear();

            // Parse plugboard settings - format: "AB CD EF" (space-separated pairs)
            // Each pair represents a physical cable connecting two letters on the plugboard
            var connections = pairs.ToUpper().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in connections)
            {
                // Validate: exactly 2 letters, both unused in other connections
                if (pair.Length == 2 &&
                    char.IsLetter(pair[0]) &&
                    char.IsLetter(pair[1]) &&
                    !plugboard.ContainsKey(pair[0]) &&
                    !plugboard.ContainsKey(pair[1]))
                {
                    // Plugboard connections are bidirectional (A<->B means A->B and B->A)
                    plugboard[pair[0]] = pair[1];
                    plugboard[pair[1]] = pair[0];
                }
            }
        }

        private void UpdateCurrentConfiguration()
        {
            currentConfiguration = new EnigmaConfiguration(
                rotorTypes,
                reflectorType,
                ringSettings,
                ringPositions,
                GetPlugboardPairs()
            );
        }

        private string GetPlugboardPairs()
        {
            var pairs = plugboard
                .Where(kvp => kvp.Key < kvp.Value)
                .Select(kvp => $"{kvp.Key}{kvp.Value}");
            return string.Join(" ", pairs);
        }

        // Apply Caesar shift to a string (used for ring settings)
        // This simulates the physical offset of rotor wiring relative to the rotor body
        private string CaesarShift(string input, int shift)
        {
            if (shift == 0) return input;

            var result = new StringBuilder(input.Length);

            foreach (char c in input)
            {
                if (char.IsLetter(c))
                {
                    // Shift each letter by the specified amount, wrapping around the alphabet
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
        // This implements the "double-stepping" mechanism of the historical Enigma
        private void RotateRotors()
        {
            // Check for notch positions to implement double stepping
            // The notch determines when a rotor causes the next rotor to step
            bool rotorANotch = ringPositions[0] == rotorNotches[rotorTypes[0]];
            bool rotorBNotch = ringPositions[1] == rotorNotches[rotorTypes[1]];
            bool rotorCNotch = ringPositions[2] == rotorNotches[rotorTypes[2]];

            // Step the middle rotor if either:
            // 1. Right rotor (C) is at its notch position (normal stepping)
            // 2. Middle rotor (B) is at its notch position (double-stepping anomaly)
            // The double-stepping occurs because the middle rotor steps again when it's at its notch
            bool stepMiddle = rotorCNotch || rotorBNotch;

            // Step the left rotor (A) if the middle rotor (B) is at its notch position
            bool stepLeft = rotorBNotch;

            // Always step the right rotor (C) - this happens before every encryption
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
        // This implements the complete signal path: plugboard -> rotors -> reflector -> rotors -> plugboard
        private char ProcessChar(char input)
        {
            if (!char.IsLetter(input))
            {
                return input;
            }

            // Convert to uppercase - Enigma only processed uppercase letters
            char c = char.ToUpper(input);

            // Apply plugboard transformation (if any connections exist)
            // The plugboard swaps pairs of letters before and after rotor processing
            if (plugboard.TryGetValue(c, out char plugboardOutput))
            {
                c = plugboardOutput;
            }

            // Step the rotors BEFORE processing the character (historical accuracy)
            RotateRotors();

            // Get effective rotor wirings considering ring settings
            // Ring settings offset the internal wiring relative to the visible rotor position
            string[] effectiveRotors = new string[3];
            for (int i = 0; i < 3; i++)
            {
                // Apply ring setting offset to rotor wiring
                int ringSetting = ringSettings[i] - 'A';

                if (ringSetting > 0)
                {
                    // Apply Caesar shift to the wiring, then rotate the string
                    // This simulates the physical offset of the wiring inside the rotor
                    string shiftedRotor = CaesarShift(rotorWirings[rotorTypes[i]], ringSetting);
                    effectiveRotors[i] = shiftedRotor.Substring(26 - ringSetting) + shiftedRotor.Substring(0, 26 - ringSetting);
                }
                else
                {
                    effectiveRotors[i] = rotorWirings[rotorTypes[i]];
                }
            }

            // Get current offsets from rotor positions
            // These represent how far each rotor has turned from position 'A'
            int offsetA = ringPositions[0] - 'A';
            int offsetB = ringPositions[1] - 'A';
            int offsetC = ringPositions[2] - 'A';

            // Convert letter to number (A=0, B=1, etc.) for mathematical operations
            int pos = c - 'A';

            // === FORWARD PATH: Right to left through the rotors (C -> B -> A) ===

            // Through rotor C (rightmost/fastest rotor)
            int shifted = (pos + offsetC) % 26;  // Apply rotor position offset
            char let = effectiveRotors[2][shifted];  // Get the substituted letter
            pos = let - 'A';  // Convert back to number
            pos = (pos - offsetC + 26) % 26;  // Remove the offset

            // Through rotor B (middle rotor)
            shifted = (pos + offsetB) % 26;
            let = effectiveRotors[1][shifted];
            pos = let - 'A';
            pos = (pos - offsetB + 26) % 26;

            // Through rotor A (leftmost/slowest rotor)
            shifted = (pos + offsetA) % 26;
            let = effectiveRotors[0][shifted];
            pos = let - 'A';
            pos = (pos - offsetA + 26) % 26;

            // === REFLECTOR: Signal bounces back ===
            // The reflector connects pairs of contacts, sending the signal back through the rotors
            char reflectorChar = ' ';
            var currentReflector = reflectorWirings[reflectorType];
            if (currentReflector.TryGetValue((char)(pos + 'A'), out reflectorChar))
            {
                pos = reflectorChar - 'A';
            }

            // === RETURN PATH: Left to right through the rotors (A -> B -> C) ===
            // Now we go backwards through the rotors, using inverse substitution

            // Through rotor A (left) - inverse direction
            shifted = (pos + offsetA) % 26;
            int index = effectiveRotors[0].IndexOf((char)(shifted + 'A'));  // Find inverse mapping
            pos = (index - offsetA + 26) % 26;

            // Through rotor B (middle) - inverse direction
            shifted = (pos + offsetB) % 26;
            index = effectiveRotors[1].IndexOf((char)(shifted + 'A'));
            pos = (index - offsetB + 26) % 26;

            // Through rotor C (right) - inverse direction
            shifted = (pos + offsetC) % 26;
            index = effectiveRotors[2].IndexOf((char)(shifted + 'A'));
            pos = (index - offsetC + 26) % 26;

            // Convert back to letter
            c = (char)(pos + 'A');

            // Apply plugboard again (plugboard is symmetric)
            // Same transformation as at the beginning
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
        public (string[] RotorTypes, char[] RingPositions, char[] RingSettings, string ReflectorType,
                Dictionary<char, char> Plugboard, string Plaintext, string Ciphertext,
                bool IsDefault, string StartingKey) GetState()
        {
            bool isDefault = ConfigurationsEqual(currentConfiguration, defaultConfiguration);
            string startingKey = new string(messageKeyPositions);

            return (
                rotorTypes.ToArray(),
                ringPositions.ToArray(),
                ringSettings.ToArray(),
                reflectorType,
                new Dictionary<char, char>(plugboard),
                plaintext.ToString(),
                ciphertext.ToString(),
                isDefault,
                startingKey
            );
        }

        private bool ConfigurationsEqual(EnigmaConfiguration a, EnigmaConfiguration b)
        {
            return a.RotorTypes.SequenceEqual(b.RotorTypes) &&
                   a.ReflectorType == b.ReflectorType &&
                   a.RingSettings.SequenceEqual(b.RingSettings) &&
                   a.PlugboardPairs == b.PlugboardPairs;
        }

        // Set or update rotors
        public void SetRotors(string[] types, string positions)
        {
            rotorTypes = types.ToArray();
            ringPositions = positions.ToUpper().ToCharArray();
            messageKeyPositions = (char[])ringPositions.Clone();
            UpdateCurrentConfiguration();
            ClearText();
        }

        // update rotors position
        public void SetRotorPositions(string positions)
        {
            // Basic validation to ensure the input length is correct, validation prevents crashes and maintains a valid machine state could be a good idea to add to other functions aswell
            
            if (positions != null && positions.Length == this.ringPositions.Length)
            {
                this.ringPositions = positions.ToUpper().ToCharArray();
                messageKeyPositions = (char[])ringPositions.Clone();
                UpdateCurrentConfiguration(); // Keep the current configuration object in sync
                ClearText(); // Reset text as the starting point has changed
            }
        }

        // Set or update ring settings
        public void SetRingSettings(string settings)
        {
            ringSettings = settings.ToUpper().ToCharArray();
            UpdateCurrentConfiguration();
            ClearText();
        }

        // Set or update plugboard
        public void SetPlugboard(string pairs)
        {
            SetupPlugboard(pairs);
            UpdateCurrentConfiguration();
            ClearText();
        }

        // Set or update reflector
        public void SetReflector(string type)
        {
            if (reflectorWirings.ContainsKey(type))
            {
                reflectorType = type;
                UpdateCurrentConfiguration();
                ClearText();
            }
        }

        // Clear only the text, preserve configuration
        public void ClearText()
        {
            plaintext.Clear();
            ciphertext.Clear();
        }

        // Reset to current default configuration
        public void Reset()
        {
            ApplyConfiguration(defaultConfiguration);
            ClearText();
        }

        // Reset to initial startup configuration
        public void ResetToInitial()
        {
            ApplyConfiguration(initialConfiguration);
            ClearText();
        }

        // Save current configuration as the new default
        public void SaveCurrentAsDefault()
        {
            UpdateCurrentConfiguration();
            defaultConfiguration = currentConfiguration.Clone();
        }
    }
}