namespace Extellect.Nuvo
{
    public class DoubleSimilarity : ISimilarity
    {
        private readonly double _tolerance;

        public Type ComparandType => typeof(double);

        public DoubleSimilarity(double tolerance)
        {
            _tolerance = tolerance;
        }

        public bool AreSimilar(object x, object y)
        {
            return ((double)x).IsWithinToleranceOf((double)y, _tolerance);
        }
    }
}
