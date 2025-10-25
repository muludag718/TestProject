namespace TestProject.P3DFile.Helpers;

public static class NumberHelpers
{
    public static int Align(this int value, int align)
    {
        if (value != 0)
        {
            return value + value.Padding(align);
        }
        return 0;
    }
    public static int Padding(this int value, int align)
    {
        return (align - value % align) % align;
    }
}
