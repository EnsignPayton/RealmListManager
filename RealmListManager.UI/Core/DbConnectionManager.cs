﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace RealmListManager.UI.Core
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
                                  "[Image] BLOB NULL," +
                                  "[Index] INTEGER NOT NULL DEFAULT 0" +
                                  ");");

            // Realmlist
            _dbConnection.Execute("CREATE TABLE IF NOT EXISTS [Realmlist] (" +
                                  "[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY," +
                                  "[Name] VARCHAR NOT NULL," +
                                  "[Url] VARCHAR NOT NULL," +
                                  "[Image] BLOB NULL," +
                                  "[Index] INTEGER NOT NULL DEFAULT 0," +
                                  "[LocationId] UNIQUEIDENTIFIER NOT NULL," +
                                  "FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION" +
                                  ");");

            // Migrations
            MigrateLocation();
            MigrateRealmlist();
        }

        private void MigrateLocation()
        {
            dynamic columns = _dbConnection.Query("PRAGMA table_info(Location);");
            foreach (var column in columns)
            {
                if (column.name == "Index") return;
            }

            _dbConnection.Execute("ALTER TABLE [Location] ADD COLUMN [Index] INTEGER NOT NULL DEFAULT 0");
        }

        private void MigrateRealmlist()
        {
            dynamic columns = _dbConnection.Query("PRAGMA table_info(Realmlist);");
            foreach (var column in columns)
            {
                if (column.name == "Index") return;
            }

            _dbConnection.Execute("ALTER TABLE [Realmlist] ADD COLUMN [Index] INTEGER NOT NULL DEFAULT 0");
        }

        public void InsertLocation(Entities.Location entity)
        {
            _dbConnection.Execute("INSERT INTO Location VALUES(@Id, @Name, @Path, @Image, @Index)",
                new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Path = entity.Path,
                    Image = entity.Image,
                    Index = entity.Index
                });
        }

        public void InsertRealmlist(Entities.Realmlist entity, Guid locationId)
        {
            _dbConnection.Execute(
                "INSERT INTO Realmlist VALUES(@Id, @Name, @Url, @Image, @Index, @LocationId)",
                new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Url = entity.Url,
                    Image = entity.Image,
                    Index = entity.Index,
                    LocationId = locationId
                });
        }

        public void UpdateLocation(Entities.Location entity)
        {
            _dbConnection.Execute(
                "UPDATE Location SET Name = @Name, Path = @Path, Image = @Image, [Index] = @Index WHERE Id = @Id",
                new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Path = entity.Path,
                    Image = entity.Image,
                    Index = entity.Index
                });
        }

        public void UpdateRealmlist(Entities.Realmlist entity, Guid locationId)
        {
            _dbConnection.Execute(
                "UPDATE Realmlist SET Name = @Name, Url = @Url, Image = @Image, [Index] = @Index, LocationId = @LocationId WHERE Id = @Id",
                new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Url = entity.Url,
                    Image = entity.Image,
                    Index = entity.Index,
                    LocationId = locationId
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

        public void DeleteLocation(Guid locationId)
        {
            _dbConnection.Execute("DELETE FROM Realmlist WHERE LocationId = @LocationId",
                new { LocationId = locationId });
            _dbConnection.Execute("DELETE FROM Location WHERE Id = @LocationId",
                new { LocationId = locationId });
        }

        public void DeleteRealmlist(Guid realmlistId)
        {
            _dbConnection.Execute("DELETE FROM Realmlist WHERE Id = @Id",
                new { Id = realmlistId });
        }
    }
}
