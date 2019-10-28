using LiteDB;
using System;
using System.IO;

namespace BotsController.DAL
{
    public sealed class LiteDbContext : IDisposable
    {
        private readonly LiteDatabase db;

        public LiteDbContext(string connection)
        {
            // create directory if needed
            var dirPath = Path.GetDirectoryName(connection);
            if (!string.IsNullOrEmpty(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            db = new LiteDatabase(connection);
        }

        public LiteCollection<T> SetCollection<T>()
        {
            return db.GetCollection<T>();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
