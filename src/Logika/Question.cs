namespace Phrasia.Logika;

public abstract class Question
{
    private const int DefaultPoints = 10;
    public string Prompt { get; }
    public string Explanation { get; }
    public int Points { get; private set; }

    protected Question(string prompt, string explanation, int points = DefaultPoints)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            throw new ArgumentException("Teks soal tidak boleh kosong.", nameof(prompt));

        if (string.IsNullOrWhiteSpace(explanation))
            throw new ArgumentException("Penjelasan tidak boleh kosong.", nameof(explanation));

        if (points <= 0)
            throw new ArgumentOutOfRangeException(nameof(points), points, "Poin harus lebih dari 0.");

        Prompt = prompt.Trim();
        Explanation = explanation.Trim();
        Points = points;
    }

    public abstract bool Evaluate(string answer);

    public virtual string BuildFeedback(bool isCorrect)
    {
        string verdict = isCorrect ? "Benar!" : "Belum tepat.";
        return $"{verdict} {Explanation}";
    }

    public override string ToString()
    {
        return $"{GetType().Name}: {Prompt}";
    }

}
