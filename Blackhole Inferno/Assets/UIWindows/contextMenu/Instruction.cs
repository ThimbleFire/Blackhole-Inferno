[System.Serializable]
public class Instruction
{
    public HUDSticker Sticker { get; set; } = null;
    public ContextMenuOption.Commands Command { get; set; }

    public Instruction(HUDSticker sticker, ContextMenuOption.Commands command) {
        Sticker = sticker;
        Command = command;
    }
}