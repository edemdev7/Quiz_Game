using System;
using System.Collections.Generic;
using System.IO;

// Interface IComparer pour trier les questions par leur valeur en points
public interface IQuestionComparer : IComparer<Question>
{
    new public int Compare(Question x, Question y)
    {
        return x.Points.CompareTo(y.Points);
    }
}

// Classe Question
public class Question
{
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAnswer { get; set; }
    public int Points { get; set; }

    // Constructeur
    public Question(string text, List<string> options, string correctAnswer, int points)
    {
        Text = text;
        Options = options;
        CorrectAnswer = correctAnswer;
        Points = points;
    }

    // Méthode pour vérifier la réponse
    public virtual bool CheckResponse(string userAnswer)
    {
        return userAnswer.ToUpper() == CorrectAnswer.ToUpper();
    }
}

// Sous-classe Question1choix pour les questions à choix unique
public class Question1choix : Question
{
    // Constructeur
    public Question1choix(string text, List<string> options, string correctAnswer, int points) 
        : base(text, options, correctAnswer, points)
    {
    }

    // Méthode pour vérifier la réponse (surcharge)
    public override bool CheckResponse(string userAnswer)
    {
        return base.CheckResponse(userAnswer);
    }
}

// Sous-classe QuestionXChoix pour les questions à choix multiples
public class QuestionXChoix : Question
{
    // Constructeur
    public QuestionXChoix(string text, List<string> options, string correctAnswer, int points) 
        : base(text, options, correctAnswer, points)
    {
    }

    // Méthode pour vérifier la réponse (surcharge)
    public override bool CheckResponse(string userAnswer)
    {
        // Diviser la réponse utilisateur en options individuelles
        string[] userAnswers = userAnswer.Split(',', StringSplitOptions.RemoveEmptyEntries);

        // Vérifier chaque réponse utilisateur
        foreach (string answer in userAnswers)
        {
            if (!Options.Contains(answer))
                return false;
        }

        // Vérifier si toutes les bonnes réponses sont incluses
        foreach (string correctOption in CorrectAnswer.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (!userAnswers.Contains(correctOption))
                return false;
        }

        return true;
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Création de questions
        Question question1 = new Question1choix("Quel est le langage de programmation le plus populaire ?", 
            new List<string> {"A) Java", "B) C#", "C) Python", "D) JavaScript"}, "B", 10);

        Question question2 = new QuestionXChoix("Quels sont les langages de programmation web ?", 
            new List<string> {"A) HTML", "B) CSS", "C) JavaScript", "D) Java"}, "A,C", 15);

        // Ajout de questions supplémentaires à choix unique
        Question question3 = new Question1choix("Quel est le langage de balisage utilisé pour créer des pages web ?", 
            new List<string> {"A) HTML", "B) CSS", "C) JavaScript", "D) Python"}, "A", 10);

        Question question4 = new Question1choix("Quel langage de programmation est utilisé pour créer des applications Android ?", 
            new List<string> {"A) Java", "B) C#", "C) Python", "D) JavaScript"}, "A", 10);

        // Création d'une liste de questions
        List<Question> quizQuestions = new List<Question>();
        quizQuestions.Add(question1);
        quizQuestions.Add(question2);
        quizQuestions.Add(question3);
        quizQuestions.Add(question4);

        // Inviter le joueur à jouer
        Console.WriteLine("Bienvenue dans le Quiz Game !");
        Console.Write("Entrez votre nom : ");
        string playerName = Console.ReadLine();

        // Pose des questions au joueur
        int totalScore = 0;
        foreach (Question question in quizQuestions)
        {
            Console.WriteLine("\nQuestion : " + question.Text);
            foreach (string option in question.Options)
            {
                Console.WriteLine(option);
            }
            Console.Write("Votre réponse : ");
            string userAnswer = Console.ReadLine();

            if (question.CheckResponse(userAnswer))
            {
                Console.WriteLine("Bonne réponse !");
                totalScore += question.Points;
            }
            else
            {
                Console.WriteLine("Mauvaise réponse !");
            }
        }

        // Afficher le score total
        Console.WriteLine("\nScore total pour " + playerName + " : " + totalScore);

        // Enregistrer les résultats dans un fichier
        WriteResultToFile(playerName, totalScore);
    }

    // Méthode polymorphe de calcul de points en fonction du type de question
    static int CalculateTotalScore(List<Question> questions)
    {
        int totalScore = 0;
        foreach (Question question in questions)
        {
            totalScore += question.Points;
        }
        return totalScore;
    }

    // Méthode pour écrire les résultats dans un fichier
    static void WriteResultToFile(string playerName, int score)
    {
        string fileName = "resultat.txt";
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine("Résultats du quiz pour " + playerName + " :");
            writer.WriteLine("Score total : " + score);
            Console.WriteLine("Résultats enregistrés dans " + fileName);
        }
    }
}