public class Instruction
{
    public HUDSticker Sticker { get; set; } = null;
    public ContextMenuOption.Commands Command { get; set; }

    public Instruction() { }
    public Instruction(HUDSticker Sticker, ContextMenuOption.Commands command) {
        Sticker = sticker;
        Command = command;
    {
}