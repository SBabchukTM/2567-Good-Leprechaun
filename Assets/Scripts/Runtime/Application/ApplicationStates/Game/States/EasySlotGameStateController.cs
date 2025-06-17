using Application.UI;
using Core.Services.Audio;
using Core;
using Cysharp.Threading.Tasks;
using Application.Services.UserData;

public class EasySlotGameStateController : BaseSlotGameStateController
{
    private readonly IUiService _uiService;
    private readonly IAudioService _audioService;
    private readonly UserDataService _userDataService;

    public EasySlotGameStateController(ILogger logger,
     IUiService uiService,
     IAudioService audioService,
     UserDataService userDataService
    ) : base(logger, uiService, audioService, userDataService)
    {
        _uiService = uiService;
        _audioService = audioService;
        _userDataService = userDataService;
    }

    protected override UniTask Initialization()
    {
        _columnCount = 3;
        return base.Initialization();
    }

    protected override void RestartLevel()
    {
        GoTo<EasySlotGameStateController>();
    }
}