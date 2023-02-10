using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Data;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.TrainStations;
using SP23.P02.Web.Features.Users;

namespace SP23.P02.Web.Controllers;

[Route("api/stations")]
[ApiController]
public class StationsController : ControllerBase
{
    private readonly DbSet<TrainStation> stations;
    private readonly DataContext dataContext;

    public StationsController(DataContext dataContext)
    {
        this.dataContext = dataContext;
        stations = dataContext.Set<TrainStation>();
    }

    [HttpGet]
    [AllowAnonymous]
    public IQueryable<TrainStationDto> GetAllStations()
    {
        return GetTrainStationDtos(stations);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{id}")]
    public ActionResult<TrainStationDto> GetStationById(int id)
    {
        var result = GetTrainStationDtos(stations.Where(x => x.Id == id)).FirstOrDefault();
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public ActionResult<TrainStationDto> CreateStation(TrainStationDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var station = new TrainStation
        {
            Name = dto.Name,
            Address = dto.Address,
            Manager = dataContext.Users.FirstOrDefault(x => x.Id == dto.ManagerId)
        };
        stations.Add(station);

        dataContext.SaveChanges();

        dto.Id = station.Id;

        return CreatedAtAction(nameof(GetStationById), new { id = dto.Id }, dto);
    }

    [HttpPut]
    [Authorize(Roles = Role.Admin)]
    [Route("{id}")]
    public ActionResult<TrainStationDto> UpdateStation(int id, TrainStationDto dto)
    {
        if (IsInvalid(dto))
        {
            return BadRequest();
        }

        var station = stations.FirstOrDefault(x => x.Id == id);
        if (station == null)
        {
            return NotFound();
        }

        station.Name = dto.Name;
        station.Address = dto.Address;
        station.Manager = dataContext.Users.FirstOrDefault(x => x.Id == dto.ManagerId);

        dataContext.SaveChanges();

        dto.Id = station.Id;

        return Ok(dto);
    }

    [HttpDelete]
    [Authorize(Roles = Role.Admin)]
    [Route("{id}")]
    public ActionResult DeleteStation(int id)
    {
        var station = stations.FirstOrDefault(x => x.Id == id);
        if (station == null)
        {
            return NotFound();
        }

        stations.Remove(station);

        dataContext.SaveChanges();

        return Ok();
    }

    private static bool IsInvalid(TrainStationDto dto)
    {
        return string.IsNullOrWhiteSpace(dto.Name) ||
               dto.Name.Length > 120 ||
               string.IsNullOrWhiteSpace(dto.Address);
    }

    private static IQueryable<TrainStationDto> GetTrainStationDtos(IQueryable<TrainStation> stations)
    {
        return stations
            .Select(x => new TrainStationDto
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                ManagerId = x.Manager.Id
    });
    }
}