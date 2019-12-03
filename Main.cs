/* MADE BY: Kat9_123
 * 
 * An encyption programme based on the enigma machine. This is still under development!!!
 * 
 * Coming Soon: Salt!
 * 
 * This programme uses the Unity game engine. Only because the language I'm best at is C# + Unity.
 * (I know Unity isn't really meant for stuff like this but idc)
 * 
 * 
 * One may use and edit this code or distribute it any way but these top comments MUST BE INCLUDED!! (Line 1 through 12)
*/
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Main : MonoBehaviour
{
    #region Public variables
    public string Input; //Input to encrypt/decrypt
    public string Output; //The output
    [Space]
    public bool Start;
    [Space]
    public bool createkey;
    public bool encrypt;
    public bool decrypt;
    [Space]
    public string KeyPath; //The path to the key/where to make it
    public int SegmentAmount; //Amount of 'Segments' (layers)
    #endregion
    #region Private variables
    private readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    #endregion
    #region StartUp
    private void Awake()
    {
        Debug.LogWarning("Ready");
    }
    private void Update()
    {
        if (Start) _Start();
    }
    void _Start()
    {
        Debug.LogWarning("Starting...");
        Input = Input.ToUpper();
        Output = null;
        System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
        st.Start();
        if (createkey)
        {
            CreateKey();
            createkey = false;
        }
        if (encrypt)
        {
            Encrypt();
            encrypt = false;
        }
        if (decrypt)
        {
            Decrypt();
            decrypt = false;
        }
        st.Stop();
        Debug.LogWarning("Done in: " + st.ElapsedMilliseconds + " Miliseconds");
        Start = false;  
    }
    #endregion


    #region Key Making
    void CreateKey()
    {
        string Decider = CreateDecider();
        string FirstSalt = Random.Range(10, 64) + "\n";
        string LastSalt = Random.Range(10, 64) + "\n";
        List<string> Segments = CreateSegments();
        WriteKey(Decider, FirstSalt, LastSalt, Segments);   
    }
    string CreateDecider()
    {
        int DeciderInt = Random.Range(1, 25); //choses random number between 1 & 25
        string DeciderStr = DeciderInt + "\n"; //String to hold the decider
        return DeciderStr;
    }
    List<string> CreateSegments()
    {
        string[] SegmentA = new string[26]; //This segment letters in an array
        string Segment = ""; //Segment string
        List<string> Segments = new List<string>();
        for (int i = 0; i < SegmentAmount; i++)
        {
            for (int z = 0; z < 26; z++)
            {
                string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                SegmentA[z] = Alpha[z].ToString();
            }
            for (int a = 0; a < 26; a++)
            {
                string temp = SegmentA[a];
                int randomIndex = Random.Range(a, 26);
                SegmentA[a] = SegmentA[randomIndex];
                SegmentA[randomIndex] = temp;
            }
            for (int b = 0; b < 26; b++)
            {
                Segment = Segment + SegmentA[b].ToString();
            }
            Segment = Segment + "\n";
            Segments.Add(Segment);
            Segment = null;
        }
        return Segments;
    }
    void WriteKey(string Decider, string FirstSalt, string LastSalt, List<string> Segments)
    {
        string path = KeyPath + "/key.sigma"; //Real path (path + /key.sigma)
        if (!File.Exists(path)) //If the file doesn't already exist
        {
            File.WriteAllText(path, Decider); //Writes the decider string to the file
            File.AppendAllText(path, FirstSalt);
            File.AppendAllText(path, LastSalt);
            for (int x = 0; x < SegmentAmount; x++) //For each segment
            {
                File.AppendAllText(path, Segments[x]); //Adds the segment to the file
            }
        }
    }
    #endregion
    #region All
    string[] ReadKey()
    {
        return File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]);
    }
    List<char> DeciderSwitch(int z, string[] Key)
    {
        int decider = int.Parse(Key[0]); //Parses the decider to an int
        List<char> SwitchAlpha = new List<char>();
        Debug.Log("Switched by: " + decider * z);
        SwitchAlpha.Clear(); //Clears the SwitchAlpha list
        int shiftAmount = z * decider; //Amount of input * the decider
        if (shiftAmount > 26)
        {
            for (int c = 0; shiftAmount > 26; c++)
            {
                shiftAmount -= 26;
            }
        }
        //Adds the last shiftAmount letters of the Alphabet to SwitchAlpha
        for (int j = 26 - shiftAmount; j < 26; j++)
        {
            SwitchAlpha.Add(Alphabet[j]); //Add the letter from AlphaList (main alphabet)
        }
        //Adds the rest of the letters
        for (int u = 0; u < 26 - shiftAmount; u++)
        {
            SwitchAlpha.Add(Alphabet[u]);
        }
        return SwitchAlpha;
    }
    #endregion
    #region Encryption
    void Encrypt()
    {
        string[] Key = ReadKey();
        for (int z = 0; z < Input.Length; z++) //For each letter in the input list
        {
            char CurrentLetterProcess = FirstInputEncryption(z, Key);
            char TempLetterProcess = 'a';
            for (int h = 4; h <= Key.Length - 4; h++)
            {
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {
                    if (CurrentLetterProcess == Alphabet[g]) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {
                        TempLetterProcess = Key[h][g];  //secondEnc is the g index in said string
                    }
                }
                CurrentLetterProcess = TempLetterProcess;
            }
            Output += CurrentLetterProcess;
        }
        AddSalt(Key);
    }
    char FirstInputEncryption(int z, string [] Key)
    {
        List<char> SwitchAlpha = DeciderSwitch(z, Key);
        for (int g = 0; g < 26; g++) //For each letter in said segment
        {
            if (Input[z] == SwitchAlpha[g]) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
            {
                return Key[3][g];  //secondEnc is the g index in said string
            }
        }
        return 'a';
    }
    void AddSalt(string[] Key)
    {
        string TempOutput = Output;
        Output = null;
        for (int i = 0; i < int.Parse(Key[1]); i++)
        {
            Output += Alphabet[Random.Range(0, 26)];
        }
        Output += TempOutput;
        for (int i = 0; i < int.Parse(Key[2]); i++)
        {
            Output += Alphabet[Random.Range(0, 26)];
        }
    }
    #endregion
    #region Decryption
    void Decrypt()
    {
        string[] Key = ReadKey();
        string InternalInput = RemoveSalt(Key);
        for (int z = 0; z < InternalInput.Length; z++) //For each letter in the input list
        {
            List<char> SwitchAlpha = DeciderSwitch(z, Key);
            char CurrentLetterProcess = InternalInput[z];
            char TempLetterProcess = 'a';   
            for (int h = Key.Length - 4; h > 2; h--)
            {
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {
                    if (Key[h][g] == CurrentLetterProcess) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {
                        if (h == 3) TempLetterProcess = SwitchAlpha[g];
                        else TempLetterProcess = Alphabet[g];
                    }
                }
                CurrentLetterProcess = TempLetterProcess;
            }
            Output += CurrentLetterProcess;
        }
    }
    string RemoveSalt(string[] Key)
    {
        string TempInput = "";
        int Max = int.Parse(Key[1]) + (Input.Length - (int.Parse(Key[1]) + int.Parse(Key[2])));
        for (int i = int.Parse(Key[1]); i < Max; i++)
        {
            TempInput += Input[i];
        }
        return TempInput;
    }
    #endregion
}
