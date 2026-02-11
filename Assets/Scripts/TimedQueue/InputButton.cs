public static class InputButtons
{
    public const byte NEUTRAL   = 0;
    // Directions (Bits 0-3)
    public const byte DOWN      = 0b00000001; // 1
    public const byte RIGHT     = 0b00000010; // 2
    public const byte UP        = 0b00000100; // 4
    public const byte LEFT      = 0b00001000; // 8

    public const byte LIGHT     = 0b00010000; // 16
    public const byte MEDIUM    = 0b00100000; // 32
    public const byte HEAVY     = 0b01000000; // 64
    public const byte SPECIAL   = 0b10000000; // 128

    public static byte GetForwardMask(bool isFlipped) => isFlipped ? LEFT : RIGHT;
    public static byte GetBackMask(bool isFlipped) => isFlipped ? RIGHT : LEFT;
}