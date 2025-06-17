using Core.UI;


public class OptionsPopupData : BasePopupData
{
    private bool _isGameOptions;

    public bool IsGameOptions => _isGameOptions;

    public OptionsPopupData(bool isGameOptions)
    {
        _isGameOptions = isGameOptions;
    }
}