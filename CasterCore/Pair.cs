namespace CasterCore
{
    public class Pair<TLeft, TRight> where TLeft : class where TRight : class
    {
        public TLeft Left;
        public TRight Right;

        public Pair(TLeft left, TRight right)
        {
            Left = left;
            Right = right;
        }

        public static bool operator ==(Pair<TLeft,TRight> leftOp, Pair<TLeft, TRight> rightOp)
        {
            return leftOp?.Left == rightOp?.Left && leftOp?.Right == rightOp?.Right;
        }

        public static bool operator !=(Pair<TLeft, TRight> leftOp, Pair<TLeft, TRight> rightOp)
        {
            return leftOp?.Left != rightOp?.Left || leftOp?.Right != rightOp?.Right;
        }


    }
}