public interface IParser<O, D>
{
    D Parse(O origem);
    List<D> Parse(List<O> origem);
}