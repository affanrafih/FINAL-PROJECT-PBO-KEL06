namespace Phrasia.Logika;

public class ArrangeSentenceQuestion : Question
{
    private const int MinPhrases = 2;
    private const int MaxShuffleAttempts = 10;
    private readonly List<string> _phrases;
    public IReadOnlyList<string> Phrases => _phrases.AsReadOnly();
    public string CorrectSentence { get; }

    public ArrangeSentenceQuestion(
        string prompt,
        string explanation,
        IEnumerable<string> orderedPhrases,
        int points = 10,
        Random? random = null)
        : base(prompt, explanation, points)
    {
        if (orderedPhrases is null)
            throw new ArgumentNullException(nameof(orderedPhrases));

        var ordered = orderedPhrases.Select(p => p?.Trim() ?? string.Empty).ToList();

        if (ordered.Count < MinPhrases)
            throw new ArgumentException($"Minimal {MinPhrases} frasa diperlukan.", nameof(orderedPhrases));

        if (ordered.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Frasa tidak boleh kosong.", nameof(orderedPhrases));

        CorrectSentence = string.Join(" ", ordered);
        _phrases = Shuffle(ordered, random ?? Random.Shared);
    }

    public override bool Evaluate(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
            return false;

        return Normalize(answer).Equals(Normalize(CorrectSentence), StringComparison.OrdinalIgnoreCase);
    }

    public override string BuildFeedback(bool isCorrect)
    {
        string feedback = base.BuildFeedback(isCorrect);

        return isCorrect
            ? feedback
            : $"{feedback} Kalimat yang benar: \"{CorrectSentence}\"";
    }

    // Merapikan spasi ganda supaya "I  visited" sama dengan "I visited".
    private static string Normalize(string text)
    {
        return string.Join(" ", text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
    }

    // Mengacak urutan kata
    private static List<string> Shuffle(List<string> ordered, Random random)
    {
        var result = new List<string>(ordered);

        for (int attempt = 0; attempt < MaxShuffleAttempts; attempt++)
        {
            for (int i = result.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (result[i], result[j]) = (result[j], result[i]);
            }

            if (!result.SequenceEqual(ordered))
                break;
        }

        return result;
    }
}
