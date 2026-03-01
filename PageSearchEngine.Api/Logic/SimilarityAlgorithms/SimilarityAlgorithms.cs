using SimMetrics.Net.Metric;

namespace PageSearchEngine.Api.Logic.SimilarityAlgorithms
{
    /// <summary>
    /// Provides methods for calculating similarity between strings using various algorithms.
    /// </summary>
    /// <remarks>This class currently includes the Jaro-Winkler similarity algorithm. Additional similarity
    /// algorithms, such as Levenshtein and Cosine Similarity, can be implemented in this class to enhance its
    /// functionality.
    /// Scale 
    /// skala z internetu
    /// 0.00-0.50 bardzo różne
    /// 0.50 - 0.70 słąbo podobne
    /// 0.70 - 0.85 podobne
    /// 0.85-0.95 bardzo podobne
    /// 0.95 - 1.00 prawie identyczne
    /// 
    /// </remarks>
    public class SimilarityAlgorithms
    {
        public double JaroWinklerSimilarity(string str1, string str2)
            {
                var jw = new JaroWinkler();
                double similarity = jw.GetSimilarity(str1, str2);
                return similarity;
        }
        // napisać więcej algorytmów podobieństwa, np. Levenshtein, Cosine Similarity, itp.
    }
}
//skala
/*
 * skala z internetu
 * 0.00-0.50 bardzo różne
 * 0.50-0.70 słąbo podobne
 * 0.70-0.85 podobne 
 * 0.85-0.95 bardzo podobne
 * 0.95-1.00 prawie identyczne
*/