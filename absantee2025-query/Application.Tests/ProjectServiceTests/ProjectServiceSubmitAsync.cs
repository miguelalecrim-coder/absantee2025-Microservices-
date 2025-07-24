using Application.Tests.ProjectServiceTests;
using Domain.Interfaces;
using Domain.Models;
using Infrastructure.DataModel;
using Moq;

public class ProjectServiceSubmiteAsync : ProjectServiceTestBase
{
   [Fact]
    public async Task Submit_ShouldCallAddAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var visitor = new ProjectDataModel
        {
            Id = id,
            Title = "Title",
            Acronym = "ACR",
            PeriodDate = new PeriodDate()
        };

        var project = new Project(id, visitor.Title, visitor.Acronym, visitor.PeriodDate);

        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);

        // Act
        await ProjectService.SubmitAsync(id, visitor.Title, visitor.Acronym, visitor.PeriodDate);

        // Assert
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldCallFactoryWithCorrectDataModel()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate 
        { 
            InitDate = new DateOnly(2025, 1, 1), 
            FinalDate = new DateOnly(2025, 12, 31) 
        };

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm =>
            dm.Id == id &&
            dm.Title == title &&
            dm.Acronym == acronym &&
            dm.PeriodDate == periodDate)), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldThrowException_WhenFactoryThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>()))
            .Throws(new InvalidOperationException("Factory error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            ProjectService.SubmitAsync(id, title, acronym, periodDate));
        Assert.Equal("Factory error", ex.Message);
        
        // Verificar que o repositório não foi chamado
        ProjectRepositoryDouble.Verify(r => r.AddAsync(It.IsAny<Project>()), Times.Never);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_ShouldThrowException_WhenAddAsyncThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>()))
            .ThrowsAsync(new InvalidOperationException("Add error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            ProjectService.SubmitAsync(id, title, acronym, periodDate));
        Assert.Equal("Add error", ex.Message);
        
        // Verificar que SaveChanges não foi chamado
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task SubmitAsync_ShouldThrowException_WhenSaveChangesAsyncThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync())
            .ThrowsAsync(new InvalidOperationException("Save error"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            ProjectService.SubmitAsync(id, title, acronym, periodDate));
        Assert.Equal("Save error", ex.Message);
        
        // Verificar que AddAsync foi chamado antes do erro
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
    }


    [Fact]
    public async Task SubmitAsync_ShouldHandleNullPeriodDate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        PeriodDate periodDate = null;

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm =>
            dm.Id == id &&
            dm.Title == title &&
            dm.Acronym == acronym &&
            dm.PeriodDate == periodDate)), Times.Once);
        
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldHandleEmptyGuid()
    {
        // Arrange
        var id = Guid.Empty;
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm =>
            dm.Id == id &&
            dm.Title == title &&
            dm.Acronym == acronym &&
            dm.PeriodDate == periodDate)), Times.Once);
        
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldHandleWhitespaceStrings()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "   ";
        var acronym = "T";
        var periodDate = new PeriodDate();

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync(project);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm =>
            dm.Id == id &&
            dm.Title == title &&
            dm.Acronym == acronym &&
            dm.PeriodDate == periodDate)), Times.Once);
        
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldExecuteOperationsInCorrectOrder()
    {
        // Arrange
        var id = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        var project = new Project(id, title, acronym, periodDate);
        ProjectFactoryDouble.Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);

        var sequence = new MockSequence();
        ProjectFactoryDouble.InSequence(sequence).Setup(f => f.Create(It.IsAny<ProjectDataModel>())).Returns(project);
        ProjectRepositoryDouble.InSequence(sequence).Setup(r => r.AddAsync(project)).ReturnsAsync(project);
        ProjectRepositoryDouble.InSequence(sequence).Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id, title, acronym, periodDate);

        // Assert
        // As verificações de sequência são feitas automaticamente pelo MockSequence
        ProjectFactoryDouble.Verify(f => f.Create(It.IsAny<ProjectDataModel>()), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task SubmitAsync_ShouldCreateUniqueDataModelForEachCall()
    {
        // Arrange
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var title = "Test Project";
        var acronym = "TP";
        var periodDate = new PeriodDate();

        var project1 = new Project(id1, title, acronym, periodDate);
        var project2 = new Project(id2, title, acronym, periodDate);

        ProjectFactoryDouble.Setup(f => f.Create(It.Is<ProjectDataModel>(dm => dm.Id == id1))).Returns(project1);
        ProjectFactoryDouble.Setup(f => f.Create(It.Is<ProjectDataModel>(dm => dm.Id == id2))).Returns(project2);
        ProjectRepositoryDouble.Setup(r => r.AddAsync(It.IsAny<Project>())).ReturnsAsync((Project p) => p);
        ProjectRepositoryDouble.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await ProjectService.SubmitAsync(id1, title, acronym, periodDate);
        await ProjectService.SubmitAsync(id2, title, acronym, periodDate);

        // Assert
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm => dm.Id == id1)), Times.Once);
        ProjectFactoryDouble.Verify(f => f.Create(It.Is<ProjectDataModel>(dm => dm.Id == id2)), Times.Once);
        
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project1), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.AddAsync(project2), Times.Once);
        ProjectRepositoryDouble.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
    }
}