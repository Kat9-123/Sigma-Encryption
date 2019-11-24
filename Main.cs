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



using System.Collections;
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
    private List<string> SegmentsKey = new List<string>();

    private string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private List<char> Alpha_List = new List<char>();
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
        #region ::Input handling::
        //Makes the input IN CAPS
        Input = Input.ToUpper();
 
        //Slits the Input(String) into Input_List(Char)
        for (int i = 0; i < Input.Length; i++) 
        {
            Input_List.Add(Input[i]); 
        }
        #endregion
    }

    void CreateKey()
    {
        #region ::Creating of the decider::
        int DeciderInt = Random.Range(1, 25); //choses random number between 1 & 25
        string DeciderStr = DeciderInt + "\n"; //String to hold the decider
        #endregion

        
        #region::Creating of the salt::
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
        #endregion


        #region ::Creating of the segments::
        //Creating Segments

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
            SegmentsKey.Add(Segment);
            Segment = null;
        }
        #endregion

        #region ::Creating of the key.sigma file at the specified path::
        string path = KeyPath + "/key.sigma"; //Real path (path + /key.sigma)
        if (!File.Exists(path)) //If the file doesn't already exist
        {
            File.WriteAllText(path, "" + DeciderStr); //Writes the decider string to the file
            for (int x = 0; x < SegmentAmount; x++) //For each segment
            {
                File.AppendAllText(path, SegmentsKey[x]); //Adds the segment to the file
            }
        }
        #endregion

    } 

    void Encrypt()
    {

        #region ::Reading the file specified with the path::
        Key = File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]); //Finds the key with the path and puts it in the key string array
        #endregion

        #region ::Encryption::
        for (int z = 0; z < Input_List.Count; z++) //For each letter in the input list
        {

            #region ::Shifts the SwitchAlpha list for extra safety::

            int decider = int.Parse(Key[0]); //Parses the decider to an int
            List<char> SwitchAplha = new List<char>(); //Declares the Alphabet char list that will be switched over

            SwitchAplha.Clear(); //Clears the SwitchAlpha list

            int shiftAmount = z * decider; //Amount of input * the decider
            //If the shiftAmount is >26 then it does shiftAmount -26 untill it is under (or) 26
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



            #endregion

            #region ::Encrypt::
            
            string TempLetterProcess = ""; //Temp string to hold letter
            string tempkey;
            string CurrentLetterProcess = Input_List[z].ToString(); //The letter that will be currently processed
            
            for (int h = 1; h <= Key.Length - 2; h++) //For the amount of segments in the key
            {
                #region ::Normal segment::
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {
                    if (CurrentLetterProcess == Alphabet[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {
                        tempkey = Key[h].ToString(); //String tempkey becomes the segment that is currently used to encrypt
                        TempLetterProcess = tempkey[g].ToString();  //secondEnc is the g index in said string
                    }
                }
                #endregion
                
                #region ::First segment:: 
                if (h == 1) //Checks if first segment
                {
                    for (int g = 0; g < 26; g++) //For each letter in said segment
                    {
                        if (CurrentLetterProcess == SwitchAplha[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                        {
                            tempkey = Key[h].ToString(); //String tempkey becomes the segment that is currently used to encrypt
                            TempLetterProcess = tempkey[g].ToString();  //secondEnc is the g index in said string
                        }
                    }
                }
                #endregion
                
                CurrentLetterProcess = TempLetterProcess;
            }
            Output = Output + CurrentLetterProcess; //Adds the currentletter to the output
            #endregion

        }
        #endregion
    }

    void Decrypt()
    {
        #region ::Reading the file specified with the path::
        Key = File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]); //Finds the key with the path and puts it in the key string array
        #endregion

        #region ::Decryption::
        for (int z = 0; z < Input_List.Count; z++) //For each letter in the input list
        {

            #region ::Shifts the tempalpha list by z*decider, z being the amount of items in the input list - 1::

            int decider = int.Parse(Key[0]); //Parses the decider to an ints
            List<char> SwitchAplha = new List<char>(); //The temp alphabet in a list





            SwitchAplha.Clear(); //Clears the temp alphabet list
            int shiftAmount = z * decider;

            if (shiftAmount > 26)
            {
                for (int c = 0; shiftAmount > 26; c++)
                {
                    shiftAmount -= 26;
                }
            }



            for (int j = 26 - shiftAmount; j < 26; j++)
            {
                SwitchAplha.Add(Alphabet[j]); //Add the letter from Alphabet (main alphabet)
            }
            //Adds the other letters to tempAlpha
            for (int u = 0; u < 26 - shiftAmount; u++)
            {
                SwitchAplha.Add(Alphabet[u]);
            }



            #endregion

            #region ::Decrypt::


            string TempLetterProcess = ""; //Temp string to hold letter
            string tempkey;
            string CurrentLetterProcess = Input_List[z].ToString(); //The letter that will be currently processed


            for (int h = Key.Length - 2; h > 0; h--) //For the amount of segments in the key
            {

                #region ::Normal segment::
                tempkey = Key[h].ToString();
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {


                    if (CurrentLetterProcess == tempkey[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {



                        TempLetterProcess = Alphabet[g].ToString();

                    }
                }
                #endregion

                
                #region ::Last segment:: 
                if (h == 1)
                {
                    tempkey = Key[1].ToString();


                    for (int g = 0; g < 26; g++) //For each letter in said segment
                    {

                        if (CurrentLetterProcess == tempkey[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                        {
                        
                            TempLetterProcess = SwitchAplha[g].ToString();
                        }
                    }

                }
                #endregion
                

                CurrentLetterProcess = TempLetterProcess;
            }
            Output = Output + CurrentLetterProcess; //Adds the currentletter to the output
            #endregion

        }
        #endregion
    }
}
