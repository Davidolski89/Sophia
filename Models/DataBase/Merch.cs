using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Models.DataBase
{
    public class Merch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public MerchType Type { get; set; }
        public DateTime Added { get; set; }
        public List<SophiaUser> SophiaUsers {get;set;}
    }
    public enum MerchType
    {
        Video,
        TextFile,
        AudioFile
    }
}
