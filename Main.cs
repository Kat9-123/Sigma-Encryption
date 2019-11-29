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
    public string Input; //Input to encrypt/decrypt
    public string Output; //The output
    [Space]
    public bool createkey;
    public bool encrypt;
    public bool decrypt;
    [Space]
    public string KeyPath; //The path to the key/where to make it
    public int SegmentAmount; //Amount of 'Segments' (layers)
    private string[] Key; //An array for all lines in the key.sigma file
    private List<string> Segments = new List<string>();
    private readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<char> SwitchAplha = new List<char>();
    private List<char> Input_List = new List<char>(); //String list of all letters inputted (empty) 

    private void Start()
    {
        StartUp();
        if(createkey) CreateKey(); 
        if(encrypt) Encrypt(); 
        if(decrypt) Decrypt();
    }
    void StartUp()
    {
        //Makes the input IN CAPS
        Input = Input.ToUpper();
 
        //Slits the Input(String) into Input_List(Char)
        for (int i = 0; i < Input.Length; i++) 
        {
            Input_List.Add(Input[i]); 
        }
    }

    #region Key Making
    void CreateKey()
    {
        CreateDecider();
        string Decider = CreateDecider();
        CreateSalt();
        CreateSegments();
        WriteKey(Decider);
    }
    string CreateDecider()
    {
        int DeciderInt = Random.Range(1, 25); //choses random number between 1 & 25
        string DeciderStr = DeciderInt + "\n"; //String to hold the decider
        return DeciderStr;
    }
    void CreateSalt()
    {
        /*
//Creating Salt
int saltAmount = Random.Range(6, 32);
List<int> SaltLocation = new List<int>();
List<int> SaltLenght = new List<int>();

for (int l = 0; l < saltAmount; l++)
{
    SaltLocation.Add(Random.Range(0, 100));
    SaltLenght.Add(Random.Range(0, 10));
}
*/
    }
    void CreateSegments()
    {
        string[] SegmentA = new string[26]; //This segment letters in an array
        string Segment = ""; //Segment string
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
    }
    void WriteKey(string Decider)
    {
        string path = KeyPath + "/key.sigma"; //Real path (path + /key.sigma)
        if (!File.Exists(path)) //If the file doesn't already exist
        {
            File.WriteAllText(path, "" + Decider); //Writes the decider string to the file
            for (int x = 0; x < SegmentAmount; x++) //For each segment
            {
                File.AppendAllText(path, Segments[x]); //Adds the segment to the file
            }
        }
    }
    #endregion

    void ReadKey()
    {
        Key = File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]);
    }
    void DeciderSwitch(int z)
    {
        int decider = int.Parse(Key[0]); //Parses the decider to an int
        SwitchAplha.Clear(); //Clears the SwitchAlpha list
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
            SwitchAplha.Add(Alphabet[j]); //Add the letter from AlphaList (main alphabet)
        }
        //Adds the rest of the letters
        for (int u = 0; u < 26 - shiftAmount; u++)
        {
            SwitchAplha.Add(Alphabet[u]);
        }
    }

    void Decrypt()
    {
        ReadKey();
        for (int z = 0; z < Input_List.Count; z++) //For each letter in the input list
        {
            DeciderSwitch(z);
            char CurrentLetterProcess = Input_List[z];
            for (int h = Key.Length - 2; h > 0; h--)
            {
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {
                    if (Key[h][g] == CurrentLetterProcess) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {
                        if(h == 1) CurrentLetterProcess  = SwitchAplha[g];
                        else CurrentLetterProcess = Alphabet[g];
                    }
                }
            }
            Output += CurrentLetterProcess;
        }
    }

    void Encrypt()
    {
        ReadKey();
        for (int z = 0; z < Input_List.Count; z++) //For each letter in the input list
        {
            DeciderSwitch(z);
            char CurrentLetterProcess = FirstInputEncryption(z);
            for (int h = 2; h <= Key.Length - 2; h++)
            {
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {
                    if (CurrentLetterProcess == Alphabet[g]) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {
                        CurrentLetterProcess =  Key[h][g];  //secondEnc is the g index in said string
                    }
                }
            }
            Output += CurrentLetterProcess;
        }
    }
    char FirstInputEncryption(int z)
    {
        for (int g = 0; g < 26; g++) //For each letter in said segment
        {
            if (Input_List[z] == SwitchAplha[g]) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
            {
                return Key[1][g];  //secondEnc is the g index in said string
            }
        }
        return 'a';
    }
}
