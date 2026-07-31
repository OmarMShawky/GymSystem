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

        createSessionViewModel.Trainers = trainers
            .OrderBy(t => t.Name)
            .Select(t => new LookupItemViewModel
            {
                Id = t.Id,
                Name = $"{t.Name} ({t.Category?.Name ?? "Unassigned"})"
            });

        return createSessionViewModel;
    }

    public async Task<Result> CreateSessionAsync(
        CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default)
    {

        var start = createSessionViewModel.StartDate!.Value;
        var end = createSessionViewModel.EndDate!.Value;

        var category = await _unitOfWork.GetRepository<Category>()
            .GetByIdAsync(createSessionViewModel.CategoryId!.Value, cancellationToken);

        if (category is null)
            return Result.NotFound("The selected category no longer exists.");

        var trainer = await _unitOfWork.GetRepository<Trainer>()
            .GetByIdAsync(createSessionViewModel.TrainerId!.Value, cancellationToken);

        if (trainer is null)
            return Result.NotFound("The selected trainer no longer exists.");

        if (trainer.CategoryId != category.Id)
            return Result.Fail("The selected trainer's specialty does not match the session category.");

        var isBusy = await _unitOfWork.GetRepository<Session>()
            .AnyAsync(s => s.TrainerId == trainer.Id
                        && s.StartDate < end
                        && start < s.EndDate,
                      cancellationToken);

        if (isBusy)
            return Result.Conflict("The trainer already has a session scheduled in that time slot.");

        var sessionRepo = _unitOfWork.GetRepository<Session>();

        var newSession = _mapper.Map<Session>(createSessionViewModel);

        newSession.Name = category.Name;

        sessionRepo.Add(newSession, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The session could not be created.");
    }

    public async Task<Result<SessionDetailsViewModel>> GetSessionDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.GetRepository<Session>()
            .GetByIdWithIncludesAsync(id, _cardIncludes, cancellationToken);

        return session is null
            ? Result.NotFound<SessionDetailsViewModel>("Session not found.")
            : _mapper.Map<SessionDetailsViewModel>(session);
    }

    public async Task<Result<EditSessionViewModel>> GetSessionForEditAsync(int id, CancellationToken cancellationToken = default)
    {
        var session = await _unitOfWork.GetRepository<Session>()
            .GetByIdWithIncludesAsync(id, _cardIncludes, cancellationToken);

        if (session is null)
            return Result.NotFound<EditSessionViewModel>("Session not found.");

        return await LoadLookupsAsync(_mapper.Map<EditSessionViewModel>(session), cancellationToken);
    }

    public async Task<EditSessionViewModel> LoadLookupsAsync(
        EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default)
    {
        var trainers = await _unitOfWork.GetRepository<Trainer>()
            .GetAllWithIncludesAsync([t => t.Category], cancellationToken: cancellationToken);

        editSessionViewModel.Trainers = trainers
            .OrderBy(t => t.Name)
            .Select(t => new LookupItemViewModel
            {
                Id = t.Id,
                Name = $"{t.Name} ({t.Category?.Name ?? "Unassigned"})"
            })
            .ToList();

        return editSessionViewModel;
    }

    public async Task<Result> UpdateSessionAsync(
        int id, EditSessionViewModel editSessionViewModel, CancellationToken cancellationToken = default)
    {
        var sessionRepo = _unitOfWork.GetRepository<Session>();

        var session = await sessionRepo.GetByIdAsync(id, cancellationToken);

        if (session is null)
            return Result.NotFound("Session not found.");

        if (ResolveStatus(session.StartDate, session.EndDate) != SessionStatus.Upcoming)
            return Result.Fail("Only upcoming sessions can be edited.");

        var start = editSessionViewModel.StartDate!.Value;
        var end = editSessionViewModel.EndDate!.Value;

        var trainer = await _unitOfWork.GetRepository<Trainer>()
            .GetByIdAsync(editSessionViewModel.TrainerId!.Value, cancellationToken);

        if (trainer is null)
            return Result.NotFound("The selected trainer no longer exists.");

        if (trainer.CategoryId != session.CategoryId)
            return Result.Fail("The selected trainer's specialty does not match the session category.");

        var isBusy = await sessionRepo
            .AnyAsync(s => s.Id != id
                        && s.TrainerId == trainer.Id
                        && s.StartDate < end
                        && start < s.EndDate,
                      cancellationToken);

        if (isBusy)
            return Result.Conflict("The trainer already has a session scheduled in that time slot.");

        session.TrainerId = trainer.Id;
        session.Description = editSessionViewModel.Description;
        session.StartDate = start;
        session.EndDate = end;

        sessionRepo.Update(session, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The session could not be updated.");
    }

    public async Task<Result> DeleteSessionAsync(int id, CancellationToken cancellationToken = default)
    {
        var sessionRepo = _unitOfWork.GetRepository<Session>();

        var session = await sessionRepo.GetByIdAsync(id, cancellationToken);

        if (session is null)
            return Result.NotFound("Session not found.");

        if (ResolveStatus(session.StartDate, session.EndDate) == SessionStatus.Ongoing)
            return Result.Conflict("An ongoing session cannot be deleted.");

        sessionRepo.Delete(session, cancellationToken);

        return (await _unitOfWork.SaveChangesAsync(cancellationToken)) > 0
            ? Result.Ok()
            : Result.Fail("The session could not be deleted.");
    }

    private static SessionStatus ResolveStatus(DateTime start, DateTime end)
    {
        var now = DateTime.Now;

        if (now < start)
            return SessionStatus.Upcoming;

        return now <= end ? SessionStatus.Ongoing : SessionStatus.Completed;
    }
}
