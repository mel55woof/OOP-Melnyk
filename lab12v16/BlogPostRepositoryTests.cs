using Xunit;
using lab12v16;
using System.Linq;
using System.Collections.Generic;
using System;

namespace lab12v16.Tests
{
    public class BlogPostRepositoryTests
    {
        [Fact]
        public void Add_ShouldAddItem_WhenItemIsValid()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();
            var post = new BlogPost(1, "Test Title", "Test Content", 10);

            // Act
            repo.Add(post);
            var result = repo.GetById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Title", result.Title);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdNotFound()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();

            // Act
            var result = repo.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();
            repo.Add(new BlogPost(1, "Post 1", "Content 1", 1));
            repo.Add(new BlogPost(2, "Post 2", "Content 2", 2));

            // Act
            var results = repo.GetAll().ToList();

            // Assert
            Assert.Equal(2, results.Count);
        }

        [Fact]
        public void Update_ShouldModifyExistingItem()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();
            repo.Add(new BlogPost(1, "Old Title", "Content", 1));
            var updatedPost = new BlogPost(1, "New Title", "Content", 1);

            // Act
            repo.Update(updatedPost);
            var result = repo.GetById(1);

            // Assert
            Assert.Equal("New Title", result.Title);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenItemNotFound()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();
            var nonExistentPost = new BlogPost(99, "Title", "Content", 1);

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repo.Update(nonExistentPost));
        }

        [Fact]
        public void Delete_ShouldRemoveItem_WhenIdExists()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();
            repo.Add(new BlogPost(1, "To Delete", "Content", 1));

            // Act
            repo.Delete(1);
            var result = repo.GetById(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenIdNotFound()
        {
            // Arrange
            var repo = new InMemoryRepository<BlogPost>();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => repo.Delete(999));
        }
    }
}
