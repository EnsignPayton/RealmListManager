using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace RealmListManager.UI.Core.Utilities
{
    public class DbConnectionManager
    {
        private readonly IDbConnection _dbConnection;

        public DbConnectionManager(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public void CreateTables()
        {
            // Location
            _dbConnection.Execute("CREATE TABLE IF NOT EXISTS [Location] (" +
                                  "[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY," +
                                  "[Name] VARCHAR NOT NULL," +
                                  "[Path] VARCHAR NOT NULL," +
                                  "[Image] BLOB NULL" +
                                  ");");

            // Realmlist
            _dbConnection.Execute("CREATE TABLE IF NOT EXISTS [Realmlist] (" +
                                  "[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY," +
                                  "[Name] VARCHAR NOT NULL," +
                                  "[Url] VARCHAR NOT NULL," +
                                  "[Image] BLOB NOT NULL," +
                                  "[LocationId] UNIQUEIDENTIFIER NOT NULL," +
                                  "FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION" +
                                  ");");
        }

        public void InsertLocation(Entities.Location entity)
        {
            _dbConnection.Execute("INSERT INTO Location VALUES(@Id, @Name, @Path, @Image)",
                new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Path = entity.Path,
                    Image = entity.Image
                });
        }

        public IEnumerable<Entities.Location> QueryLocations()
        {
            return _dbConnection.Query<Entities.Location>("SELECT * FROM Location");
        }

        public IEnumerable<Entities.Realmlist> QueryRealmlistsByLocation(Guid locationId)
        {
            return _dbConnection.Query<Entities.Realmlist>("SELECT * FROM Realmlist WHERE LocationId = @LocationId",
                new {LocationId = locationId});
        }
    }
}
