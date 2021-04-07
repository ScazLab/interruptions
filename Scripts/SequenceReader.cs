using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SequenceReader
{
    // read once and store sequence data for path task
    public static int pathSequenceIndex = 0; // TODO - must increment after question is shown
    //public static string pathStructureCSV = "Assets/CSV/path-structure.csv";
    public static string[,] pathStructureCSV = Prompts.pathStructure;
    //public static string pathQuestionsCSV = "Assets/CSV/path-questions.csv";
    public static string[,] pathQuestionsCSV = Prompts.pathQuestions;
    public static IDictionary<int, PathItem> pathStructure = PathStructure.populateDictionary(pathStructureCSV);
    public static List<PathQuestion> pathSequence = PathQuestion.getQuestions(pathQuestionsCSV);

    // read once and store sequence data for math task
    public static int mathSequenceIndex = 0;
    //public static string mathQuestionsCSV = "Assets/CSV/math-questions.csv";
    public static string[,] mathQuestions = Prompts.mathQuestions;
    public static List<MathQuestion> mathSequence = MathQuestion.getQuestions(mathQuestions);

    // read once and store sequence data for stroop task
    public static int stroopSequenceIndex = 0;
    //public static string stroopQuestionsCSV = "Assets/CSV/stroop-questions.csv";
    public static string[,] stroopQuestions = Prompts.stroopQuestions;
    public static List<StroopQuestion> stroopSequence = StroopQuestion.getQuestions(stroopQuestions);

    // read once and store sequence data for hanoi task
    public static int hanoiSequenceIndex = 0;
    //public static string hanoiQuestionsCSV = "Assets/CSV/hanoi-questions.csv";
    public static string[,] hanoiQuestions = Prompts.hanoiQuestions;
    public static List<HanoiQuestion> hanoiSequence = HanoiQuestion.getQuestions(hanoiQuestions);

    // read once and store sequence data for noise interruption
    public static int noiseSequenceIndex = 0;
    //public static string noiseQuestionsCSV = "Assets/CSV/noise-questions.csv";
    //public static List<NoiseQuestion> noiseSequence = NoiseQuestion.getQuestions(noiseQuestionsCSV);

    // read once and store sequence data for social interruption
    public static int socialSequenceIndex = 0;
    //public static string socialQuestionsCSV = "Assets/CSV/social-questions.csv";
    //public static List<SocialQuestion> socialSequence = SocialQuestion.getQuestions(socialQuestionsCSV);

    public class SocialQuestion {
        public int sound;

        public static SocialQuestion getQuestion(string csvLine){
            string[] values = csvLine.Split(',');
            SocialQuestion q = new SocialQuestion();
            q.sound = Int32.Parse(values[0]);
            return q;
        }

        public static List<SocialQuestion> getQuestions(string csv){
            List<SocialQuestion> questions = File.ReadAllLines(csv)
                .Skip(1)
                .Select(v => SocialQuestion.getQuestion(v))
                .ToList();
            return questions;
        }
    }

    public class NoiseQuestion {
        public int sound;

        public static NoiseQuestion getQuestion(string csvLine){
            string[] values = csvLine.Split(',');
            NoiseQuestion q = new NoiseQuestion();
            q.sound = Int32.Parse(values[0]);
            return q;
        }

        public static List<NoiseQuestion> getQuestions(string csv){
            List<NoiseQuestion> questions = File.ReadAllLines(csv)
                .Skip(1)
                .Select(v => NoiseQuestion.getQuestion(v))
                .ToList();
            return questions;
        }
    }

    public class HanoiPiece {
        public int peg; // 0 = left
        public int height; // 0 = bottom of peg stack
        public int piece_size; // 0 = smallest
    }

    // assumption: # of pegs are fixed ( = 3: left middle right)
    // assumption: we can increase # of blocks ( = size of orientation list )
    public class HanoiState {
        // index 0 = smallest block
        // index last = largest block
        // int value = peg # to occupy: 1 = left, 2 = middle, 3 = right
        // public List<int> orientation_pieces;
        public List<HanoiPiece> orientation; // sorted by ascending piece_size, index 0 = smallest

        public HanoiState(string peg1, string peg2, string peg3, int size){
            this.orientation = CreateList<HanoiPiece>(size); // initialize number of pieces
            for (int i = 0; i < peg1.Length; i++) { this.orientation[Int32.Parse(peg1[i].ToString())] = new HanoiPiece() { peg = 0, height = peg1.Length - i - 1, piece_size = i }; }
            for (int j = 0; j < peg2.Length; j++) { this.orientation[Int32.Parse(peg2[j].ToString())] = new HanoiPiece() { peg = 1, height = peg2.Length - j - 1, piece_size = j }; }
            for (int k = 0; k < peg3.Length; k++) { this.orientation[Int32.Parse(peg3[k].ToString())] = new HanoiPiece() { peg = 2, height = peg3.Length - k - 1, piece_size = k }; }

            /*s
            this.orientation_pieces = CreateList<int>(size); // initialize number of blocks
            for (int i = 0; i < peg1.Length; i++){ this.orientation_pieces[Int32.Parse(peg1[i].ToString())] = 0; } // left peg
            for (int j = 0; j < peg2.Length; j++){ this.orientation_pieces[Int32.Parse(peg2[j].ToString())] = 1; } // middle peg
            for (int k = 0; k < peg3.Length; k++){ this.orientation_pieces[Int32.Parse(peg3[k].ToString())] = 2; } // right peg
            */
        }

        // no-LINQ helper for initializing a list of given size
        private static List<T> CreateList<T>(int capacity){
            List<T> coll = new List<T>(capacity);
            for(int i = 0; i < capacity; i++)
                coll.Add(default(T));

            return coll;
        }
    }

    public class HanoiQuestion {
        public HanoiState start;
        public HanoiState goal;
        public int moves_to_interrupt;

        public static HanoiQuestion getQuestion(string[] values){
            HanoiQuestion q = new HanoiQuestion();
            q.start = new HanoiState(values[0], values[1], values[2], values[0].Length + values[1].Length + values[2].Length);
            q.goal = new HanoiState(values[3], values[4], values[5], values[3].Length + values[4].Length + values[5].Length);
            q.moves_to_interrupt = Convert.ToInt32(values[6]);
            return q;
        }

        public static List<HanoiQuestion> getQuestions(string[,] arr){
            List<HanoiQuestion> questions = new List<HanoiQuestion>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string[] temp = new string[arr.GetLength(1)];
                for (int n = 0; n < temp.Length; n++)
                {
                    temp[n] = arr[i, n];
                }
                questions.Add(HanoiQuestion.getQuestion(temp));
            }
            return questions;
        }
    }

    public class StroopQuestion {
        public string c1_text;
        public string c1_color;

        public string c2_text;
        public string c2_color;

        public int answer = 0; // 0 = false / doesn't match, 1= true / match

        public static StroopQuestion getQuestion(string[] values){
            StroopQuestion q = new StroopQuestion();
            q.c1_text = values[0];
            q.c1_color = values[1];

            q.c2_text = values[2];
            q.c2_color = values[3];

            q.answer = Int32.Parse(values[4]);
            return q;
        }

        public static List<StroopQuestion> getQuestions(string[,] arr){
            List<StroopQuestion> questions = new List<StroopQuestion>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string[] temp = new string[arr.GetLength(1)];
                for (int n = 0; n < temp.Length; n++)
                {
                    temp[n] = arr[i, n];
                }
                questions.Add(StroopQuestion.getQuestion(temp));
            }
            return questions;
        }
    }

    public class MathQuestion {
        public string c1_first_operand;
        public string c1_second_operand;
        public string c1_operator;

        public string c2_first_operand;
        public string c2_second_operand;
        public string c2_operator;

        public int answer = 0; // 1 = left card, 2 = right card

        public static MathQuestion getQuestion(string[] values){
            //string[] values = csvLine.Split(' ');
            MathQuestion q = new MathQuestion();
            q.c1_first_operand = values[0];
            q.c1_second_operand = values[1];
            q.c1_operator = values[2];

            q.c2_first_operand = values[3];
            q.c2_second_operand = values[4];
            q.c2_operator = values[5];

            q.answer = Int32.Parse(values[6]);
            return q;
        }

        public static List<MathQuestion> getQuestions(string[,] arr){
            List<MathQuestion> questions = new List<MathQuestion>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string[] temp = new string[arr.GetLength(1)];
                for (int n = 0; n < temp.Length; n++)
                {
                    temp[n] = arr[i, n];
                }
                questions.Add(MathQuestion.getQuestion(temp));
            }
            return questions;
        }
    }

    public class PathQuestion {
        public List<PathItem> question;
        public List<PathItem> answer_order;
        public int interrupts;

        public static PathQuestion getQuestion(string[] values){
            //string[] values = csvLine.Split(',');
            PathQuestion q = new PathQuestion();
            q.question = values[0]
                .Select(c => pathStructure[Int32.Parse(c.ToString())])
                .ToList();
            q.answer_order = values[1]
                .Select(c => pathStructure[Int32.Parse(c.ToString())])
                .ToList();
            q.interrupts = Int32.Parse(values[2]); 
            return q;
        }

        public static List<PathQuestion> getQuestions(string[,] arr){
            List<PathQuestion> questions = new List<PathQuestion>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string[] temp = new string[arr.GetLength(1)];
                for (int n = 0; n < temp.Length; n++)
                {
                    temp[n] = arr[i, n];
                }
                questions.Add(PathQuestion.getQuestion(temp));
            }
            return questions;
            /*
            List<PathQuestion> questions = File.ReadAllLines(csv)
                .Skip(1)
                .Select(v => PathQuestion.getQuestion(v))
                .ToList();
            return questions;
            */
        }
    }

    public class PathStructure {

        public static IDictionary<int, PathItem> populateDictionary(string[,] arr){
            IDictionary<int, PathItem> structure = new Dictionary<int, PathItem>();
            List<PathItem> items = getPaths(arr);
            foreach (PathItem item in items){
                structure.Add(item.id, item);
            }
            return structure;
        }

        public static List<PathItem> getPaths(string[,] arr){
            List<PathItem> questions = new List<PathItem>();
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                string[] temp = new string[arr.GetLength(1)];
                for (int n = 0; n < temp.Length; n++)
                {
                    temp[n] = arr[i, n];
                }
                questions.Add(PathItem.getPathInfo(temp));
            }
            return questions;
            
            /*
            return File.ReadAllLines(csv)
                .Skip(1)
                .Select(v => PathItem.getPathInfo(v))
                .ToList();
            */
        }
    }

    public class PathItem {
        public int id = 0;
        public string video = "";
        public string image = "";
        public float time = 5.0f;

        public static PathItem getPathInfo(string[] values){
            //string[] values = csvLine.Split(',');
            PathItem item = new PathItem();
            item.id = Int32.Parse(values[0]);
            item.video = values[1];
            item.image = values[2];
            item.time = float.Parse(values[3]);
            return item;
        }
    }
}