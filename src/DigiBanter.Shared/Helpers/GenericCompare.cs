namespace DigiBanter.Shared.Helpers
{
    public class GenericCompare<T> : IEqualityComparer<T> where T : class
    {
        private Func<T, object> _expr { get; set; }
        public GenericCompare(Func<T, object> expr)
        {
            _expr = expr;
        }
        public bool Equals(T x, T y)
        {
            var first = _expr.Invoke(x);
            var sec = _expr.Invoke(y);
            if (first != null && first.Equals(sec))
                return true;
            else
                return false;
        }
        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }


}
