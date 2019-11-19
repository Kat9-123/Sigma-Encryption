/*
 *
 *
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
    public string[] Key; //An array for all lines in the key.sigma file
    public int SegmentAmount; //Amount of 'Segments' (layers)
    public List<string> SegmentsKey = new List<string>();
    



    private void Start()
    {
        if(createkey) CreateKey(); //Go to key create function
        if(encrypt) Encrypt(); //Go to encrypt function
        if(decrypt) Decrypt();
    }

    void CreateKey()
    {
        //Creating of the Decider

        int DeciderInt = Random.Range(1, 25); //choses random number between 1 & 25
        string DeciderStr = DeciderInt + "\n"; //String to hold the decider




        //Creating Salt
        int saltAmount = Random.Range(6, 32);
        List<int> SaltLocation = new List<int>();
        List<int> SaltLenght = new List<int>();





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
            Debug.Log(Segment);
            SegmentsKey.Add(Segment);
            Segment = null;



        }



        string path = KeyPath + "/key.sigma"; //Real path (path + /key.sigma)
        if (!File.Exists(path)) //If the file doesn't already exist
        {
            File.WriteAllText(path, "" + DeciderStr); //Writes the decider string to the file
            for (int x = 0; x < SegmentAmount; x++) //For each segment
            {
                File.AppendAllText(path, SegmentsKey[x]); //Adds the segment to the file
            }


        }





    } //Key create function



    void Encrypt() //Encrypt function
    {

        Key = File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]); //Finds the key with the path and puts it in the key string array



        string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //The alphabet in string form
        List<string> AlphaList = new List<string>(); //Empty list to contain the alphabet
        for (int x = 0; x < 26; x++) //For each letter in the alpha(bet) string
        {

            AlphaList.Add(Alpha[x].ToString()); //Adds the x letter from the alpha(bet) string to the Alpha(bet)List string list
        }

        List<string> InpL = new List<string>(); //String list of all letters inputted (empty) 
        for (int i = 0; i < Input.Length; i++) //For loop each letter in the Input string
        {

            InpL.Add(Input[i].ToString()); //It takes the i letter from the Input and adds it to (InpL) the input list

        }
        for (int z = 0; z < InpL.Count; z++) //For each letter in the input list
        {


            string CurrentEnc = InpL[z]; //The letter that will be currently processed
            string secondEnc = ""; //Temp string to hold letter
            int decider = int.Parse(Key[0]); //Parses the decider to an ints
            List<string> tempAplha = new List<string>(); //The temp alphabet in a list



            
            #region ::Shifts the tempalpha list by z*decider, z being the amount of items in the input list - 1::

            tempAplha.Clear(); //Clears the temp alphabet list
            //adds all letters from the alphabet to firstaplha exept for the z*decider's (z = ammount of items in inpL - 1)
            int shiftAmount = z * decider;

            if (shiftAmount > 26)
            {
                shiftAmount = shiftAmount - 26 * (z - 1);

            }




            for (int j = 26 - shiftAmount; j < 26; j++)
            {
                tempAplha.Add(AlphaList[j]); //Add the letter from AlphaList (main alphabet)
            }
            //Adds the other letters to tempAlpha
            for (int u = 0; u < 26 - shiftAmount; u++)
            {
                tempAplha.Add(AlphaList[u]);
            }
            #endregion



            #region ::Main encrypition::
            for (int h = 1; h <= Key.Length - 2; h++) //For the amount of segments in the key
            {

                for (int g = 0; g < 26; g++) //For each letter in said segment
                {

                    if (CurrentEnc == Alpha[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {

                        string tempkey = Key[h].ToString(); //String tempkey becomes the segment that is currently used to encrypt
                        secondEnc = tempkey[g].ToString();  //secondEnc is the g index in said string
                    }
                }
                
                if (h == 1) //Checks if first segment
                {
                    for (int g = 0; g < 26; g++) //For each letter in said segment
                    {

                        if (CurrentEnc == tempAplha[g]) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                        {

                            string tempkey = Key[h].ToString(); //String tempkey becomes the segment that is currently used to encrypt
                            secondEnc = tempkey[g].ToString();  //secondEnc is the g index in said string
                        }
                    }

                }
                
                

                CurrentEnc = secondEnc; //currentEnc becomes secondEnc
            }
            #endregion






            Output = Output + CurrentEnc; //Adds the currentletter to the output






        }
    }

    void Decrypt()
    {
        Key = File.ReadAllText(KeyPath + "/key.sigma").Split("\n"[0]); //Finds the key with the path and puts it in the key string array
        string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //The alphabet in string form
        List<string> AlphaList = new List<string>(); //Empty list to contain the alphabet
        for (int x = 0; x < 26; x++) //For each letter in the alpha(bet) string
        {

            AlphaList.Add(Alpha[x].ToString()); //Adds the x letter from the alpha(bet) string to the Alpha(bet)List string list
        }

        List<string> InpL = new List<string>(); //String list of all letters inputted (empty) 
        for (int i = 0; i < Input.Length; i++) //For loop each letter in the Input string
        {

            InpL.Add(Input[i].ToString()); //It takes the i letter from the Input and adds it to (InpL) the input list

        }






        for (int z = 0; z < InpL.Count; z++) //For each letter in the input list
        {


            string CurrentEnc = InpL[z]; //The letter that will be currently processed
            string secondEnc = ""; //Temp string to hold letter
            int decider = int.Parse(Key[0]); //Parses the decider to an ints
            List<string> tempAplha = new List<string>(); //The temp alphabet in a list

            #region ::Shifts the tempalpha list by z*decider, z being the amount of items in the input list - 1::

            tempAplha.Clear(); //Clears the temp alphabet list
            //adds all letters from the alphabet to firstaplha exept for the z*decider's (z = ammount of items in inpL - 1)
            int shiftAmount = z * decider;

            if (shiftAmount > 26)
            {
                shiftAmount = shiftAmount - 26 * (z - 1);

            }




            for (int j = 26 - shiftAmount; j < 26; j++)
            {
                tempAplha.Add(AlphaList[j]); //Add the letter from AlphaList (main alphabet)
            }
            //Adds the other letters to tempAlpha
            for (int u = 0; u < 26 - shiftAmount; u++)
            {
                tempAplha.Add(AlphaList[u]);
            }
            #endregion






            #region ::Main decryption::

            for (int h = Key.Length - 2; h > 0; h--) //For the amount of segments in the key
            {

                string tempkey = Key[h].ToString(); //String tempkey becomes the segment that is currently used to encrypt
                Debug.Log(tempkey);
                Debug.Log(h);
                for (int g = 0; g < 26; g++) //For each letter in said segment
                {


                    

                    if (CurrentEnc == tempkey[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                    {



                        secondEnc = Alpha[g].ToString();

                    }
                }
                
                
                if (h == 1)
                {
                    for (int g = 0; g < 26; g++) //For each letter in said segment
                    {

                        if (CurrentEnc == tempkey[g].ToString()) //If the that is being processed currently is equal to a letter in the alphabet. g=the index of said letter
                        {
                            secondEnc = tempAplha[g];
                        }
                    }

                }
                
                

                CurrentEnc = secondEnc; //currentEnc becomes secondEnc
 
            }
            #endregion
            Output = Output + CurrentEnc; //Adds the currentletter to the output
        }
    }
}
