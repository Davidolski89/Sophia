using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Sophia.Models.Rogatio;

namespace Sophia.Models.DataBase
{
    // Datenbank Context
    // Schnittstelle von Klassen/Objekten zu Datenbanktabellen
    // Das DbSet<Survey> ist eine Liste von Objekten vom Typ Survey. Also alle einträge aus der Tabelle Survey in der DB
    public class SophiaDB : IdentityDbContext<SophiaUser>
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answeres { get; set; }
        public DbSet<Merch> Merch { get; set; }
        public DbSet<SophiaUser> SophiaUsers { get; set; }
        public SophiaDB(DbContextOptions<SophiaDB> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Playlist>().HasKey(c => c.Id);
            //modelBuilder.Entity<MusicFile>().HasKey(c => c.Id);
            //modelBuilder.Entity<PlaylistMusicFile>().HasKey(x => new { x.MusicFileId, x.PlaylistId });
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<Question>()
                .HasOne(x => x.Survey)
                .WithMany(y => y.Questions)
                .HasForeignKey(z => z.SurveyId);
            modelBuilder.Entity<Survey>()
                .HasMany(b => b.Questions)
                .WithOne();

            modelBuilder.Entity<Question>()
                .HasMany(x => x.Answers)
                .WithOne();
            modelBuilder.Entity<Answer>()
                .HasOne(b => b.Question)
                .WithMany(v => v.Answers)
                .HasForeignKey(c => c.QuestionId);

            //modelBuilder.Entity<Answer>()
            //    .HasOne(b => b.SophiaUser)
            //    .WithMany(v => v.Answers)
            //    .HasForeignKey(c => c.SophiaUserId);
            //modelBuilder.Entity<SophiaUser>()
            //    .HasMany(c => c.Answers)
            //    .WithOne();
        }







        static bool check()
        {
            string path = Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName, "Database", "Sophia.db");
            SqliteConnection sqlite = new SqliteConnection("Data Source=" + path);
            SqliteCommand checkfortable = new SqliteCommand("select * from AspNetRoles;", sqlite);
            try
            {
                sqlite.Open();
                checkfortable.ExecuteNonQuery();
                sqlite.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static void generate()
        {
            string dbDirectory = Path.Combine(Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName, "Database");
            string creationString;
            using (StreamReader reader = new StreamReader(Path.Combine(dbDirectory, "SophiaDBCreationString")))
            {
                creationString = reader.ReadToEnd();
            }
            string path = Path.Combine(dbDirectory, "Sophia.db");
            SqliteConnection sqlite = new SqliteConnection("Data Source=" + path);
            SqliteCommand createAllTables = new SqliteCommand(creationString, sqlite);
            sqlite.Open();
            createAllTables.ExecuteNonQuery();
            sqlite.Close();
        }
        public static void CheckAndCreateDatabase()
        {
            if (!check())
                generate();
        }
    }
}
