using AutoMapper;
using System.Linq.Expressions;

namespace GymSystem.BusinessLogic.Services;

public class SessionService(IUnitOfWork unitOfWork, IMapper mapper) : ISessionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    private static readonly Expression<Func<Session, object>>[] _cardIncludes =
    [
        s => s.Trainer,
        s => s.Category,
        s => s.SessionMembers
    ];

    public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default)
    {
        var sessions = await _unitOfWork.GetRepository<Session>()
            .GetAllWithIncludesAsync(_cardIncludes, cancellationToken: cancellationToken);

        return _mapper.Map<IEnumerable<SessionViewModel>>(sessions.OrderBy(s => s.StartDate));
    }

    public async Task<CreateSessionViewModel> LoadLookupsAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default)
    {
        var categories = await _unitOfWork.GetRepository<Category>()
            .GetAllAsync(cancellationToken: cancellationToken);

        var trainers = await _unitOfWork.GetRepository<Trainer>()
            .GetAllWithIncludesAsync([t => t.Category], cancellationToken: cancellationToken);

        createSessionViewModel.Categories =
            _mapper.Map<IEnumerable<LookupItemViewModel>>(categories.OrderBy(c => c.Name));

        // Show the specialty alongside the name so the matching rule is obvious in the UI.
        createSessionViewModel.Trainers = trainers
            .OrderBy(t => t.Name)
            .Select(t => new LookupItemViewModel
            {
                Id = t.Id,
                Name = $"{t.Name} ({t.Category?.Name ?? "Unassigned"})"
            });

        return createSessionViewModel;
    }

    public async Task<CreateSessionResult> CreateSessionAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default)
    {
        // ModelState validation runs first, so the required values are present here.
        var start = createSessionViewModel.StartDate!.Value;
        var end = createSessionViewModel.EndDate!.Value;

        var category = await _unitOfWork.GetRepository<Category>()
            .GetByIdAsync(createSessionViewModel.CategoryId!.Value, cancellationToken);

        if (category is null)
            return CreateSessionResult.CategoryNotFound;

        var trainer = await _unitOfWork.GetRepository<Trainer>()
            .GetByIdAsync(createSessionViewModel.TrainerId!.Value, cancellationToken);

        if (trainer is null)
            return CreateSessionResult.TrainerNotFound;

        // A Boxing session needs a Boxing trainer.
        if (trainer.CategoryId != category.Id)
            return CreateSessionResult.SpecialtyMismatch;

        var isBusy = await _unitOfWork.GetRepository<Session>()
            .AnyAsync(s => s.TrainerId == trainer.Id
                        && s.StartDate < end
                        && start < s.EndDate,
                      cancellationToken);

        if (isBusy)
            return CreateSessionResult.TrainerBusy;

        var sessionRepo = _unitOfWork.GetRepository<Session>();

        var newSession = _mapper.Map<Session>(createSessionViewModel);

        // Session has no name on the form; the category names the session.
        newSession.Name = category.Name;

        sessionRepo.Add(newSession, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? CreateSessionResult.Success
            : CreateSessionResult.SaveFailed;
    }

}
