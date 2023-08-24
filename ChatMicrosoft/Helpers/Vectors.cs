namespace ChatMicrosoft.Helpers
{
    public static class Vectors
    {
        public static float ConsineSimilarity(float[] vect1, float[] vect2)
        {
            return ProdottoScalare(vect1, vect2) / (Norma(vect1) * Norma(vect2));
        }

        public static float ProdottoScalare(float[] vect1, float[] vect2)
        {
            if (vect1.Length != vect2.Length)
            {
                throw new Exception("Operazione non definita tra vettori di dimensioni diverse");
            }

            float dotProduct = 0;
            for (int i = 0; i < vect2.Length; i++)
            {
                dotProduct += vect1[i] * vect2[i];
            }

            return dotProduct;
        }

        public static float Norma(float[] vector)
        {
            float ret = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                ret += vector[i] * vector[i];
            }

            return (float) Math.Sqrt(ret);
        }
    }
}
