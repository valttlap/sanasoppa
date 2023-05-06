using Bogus;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Tests.Repositories
{
    public class RefreshTokenRepositoryTest
    {
        private DataContext? _context;
        private RefreshTokenRepository? _repository;
        private Faker _faker = new Faker();

        [SetUp]
        public void Setup()
        {
            var options = RepostitoryTestUtils.CreateNewContextOptions();
            _context = new DataContext(options);
            _repository = new RefreshTokenRepository(_context);
        }

        [TearDown]
        public async Task TearDownAsync()
        {
            foreach (var entity in _context!.ChangeTracker.Entries())
            {
                entity.State = EntityState.Detached;
            }

            await _context!.Database.EnsureDeletedAsync();

            _context!.Dispose();
        }

        private string HashToken(string token)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: token,
                    salt: Array.Empty<byte>(),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 32)
                );
        }

        [Test]
        public async Task AddRefreshToken_ShouldCreateNewRefreshToken()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(7),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = false,
                User = user
            };

            var originalToken = refreshToken.Token;

            // Act
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();
            RefreshToken? result = await _context!.RefreshTokens.FindAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Token, Is.EqualTo(HashToken(originalToken)));
            Assert.That(result.Expires, Is.InRange(DateTime.UtcNow.AddDays(7).AddSeconds(-1), DateTime.UtcNow.AddDays(7).AddSeconds(1))); // Allow 1 second difference in case of slow test execution
            Assert.That(result.ClientId, Is.EqualTo(refreshToken.ClientId));
            Assert.That(result.Revoked, Is.EqualTo(false));
        }

        [Test]
        public async Task ValidateRefreshTokenAsync_ShouldReturnTrueForValidToken()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(7),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = false,
                User = user
            };

            var originalToken = refreshToken.Token;
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.ValidateRefreshTokenAsync(originalToken, user.Id, refreshToken.ClientId);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ValidateRefreshTokenAsync_ShouldReturnFalseForExpiredToken()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(-1),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = false,
                User = user
            };

            var originalToken = refreshToken.Token;
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.ValidateRefreshTokenAsync(originalToken, user.Id, refreshToken.ClientId);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ValidateRefreshTokenAsync_ShouldReturnFalseForRevokedToken()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(7),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = true,
                User = user
            };

            var originalToken = refreshToken.Token;
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.ValidateRefreshTokenAsync(originalToken, user.Id, refreshToken.ClientId);

            // Assert
            Assert.That(result, Is.False);
        }
        [Test]

        public async Task ValidateRefreshTokenAsync_ShouldReturnFalseForWrongClientId()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(7),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = false,
                User = user
            };

            var originalToken = refreshToken.Token;
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.ValidateRefreshTokenAsync(originalToken, user.Id, _faker.Random.Uuid().ToString());

            // Assert
            Assert.That(result, Is.False);
        }


        [Test]
        public async Task RevokeRefreshTokenAsyncByTokenStringAsync()
        {
           // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            var refreshToken = new RefreshToken
            {
                Token = _faker.Random.AlphaNumeric(32),
                Expires = DateTime.UtcNow.AddDays(7),
                ClientId = _faker.Random.Uuid().ToString(),
                Revoked = false,
                User = user
            };

            var originalToken = refreshToken.Token;
            _repository!.AddRefreshToken(refreshToken);
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.RevokeRefreshTokenAsync(refreshToken.Token);
            var isRevoked = await _context.RefreshTokens.AnyAsync(x => x.Token == HashToken(originalToken) && x.Revoked);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(isRevoked, Is.True);
        }

        [Test]
        public async Task RevokeAllUserRefreshTokensAsync_ShouldRevokeUserRefreshTokensAsync()
        {
            // Arrange
            var user = new AppUser
            {
                Email = _faker.Internet.Email(),
                UserName = _faker.Internet.UserName()
            };
            _context!.Users.Add(user);
            // Add 5 tokens for the user
            for (int i = 0; i < 5; i++)
            {
                var refreshToken = new RefreshToken
                {
                    Token = _faker.Random.AlphaNumeric(32),
                    Expires = DateTime.UtcNow.AddDays(7),
                    ClientId = _faker.Random.Uuid().ToString(),
                    Revoked = false,
                    User = user
                };
                _repository!.AddRefreshToken(refreshToken);
            }
            await _context!.SaveChangesAsync();

            // Act
            var result = await _repository!.RevokeAllUserRefreshTokensAsync(user.Id);
            var hasValidTokens = await _context.RefreshTokens.AnyAsync(x => x.User.Id == user.Id && !x.Revoked);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hasValidTokens, Is.False);
        }

    }
}
