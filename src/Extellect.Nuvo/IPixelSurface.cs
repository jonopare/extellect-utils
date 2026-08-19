namespace Extellect.Nuvo
{
    public interface IPixelSurface
    {
        int Width { get; }
        int Height { get; }
        Color this[int x, int y] { get; set; }
    }
}
