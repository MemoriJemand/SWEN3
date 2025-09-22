using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DocumentManagementTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void nonQueryInsert()
        {
            var mockSet = new Mock<DbSet<DocumentData>>();

            var mockContext = new Mock<DocumentContext>();
            mockContext.Setup(m => m.Documents).Returns(mockSet.Object);

            var repository = new DocumentRepository(mockContext.Object);
            repository.Insert(new DocumentData());

            mockSet.Verify(m => m.Add(It.IsAny<DocumentData>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}