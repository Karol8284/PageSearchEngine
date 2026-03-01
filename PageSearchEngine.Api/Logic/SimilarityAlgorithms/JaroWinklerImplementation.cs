using SimMetrics.Net.Metric;

namespace PageSearchEngine.Api.Logic.SimilarityAlgorithms
{
    public class JaroWinklerImplementation
    {
        public JaroWinklerImplementation(string str1, string str2)
        {
            var jw = new JaroWinkler();
            double similarity = jw.GetSimilarity(str1, str2);
            return similarity;
        }
        
    }
}
