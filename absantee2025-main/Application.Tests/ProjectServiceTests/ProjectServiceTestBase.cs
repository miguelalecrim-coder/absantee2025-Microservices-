

using Application.Messaging;
using AutoMapper;
using Domain.Factory;
using Domain.IRepository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Application.Tests.ProjectServiceTests;

public abstract class ProjectServiceTestBase
{
    protected Mock<IProjectFactory> ProjectFactoryDouble;
    protected Mock<IProjectRepository> ProjectRepositoryDouble;
    protected AbsanteeContext _context;
    protected Mock<IMapper> MapperDouble;
    protected Mock<IMessagePublisher> PublisherDouble;

    protected ProjectService ProjectService;

    private static readonly Random _random = new();

    protected ProjectServiceTestBase()
    {
        var options = new DbContextOptionsBuilder<AbsanteeContext>()
              .UseInMemoryDatabase(Guid.NewGuid().ToString())
              .Options;

        _context = new AbsanteeContext(options);

        ProjectRepositoryDouble = new Mock<IProjectRepository>();
        ProjectFactoryDouble = new Mock<IProjectFactory>();
        MapperDouble = new Mock<IMapper>();
        PublisherDouble = new Mock<IMessagePublisher>();


        ProjectService = new ProjectService(ProjectRepositoryDouble.Object, ProjectFactoryDouble.Object, PublisherDouble.Object, MapperDouble.Object);

    }

}