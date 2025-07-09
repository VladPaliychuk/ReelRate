using AutoMapper;
using Microsoft.Extensions.Logging;
using Movie.BLL.DTOs.RatingDTOs;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository.Interfaces;

namespace Movie.BLL.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _ratingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RatingService> _logger;
    private readonly IMapper _mapper;
    
    public RatingService(
        IRatingRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<RatingService> logger,
        IMapper mapper)
    {
        _ratingRepository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<RatingGetDto> UpdateRatingAsync(RatingUpdateDto updateDto)
    {
        var rating = await _ratingRepository.GetByFilmIdAsync(updateDto.FilmId);
        
        if (rating == null)
        {
            _logger.LogWarning("Rating for film with Film ID {FilmId} not found.", updateDto.FilmId);
            throw new KeyNotFoundException($"Rating for film with Film ID {updateDto.FilmId} not found.");
        }
        
        rating.Score = ((rating.Score * rating.Count) + updateDto.Score) / (rating.Count + 1);
        rating.Count++;
        
        await _ratingRepository.UpdateAsync(rating);
        await _unitOfWork.SaveChangesAsync();
        
        return _mapper.Map<RatingGetDto>(rating);
    }
    
    //rating.Score = ((rating.Score * rating.Count) + newScore) / (rating.Count + 1);
    //rating.Count++;
}