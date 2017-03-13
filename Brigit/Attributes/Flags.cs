namespace Brigit.Attributes
{
    enum Flag
    {
        // Part where this flag gets set has not been encountered
        // All Flags will start as Unset
        Unset = 0x0,
        // Somesort of choice was made settings this Flag as either True
        // or False
        True = 0x1,
        False = 0x2,
        // I don't know if i'll keep this but this means that
        // that the flag is neither true or false, and it doesn't matter
        DontCare = 0x3
    }
}